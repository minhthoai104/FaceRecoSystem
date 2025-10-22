using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.IO;
using System.Linq;

namespace FaceRecoSystem
{
    public enum LivenessStatus { Real, PaperSpoof, ScreenSpoof, Error }

    public class LivenessResult
    {
        public LivenessStatus Status { get; set; }
        public float RealScore { get; set; }
        public float PaperScore { get; set; }
        public float ScreenScore { get; set; }
    }

    public static class LivenessHelper
    {
        private static readonly InferenceSession _session;
        private const int ModelInputSize = 80;

        static LivenessHelper()
        {
            string modelPath = "D:\\Work\\Project\\FaceRecoSystem\\models\\fas_model.onnx";
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException("Không tìm thấy model Liveness: fas_model.onnx");
            }
            _session = new InferenceSession(modelPath);
        }

        public static LivenessResult CheckLiveness(Mat face)
        {
            if (face == null || face.Empty())
            {
                return new LivenessResult { Status = LivenessStatus.Error };
            }

             var resized = face.Resize(new Size(ModelInputSize, ModelInputSize));
            var inputTensor = new DenseTensor<float>(new[] { 1, 3, ModelInputSize, ModelInputSize });

            for (int y = 0; y < ModelInputSize; y++)
            {
                for (int x = 0; x < ModelInputSize; x++)
                {
                    var pixel = resized.At<Vec3b>(y, x);
                    inputTensor[0, 0, y, x] = pixel.Item0; // B
                    inputTensor[0, 1, y, x] = pixel.Item1; // G
                    inputTensor[0, 2, y, x] = pixel.Item2; // R
                }
            }

            var inputs = new[] { NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor) };
             var results = _session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();

            var probabilities = Softmax(output);

            float paperScore = probabilities[0];
            float realScore = probabilities[1];
            float screenScore = probabilities[2];

            int predictedLabel = Array.IndexOf(probabilities, probabilities.Max());
            LivenessStatus status;
            switch (predictedLabel)
            {
                case 0: status = LivenessStatus.PaperSpoof; break;
                case 1: status = LivenessStatus.Real; break;
                case 2: status = LivenessStatus.ScreenSpoof; break;
                default: status = LivenessStatus.Error; break;
            }

            return new LivenessResult
            {
                Status = status,
                RealScore = realScore,
                PaperScore = paperScore,
                ScreenScore = screenScore
            };
        }

        private static float[] Softmax(float[] scores)
        {
            var maxVal = scores.Max();
            var exp = scores.Select(s => Math.Exp(s - maxVal));
            var sumExp = exp.Sum();
            return exp.Select(e => (float)(e / sumExp)).ToArray();
        }
    }
}