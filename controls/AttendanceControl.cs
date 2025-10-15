using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace FaceRecoSystem.controls
{
    public partial class AttendanceControl : UserControl
    {
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private readonly Recognizer _rec;
        private readonly AntiSpoofDetector _antiSpoof;

        private VideoCapture _cap;
        private Thread _thread;
        private bool _running;
        private Mat _frame = new();

        private string _curName = "";
        private DateTime _firstSeen = DateTime.MinValue;
        private DateTime _lastCheck = DateTime.MinValue;

        private readonly CascadeClassifier _faceCascade;

        // smoothing / anti-flicker
        private Rect[] _lastFaces = Array.Empty<Rect>();
        private DateTime _lastFaceTime = DateTime.MinValue;

        public AttendanceControl(FaceRecognition fr, FaceDatabase db)
        {
            InitializeComponent();
            _fr = fr;
            _db = db;
            _rec = new Recognizer(db.Known);
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
            _antiSpoof = new AntiSpoofDetector(Path.Combine(modelPath, "anti-spoof-mn3.onnx"));
            _faceCascade = new CascadeClassifier(Path.Combine(modelPath, "haarcascade_frontalface_default.xml"));
        }

        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void StartCamera()
        {
            _cap = new VideoCapture(0);
            if (!_cap.IsOpened())
            {
                MessageBox.Show("Không thể mở camera!");
                return;
            }

            _cap.FrameWidth = 640;
            _cap.FrameHeight = 480;
            _running = true;

            _thread = new Thread(() =>
            {
                int frameCount = 0;
                while (_running)
                {
                    var temp = new Mat();
                    if (!_cap.Read(temp) || temp.Empty())
                    {
                        temp.Dispose();
                        Thread.Sleep(30);
                        continue;
                    }

                    frameCount++;
                    // process every 3 frames to reduce load
                    if (frameCount % 3 == 0)
                        ProcessFrame(temp);

                    ShowFrame(temp);
                    temp.Dispose();
                    Thread.Sleep(8);
                }
            })
            { IsBackground = true };

            _thread.Start();
        }

        private void ProcessFrame(Mat frame)
        {
            try
            {
                // downscale for detection (faster)
                using var small = new Mat();
                Cv2.Resize(frame, small, new Size(frame.Width / 2, frame.Height / 2));
                using var gray = new Mat();
                Cv2.CvtColor(small, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(gray, gray);

                var facesSmall = _faceCascade.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new Size(60, 60));

                Rect[] faces;
                if (facesSmall.Length == 0)
                {
                    // keep old boxes for short time to avoid flicker
                    if ((DateTime.Now - _lastFaceTime).TotalSeconds < 1 && _lastFaces.Length > 0)
                        faces = _lastFaces;
                    else
                    {
                        _lastFaces = Array.Empty<Rect>();
                        _lastFaceTime = DateTime.MinValue;
                        UpdateStatus("Không thấy khuôn mặt");
                        return;
                    }
                }
                else
                {
                    // scale boxes to original frame
                    faces = facesSmall.Select(f => new Rect(f.X * 2, f.Y * 2, f.Width * 2, f.Height * 2)).ToArray();
                    _lastFaces = faces;
                    _lastFaceTime = DateTime.Now;
                }

                foreach (var face in faces)
                {
                    if (face.Width <= 0 || face.Height <= 0) continue;
                    var safe = ClampRect(face, frame.Width, frame.Height);
                    if (safe.Width <= 0 || safe.Height <= 0) continue;

                    using var faceMat = new Mat(frame, safe);
                    Cv2.Resize(faceMat, faceMat, new Size(150, 150));

                    bool isLive = true;
                    try { isLive = _antiSpoof.IsLive(faceMat); } catch { isLive = true; }

                    using var bmp = BitmapConverter.ToBitmap(faceMat);
                    using var img = FaceRecognition.LoadImage(bmp);
                    var enc = _fr.FaceEncodings(img).FirstOrDefault();
                    if (enc == null) continue;

                    var best = _db.FindClosestMatch(enc);
                    enc.Dispose();

                    string name = "Unknown";
                    double conf = 0;
                    if (best.HasValue)
                    {
                        name = best.Value.name;
                        conf = best.Value.confidence;
                    }

                    Scalar color = isLive ? Scalar.LimeGreen : Scalar.Red;
                    string label = $"{name} ({conf:F1}%)";
                    Cv2.Rectangle(frame, safe, color, 2);
                    Cv2.PutText(frame, label, new Point(safe.X, safe.Y - 10),
                        HersheyFonts.HersheySimplex, 0.6, color, 2);

                    if (!isLive || name == "Unknown" || conf < 60)
                    {
                        UpdateStatus("Giả mạo hoặc không khớp khuôn mặt!");
                        _curName = "";
                        continue;
                    }

                    // stable 3 seconds
                    if (_curName != name)
                    {
                        _curName = name;
                        _firstSeen = DateTime.Now;
                    }

                    double stableSec = (DateTime.Now - _firstSeen).TotalSeconds;
                    if (stableSec < 3)
                    {
                        UpdateStatus($"Giữ khuôn mặt ổn định {3 - stableSec:F1}s...");
                        continue;
                    }

                    // check-in every 10s max
                    if ((DateTime.Now - _lastCheck).TotalSeconds > 10)
                    {
                        SaveAttendanceByName(name, bmp);
                        UpdateStatus($"✅ Đã chấm công: {name}");
                        _lastCheck = DateTime.Now;
                    }

                    if (_db.TryGetInfo(name, out var info))
                    {
                        List<string> lines = new()
                        {
                            $"Full Name: {info.FullName}",
                            $"Age: {info.Age}",
                            $"Gender: {info.Gender}",
                            $"Address: {info.Address}",
                            $"Conf: {conf:F1}%"
                        };
                        DrawInfoBox(frame, safe, lines, color);
                    }
                    else
                    {
                        List<string> lines = new()
                        {
                            $"Full Name: {name}",
                            $"Age: Unknown",
                            $"Gender: Unknown",
                            $"Address: Unknown",
                            $"Conf: {conf:F1}%"
                        };
                        DrawInfoBox(frame, safe, lines, color);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProcessFrame] Lỗi: {ex.Message}");
            }
        }

        private Rect ClampRect(Rect rect, int frameWidth, int frameHeight)
        {
            int x = Math.Max(0, rect.X);
            int y = Math.Max(0, rect.Y);
            int w = Math.Min(rect.Width, frameWidth - x);
            int h = Math.Min(rect.Height, frameHeight - y);
            return new Rect(x, y, w, h);
        }

        private void DrawInfoBox(Mat frame, Rect face, List<string> lines, Scalar color)
        {
            int padding = 6;
            int lineH = 18;
            int width = lines.Any() ? lines.Max(t => (int)(t.Length * 8.5)) + padding * 2 : 120;
            int height = lineH * lines.Count + padding * 2;
            int y = face.Top - height - 10;
            if (y < 0) y = face.Bottom + 10;

            var rect = new Rect(face.Left, y, width, height);
            Cv2.Rectangle(frame, rect, color, 1);
            int yPos = rect.Y + 18;
            foreach (var t in lines)
            {
                Cv2.PutText(frame, t, new Point(rect.X + 8, yPos),
                    HersheyFonts.HersheySimplex, 0.45, Scalar.White, 1);
                yPos += lineH;
            }
        }

        private void ShowFrame(Mat frame)
        {
            if (picCamera.InvokeRequired)
            {
                picCamera.Invoke(new Action(() =>
                {
                    picCamera.Image?.Dispose();
                    picCamera.Image = BitmapConverter.ToBitmap(frame);
                }));
            }
            else
            {
                picCamera.Image?.Dispose();
                picCamera.Image = BitmapConverter.ToBitmap(frame);
            }
        }

        private void UpdateStatus(string msg)
        {
            if (lblStatus.InvokeRequired)
                lblStatus.Invoke(new Action(() => lblStatus.Text = msg));
            else
                lblStatus.Text = msg;
        }

        private void SaveAttendanceByName(string name, Bitmap bmp)
        {
            try
            {
                var user = _db.GetPersonByName(name);
                if (user == null) return;

                using var conn = new SqlConnection(DatabaseHelper.ConnectionString);
                conn.Open();

                string userId = user.UserID;
                using var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();

                string check = "SELECT COUNT(*) FROM Attendance WHERE UserID=@U AND CAST(CheckInTime AS DATE)=CAST(GETDATE() AS DATE)";
                int exists = (int)new SqlCommand(check, conn)
                {
                    Parameters = { new SqlParameter("@U", userId) }
                }.ExecuteScalar();

                if (exists == 0)
                {
                    string insert = "INSERT INTO Attendance (UserID, CheckInTime, CheckInImage, CreatedAt, UpdatedAt) VALUES (@U,@T,@Img,@N,@N)";
                    using var cmd = new SqlCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@U", userId);
                    cmd.Parameters.AddWithValue("@T", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Img", bytes);
                    cmd.Parameters.AddWithValue("@N", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string update = "UPDATE Attendance SET CheckOutTime=@T,CheckOutImage=@Img,UpdatedAt=@N WHERE UserID=@U AND CAST(CheckInTime AS DATE)=CAST(GETDATE() AS DATE)";
                    using var cmd = new SqlCommand(update, conn);
                    cmd.Parameters.AddWithValue("@U", userId);
                    cmd.Parameters.AddWithValue("@T", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Img", bytes);
                    cmd.Parameters.AddWithValue("@N", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Attendance] Lỗi lưu: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _running = false;
                _thread?.Join(300);
                _cap?.Release();
                _cap?.Dispose();
                _antiSpoof?.Dispose();
                _faceCascade?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
