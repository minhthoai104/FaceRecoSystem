using FaceRecognitionDotNet;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Size = OpenCvSharp.Size;
using Point = OpenCvSharp.Point;

namespace FaceRecoSystem.core
{
    public class PersonManager
    {
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;

        public PersonManager(FaceRecognition fr, FaceDatabase db)
        {
            _fr = fr;
            _db = db;
        }

        public byte[] GetFaceEncodingAsBytes(Mat face) => _db.GetFaceEncodingAsBytes(face);

        public bool Update(User user)
        {
            try
            {
                _db.UpdateUser(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Update] {ex.Message}");
                return false;
            }
        }

        public bool AddNewUser(User newUser, List<Mat> faces)
        {
            try
            {
                _db.SaveFaceImagesAndEncodings(newUser, faces);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddNewUser] {ex.Message}");
                return false;
            }
        }

        public List<Mat> CaptureThreeAngles(string name)
        {
            using var capture = new VideoCapture(0);
            using var faceCascade = new CascadeClassifier(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "haarcascade_frontalface_default.xml")
            );

            var faces = new List<Mat>();
            var encodings = new List<FaceEncoding>();
            int shot = 0;

            string[] angles = { "Front", "Left", "Right" };
            Scalar[] colors = { Scalar.LimeGreen, Scalar.DeepSkyBlue, Scalar.Yellow };
            var window = new Window("Face Capture", WindowFlags.AutoSize);

            while (shot < 3)
            {
                using var frame = new Mat();
                capture.Read(frame);
                if (frame.Empty()) continue;

                using var gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                var detected = faceCascade.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new Size(80, 80));

                string msg = $"Step {shot + 1}/3: Look {angles[shot]} - Press SPACE to capture";
                Scalar color = colors[shot];

                if (detected.Length == 1)
                    Cv2.Rectangle(frame, detected[0], color, 2);
                else
                {
                    msg = detected.Length == 0 ? "No face detected!" : "Multiple faces detected!";
                    color = Scalar.Red;
                }

                Cv2.PutText(frame, msg, new Point(10, 30), HersheyFonts.HersheyComplex, 0.7, color, 2);
                window.ShowImage(frame);

                int key = Cv2.WaitKey(10);
                if (key == 27) break; // ESC

                if (key == 32 && detected.Length == 1) // SPACE
                {
                    var rect = detected[0];
                    Mat faceImg = new Mat(frame, rect).Clone();

                    using var img = faceImg.ToFaceRecognitionImage();
                    var enc = _fr.FaceEncodings(img).FirstOrDefault();

                    if (enc != null)
                    {
                        // Compare with previous captures to ensure same person
                        if (encodings.Count > 0)
                        {
                            double dist = FaceRecognition.FaceDistance(encodings[0], enc);
                            if (dist > 0.55)
                            {
                                MessageBox.Show("Face mismatch detected. Please restart capture!", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                faces.ForEach(f => f.Dispose());
                                encodings.ForEach(e => e.Dispose());
                                faces.Clear();
                                encodings.Clear();
                                shot = 0;
                                continue;
                            }
                        }

                        faces.Add(faceImg);
                        encodings.Add(enc);
                        shot++;
                        Thread.Sleep(800);
                    }
                }
            }

            Cv2.DestroyAllWindows();
            return faces.Count == 3 ? faces : null;
        }

        public bool DeletePersonByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            try
            {
                _db.DeleteUser(name);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeletePersonByName] {ex.Message}");
                return false;
            }
        }

        public bool CheckIn(string userId, System.Drawing.Image img)
        {
            try
            {
                using var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.ConnectionString);
                conn.Open();

                string sql = "INSERT INTO Attendance (UserID, CheckInTime, CheckInImage) VALUES (@UserID, GETDATE(), @Image)";
                using var cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarBinary, -1).Value = ImageToBytes(img);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during check-in: " + ex.Message);
                return false;
            }
        }

        public bool CheckOut(string userId, System.Drawing.Image img)
        {
            try
            {
                using var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.ConnectionString);
                conn.Open();

                string sql = @"UPDATE Attendance 
                               SET CheckOutTime = GETDATE(), CheckOutImage = @Image 
                               WHERE UserID = @UserID AND CheckOutTime IS NULL";
                using var cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarBinary, -1).Value = ImageToBytes(img);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during check-out: " + ex.Message);
                return false;
            }
        }

        private static byte[] ImageToBytes(System.Drawing.Image img)
        {
            if (img == null) return null;
            using var ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public static Mat EnsureBgr(Mat input)
        {
            if (input == null || input.Empty())
                throw new ArgumentException("Ảnh đầu vào rỗng!");

            if (input.Channels() == 4)
            {
                Mat bgr = new Mat();
                Cv2.CvtColor(input, bgr, ColorConversionCodes.BGRA2BGR);
                return bgr;
            }

            return input.Clone();
        }

    }

    // ===== Mat Extension =====
    public static class MatExtensions
    {
        public static FaceRecognitionDotNet.Image ToFaceRecognitionImage(this Mat mat)
        {
            if (mat.Empty())
                throw new ArgumentException("Empty Mat image");

            using var bmp = BitmapConverter.ToBitmap(mat);

            // Save bitmap as temporary file
            string tempPath = Path.Combine(Path.GetTempPath(), $"face_temp_{Guid.NewGuid():N}.bmp");
            bmp.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            // Load image into FaceRecognitionDotNet
            var img = FaceRecognition.LoadImageFile(tempPath);

            // Delete the temp file after load (slight delay to avoid Marshal issues)
            new Thread(() =>
            {
                Thread.Sleep(300);
                try { File.Delete(tempPath); } catch { }
            })
            { IsBackground = true }.Start();

            return img;
        }

    }
}
