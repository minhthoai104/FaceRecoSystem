using FaceRecognitionDotNet;
//using FaceRecoSystem.lib;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace FaceLibrary
{
    //================================================================================
    // CẤU TRÚC DỮ LIỆU TRẢ VỀ CHO GIAO DIỆN
    //================================================================================
    #region ProcessResult
    public class ProcessResult
    {
        public int Id { get; set; }
        public Rect BoundingBox { get; set; }
        public string Label { get; set; }
        public Scalar Color { get; set; }
        public Mat FaceCrop { get; set; } // Ảnh cắt ra để hiển thị preview
    }
    #endregion

    //================================================================================
    // DỊCH VỤ XỬ LÝ TRUNG TÂM
    //================================================================================
    #region FaceProcessingService
    public class FaceProcessingService : IDisposable
    {
        // --- Các bộ phát hiện ---
        private readonly HaarCascadeDetector _faceDetector;
        private readonly LivenessDetector _livenessDetector;
        private readonly FaceRecognitionService _faceRecService;
        private readonly YoloSpoofDetector _spoofObjectDetector;

        // --- Dữ liệu tracking ---
        private readonly ConcurrentDictionary<int, TrackedPerson> _trackedPeople = new ConcurrentDictionary<int, TrackedPerson>();
        private const int MaxFramesUnseen = 10;
        private const double SmoothingAlpha = 0.15;

        // --- Dữ liệu cho luồng YOLO ---
        private readonly object _yoloLock = new object();
        private Mat _yoloInputFrame = new Mat();
        private List<Rect> _yoloSpoofRects = new List<Rect>();
        private Task _yoloTask;
        private CancellationTokenSource _cts;

        public FaceProcessingService(string modelsDir, string knownFacesPath)
        {
            // --- Khởi tạo tất cả các model ---
            string haarCascadePath = Path.Combine("D:\\Work\\Project\\FaceRecoSystem\\models\\haarcascade_frontalface_default.xml");
            string livenessModelPath = Path.Combine("D:\\Work\\Project\\FaceRecoSystem\\models\\anti-spoof-mn3.onnx");
            string dlibRecModel = Path.Combine("D:\\Work\\Project\\FaceRecoSystem\\models\\dlib_face_recognition_resnet_model_v1.dat");
            string dlibShapeModel = Path.Combine("D:\\Work\\Project\\FaceRecoSystem\\models\\shape_predictor_68_face_landmarks.dat");
            string yoloModelPath = Path.Combine("D:\\Work\\Project\\FaceRecoSystem\\models\\yolov5s.onnx");

            // Kiểm tra file
            if (!File.Exists(haarCascadePath) || !File.Exists(livenessModelPath) || !File.Exists(dlibRecModel) || !File.Exists(dlibShapeModel) || !File.Exists(yoloModelPath))
            {
                throw new FileNotFoundException("Một hoặc nhiều file model không được tìm thấy. Vui lòng kiểm tra lại thư mục 'models'.");
            }

            _faceDetector = new HaarCascadeDetector(haarCascadePath);
            _livenessDetector = new LivenessDetector(livenessModelPath);
            _faceRecService = new FaceRecognitionService(modelsDir, knownFacesPath);
            _spoofObjectDetector = new YoloSpoofDetector(yoloModelPath);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _yoloTask = Task.Run(() => YoloProcessingLoop(_spoofObjectDetector, _cts.Token));
        }

        public List<ProcessResult> ProcessFrame(Mat frame)
        {
            var results = new List<ProcessResult>();

            // Cung cấp frame cho luồng YOLO
            lock (_yoloLock)
            {
                _yoloInputFrame?.Dispose();
                _yoloInputFrame = frame.Clone();
            }

            // Lấy kết quả mới nhất từ luồng YOLO
            List<Rect> currentSpoofRects;
            lock (_yoloLock)
            {
                currentSpoofRects = new List<Rect>(_yoloSpoofRects);
            }

            // Vẽ các khung spoof (nếu có)
            foreach (var rect in currentSpoofRects)
            {
                Cv2.Rectangle(frame, rect, Scalar.Magenta, 2);
            }

            // Thực hiện pipeline
            Rect[] currentDetections = _faceDetector.Detect(frame);

            //foreach (var face in currentDetections)
            //{
            //    float scaleFactor = 2.7f;
            //    var centerX = face.X + face.Width / 2;
            //    var centerY = face.Y + face.Height / 2;
            //    var newWidth = (int)(face.Width * scaleFactor);
            //    var newHeight = (int)(face.Height * scaleFactor);
            //    var newX = Math.Max(0, centerX - newWidth / 2);
            //    var newY = Math.Max(0, centerY - newHeight / 2);
            //    if (newX + newWidth > face.Width) newWidth = face.Width - newX;
            //    if (newY + newHeight > face.Height) newHeight = face.Height - newY;
            //    var livenessRect = new Rect(newX, newY, newWidth, newHeight);
            //    var liveness_roi = new Mat(frame, livenessRect);
            //    var liveness = LivenessHelper.CheckLiveness(liveness_roi);

            //    Console.WriteLine(liveness.Status);
            //}



            UpdateTrackers(currentDetections);

            foreach (var person in _trackedPeople.Values)
            {
                if (person.FramesUnseen > 0) continue;

                person.FramesSinceFirstSeen++;

                string label;
                Scalar color;

                // Pipeline xử lý cho từng người (giữ nguyên logic cũ)
                if (person.IsVerifiedReal)
                {
                    if (!person.RecognitionAttempted)
                    {
                        person.RecognitionAttempted = true;
                        person.RecognizedName = "Recognizing...";
                        var roiRect = ClampRect(person.SmoothedBox, frame.Width, frame.Height);
                        if (roiRect.Width > 0)
                        {
                            var faceRoiClone = new Mat(frame, roiRect).Clone();
                            Task.Run(() =>
                            {
                                try
                                {
                                    string name = _faceRecService.Recognize(faceRoiClone);
                                    if (_trackedPeople.TryGetValue(person.Id, out var p)) p.RecognizedName = name;
                                }
                                finally { faceRoiClone.Dispose(); }
                            });
                        }
                    }
                    label = person.RecognizedName ?? "Unknown";
                    color = Scalar.LimeGreen;
                    if (label == "Unknown")
                    {
                        person.UnknownCooldownCounter++;
                        if (person.UnknownCooldownCounter > 10) person.ResetRecognitionState();
                    }
                }
                else if (person.IsVerifiedFake)
                {
                    label = "Fake";
                    color = Scalar.Red;
                    if (!person.IsLockedByYolo)
                    {
                        person.FakeCooldownCounter++;
                        if (person.FakeCooldownCounter > 10) person.ResetVerificationState();
                    }
                }
                else
                {
                    const int yoloGraceFrames = 5;
                    if (person.FramesSinceFirstSeen <= yoloGraceFrames)
                    {
                        if (currentSpoofRects.Any(spoofRect => spoofRect.Contains(person.Center)))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"[YOLO ALERT] Khuôn mặt ID {person.Id} bị phát hiện. Ép buộc FAKE.");
                            Console.ResetColor();
                            person.ForceFake();
                        }
                    }

                    if (!person.IsVerifiedFake && person.FramesSinceFirstSeen > yoloGraceFrames)
                    {
                        var expandedRect = ExpandBox(person.SmoothedBox, frame.Width, frame.Height, 0.3);
                        var faceRoiLive = new Mat(frame, expandedRect);
                        if (faceRoiLive.Width > 0 && faceRoiLive.Height > 0)
                        {
                            var (realScore, spoofScore) = _livenessDetector.Analyze(faceRoiLive);
                            const float realFrameThreshold = 0.730f;
                            bool isRealThisFrame = realScore < realFrameThreshold;
                            person.UpdateLiveness(isRealThisFrame);
                        }
                    }

                    if (person.IsVerifiedReal) { label = "Real"; color = Scalar.LimeGreen; }
                    else if (person.IsVerifiedFake) { label = "Fake"; color = Scalar.Red; }
                    else { label = "Dang xac thuc..."; color = Scalar.Yellow; }
                }

                var faceCrop = new Mat(frame, ClampRect(person.SmoothedBox, frame.Width, frame.Height));
                results.Add(new ProcessResult
                {
                    Id = person.Id,
                    BoundingBox = person.SmoothedBox,
                    Label = label,
                    Color = color,
                    FaceCrop = faceCrop.Clone()
                });
                faceCrop.Dispose();
            }

            return results;
        }

        public void AddNewFace(string name, Mat frame, Rect faceBox)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("[CANCELLED] Tên không hợp lệ. Đã hủy lưu.");
                return;
            }

            var faceToSave = new Mat(frame, ClampRect(faceBox, frame.Width, frame.Height));
            if (faceToSave.Width > 0 && faceToSave.Height > 0)
            {
                _faceRecService.AddNewFace(name, faceToSave);
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            try { _yoloTask?.Wait(); } catch (Exception) { /* Ignored */ }
            _yoloInputFrame?.Dispose();
            _cts?.Dispose();

            _faceDetector?.Dispose();
            _livenessDetector?.Dispose();
            _faceRecService?.Dispose();
            _spoofObjectDetector?.Dispose();
        }

        // --- Các phương thức xử lý nội bộ ---

        private void YoloProcessingLoop(YoloSpoofDetector detector, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Mat frameToProcess = null;
                lock (_yoloLock)
                {
                    if (!_yoloInputFrame.Empty())
                    {
                        frameToProcess = _yoloInputFrame.Clone();
                    }
                }

                if (frameToProcess != null)
                {
                    var detectedRects = detector.Detect(frameToProcess);
                    lock (_yoloLock)
                    {
                        _yoloSpoofRects = detectedRects;
                    }
                    frameToProcess.Dispose();
                }

                try
                {
                    Task.Delay(100, token).Wait(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            Console.WriteLine("Luồng xử lý YOLO đã dừng.");
        }

        private void UpdateTrackers(Rect[] currentDetections)
        {
            foreach (var person in _trackedPeople.Values)
            {
                person.FramesUnseen++;
            }

            var usedDetectionIndices = new HashSet<int>();

            foreach (var person in _trackedPeople.Values)
            {
                double minDistance = double.MaxValue;
                int bestMatchIndex = -1;

                for (int i = 0; i < currentDetections.Length; i++)
                {
                    if (usedDetectionIndices.Contains(i)) continue;
                    var detectionCenter = new Point(currentDetections[i].X + currentDetections[i].Width / 2, currentDetections[i].Y + currentDetections[i].Height / 2);
                    double dist = Point.Distance(person.Center, detectionCenter);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        bestMatchIndex = i;
                    }
                }

                const double distanceThreshold = 75.0;
                if (bestMatchIndex != -1 && minDistance < distanceThreshold)
                {
                    if (person.FramesUnseen > 1)
                    {
                        person.ResetVerificationState();
                    }
                    person.SmoothedBox = SmoothRect(person.SmoothedBox, currentDetections[bestMatchIndex], SmoothingAlpha);
                    person.FramesUnseen = 0;
                    usedDetectionIndices.Add(bestMatchIndex);
                }
            }

            var idsToRemove = _trackedPeople.Where(kv => kv.Value.FramesUnseen > MaxFramesUnseen).Select(kv => kv.Key).ToList();
            foreach (var id in idsToRemove)
            {
                _trackedPeople.TryRemove(id, out _);
            }

            for (int i = 0; i < currentDetections.Length; i++)
            {
                if (!usedDetectionIndices.Contains(i))
                {
                    var newPerson = new TrackedPerson(currentDetections[i]);
                    _trackedPeople.TryAdd(newPerson.Id, newPerson);
                }
            }
        }

        private static Rect SmoothRect(Rect previous, Rect current, double alpha)
        {
            int newX = (int)(alpha * current.X + (1 - alpha) * previous.X);
            int newY = (int)(alpha * current.Y + (1 - alpha) * previous.Y);
            int newW = (int)(alpha * current.Width + (1 - alpha) * previous.Width);
            int newH = (int)(alpha * current.Height + (1 - alpha) * previous.Height);
            return new Rect(newX, newY, newW, newH);
        }
        private static Rect ExpandBox(Rect box, int frameW, int frameH, double factor)
        {
            int extraW = (int)(box.Width * factor);
            int extraH = (int)(box.Height * factor);
            int newX = Math.Max(0, box.X - extraW / 2);
            int newY = Math.Max(0, box.Y - extraH / 2);
            int newW = Math.Min(frameW - newX, box.Width + extraW);
            int newH = Math.Min(frameH - newY, box.Height + extraH);
            return new Rect(newX, newY, newW, newH);
        }
        private static Rect ClampRect(Rect rect, int frameWidth, int frameHeight)
        {
            int x = Math.Max(0, rect.X);
            int y = Math.Max(0, rect.Y);
            int w = Math.Min(rect.Width, frameWidth - x);
            int h = Math.Min(rect.Height, frameHeight - y);
            return new Rect(x, y, Math.Max(0, w), Math.Max(0, h));
        }
    }
    #endregion

    //================================================================================
    // CÁC LỚP HELPER (Nội bộ của thư viện)
    //================================================================================
    #region HaarCascadeDetector
    internal class HaarCascadeDetector : IDisposable
    {
        /* ... implementation from Program.cs ... */
        private readonly CascadeClassifier _faceDetector;
        public HaarCascadeDetector(string modelPath) { _faceDetector = new CascadeClassifier(modelPath); }
        public Rect[] Detect(Mat image)
        {
            var gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            return _faceDetector.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new Size(60, 60)); // 60 x 60
        }
        public void Dispose() => _faceDetector?.Dispose();
    }
    #endregion

    #region LivenessDetector
    internal class LivenessDetector : IDisposable
    {
        /* ... implementation from Program.cs ... */
        private readonly InferenceSession _session;
        private readonly Size _inputSize = new Size(128, 128);
        private readonly string _inputName;
        public LivenessDetector(string modelPath)
        {
            _session = new InferenceSession(modelPath, new SessionOptions());
            _inputName = _session.InputMetadata.Keys.First();
            Console.WriteLine("INFO: ONNX Runtime (Liveness) được cấu hình để chạy trên CPU.");
        }
        public (float realScore, float spoofScore) Analyze(Mat faceRoi)
        {
            if (faceRoi.Empty()) return (0f, 1f);
            var resized = new Mat();
            Cv2.Resize(faceRoi, resized, _inputSize);
            var inputTensor = new DenseTensor<float>(new[] { 1, 3, _inputSize.Height, _inputSize.Width });
            for (int y = 0; y < _inputSize.Height; y++)
            {
                for (int x = 0; x < _inputSize.Width; x++)
                {
                    var pixel = resized.Get<Vec3b>(y, x);
                    inputTensor[0, 0, y, x] = (pixel.Item0 - 127.5f) / 128.0f;
                    inputTensor[0, 1, y, x] = (pixel.Item1 - 127.5f) / 128.0f;
                    inputTensor[0, 2, y, x] = (pixel.Item2 - 127.5f) / 128.0f;
                }
            }
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_inputName, inputTensor) };
            var results = _session.Run(inputs);
            var output = results.First().AsTensor<float>().ToArray();
            var probabilities = Softmax(output);
            return (probabilities[1], probabilities[0]);
        }
        private float[] Softmax(float[] logits)
        {
            var maxLogit = logits.Max();
            var exps = logits.Select(x => (float)Math.Exp(x - maxLogit)).ToArray();
            var sumExps = exps.Sum();
            return exps.Select(x => x / sumExps).ToArray();
        }
        public void Dispose() => _session?.Dispose();
    }
    #endregion

    #region FaceRecognitionService
    internal class FaceRecognitionService : IDisposable
    {
        /* ... implementation from Program.cs ... */
        private readonly FaceRecognition _faceRecognition;
        private readonly string _knownFacesDir;
        private readonly Dictionary<string, List<FaceEncoding>> _knownEncodings = new Dictionary<string, List<FaceEncoding>>();

        public FaceRecognitionService(string modelsDir, string knownFacesDir)
        {
            _faceRecognition = FaceRecognition.Create(modelsDir);
            _knownFacesDir = knownFacesDir;
            Directory.CreateDirectory(_knownFacesDir);
            LoadKnownFaces();
        }

        private void LoadKnownFaces()
        {
            Console.WriteLine("[INFO] Đang tải cơ sở dữ liệu khuôn mặt từ thư mục...");
            var peopleDirs = Directory.GetDirectories(_knownFacesDir);
            foreach (var dir in peopleDirs)
            {
                var personName = new DirectoryInfo(dir).Name;
                var imageFiles = Directory.GetFiles(dir, "*.jpg").Concat(Directory.GetFiles(dir, "*.png")).ToArray();
                if (!imageFiles.Any()) continue;

                _knownEncodings[personName] = new List<FaceEncoding>();
                foreach (var imagePath in imageFiles)
                {
                    try
                    {
                        var img = FaceRecognition.LoadImageFile(imagePath);
                        var encoding = _faceRecognition.FaceEncodings(img).FirstOrDefault();
                        if (encoding != null)
                        {
                            _knownEncodings[personName].Add(encoding);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[WARNING] Không thể tải ảnh: {imagePath}. Lỗi: {e.Message}");
                    }
                }
                if (_knownEncodings[personName].Any())
                    Console.WriteLine($"  - Đã tải {_knownEncodings[personName].Count} mẫu cho '{personName}'.");
            }
            Console.WriteLine($"[INFO] Tải xong! Có {_knownEncodings.Count(kv => kv.Value.Any())} người trong CSDL.");
        }

        public string Recognize(Mat faceRoi)
        {
            byte[] imageBytes = faceRoi.ImEncode(".jpg");
            if (imageBytes == null || imageBytes.Length == 0) return "Unknown";

            var ms = new MemoryStream(imageBytes);
            var tempBmp = new Bitmap(ms);
            var image = FaceRecognition.LoadImage(tempBmp);

            var unknownEncoding = _faceRecognition.FaceEncodings(image).FirstOrDefault();

            if (unknownEncoding == null || !_knownEncodings.Any())
            {
                unknownEncoding?.Dispose();
                return "Unknown";
            }

            string bestMatchName = "Unknown";
            double bestDistance = 0.4;

            foreach (var person in _knownEncodings)
            {
                if (!person.Value.Any()) continue;
                var distances = FaceRecognition.FaceDistances(person.Value, unknownEncoding);
                var minDistance = distances.Min();
                if (minDistance < bestDistance)
                {
                    bestDistance = minDistance;
                    bestMatchName = person.Key;
                }
            }

            unknownEncoding.Dispose();
            return bestMatchName;
        }

        public void AddNewFace(string name, Mat faceRoi)
        {
            var personDir = Path.Combine(_knownFacesDir, name);
            Directory.CreateDirectory(personDir);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            var imagePath = Path.Combine(personDir, $"{timestamp}.jpg");
            faceRoi.SaveImage(imagePath);
            Console.WriteLine($"[INFO] Đã lưu ảnh mới vào: {imagePath}");

            var img = FaceRecognition.LoadImageFile(imagePath);
            var encoding = _faceRecognition.FaceEncodings(img).FirstOrDefault();
            if (encoding != null)
            {
                if (_knownEncodings.ContainsKey(name))
                {
                    _knownEncodings[name].Add(encoding);
                }
                else
                {
                    _knownEncodings[name] = new List<FaceEncoding> { encoding };
                }
                Console.WriteLine($"[SUCCESS] Đã học khuôn mặt mới cho '{name}'.");
            }
        }

        public void Dispose()
        {
            _faceRecognition?.Dispose();
            foreach (var encodings in _knownEncodings.Values)
            {
                foreach (var encoding in encodings)
                {
                    encoding.Dispose();
                }
            }
        }
    }
    #endregion

    #region YoloSpoofDetector
    internal class YoloSpoofDetector : IDisposable
    {
        /* ... implementation from Program.cs ... */
        private readonly InferenceSession _session;
        private readonly string _inputName;
        private readonly Size _inputSize = new Size(640, 640);
        private readonly int _cellPhoneClassId = 67;

        public YoloSpoofDetector(string modelPath)
        {
            _session = new InferenceSession(modelPath, new SessionOptions());
            _inputName = _session.InputMetadata.Keys.First();
            Console.WriteLine("INFO: ONNX Runtime (YOLOv5) được cấu hình để chạy trên CPU.");
        }

        public List<Rect> Detect(Mat frame)
        {
            var inputTensor = Preprocess(frame, out float scale);
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_inputName, inputTensor) };
            var results = _session.Run(inputs);
            var output = results.First().AsTensor<float>();
            return Postprocess(output, scale);
        }

        private DenseTensor<float> Preprocess(Mat frame, out float scale)
        {
            int inputWidth = _inputSize.Width;
            int inputHeight = _inputSize.Height;
            scale = Math.Min((float)inputWidth / frame.Width, (float)inputHeight / frame.Height);
            int newWidth = (int)(frame.Width * scale);
            int newHeight = (int)(frame.Height * scale);

            var resized = new Mat();
            Cv2.Resize(frame, resized, new Size(newWidth, newHeight));

            var padded = new Mat(inputHeight, inputWidth, MatType.CV_8UC3, new Scalar(114, 114, 114));
            var roi = new Rect(0, 0, newWidth, newHeight);
            resized.CopyTo(new Mat(padded, roi));

            var tensor = new DenseTensor<float>(new[] { 1, 3, inputHeight, inputWidth });
            for (int y = 0; y < inputHeight; y++)
            {
                for (int x = 0; x < inputWidth; x++)
                {
                    var pixel = padded.Get<Vec3b>(y, x);
                    tensor[0, 0, y, x] = pixel.Item2 / 255f;
                    tensor[0, 1, y, x] = pixel.Item1 / 255f;
                    tensor[0, 2, y, x] = pixel.Item0 / 255f;
                }
            }
            return tensor;
        }

        private List<Rect> Postprocess(Tensor<float> output, float scale)
        {
            var boxes = new List<Rect>();
            var confidences = new List<float>();
            float confThreshold = 0.3f;
            float nmsThreshold = 0.4f;

            for (int i = 0; i < output.Dimensions[1]; i++)
            {
                float objectness = output[0, i, 4];
                if (objectness < confThreshold) continue;

                var classScores = new List<float>();
                for (int j = 5; j < output.Dimensions[2]; j++)
                {
                    classScores.Add(output[0, i, j]);
                }

                int maxClassIndex = classScores.IndexOf(classScores.Max());
                float maxClassScore = classScores[maxClassIndex];
                float confidence = objectness * maxClassScore;

                if (confidence > confThreshold && maxClassIndex == _cellPhoneClassId)
                {
                    float cx = output[0, i, 0] / scale;
                    float cy = output[0, i, 1] / scale;
                    float w = output[0, i, 2] / scale;
                    float h = output[0, i, 3] / scale;
                    int x = (int)(cx - w / 2);
                    int y = (int)(cy - h / 2);
                    boxes.Add(new Rect(x, y, (int)w, (int)h));
                    confidences.Add(confidence);
                }
            }

            CvDnn.NMSBoxes(boxes, confidences, confThreshold, nmsThreshold, out int[] indices);
            return indices.Select(i => boxes[i]).ToList();
        }

        public void Dispose() => _session?.Dispose();
    }
    #endregion

    #region TrackedPerson
    internal class TrackedPerson
    {
        /* ... implementation from Program.cs ... */
        private static int _nextId = 0;
        public int Id { get; }
        public Rect SmoothedBox { get; set; }
        public int FramesUnseen { get; set; } = 0;
        public bool IsVerifiedReal { get; set; } = false;
        public bool IsVerifiedFake { get; private set; } = false;

        // CẬP NHẬT: Giảm kích thước bộ đệm để phản ứng nhanh hơn
        private const int LivenessBufferSize = 3;
        private readonly Queue<bool> _livenessHistory = new Queue<bool>();
        public int FakeCooldownCounter { get; set; } = 0;
        public string RecognizedName { get; set; } = null;
        public bool RecognitionAttempted { get; set; } = false;
        public int UnknownCooldownCounter { get; set; } = 0;
        public bool IsLockedByYolo { get; private set; } = false;

        public int FramesSinceFirstSeen { get; set; } = 0;

        public TrackedPerson(Rect initialBox)
        {
            Id = _nextId++;
            SmoothedBox = initialBox;
        }

        public Point Center => new Point(SmoothedBox.X + SmoothedBox.Width / 2, SmoothedBox.Y + SmoothedBox.Height / 2);

        public void UpdateLiveness(bool isRealThisFrame)
        {
            if (IsVerifiedReal || IsVerifiedFake) return;
            _livenessHistory.Enqueue(isRealThisFrame);
            if (_livenessHistory.Count > LivenessBufferSize) _livenessHistory.Dequeue();

            // Logic to lock Real still needs 3 consecutive frames
            const int consecutiveRealFramesNeeded = 3;
            if (_livenessHistory.Count >= consecutiveRealFramesNeeded)
            {
                // **FIXED**: Instead of TakeLast, just check if all items in the full queue are true.
                // This is logically equivalent since the queue size is capped at 3.
                if (_livenessHistory.All(isReal => isReal))
                {
                    IsVerifiedReal = true;
                    return;
                }
            }

            // Logic to lock Fake when the buffer is full
            if (_livenessHistory.Count == LivenessBufferSize)
            {
                // CẬP NHẬT: Ngưỡng để khóa Fake
                const int fakeFramesNeededToLock = 2; // Only need 2/3 frames to be Fake
                if (_livenessHistory.Count(isReal => !isReal) >= fakeFramesNeededToLock) IsVerifiedFake = true;
            }
        }

        public void ResetVerificationState()
        {
            IsVerifiedReal = false;
            IsVerifiedFake = false;
            _livenessHistory.Clear();
            FakeCooldownCounter = 0;
            IsLockedByYolo = false;
            FramesSinceFirstSeen = 0;
            ResetRecognitionState();
        }

        public void ResetRecognitionState()
        {
            RecognitionAttempted = false;
            RecognizedName = null;
            UnknownCooldownCounter = 0;
        }

        public void ForceFake()
        {
            if (IsVerifiedReal) return;
            IsVerifiedFake = true;
            IsLockedByYolo = true;
            _livenessHistory.Clear();
        }
    }
    #endregion
}

//namespace FaceLibrary
//{
//    //================================================================================
//    // CẤU TRÚC DỮ LIỆU TRẢ VỀ CHO GIAO DIỆN
//    //================================================================================
//    public class ProcessResult
//    {
//        public int Id { get; set; }
//        public Rect BoundingBox { get; set; }
//        public string Label { get; set; }
//        public Scalar Color { get; set; }
//        public Mat FaceCrop { get; set; } // Ảnh cắt ra để hiển thị preview
//    }

//    //================================================================================
//    // DỊCH VỤ XỬ LÝ TRUNG TÂM (PUBLIC API)
//    //================================================================================
//    public class FaceProcessingService : IDisposable
//    {
//        // --- Các bộ phát hiện (sử dụng các helper class nội bộ) ---
//        private readonly HaarCascadeDetector _faceDetector;
//        private readonly LivenessDetector _livenessDetector;
//        private readonly FaceRecognitionService _faceRecService;
//        private readonly YoloSpoofDetector _spoofObjectDetector;

//        // --- Dữ liệu tracking ---
//        private readonly ConcurrentDictionary<int, TrackedPerson> _trackedPeople = new ConcurrentDictionary<int, TrackedPerson>();
//        private const int MaxFramesUnseen = 10;
//        private const double SmoothingAlpha = 0.15;

//        // --- Dữ liệu cho luồng YOLO ---
//        private readonly object _yoloLock = new object();
//        private Mat _yoloInputFrame = new Mat();
//        private List<Rect> _yoloSpoofRects = new List<Rect>();
//        private Task _yoloTask;
//        private CancellationTokenSource _cts;

//        public FaceProcessingService(string modelsDir, string knownFacesPath)
//        {
//            string haarCascadePath = Path.Combine(modelsDir, "haarcascade_frontalface_default.xml");
//            string livenessModelPath = Path.Combine(modelsDir, "anti-spoof-mn3.onnx");
//            string dlibRecModel = Path.Combine(modelsDir, "dlib_face_recognition_resnet_model_v1.dat");
//            string dlibShapeModel = Path.Combine(modelsDir, "shape_predictor_68_face_landmarks.dat");
//            string yoloModelPath = Path.Combine(modelsDir, "yolov5s.onnx");

//            if (!File.Exists(haarCascadePath) || !File.Exists(livenessModelPath) || !File.Exists(dlibRecModel) || !File.Exists(dlibShapeModel) || !File.Exists(yoloModelPath))
//            {
//                throw new FileNotFoundException("Một hoặc nhiều file model không được tìm thấy. Vui lòng kiểm tra lại thư mục 'models'.");
//            }

//            _faceDetector = new HaarCascadeDetector(haarCascadePath);
//            _livenessDetector = new LivenessDetector(livenessModelPath);
//            _faceRecService = new FaceRecognitionService(modelsDir, knownFacesPath);
//            _spoofObjectDetector = new YoloSpoofDetector(yoloModelPath);
//        }

//        public void Start()
//        {
//            _cts = new CancellationTokenSource();
//            _yoloTask = Task.Run(() => YoloProcessingLoop(_spoofObjectDetector, _cts.Token));
//        }

//        public List<ProcessResult> ProcessFrame(Mat frame)
//        {
//            var results = new List<ProcessResult>();

//            lock (_yoloLock)
//            {
//                _yoloInputFrame?.Dispose();
//                _yoloInputFrame = frame.Clone();
//            }

//            List<Rect> currentSpoofRects;
//            lock (_yoloLock)
//            {
//                currentSpoofRects = new List<Rect>(_yoloSpoofRects);
//            }

//            foreach (var rect in currentSpoofRects)
//            {
//                Cv2.Rectangle(frame, rect, Scalar.Magenta, 2);
//            }

//            Rect[] currentDetections = _faceDetector.Detect(frame);
//            UpdateTrackers(currentDetections);

//            foreach (var person in _trackedPeople.Values)
//            {
//                if (person.FramesUnseen > 0) continue;
//                person.FramesSinceFirstSeen++;
//                string label;
//                Scalar color;

//                if (person.IsVerifiedReal)
//                {
//                    if (!person.RecognitionAttempted)
//                    {
//                        person.RecognitionAttempted = true;
//                        person.RecognizedName = "Recognizing...";
//                        var roiRect = ClampRect(person.SmoothedBox, frame.Width, frame.Height);
//                        if (roiRect.Width > 0)
//                        {
//                            var faceRoiClone = new Mat(frame, roiRect).Clone();
//                            Task.Run(() =>
//                            {
//                                try
//                                {
//                                    string name = _faceRecService.Recognize(faceRoiClone);
//                                    if (_trackedPeople.TryGetValue(person.Id, out var p)) p.RecognizedName = name;
//                                }
//                                finally { faceRoiClone.Dispose(); }
//                            });
//                        }
//                    }
//                    label = person.RecognizedName ?? "Unknown";
//                    color = Scalar.LimeGreen;
//                    if (label == "Unknown")
//                    {
//                        person.UnknownCooldownCounter++;
//                        if (person.UnknownCooldownCounter > 90) person.ResetRecognitionState();
//                    }
//                }
//                else if (person.IsVerifiedFake)
//                {
//                    label = "Fake";
//                    color = Scalar.Red;
//                    if (!person.IsLockedByYolo)
//                    {
//                        person.FakeCooldownCounter++;
//                        if (person.FakeCooldownCounter > 60) person.ResetVerificationState();
//                    }
//                }
//                else
//                {
//                    const int yoloGraceFrames = 5;
//                    if (person.FramesSinceFirstSeen <= yoloGraceFrames)
//                    {
//                        if (currentSpoofRects.Any(spoofRect => spoofRect.Contains(person.Center)))
//                        {
//                            Console.ForegroundColor = ConsoleColor.Magenta;
//                            Console.WriteLine($"[YOLO ALERT] Khuôn mặt ID {person.Id} bị phát hiện. Ép buộc FAKE.");
//                            Console.ResetColor();
//                            person.ForceFake();
//                        }
//                    }

//                    if (!person.IsVerifiedFake && person.FramesSinceFirstSeen > yoloGraceFrames)
//                    {
//                        var expandedRect = ExpandBox(person.SmoothedBox, frame.Width, frame.Height, 0.3);
//                        using var faceRoiLive = new Mat(frame, expandedRect);
//                        if (faceRoiLive.Width > 0 && faceRoiLive.Height > 0)
//                        {
//                            var (realScore, spoofScore) = _livenessDetector.Analyze(faceRoiLive);
//                            const float realFrameThreshold = 0.80f;
//                            bool isRealThisFrame = realScore > realFrameThreshold;
//                            person.UpdateLiveness(isRealThisFrame);
//                        }
//                    }

//                    if (person.IsVerifiedReal) { label = "Real"; color = Scalar.LimeGreen; }
//                    else if (person.IsVerifiedFake) { label = "Fake"; color = Scalar.Red; }
//                    else { label = "Dang xac thuc..."; color = Scalar.Yellow; }
//                }

//                var faceCrop = new Mat(frame, ClampRect(person.SmoothedBox, frame.Width, frame.Height));
//                results.Add(new ProcessResult
//                {
//                    Id = person.Id,
//                    BoundingBox = person.SmoothedBox,
//                    Label = label,
//                    Color = color,
//                    FaceCrop = faceCrop.Clone()
//                });
//                faceCrop.Dispose();
//            }

//            return results;
//        }

//        public void AddNewFace(string name, Mat frame, Rect faceBox)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                Console.WriteLine("[CANCELLED] Tên không hợp lệ. Đã hủy lưu.");
//                return;
//            }
//            using var faceToSave = new Mat(frame, ClampRect(faceBox, frame.Width, frame.Height));
//            if (faceToSave.Width > 0 && faceToSave.Height > 0)
//            {
//                _faceRecService.AddNewFace(name, faceToSave);
//            }
//        }

//        public void Dispose()
//        {
//            _cts?.Cancel();
//            try { _yoloTask?.Wait(1000); } catch (Exception) { /* Ignored */ }
//            _yoloInputFrame?.Dispose();
//            _cts?.Dispose();
//            _faceDetector?.Dispose();
//            _livenessDetector?.Dispose();
//            _faceRecService?.Dispose();
//            _spoofObjectDetector?.Dispose();
//        }

//        private void YoloProcessingLoop(YoloSpoofDetector detector, CancellationToken token)
//        {
//            while (!token.IsCancellationRequested)
//            {
//                Mat frameToProcess = null;
//                lock (_yoloLock)
//                {
//                    if (!_yoloInputFrame.Empty())
//                    {
//                        frameToProcess = _yoloInputFrame.Clone();
//                    }
//                }
//                if (frameToProcess != null)
//                {
//                    var detectedRects = detector.Detect(frameToProcess);
//                    lock (_yoloLock)
//                    {
//                        _yoloSpoofRects = detectedRects;
//                    }
//                    frameToProcess.Dispose();
//                }
//                try
//                {
//                    Task.Delay(100, token).Wait(token);
//                }
//                catch (OperationCanceledException)
//                {
//                    break;
//                }
//            }
//            Console.WriteLine("Luồng xử lý YOLO đã dừng.");
//        }

//        private void UpdateTrackers(Rect[] currentDetections)
//        {
//            foreach (var person in _trackedPeople.Values)
//            {
//                person.FramesUnseen++;
//            }
//            var usedDetectionIndices = new HashSet<int>();
//            foreach (var person in _trackedPeople.Values)
//            {
//                double minDistance = double.MaxValue;
//                int bestMatchIndex = -1;
//                for (int i = 0; i < currentDetections.Length; i++)
//                {
//                    if (usedDetectionIndices.Contains(i)) continue;
//                    var detectionCenter = new Point(currentDetections[i].X + currentDetections[i].Width / 2, currentDetections[i].Y + currentDetections[i].Height / 2);
//                    double dist = Point.Distance(person.Center, detectionCenter);
//                    if (dist < minDistance)
//                    {
//                        minDistance = dist;
//                        bestMatchIndex = i;
//                    }
//                }
//                const double distanceThreshold = 75.0;
//                if (bestMatchIndex != -1 && minDistance < distanceThreshold)
//                {
//                    if (person.FramesUnseen > 1)
//                    {
//                        person.ResetVerificationState();
//                    }
//                    person.SmoothedBox = SmoothRect(person.SmoothedBox, currentDetections[bestMatchIndex], SmoothingAlpha);
//                    person.FramesUnseen = 0;
//                    usedDetectionIndices.Add(bestMatchIndex);
//                }
//            }
//            var idsToRemove = _trackedPeople.Where(kv => kv.Value.FramesUnseen > MaxFramesUnseen).Select(kv => kv.Key).ToList();
//            foreach (var id in idsToRemove)
//            {
//                _trackedPeople.TryRemove(id, out _);
//            }
//            for (int i = 0; i < currentDetections.Length; i++)
//            {
//                if (!usedDetectionIndices.Contains(i))
//                {
//                    var newPerson = new TrackedPerson(currentDetections[i]);
//                    _trackedPeople.TryAdd(newPerson.Id, newPerson);
//                }
//            }
//        }

//        private static Rect SmoothRect(Rect previous, Rect current, double alpha)
//        {
//            int newX = (int)(alpha * current.X + (1 - alpha) * previous.X);
//            int newY = (int)(alpha * current.Y + (1 - alpha) * previous.Y);
//            int newW = (int)(alpha * current.Width + (1 - alpha) * previous.Width);
//            int newH = (int)(alpha * current.Height + (1 - alpha) * previous.Height);
//            return new Rect(newX, newY, newW, newH);
//        }
//        private static Rect ExpandBox(Rect box, int frameW, int frameH, double factor)
//        {
//            int extraW = (int)(box.Width * factor);
//            int extraH = (int)(box.Height * factor);
//            int newX = Math.Max(0, box.X - extraW / 2);
//            int newY = Math.Max(0, box.Y - extraH / 2);
//            int newW = Math.Min(frameW - newX, box.Width + extraW);
//            int newH = Math.Min(frameH - newY, box.Height + extraH);
//            return new Rect(newX, newY, newW, newH);
//        }
//        private static Rect ClampRect(Rect rect, int frameWidth, int frameHeight)
//        {
//            int x = Math.Max(0, rect.X);
//            int y = Math.Max(0, rect.Y);
//            int w = Math.Min(rect.Width, frameWidth - x);
//            int h = Math.Min(rect.Height, frameHeight - y);
//            return new Rect(x, y, Math.Max(0, w), Math.Max(0, h));
//        }
//    }

//    //================================================================================
//    // CÁC LỚP HELPER (Nội bộ của thư viện, được điền đầy đủ)
//    //================================================================================
//    internal class HaarCascadeDetector : IDisposable
//    {
//        private readonly CascadeClassifier _faceDetector;
//        public HaarCascadeDetector(string modelPath) { _faceDetector = new CascadeClassifier(modelPath); }
//        public Rect[] Detect(Mat image)
//        {
//            using var gray = new Mat();
//            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
//            return _faceDetector.DetectMultiScale(gray, 1.1, 13, HaarDetectionTypes.ScaleImage, new Size(60, 60));
//        }
//        public void Dispose() => _faceDetector?.Dispose();
//    }
//    internal class LivenessDetector : IDisposable
//    {
//        private readonly InferenceSession _session;
//        private readonly Size _inputSize = new Size(128, 128);
//        private readonly string _inputName;
//        public LivenessDetector(string modelPath)
//        {
//            _session = new InferenceSession(modelPath, new SessionOptions());
//            _inputName = _session.InputMetadata.Keys.First();
//            // Console.WriteLine("INFO: ONNX Runtime (Liveness) được cấu hình để chạy trên CPU.");
//        }
//        public (float realScore, float spoofScore) Analyze(Mat faceRoi)
//        {
//            if (faceRoi.Empty()) return (0f, 1f);
//            using var resized = new Mat();
//            Cv2.Resize(faceRoi, resized, _inputSize);
//            var inputTensor = new DenseTensor<float>(new[] { 1, 3, _inputSize.Height, _inputSize.Width });
//            for (int y = 0; y < _inputSize.Height; y++)
//            {
//                for (int x = 0; x < _inputSize.Width; x++)
//                {
//                    var pixel = resized.Get<Vec3b>(y, x);
//                    inputTensor[0, 0, y, x] = (pixel.Item0 - 127.5f) / 128.0f;
//                    inputTensor[0, 1, y, x] = (pixel.Item1 - 127.5f) / 128.0f;
//                    inputTensor[0, 2, y, x] = (pixel.Item2 - 127.5f) / 128.0f;
//                }
//            }
//            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_inputName, inputTensor) };
//            using var results = _session.Run(inputs);
//            var output = results.First().AsTensor<float>().ToArray();
//            var probabilities = Softmax(output);
//            return (probabilities[1], probabilities[0]);
//        }
//        private float[] Softmax(float[] logits)
//        {
//            var maxLogit = logits.Max();
//            var exps = logits.Select(x => (float)Math.Exp(x - maxLogit)).ToArray();
//            var sumExps = exps.Sum();
//            return exps.Select(x => x / sumExps).ToArray();
//        }
//        public void Dispose() => _session?.Dispose();
//    }
//    internal class FaceRecognitionService : IDisposable
//    {
//        private readonly FaceRecognition _faceRecognition;
//        private readonly string _knownFacesDir;
//        private readonly Dictionary<string, List<FaceEncoding>> _knownEncodings = new Dictionary<string, List<FaceEncoding>>();
//        public FaceRecognitionService(string modelsDir, string knownFacesDir)
//        {
//            _faceRecognition = FaceRecognition.Create(modelsDir);
//            _knownFacesDir = knownFacesDir;
//            Directory.CreateDirectory(_knownFacesDir);
//            LoadKnownFaces();
//        }
//        private void LoadKnownFaces()
//        {
//            Console.WriteLine("[INFO] Đang tải cơ sở dữ liệu khuôn mặt từ thư mục...");
//            var peopleDirs = Directory.GetDirectories(_knownFacesDir);
//            foreach (var dir in peopleDirs)
//            {
//                var personName = new DirectoryInfo(dir).Name;
//                var imageFiles = Directory.GetFiles(dir, "*.jpg").Concat(Directory.GetFiles(dir, "*.png")).ToArray();
//                if (!imageFiles.Any()) continue;
//                _knownEncodings[personName] = new List<FaceEncoding>();
//                foreach (var imagePath in imageFiles)
//                {
//                    try
//                    {
//                        using var img = FaceRecognition.LoadImageFile(imagePath);
//                        var encoding = _faceRecognition.FaceEncodings(img).FirstOrDefault();
//                        if (encoding != null) _knownEncodings[personName].Add(encoding);
//                    }
//                    catch (Exception e) { Console.WriteLine($"[WARNING] Không thể tải ảnh: {imagePath}. Lỗi: {e.Message}"); }
//                }
//                if (_knownEncodings[personName].Any()) Console.WriteLine($"  - Đã tải {_knownEncodings[personName].Count} mẫu cho '{personName}'.");
//            }
//            Console.WriteLine($"[INFO] Tải xong! Có {_knownEncodings.Count(kv => kv.Value.Any())} người trong CSDL.");
//        }
//        public string Recognize(Mat faceRoi)
//        {
//            byte[] imageBytes = faceRoi.ImEncode(".jpg");
//            if (imageBytes == null || imageBytes.Length == 0) return "Unknown";
//            using var ms = new MemoryStream(imageBytes);
//            using var tempBmp = new Bitmap(ms);
//            using var image = FaceRecognition.LoadImage(tempBmp);
//            var unknownEncoding = _faceRecognition.FaceEncodings(image).FirstOrDefault();
//            if (unknownEncoding == null || !_knownEncodings.Any())
//            {
//                unknownEncoding?.Dispose();
//                return "Unknown";
//            }
//            string bestMatchName = "Unknown";
//            double bestDistance = 0.4;
//            foreach (var person in _knownEncodings)
//            {
//                if (!person.Value.Any()) continue;
//                var distances = FaceRecognition.FaceDistances(person.Value, unknownEncoding);
//                var minDistance = distances.Min();
//                if (minDistance < bestDistance)
//                {
//                    bestDistance = minDistance;
//                    bestMatchName = person.Key;
//                }
//            }
//            unknownEncoding.Dispose();
//            return bestMatchName;
//        }
//        public void AddNewFace(string name, Mat faceRoi)
//        {
//            var personDir = Path.Combine(_knownFacesDir, name);
//            Directory.CreateDirectory(personDir);
//            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
//            var imagePath = Path.Combine(personDir, $"{timestamp}.jpg");
//            faceRoi.SaveImage(imagePath);
//            Console.WriteLine($"[INFO] Đã lưu ảnh mới vào: {imagePath}");
//            using var img = FaceRecognition.LoadImageFile(imagePath);
//            var encoding = _faceRecognition.FaceEncodings(img).FirstOrDefault();
//            if (encoding != null)
//            {
//                if (_knownEncodings.ContainsKey(name)) _knownEncodings[name].Add(encoding);
//                else _knownEncodings[name] = new List<FaceEncoding> { encoding };
//                Console.WriteLine($"[SUCCESS] Đã học khuôn mặt mới cho '{name}'.");
//            }
//        }
//        public void Dispose()
//        {
//            _faceRecognition?.Dispose();
//            foreach (var encodings in _knownEncodings.Values)
//            {
//                foreach (var encoding in encodings) encoding.Dispose();
//            }
//        }
//    }
//    internal class YoloSpoofDetector : IDisposable
//    {
//        private readonly InferenceSession _session;
//        private readonly string _inputName;
//        private readonly Size _inputSize = new Size(640, 640);
//        private readonly int _cellPhoneClassId = 67;
//        public YoloSpoofDetector(string modelPath)
//        {
//            _session = new InferenceSession(modelPath, new SessionOptions());
//            _inputName = _session.InputMetadata.Keys.First();
//            // Console.WriteLine("INFO: ONNX Runtime (YOLOv5) được cấu hình để chạy trên CPU.");
//        }
//        public List<Rect> Detect(Mat frame)
//        {
//            var inputTensor = Preprocess(frame, out float scale);
//            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_inputName, inputTensor) };
//            using var results = _session.Run(inputs);
//            var output = results.First().AsTensor<float>();
//            return Postprocess(output, scale);
//        }
//        private DenseTensor<float> Preprocess(Mat frame, out float scale)
//        {
//            int inputWidth = _inputSize.Width;
//            int inputHeight = _inputSize.Height;
//            scale = Math.Min((float)inputWidth / frame.Width, (float)inputHeight / frame.Height);
//            int newWidth = (int)(frame.Width * scale);
//            int newHeight = (int)(frame.Height * scale);
//            using var resized = new Mat();
//            Cv2.Resize(frame, resized, new Size(newWidth, newHeight));
//            using var padded = new Mat(inputHeight, inputWidth, MatType.CV_8UC3, new Scalar(114, 114, 114));
//            var roi = new Rect(0, 0, newWidth, newHeight);
//            resized.CopyTo(new Mat(padded, roi));
//            var tensor = new DenseTensor<float>(new[] { 1, 3, inputHeight, inputWidth });
//            for (int y = 0; y < inputHeight; y++)
//            {
//                for (int x = 0; x < inputWidth; x++)
//                {
//                    var pixel = padded.Get<Vec3b>(y, x);
//                    tensor[0, 0, y, x] = pixel.Item2 / 255f;
//                    tensor[0, 1, y, x] = pixel.Item1 / 255f;
//                    tensor[0, 2, y, x] = pixel.Item0 / 255f;
//                }
//            }
//            return tensor;
//        }
//        private List<Rect> Postprocess(Tensor<float> output, float scale)
//        {
//            var boxes = new List<Rect>();
//            var confidences = new List<float>();
//            float confThreshold = 0.3f;
//            float nmsThreshold = 0.4f;
//            for (int i = 0; i < output.Dimensions[1]; i++)
//            {
//                float objectness = output[0, i, 4];
//                if (objectness < confThreshold) continue;
//                var classScores = new List<float>();
//                for (int j = 5; j < output.Dimensions[2]; j++) classScores.Add(output[0, i, j]);
//                int maxClassIndex = classScores.IndexOf(classScores.Max());
//                float maxClassScore = classScores[maxClassIndex];
//                float confidence = objectness * maxClassScore;
//                if (confidence > confThreshold && maxClassIndex == _cellPhoneClassId)
//                {
//                    float cx = output[0, i, 0] / scale;
//                    float cy = output[0, i, 1] / scale;
//                    float w = output[0, i, 2] / scale;
//                    float h = output[0, i, 3] / scale;
//                    int x = (int)(cx - w / 2);
//                    int y = (int)(cy - h / 2);
//                    boxes.Add(new Rect(x, y, (int)w, (int)h));
//                    confidences.Add(confidence);
//                }
//            }
//            CvDnn.NMSBoxes(boxes, confidences, confThreshold, nmsThreshold, out int[] indices);
//            return indices.Select(i => boxes[i]).ToList();
//        }
//        public void Dispose() => _session?.Dispose();
//    }
//    internal class TrackedPerson
//    {
//        private static int _nextId = 0;
//        public int Id { get; }
//        public Rect SmoothedBox { get; set; }
//        public int FramesUnseen { get; set; } = 0;
//        public bool IsVerifiedReal { get; set; } = false;
//        public bool IsVerifiedFake { get; private set; } = false;
//        private const int LivenessBufferSize = 3;
//        private readonly Queue<bool> _livenessHistory = new Queue<bool>();
//        public int FakeCooldownCounter { get; set; } = 0;
//        public string RecognizedName { get; set; } = null;
//        public bool RecognitionAttempted { get; set; } = false;
//        public int UnknownCooldownCounter { get; set; } = 0;
//        public bool IsLockedByYolo { get; private set; } = false;
//        public int FramesSinceFirstSeen { get; set; } = 0;
//        public TrackedPerson(Rect initialBox)
//        {
//            Id = _nextId++;
//            SmoothedBox = initialBox;
//        }
//        public Point Center => new Point(SmoothedBox.X + SmoothedBox.Width / 2, SmoothedBox.Y + SmoothedBox.Height / 2);
//        public void UpdateLiveness(bool isRealThisFrame)
//        {
//            if (IsVerifiedReal || IsVerifiedFake) return;
//            _livenessHistory.Enqueue(isRealThisFrame);
//            if (_livenessHistory.Count > LivenessBufferSize) _livenessHistory.Dequeue();
//            const int consecutiveRealFramesNeeded = 3;
//            if (_livenessHistory.Count >= consecutiveRealFramesNeeded)
//            {
//                if (_livenessHistory.All(isReal => isReal))
//                {
//                    IsVerifiedReal = true;
//                    return;
//                }
//            }
//            if (_livenessHistory.Count == LivenessBufferSize)
//            {
//                const int fakeFramesNeededToLock = 2;
//                if (_livenessHistory.Count(isReal => !isReal) >= fakeFramesNeededToLock) IsVerifiedFake = true;
//            }
//        }
//        public void ResetVerificationState()
//        {
//            IsVerifiedReal = false;
//            IsVerifiedFake = false;
//            _livenessHistory.Clear();
//            FakeCooldownCounter = 0;
//            IsLockedByYolo = false;
//            FramesSinceFirstSeen = 0;
//            ResetRecognitionState();
//        }
//        public void ResetRecognitionState()
//        {
//            RecognitionAttempted = false;
//            RecognizedName = null;
//            UnknownCooldownCounter = 0;
//        }
//        public void ForceFake()
//        {
//            if (IsVerifiedReal) return;
//            IsVerifiedFake = true;
//            IsLockedByYolo = true;
//            _livenessHistory.Clear();
//        }
//    }
//}