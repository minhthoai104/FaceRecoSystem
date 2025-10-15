using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace FaceRecoSystem
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
        public bool SavePersonToDatabase(
            string userId,
            string fullName,
            int age,
            string gender,
            string address,
            byte[] faceFrontVec,
            byte[] faceLeftVec,
            byte[] faceRightVec)
        {
            try
            {
                using var connection = new SqlConnection(DatabaseHelper.ConnectionString);
                connection.Open();

                string query = @"
                    INSERT INTO Users 
                        (UserID, FullName, Age, Gender, Address, 
                         FaceFront, FaceLeft, FaceRight, CreatedAt, UpdatedAt)
                    VALUES 
                        (@UserID, @FullName, @Age, @Gender, @Address, 
                         @FaceFront, @FaceLeft, @FaceRight, @CreatedAt, @UpdatedAt)";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.Add("@FaceFront", SqlDbType.VarBinary, -1).Value = (object)faceFrontVec ?? DBNull.Value;
                cmd.Parameters.Add("@FaceLeft", SqlDbType.VarBinary, -1).Value = (object)faceLeftVec ?? DBNull.Value;
                cmd.Parameters.Add("@FaceRight", SqlDbType.VarBinary, -1).Value = (object)faceRightVec ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nhân viên vào DB:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<User> GetAllPersons()
        {
            var persons = new List<User>();
            using (var connection = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT UserID, FullName, Age, Gender, Address, FaceFront FROM Users";
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var person = new User
                                {
                                    UserID = reader["UserID"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Age = Convert.ToInt32(reader["Age"]),
                                    Gender = reader["Gender"].ToString(),
                                    Address = reader["Address"].ToString()
                                };

                                if (reader["FaceFront"] != DBNull.Value)
                                {
                                    byte[] faceData = (byte[])reader["FaceFront"];
                                    person.FaceFrontPath = SaveByteArrayToTempFile(faceData, person.UserID, "Front");
                                }
                                persons.Add(person);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[PersonManager] GetAllPersons error: " + ex.Message);
                    }
                }
            }
            return persons;
        }

        public string[] AddPersonUI(string name, int age, string gender, string address)
        {
            return CaptureFacesOnly(name, age, gender, address, true);
        }

        // ==================== CẬP NHẬT NGƯỜI DÙNG ====================
        public async Task UpdatePersonUI(string name, int age, string gender, string address, bool recapture)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var connection = new SqlConnection(DatabaseHelper.ConnectionString);
            await connection.OpenAsync();

            if (!recapture)
            {
                string query = "UPDATE Users SET Age=@Age, Gender=@Gender, Address=@Address, UpdatedAt=@UpdatedAt WHERE FullName=@FullName";
                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@FullName", name);
                int rows = await cmd.ExecuteNonQueryAsync();

                MessageBox.Show(rows > 0 ? "Cập nhật thông tin thành công!" : "Không tìm thấy người cần cập nhật!", "Kết quả");
                return;
            }

            CaptureFacesOnly(name, age, gender, address, true);
        }

        public string[] CaptureFacesOnly(string name, int age, string gender, string address, bool isNew)
        {
            using var capture = new VideoCapture(0);
            using var faceCascade = new CascadeClassifier(@"D:\Work\Project\FaceRecoSystem\models\haarcascade_frontalface_default.xml");

            var faces = new List<Mat>();
            var encodings = new List<FaceEncoding>();
            int shot = 0;
            var window = new Window(isNew ? "Add Face" : "Update Face", WindowFlags.AutoSize);
            string[] angles = { "Front", "Turn Left", "Turn Right" };
            Scalar[] colors = { Scalar.LimeGreen, Scalar.DeepSkyBlue, Scalar.Yellow };

            while (shot < 3)
            {
                using var frame = new Mat();
                capture.Read(frame);
                if (frame.Empty()) continue;

                using var gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                var detectedFaces = faceCascade.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new Size(80, 80));

                string message = $"Step {shot + 1}/3: Look {angles[shot]} - Press SPACE";
                Scalar color = colors[shot];

                if (detectedFaces.Length == 1)
                    Cv2.Rectangle(frame, detectedFaces[0], color, 2);
                else if (detectedFaces.Length > 1)
                {
                    message = "Multiple faces detected!";
                    color = Scalar.Red;
                }
                else
                {
                    message = "No face detected!";
                    color = Scalar.Red;
                }

                Cv2.PutText(frame, message, new Point(10, 30),
                    HersheyFonts.HersheyComplex, 0.7, color, 2);

                window.ShowImage(frame);
                int key = Cv2.WaitKey(10);
                if (key == 27) break;

                if (key == 32 && detectedFaces.Length == 1)
                {
                    var rect = detectedFaces[0];
                    Mat faceImg = new Mat(frame, rect).Clone();

                    using var img = faceImg.ToFaceRecognitionImage();
                    var enc = _fr.FaceEncodings(img).FirstOrDefault();
                    if (enc != null)
                    {
                        bool valid = true;
                        if (encodings.Count > 0)
                        {
                            double dist = FaceRecognition.FaceDistance(encodings[0], enc);
                            Console.WriteLine($"Distance with first image: {dist:F3}");

                            // Nếu khác quá (ví dụ > 0.55) thì không cùng người
                            if (dist > 0.55)
                            {
                                Cv2.DestroyAllWindows();
                                MessageBox.Show(
                                    $"Ảnh thứ {shot + 1} không trùng khớp với ảnh đầu tiên!\nVui lòng chụp lại từ đầu để đảm bảo cùng 1 người.",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning
                                );
                                // Dọn dẹp và restart
                                foreach (var f in faces) f.Dispose();
                                faces.Clear();
                                encodings.Clear();
                                shot = 0;
                                capture.Release();
                                capture.Open(0);
                                window = new Window(isNew ? "Add Face" : "Update Face", WindowFlags.AutoSize);
                                continue;
                            }
                        }

                        if (encodings.Count > 0)
                        {
                            var dists = encodings.Select(e => FaceRecognition.FaceDistance(e, enc)).ToList();
                            double avg = dists.Average();
                            valid = avg <= 0.45;
                        }

                        if (valid)
                        {
                            faces.Add(faceImg);
                            encodings.Add(enc);
                            shot++;
                            Thread.Sleep(700);
                        }
                    }
                }
            }

            Cv2.DestroyAllWindows();

            if (faces.Count == 3)
            {
                string safeName = RemoveVietnameseDiacritics(name);
                string folder = Path.Combine(_db.FacesPath, $"{safeName}_{DateTime.Now:MMddHHmm}");
                Directory.CreateDirectory(folder);

                string pathFront = Path.Combine(folder, $"{safeName}_Front.jpg");
                string pathLeft = Path.Combine(folder, $"{safeName}_Left.jpg");
                string pathRight = Path.Combine(folder, $"{safeName}_Right.jpg");

                Cv2.ImWrite(pathFront, faces[0]);
                Cv2.ImWrite(pathLeft, faces[1]);
                Cv2.ImWrite(pathRight, faces[2]);
                return new[] { pathFront, pathLeft, pathRight };
            }

            foreach (var f in faces) f.Dispose();
            foreach (var e in encodings) e.Dispose();

            return null;
        }


        public bool UpdatePerson(User user)
        {
            using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string query = @"UPDATE Users SET 
                            Age = @Age, 
                            Gender = @Gender,
                            Address = @Address,
                            FaceFront = @FaceFront,
                            FaceLeft = @FaceLeft,
                            FaceRight = @FaceRight,
                            UpdatedAt = @UpdatedAt
                         WHERE FullName = @FullName";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Age", user.Age);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@FaceFront", (object)user.FaceFrontEncoding ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FaceLeft", (object)user.FaceLeftEncoding?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FaceRight", (object)user.FaceRightEncoding ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public User GetPersonByName(string fullName)
        {
            using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Users WHERE FullName = @FullName";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader["UserID"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Gender = reader["Gender"].ToString(),
                                Address = reader["Address"].ToString(),
                                FaceFrontEncoding = reader["FaceFront"] as byte[],
                                FaceLeftEncoding = reader["FaceLeft"] as byte[],
                                FaceRightEncoding = reader["FaceRight"] as byte[],
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<User> FindUserByNameAsync(string name)
        {
            User user = null;
            await Task.Run(() =>
            {
                using var connection = new SqlConnection(DatabaseHelper.ConnectionString);
                string query = "SELECT * FROM Users WHERE FullName = @FullName";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", name);
                try
                {
                    connection.Open();
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = reader["UserID"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Gender = reader["Gender"].ToString(),
                            Address = reader["Address"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };

                        if (reader["FaceFront"] != DBNull.Value)
                            user.FaceFrontPath = SaveByteArrayToTempFile((byte[])reader["FaceFront"], user.UserID, "Front");
                        if (reader["FaceLeft"] != DBNull.Value)
                            user.FaceLeftPath = SaveByteArrayToTempFile((byte[])reader["FaceLeft"], user.UserID, "Left");
                        if (reader["FaceRight"] != DBNull.Value)
                            user.FaceRightPath = SaveByteArrayToTempFile((byte[])reader["FaceRight"], user.UserID, "Right");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm người dùng: " + ex.Message);
                }
            });
            return user;
        }
        public async Task<bool> UpdatePersonAsync(string name, int age, string gender, string address, Bitmap newFaceImage)
        { 
            return await Task.Run(() => 
            { 
                try 
                { 
                    var person = _db.GetPersonByName(name); 
                    if (person == null) return false; 
                    person.Age = age; 
                    person.Gender = gender; 
                    person.Address = address; 
                    if (newFaceImage != null) 
                    { 
                        using var mat = BitmapConverter.ToMat(newFaceImage);
                        using var frImg = mat.ToFaceRecognitionImage(); 
                        var enc = _fr.FaceEncodings(frImg).FirstOrDefault(); 
                        if (enc != null) 
                        { 
                            _db.UpdateEncoding(name, enc); 
                            string folder = Path.Combine(_db.FacesPath, $"{name}_{person.UserID}"); 
                            Directory.CreateDirectory(folder); 
                            string newPath = Path.Combine(folder, $"{name}_updated.jpg"); 
                            Cv2.ImWrite(newPath, mat); 
                        } 
                    } 
                    _db.SavePerson(person); 
                    return true; 
                } 
                catch (Exception ex) 
                { 
                    Console.WriteLine("Update failed: " + ex.Message); 
                    return false; 
                } 
            }); 
        } 
        public bool DeletePersonByName(string name) 
        { 
            try 
            { 
                _db.Delete(name); 
                return true; 
            } catch (Exception ex) 
            { 
                Console.WriteLine("[PersonManager] Delete error: " + ex.Message); 
                return false; 
            } 
        }
        public int CheckIn(string userId, System.Drawing.Image checkInImage) 
        { 
            using (var connection = new SqlConnection(DatabaseHelper.ConnectionString)) 
            { 
                string query = "INSERT INTO Attendance (UserID, CheckInTime, CheckInImage) " +
                    "OUTPUT INSERTED.AttendanceID " +
                    "VALUES (@UserID, @CheckInTime, @CheckInImage)"; 
                using var command = new SqlCommand(query, connection); 
                command.Parameters.AddWithValue("@UserID", userId); 
                command.Parameters.AddWithValue("@CheckInTime", DateTime.Now);
                command.Parameters.Add("@CheckInImage", System.Data.SqlDbType.VarBinary, -1) .Value = ImageToByteArray(checkInImage); 
                try 
                { 
                    connection.Open(); 
                    return (int)command.ExecuteScalar(); 
                } catch (Exception ex) 
                {
                    MessageBox.Show("Lỗi khi check-in: " + ex.Message); 
                    return -1; 
                } 
            } 
        } 
        public bool CheckOut(string userId, System.Drawing.Image checkOutImage) 
        { 
            using (var connection = new SqlConnection(DatabaseHelper.ConnectionString)) 
            { 
                string query = "UPDATE Attendance SET CheckOutTime = @CheckOutTime, CheckOutImage = @CheckOutImage, UpdatedAt = @UpdatedAt " + 
                    "WHERE AttendanceID = (SELECT TOP 1 AttendanceID FROM Attendance " + 
                    "WHERE UserID = @UserID AND CheckOutTime IS NULL " + 
                    "ORDER BY CheckInTime DESC)";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId); 
                command.Parameters.AddWithValue("@CheckOutTime", DateTime.Now); 
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now); 
                command.Parameters.Add("@CheckOutImage", System.Data.SqlDbType.VarBinary, -1) .Value = ImageToByteArray(checkOutImage); 
                try 
                {
                    connection.Open(); 
                    int rowsAffected = command.ExecuteNonQuery(); 
                    return rowsAffected > 0; 
                } catch (Exception ex) 
                { 
                    MessageBox.Show("Lỗi khi checkout: " + ex.Message); return false;
                }
            }
        }

        public byte[] GetFaceEncodingAsBytes(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;

            using var img = FaceRecognition.LoadImageFile(imagePath);
            var encodings = _fr.FaceEncodings(img).ToList();

            if (encodings.Count == 0)
                return null;

            var encoding = encodings[0];
            var floatArray = encoding.GetRawEncoding();
            var bytes = new byte[floatArray.Length * sizeof(float)];
            Buffer.BlockCopy(floatArray, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string RemoveVietnameseDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Chuẩn hóa Unicode Form D (phân tách ký tự + dấu)
            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC);

            // Bỏ ký tự đặc biệt, dấu cách
            result = new string(result.Where(c => char.IsLetterOrDigit(c)).ToArray());

            return result;
        }

        private static string SaveByteArrayToTempFile(byte[] bytes, string userId, string angle)
        {
            if (bytes == null || bytes.Length == 0) return null;
            string tempDir = Path.Combine(Path.GetTempPath(), "FaceReco");
            Directory.CreateDirectory(tempDir);

            string tempPath = Path.Combine(tempDir, $"{userId}_{angle}_{Guid.NewGuid()}.jpg");
            File.WriteAllBytes(tempPath, bytes);
            return tempPath;
        }

        private static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            if (image == null) return null;
            using var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }

    public static class MatExtensions
    {
        public static FaceRecognitionDotNet.Image ToFaceRecognitionImage(this Mat mat)
        {
            if (mat.Empty())
                throw new ArgumentException("Mat is empty");

            if (mat.Channels() != 3)
                Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);

            using var bmp = BitmapConverter.ToBitmap(mat);
            string tempPath = Path.GetTempFileName();
            bmp.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);
            var img = FaceRecognition.LoadImageFile(tempPath);
            File.Delete(tempPath);
            return img;
        }

        public static Location ToFaceRecognitionLocation(this Rect rect)
        {
            return new Location(rect.Top, rect.Right, rect.Bottom, rect.Left);
        }
    }
}
