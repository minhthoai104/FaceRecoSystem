using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace FaceRecoSystem
{
    public class CameraService : IDisposable
    {
        private bool _usehaarCascade = true;
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private readonly Recognizer _rec;
        private readonly object _lock = new();
        private CascadeClassifier _haarCascade;
        private VideoCapture _capture;
        private Thread _cameraThread;
        private bool _running;
        private PictureBox _previewBox;
        private int _cameraIndex;

        public CascadeClassifier FaceDetector { get; set; }

        public CameraService(FaceRecognition fr, FaceDatabase db) : this(fr, db, 0)
        {
        }

        public CameraService(FaceRecognition fr, FaceDatabase db, int cameraIndex)
        {
            _fr = fr;
            _db = db;
            _rec = new Recognizer(db.Known, tolerance: 0.6);
            _cameraIndex = cameraIndex;
            try
            {
                string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "haarcascade_frontalface_default.xml");
                if (File.Exists(modelPath))
                {
                    _haarCascade = new CascadeClassifier(modelPath);
                    _usehaarCascade = true;
                    Console.WriteLine("[Haarcascade] Model loaded successfully.");
                }
                else
                {
                    Console.WriteLine("[Haarcascade] Model not found, using HaarCascade fallback.");
                    _usehaarCascade = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Haarcascade] Load error: {ex.Message}");
                _usehaarCascade = false;
            }
        }

        public bool Start(int camIndex = 0)
        {
            _capture = new VideoCapture(camIndex, VideoCaptureAPIs.DSHOW);
            _capture.Set(VideoCaptureProperties.FrameWidth, 1280);
            _capture.Set(VideoCaptureProperties.FrameHeight, 720);

            _running = _capture.IsOpened();
            return _running;
        }

        public Bitmap CaptureFrame()
        {
            if (!_running || _capture == null)
                return null;

            using var mat = new Mat();
            if (!_capture.Read(mat) || mat.Empty())
                return null;

            return BitmapConverter.ToBitmap(mat);
        }

        public void StartCamera(PictureBox previewBox)
        {
            if (_running) return;

            _previewBox = previewBox;
            try
            {
                _capture = new VideoCapture(_cameraIndex, VideoCaptureAPIs.DSHOW);

                if (!_capture.IsOpened())
                {
                    MessageBox.Show($"Không thể mở camera có chỉ số {_cameraIndex}. Hãy thử chọn camera khác hoặc kiểm tra lại kết nối DroidCam.", "Lỗi Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi khởi động camera: {ex.Message}", "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _running = true;
            _cameraThread = new Thread(CameraLoop)
            {
                IsBackground = true
            };
            _cameraThread.Start();
        }

        private void CameraLoop()
        {
            var frame = new Mat();
            while (_running)
            {
                try
                {
                    lock (_lock)
                    {
                        if (_capture == null || _capture.IsDisposed) break;
                        _capture.Read(frame);
                    }
                    if (frame.Empty()) continue;

                    if (FaceDetector != null)
                    {
                        using var gray = new Mat();
                        Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                        var faces = FaceDetector.DetectMultiScale(
                            gray, 1.1, 4,
                            flags: HaarDetectionTypes.ScaleImage,
                            minSize: new Size(60, 60)
                        );

                        foreach (var rect in faces)
                            Cv2.Rectangle(frame, rect, Scalar.Green, 2);
                    }

                    var bmp = BitmapConverter.ToBitmap(frame);
                    _previewBox?.Invoke(new Action(() =>
                    {
                        _previewBox.Image?.Dispose();
                        _previewBox.Image = bmp;
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Camera loop error: {ex.Message}");
                }

                Thread.Sleep(30);
            }

            frame.Dispose();
        }

        public void StopCamera()
        {
            _running = false;
            try
            {
                _cameraThread?.Join(200);
            }
            catch { /* ignore */ }

            lock (_lock)
            {
                if (_capture != null)
                {
                    try { _capture.Release(); } catch { }
                    try { _capture.Dispose(); } catch { }
                    _capture = null;
                }
            }
        }

        public void StartRecognition()
        {
            using var cap = new VideoCapture(_cameraIndex, VideoCaptureAPIs.DSHOW);
            if (!cap.IsOpened())
            {
                MessageBox.Show("Không thể mở camera!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cap.Set(VideoCaptureProperties.FrameWidth, 1280);
            cap.Set(VideoCaptureProperties.FrameHeight, 720);

            var frame = new Mat();
            Cv2.NamedWindow("Camera");

            while (true)
            {
                cap.Read(frame);
                if (frame.Empty()) continue;

                using var small = new Mat();
                Cv2.Resize(frame, small, new Size(), 0.25, 0.25);

                using var bmp = BitmapConverter.ToBitmap(small);
                string tempPath = Path.GetTempFileName();
                bmp.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

                using var frImg = FaceRecognition.LoadImageFile(tempPath);
                File.Delete(tempPath);

                var faceLocations = _fr.FaceLocations(frImg, 1, Model.Hog).ToList();
                foreach (var loc in faceLocations)
                {
                    var rect = new Rect(
                        (int)(loc.Left / 0.25),
                        (int)(loc.Top / 0.25),
                        (int)((loc.Right - loc.Left) / 0.25),
                        (int)((loc.Bottom - loc.Top) / 0.25)
                    );

                    var encoding = _fr.FaceEncodings(frImg, new[] { loc }).FirstOrDefault();
                    if (encoding != null)
                    {
                        // -> Recognize returns 4-tuple now: (name, distance, cosine, confidence)
                        var (name, distance, cosine, conf) = _rec.Recognize(encoding);

                        // Color and label logic unchanged, use conf
                        Cv2.Rectangle(frame, rect, conf > 50 ? Scalar.LimeGreen : Scalar.Red, 2);
                        Cv2.PutText(frame, $"{name} ({conf:F1}%)",
                            new Point(rect.X, rect.Y - 10),
                            HersheyFonts.HersheySimplex, 0.6, Scalar.White, 2);
                    }
                }

                Cv2.ImShow("Camera", frame);
                if (Cv2.WaitKey(1) == 27) break;
            }

            Cv2.DestroyAllWindows();
        }

        public void StartRecognitionWithAttendance()
        {
            using var cap = new VideoCapture(_cameraIndex, VideoCaptureAPIs.DSHOW);
            if (!cap.IsOpened())
            {
                MessageBox.Show("Không thể mở camera!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cap.Set(VideoCaptureProperties.FrameWidth, 1280);
            cap.Set(VideoCaptureProperties.FrameHeight, 720);

            var frame = new Mat();
            Cv2.NamedWindow("Attendance Camera");

            while (true)
            {
                cap.Read(frame);
                if (frame.Empty()) continue;

                using var small = new Mat();
                Cv2.Resize(frame, small, new Size(), 0.25, 0.25);

                using var bmp = BitmapConverter.ToBitmap(small);
                string tmp = Path.GetTempFileName();
                bmp.Save(tmp, System.Drawing.Imaging.ImageFormat.Bmp);

                using var frImg = FaceRecognition.LoadImageFile(tmp);
                File.Delete(tmp);

                var faceLocs = _fr.FaceLocations(frImg, 1, Model.Hog).ToList();
                foreach (var loc in faceLocs)
                {
                    var rect = new Rect(
                        (int)(loc.Left / 0.25),
                        (int)(loc.Top / 0.25),
                        (int)((loc.Right - loc.Left) / 0.25),
                        (int)((loc.Bottom - loc.Top) / 0.25)
                    );

                    var encoding = _fr.FaceEncodings(frImg, new[] { loc }).FirstOrDefault();
                    if (encoding == null) continue;

                    // -> Recognize returns 4-tuple now
                    var (name, distance, cosine, conf) = _rec.Recognize(encoding);

                    Scalar color = conf > 50 ? Scalar.LimeGreen : Scalar.Red;

                    Cv2.Rectangle(frame, rect, color, 2);
                    Cv2.PutText(frame, $"{name} ({conf:F1}%)",
                        new Point(rect.X, rect.Y - 10),
                        HersheyFonts.HersheySimplex, 0.6, Scalar.White, 2);

                    // Attendance logic: chỉ mark nếu không Unknown và confidence đủ cao
                    if (!string.Equals(name, "Unknown", StringComparison.OrdinalIgnoreCase) && conf >= 60)
                    {
                        // Use a clone of frame to avoid issues with Mat lifetime
                        using var frameClone = frame.Clone();
                        _db.MarkAttendance(name, frameClone);
                        // frameClone will be disposed by MarkAttendance (or when leaving using)
                    }
                }

                Cv2.ImShow("Attendance Camera", frame);
                if (Cv2.WaitKey(1) == 27) break;
            }

            Cv2.DestroyAllWindows();
        }

        public void Dispose()
        {
            StopCamera();
            if (_capture != null)
            {
                try { _capture.Release(); } catch { }
                try { _capture.Dispose(); } catch { }
            }
        }
    }
}
