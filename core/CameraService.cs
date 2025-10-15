using FaceRecognitionDotNet;
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
        private ScrfdDetector _scrfd;
        private bool _useScrfd = true;
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private readonly Recognizer _rec;
        private readonly object _lock = new();

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
            _rec = new Recognizer(db.Known, tolerance: 0.42);
            _cameraIndex = cameraIndex;
            try
            {
                string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "scrfd_person_2.5g.onnx");
                if (File.Exists(modelPath))
                {
                    _scrfd = new ScrfdDetector(modelPath);
                    _useScrfd = true;
                    Console.WriteLine("[SCRFD] Model loaded successfully.");
                }
                else
                {
                    Console.WriteLine("[SCRFD] Model not found, using HaarCascade fallback.");
                    _useScrfd = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SCRFD] Load error: {ex.Message}");
                _useScrfd = false;
            }
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
                    // Thêm lock để đảm bảo capture không bị dispose khi đang Read
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
            _cameraThread?.Join(200);

            // Thêm lock để tránh xung đột thread khi dispose
            lock (_lock)
            {
                _capture?.Release();
                _capture?.Dispose();
                _capture = null;
            }
        }

        public Bitmap CaptureFrame()
        {
            if (_previewBox?.Image == null)
                throw new Exception("❌ Chưa có hình ảnh từ camera!");
            return new Bitmap(_previewBox.Image);
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
                        var (name, conf) = _rec.Recognize(encoding);
                        Cv2.Rectangle(frame, rect, conf > 50 ? Scalar.LimeGreen : Scalar.Red, 2);
                        Cv2.PutText(frame, $"{name} ({conf:F1}%)",
                            new Point(rect.X, rect.Y - 10),
                            HersheyFonts.HersheySimplex, 0.6, Scalar.White, 2);
                    }
                }

                Cv2.ImShow("Camera", frame);
                if (Cv2.WaitKey(1) == 27) break; // ESC thoát
            }

            Cv2.DestroyAllWindows();
        }

        public void Dispose()
        {
            StopCamera();
        }
    }
}

