using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = OpenCvSharp.Size;

namespace FaceRecoSystem
{
    internal class DepthEstimator : IDisposable
    {
        private readonly InferenceSession _session;
        private readonly int _inputSize = 384;

        public DepthEstimator(string modelPath)
        {
            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Depth model not found: {modelPath}");
            _session = new InferenceSession(modelPath);
        }

        public double Estimate(Mat face)
        {
            try
            {
                using var resized = face.Resize(new Size(_inputSize, _inputSize));
                var floatMat = new Mat();
                resized.ConvertTo(floatMat, MatType.CV_32FC3, 1.0 / 255.0);
                Cv2.CvtColor(floatMat, floatMat, ColorConversionCodes.BGR2RGB);

                var tensor = new DenseTensor<float>(new[] { 1, 3, _inputSize, _inputSize });
                for (int y = 0; y < _inputSize; y++)
                {
                    for (int x = 0; x < _inputSize; x++)
                    {
                        var v = floatMat.At<Vec3f>(y, x);
                        tensor[0, 0, y, x] = v.Item0;
                        tensor[0, 1, y, x] = v.Item1;
                        tensor[0, 2, y, x] = v.Item2;
                    }
                }

                using var results = _session.Run(new[] { NamedOnnxValue.CreateFromTensor("input", tensor) });
                var outArr = results.First().AsEnumerable<float>().ToArray();
                if (outArr.Length == 0) return 0.0;

                double mean = outArr.Average();
                double variance = outArr.Select(v => Math.Pow(v - mean, 2)).Average();
                double stddev = Math.Sqrt(variance);

                // lấy phần đầu (ví dụ 1/8) để ước lượng background mean - giúp phân biệt ảnh phẳng
                int bgCount = Math.Max(1, outArr.Length / 8);
                double bgMean = outArr.Take(bgCount).Average();
                double relMean = Math.Abs(mean - bgMean);

                double score = Clamp(stddev * 30.0 + relMean * 2.5, 0.0, 1.0);
                return score;
            }
            catch
            {
                return 0.0;
            }
        }
        private static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
