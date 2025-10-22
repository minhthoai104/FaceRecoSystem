using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;
using WMPLib;

namespace FaceRecoSystem.controls
{
    public partial class AttendanceControl : UserControl
    {
        private WindowsMediaPlayer _player;
        public event EventHandler RegisterRequested;
        private int _holdTime;
        private int _cooldownAfterCheckin;
        private int _resetInfoPanelDelay;

        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private readonly Recognizer _rec;
        private readonly VideoCapture _cap;
        private readonly CascadeClassifier _faceCascade;
        private bool _running = false;
        private readonly Dictionary<string, DateTime> _cooldown = new();
        private readonly Dictionary<string, DateTime> _recognizedTime = new();
        private System.Timers.Timer _timer;
        private struct RecognitionResult
        {
            public OpenCvSharp.Rect Rectangle;
            public string Text;
            public Scalar Color;
        }

        private readonly object _resultsLock = new object();
        private List<RecognitionResult> _lastResults = new List<RecognitionResult>();
        private string _loadedMp3File = @"D:\Work\Project\FaceRecoSystem\sound\tiengdong.com_speech_1761030835368.mp3";

        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _resetTimer;

        private readonly object _frameLock = new();
        private Mat _latestFrame = new Mat();

        private const int RECOGNITION_DELAY_MS = 150;
        private const double RECOGNITION_THRESHOLD = 0.6;

        public AttendanceControl(string connString)
        {
            InitializeComponent();
            InitializeCustomComponents();
            try
            {
                LoadMp3Once();
                _fr = FaceRecognition.Create("models");
                _db = new FaceDatabase(_fr, connString);
                _rec = new Recognizer(_db.Known, 0.6);
                //string rtspUrl = "rtsp://admin:LVFQAN@192.168.4.38:554/ch1/main";
                //_cap = new VideoCapture(rtspUrl);
                _cap = new VideoCapture(0);
                _faceCascade = new CascadeClassifier(@"models\haarcascade_frontalface_default.xml");
                this.Load += AttendanceControl_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMp3Once()
        {
            try
            {
                if (File.Exists(_loadedMp3File))
                {
                    if (_player == null)
                        _player = new WindowsMediaPlayer();

                    _player.URL = _loadedMp3File;
                    _player.settings.volume = 100;
                    Console.WriteLine("[Âm thanh] File đã nạp sẵn thành công.");
                }
                else
                {
                    Console.WriteLine("[Âm thanh] Không tìm thấy file khi load sẵn.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Âm thanh] Lỗi khi load file: {ex.Message}");
            }
        }

        private void InitializeCustomComponents()
        {
            ResetToDefault();
            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
            ClockTimer_Tick(null, null);

            _resetTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _resetTimer.Tick += (s, e) =>
            {
                ResetInfoPanel();
                _resetTimer.Stop();
            };
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblClockTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblClockDate.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy", new CultureInfo("vi-VN"));
        }

        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            _holdTime = AppSettings.HoldTimeForAttendance;
            _cooldownAfterCheckin = AppSettings.CooldownAfterCheckin;
            _resetInfoPanelDelay = AppSettings.ResetInfoPanelDelay;
            _resetTimer.Interval = _resetInfoPanelDelay > 0 ? _resetInfoPanelDelay : 5000;

            if (_cap == null || !_cap.IsOpened())
            {
                MessageBox.Show("Không thể kết nối tới camera.", "Lỗi Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _running = true;
            StartCameraLoop();
            StartRecognitionLoop();
        }

        private void StartCameraLoop()
        {
            Task.Run(() =>
            {
                var frame = new Mat();
                while (_running && _cap.IsOpened())
                {
                    _cap.Read(frame);
                    if (frame.Empty()) continue;

                    lock (_frameLock) { frame.CopyTo(_latestFrame); }

                    using (var bmp = BitmapConverter.ToBitmap(frame))
                    {
                        UpdateCameraFrame((Bitmap)bmp.Clone());
                    }
                    Thread.Sleep(30);
                }
                frame.Dispose();
            });
        }

        private void StartRecognitionLoop()
        {
            Task.Run(() =>
            {
                while (_running)
                {
                    Mat frameCopy;
                    lock (_frameLock) { frameCopy = _latestFrame.Clone(); }

                    if (frameCopy != null && !frameCopy.Empty())
                    {
                        ProcessRecognition(frameCopy);
                        frameCopy.Dispose();
                    }
                    Thread.Sleep(RECOGNITION_DELAY_MS);
                }
            });
        }

        private void ProcessRecognition(Mat frame)
        {
            var currentResults = new List<RecognitionResult>();
            bool isProcessingRecognizedFace = false;

            using var smallFrame = new Mat();
            double scale = 0.5;
            Cv2.Resize(frame, smallFrame, new OpenCvSharp.Size(0, 0), scale, scale);

            try
            {
                using var gray = new Mat();
                Cv2.CvtColor(smallFrame, gray, ColorConversionCodes.BGR2GRAY);
                var faces = _faceCascade.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(40, 40));

                foreach (var smallRect in faces)
                {
                    var rect = new Rect((int)(smallRect.X / scale), (int)(smallRect.Y / scale), (int)(smallRect.Width / scale), (int)(smallRect.Height / scale));

                    string name = "Unknown";
                    double distance = double.MaxValue;
                    double confidence = 0;
                    Scalar rectColor = Scalar.Yellow;

                    float scaleFactor = 2f;
                    int newWidth = (int)(rect.Width * scaleFactor);
                    int newHeight = (int)(rect.Height * scaleFactor);
                    int newX = Math.Max(0, rect.X + rect.Width / 2 - newWidth / 2);
                    int newY = Math.Max(0, rect.Y + rect.Height / 2 - newHeight / 2);
                    if (newX + newWidth > frame.Cols) newWidth = frame.Cols - newX;
                    if (newY + newHeight > frame.Rows) newHeight = frame.Rows - newY;

                    if (newWidth > 0 && newHeight > 0)
                    {
                        var livenessRect = new OpenCvSharp.Rect(newX, newY, newWidth, newHeight);
                        using var liveness_roi = new Mat(frame, livenessRect);
                        var liveness = LivenessHelper.CheckLiveness(liveness_roi);

                        if (liveness.Status != LivenessStatus.Real)
                        {
                            UpdateStatus($"Phát hiện giả mạo ({liveness.Status})");
                            rectColor = Scalar.Red;
                            currentResults.Add(new RecognitionResult { Rectangle = rect, Text = "Fake", Color = rectColor });
                            isProcessingRecognizedFace = true;
                            continue;
                        }
                    }

                    try
                    {
                        using var faceROI = new Mat(frame, rect);
                        using var bmp = BitmapConverter.ToBitmap(faceROI);
                        using var img = FaceRecognition.LoadImage(bmp);
                        var encs = _fr.FaceEncodings(img).ToArray();

                        if (encs.Length > 0)
                        {
                            var (recognizedName, recognizedDistance, _, conf) = _rec.Recognize(encs[0]);
                            confidence = conf;
                            distance = recognizedDistance;

                            bool isValid = (confidence >= 80.0) && (distance <= 0.45);
                            if (!isValid)
                                recognizedName = "Unknown";

                            Console.WriteLine($"[Recognize] Name={recognizedName}, Dist={distance:F3}, Conf={confidence:F1}% (Valid={isValid})");

                            name = recognizedName;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Recognize] Lỗi: {ex.Message}");
                        continue;
                    }

                    string textToShow;
                    var safeName = RemoveDiacritics(name);

                    if (name != "Unknown")
                    {
                        rectColor = Scalar.Green;
                        isProcessingRecognizedFace = true;
                        ProcessAttendance(name, frame);
                        textToShow = $"{safeName} ({confidence:F0}%)";
                    }
                    else
                    {
                        textToShow = "Unknown";
                    }

                    currentResults.Add(new RecognitionResult { Rectangle = rect, Text = textToShow, Color = rectColor });
                }

                if (!faces.Any())
                {
                    UpdateStatus("Vui lòng nhìn vào camera");
                }
                else if (!isProcessingRecognizedFace)
                {
                    UpdateStatus("Có người trong camera");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProcessRecognition] Lỗi: {ex}");
            }


            lock (_resultsLock)
            {
                _lastResults = currentResults;
            }
        }

        private void ProcessAttendance(string name, Mat frame)
        {
            if (!_recognizedTime.ContainsKey(name))
                _recognizedTime[name] = DateTime.Now;

            var elapsed = DateTime.Now - _recognizedTime[name];
            if (elapsed.TotalSeconds >= _holdTime)
            {
                if (_db.TryGetInfo(name, out var user))
                {
                    Cv2.ImEncode(".jpg", frame, out byte[] snapshotBytes);
                    _ = Task.Run(async () =>
                    {
                        string result = await AttendanceHelper.ProcessCheckInOrCheckOut(user.UserID, snapshotBytes);
                        if (result != "WAIT")
                        {
                            this.Invoke(new Action(() => UpdateAttendanceUI(result, user, snapshotBytes)));
                        }
                    });
                }
                _recognizedTime.Remove(name);
            }
            else
            {
                UpdateStatus($"Giữ {_holdTime - (int)elapsed.TotalSeconds}s để chấm công: {name}");
            }
        }

        private void UpdateCameraFrame(Bitmap frame)
        {
            if (picCamera.InvokeRequired)
            {
                picCamera.Invoke(new Action<Bitmap>(UpdateCameraFrame), frame);
                return;
            }

            List<RecognitionResult> resultsToDraw;
            lock (_resultsLock)
            {
                resultsToDraw = new List<RecognitionResult>(_lastResults);
            }

            using (var gfx = Graphics.FromImage(frame))
            {
                gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                using (var font = new Font("Segoe UI", 14, FontStyle.Bold))
                {
                    foreach (var res in resultsToDraw)
                    {
                        using (var pen = new Pen(Color.FromArgb((int)res.Color.Val2, (int)res.Color.Val1, (int)res.Color.Val0), 3))
                        {
                            gfx.DrawRectangle(pen, res.Rectangle.X, res.Rectangle.Y, res.Rectangle.Width, res.Rectangle.Height);
                        }
                        var textSize = gfx.MeasureString(res.Text, font);
                        var textLocation = new System.Drawing.Point(res.Rectangle.X, res.Rectangle.Y - (int)textSize.Height);
                        gfx.FillRectangle(Brushes.Black, new RectangleF(textLocation, textSize));
                        gfx.DrawString(res.Text, font, Brushes.White, textLocation);
                    }
                }
            }
            picCamera.Image?.Dispose();
            picCamera.Image = frame;
        }

        private void UpdateStatus(string message)
        {
            if (lblInstruction.InvokeRequired)
            {
                lblInstruction.Invoke(new Action<string>(UpdateStatus), message);
                return;
            }
            lblInstruction.Text = message;
        }

        private void UpdateAttendanceUI(string result, User user, byte[] snapshotBytes)
        {
            try
            {
                using var ms = new MemoryStream(snapshotBytes);
                var bmpForUI = new Bitmap(ms);

                lblFullName.Text = $"Họ và tên: {user.FullName}";
                lblEmployeeID.Text = $"Mã NV: {user.UserID}";
                lblTimestamp.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                picAvatar.Image?.Dispose();
                picAvatar.Image = (Bitmap)bmpForUI.Clone();

                if (result == "CHECKIN" || result == "CHECKOUT")
                {
                    PlayLoadedMp3();
                    pnlStatusResult.BackColor = Color.SeaGreen;
                    lblStatusResult.Text = (result == "CHECKIN") ? "Đã Checkin" : "Đã Checkout";
                    AddRecentActivity(user.FullName, result.ToLower(), true);
                    int cooldownMinutes = user.CheckInCooldownInMinutes ?? _cooldownAfterCheckin;
                    _cooldown[user.FullName] = DateTime.Now.AddMinutes(cooldownMinutes);
                }
                else if (result?.StartsWith("WAIT") == true)
                {
                    pnlStatusResult.BackColor = Color.Orange;
                    lblStatusResult.Text = "Vui lòng chờ";
                    AddRecentActivity(user.FullName, "Waiting", false);
                }
                else
                {
                    string errorMessage = result?.StartsWith("ERROR:") == true ? result.Substring(7).Trim() : "Lỗi khi lưu dữ liệu!";
                    pnlStatusResult.BackColor = Color.Crimson;
                    lblStatusResult.Text = errorMessage;
                    AddRecentActivity(user.FullName, $"Lỗi: {errorMessage}", false);
                }
                pnlStatusResult.Visible = true;
                _resetTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UI update] Lỗi: {ex}");
            }
        }

        private void ResetToDefault()
        {
            UpdateStatus("Vui lòng nhìn thẳng vào camera");
            ResetInfoPanel();
        }

        private void ResetInfoPanel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ResetInfoPanel));
                return;
            }
            lblFullName.Text = "Họ và tên: ";
            lblEmployeeID.Text = "Mã NV: ";
            lblTimestamp.Text = "Thời gian: ";
            picAvatar.Image?.Dispose();
            picAvatar.Image = null;
            pnlStatusResult.Visible = false;
        }

        private void AddRecentActivity(string name, string status, bool isSuccess)
        {
            if (lsvRecentActivity.InvokeRequired)
            {
                lsvRecentActivity.Invoke(new Action<string, string, bool>(AddRecentActivity), name, status, isSuccess);
                return;
            }
            string log = $"[{DateTime.Now:HH:mm:ss}] {name} - {status}";
            var item = new ListViewItem(log) { ForeColor = isSuccess ? Color.DarkGreen : Color.DarkRed };
            lsvRecentActivity.Items.Insert(0, item);
            if (lsvRecentActivity.Items.Count > 10) lsvRecentActivity.Items.RemoveAt(10);
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            return new string(chars.ToArray()).Normalize(System.Text.NormalizationForm.FormC);
        }
        #region Sound Player
        private void PlayLoadedMp3()
        {
            try
            {
                if (_player == null)
                {
                    Console.WriteLine("[Âm thanh] Chưa nạp file MP3.");
                    return;
                }

                _player.controls.currentPosition = 0; _player.controls.play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Âm thanh] Lỗi phát MP3: {ex.Message}");
            }
        }

        #endregion

        public void StopCamera()
        {
            try
            {
                _running = false;
                Task.Delay(300).Wait();

                _cap?.Release();
                _cap?.Dispose();

                lock (_frameLock)
                {
                    _latestFrame?.Dispose();
                    _latestFrame = null;
                }

                Console.WriteLine("[AttendanceControl] Camera stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AttendanceControl] StopCamera error: {ex.Message}");
            }
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterRequested?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _running = false;
                Task.Delay(500).Wait();
                lock (_frameLock)
                {
                    _latestFrame?.Dispose();
                    _latestFrame = null;
                }
                _clockTimer?.Dispose();
                _resetTimer?.Dispose();
                _cap?.Release();
                _cap?.Dispose();
                _faceCascade?.Dispose();
                _db?.Dispose();
                _fr?.Dispose();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}