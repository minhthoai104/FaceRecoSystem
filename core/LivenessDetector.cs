using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Size = OpenCvSharp.Size;

namespace FaceRecoSystem
{
    internal class LivenessDetector : IDisposable
    {
        private readonly InferenceSession _session;
        private Mat? _prevGray;
        private double _avgMotion = 0;
        private int _frameCount = 0;
        private bool _isLive = true;

        public LivenessDetector(string modelPath)
        {
            if (!System.IO.File.Exists(modelPath))
                throw new FileNotFoundException($"Không tìm thấy model: {modelPath}");
            _session = new InferenceSession(modelPath);
        }

        public bool Analyze(Mat faceFrame)
        {
            if (faceFrame == null || faceFrame.Empty())
                return true;

            bool modelLive = PredictAntiSpoof(faceFrame);

            bool motionLive = AnalyzeFrameMotion(faceFrame);

            _isLive = modelLive && motionLive;

            Console.WriteLine($"[Liveness] Model={(modelLive ? "LIVE" : "FAKE")}, Motion={(motionLive ? "LIVE" : "FAKE")} => FINAL={(_isLive ? "LIVE ✅" : "FAKE ❌")}");
            return _isLive;
        }

        private bool PredictAntiSpoof(Mat faceFrame)
        {
            try
            {
                using var resized = faceFrame.Resize(new Size(128, 128));
                resized.ConvertTo(resized, MatType.CV_32FC3, 1.0 / 255.0);
                Cv2.CvtColor(resized, resized, ColorConversionCodes.BGR2RGB);

                var inputTensor = new DenseTensor<float>(new[] { 1, 3, 128, 128 });
                for (int y = 0; y < 128; y++)
                {
                    for (int x = 0; x < 128; x++)
                    {
                        var pixel = resized.At<Vec3f>(y, x);
                        inputTensor[0, 0, y, x] = pixel.Item0;
                        inputTensor[0, 1, y, x] = pixel.Item1;
                        inputTensor[0, 2, y, x] = pixel.Item2;
                    }
                }

                var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("actual_input_1", inputTensor)
        };

                using var results = _session.Run(inputs);
                var output = results.First().AsEnumerable<float>().ToArray();

                float realScore = output[1];
                float fakeScore = output[0];

                double confidence = realScore / (realScore + fakeScore + 1e-6);
                bool isLive = confidence > 0.65 && realScore > fakeScore * 1.1;

                _avgMotion = 0.8 * _avgMotion + 0.2 * (isLive ? 1 : 0);
                bool smoothedLive = _avgMotion > 0.5;

                Console.WriteLine($"[AntiSpoof] Real={realScore:F3}, Fake={fakeScore:F3}, Conf={confidence:F2}, => {(smoothedLive ? "LIVE" : "FAKE")}");
                return smoothedLive;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AntiSpoof Error] {ex.Message}");
                return true;
            }
        }


        private bool AnalyzeFrameMotion(Mat frame)
        {
            try
            {
                using var gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                bool result = true;
                if (_prevGray != null && _prevGray.Size() == gray.Size())
                {
                    using var diff = new Mat();
                    Cv2.Absdiff(gray, _prevGray, diff);
                    double motion = Cv2.Mean(diff).Val0;

                    _frameCount++;
                    _avgMotion = (_avgMotion * (_frameCount - 1) + motion) / _frameCount;

                    Cv2.MeanStdDev(gray, out var mean, out var std);
                    double brightnessVar = std.Val0;
                    Scalar meanVal = Cv2.Mean(frame);
                    double lightLevel = (meanVal.Val0 + meanVal.Val1 + meanVal.Val2) / 3.0;

                    using var lap = new Mat();
                    Cv2.Laplacian(gray, lap, MatType.CV_64F);
                    Cv2.MeanStdDev(lap, out _, out var lapStd);
                    double texture = lapStd.Val0;

                    bool motionOK = motion > 0.5 || _avgMotion > 0.6;
                    bool lightOK = brightnessVar > 5 && lightLevel < 240;
                    bool textureOK = texture > 6;

                    result = (motionOK && textureOK) || (textureOK && lightOK);
                    Console.WriteLine($"[Motion] M={motion:F2}, Var={brightnessVar:F2}, Tex={texture:F2} => {(result ? "LIVE" : "FAKE")}");
                }

                _prevGray = gray.Clone();
                return result;
            }
            catch
            {
                return true;
            }
        }

        public bool IsLive() => _isLive;

        public void Reset()
        {
            _prevGray?.Dispose();
            _prevGray = null;
            _avgMotion = 0;
            _frameCount = 0;
            _isLive = true;
        }

        public void Dispose()
        {
            _session?.Dispose();
            _prevGray?.Dispose();
        }
    }
}
