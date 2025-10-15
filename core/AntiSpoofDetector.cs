using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Size = OpenCvSharp.Size;
namespace FaceRecoSystem
{
    public class AntiSpoofDetector : IDisposable
    {
        private readonly InferenceSession _session;

        public AntiSpoofDetector(string modelPath)
        {
            _session = new InferenceSession(modelPath);
        }

        public bool IsLive(Mat faceFrame)
        {
            // Resize ảnh khuôn mặt về đúng kích thước model (MN3 thường 80x80)
            Mat resized = faceFrame.Resize(new Size(128, 128));
            resized.ConvertTo(resized, MatType.CV_32FC3, 1.0 / 255.0);

            // BGR -> RGB
            Cv2.CvtColor(resized, resized, ColorConversionCodes.BGR2RGB);

            // Chuyển sang tensor [1, 3, 80, 80]
            var inputTensor = new DenseTensor<float>(new[] { 1, 3, 128, 128 });
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    var pixel = resized.At<Vec3f>(y, x);
                    inputTensor[0, 0, y, x] = pixel.Item2; // R
                    inputTensor[0, 1, y, x] = pixel.Item1; // G
                    inputTensor[0, 2, y, x] = pixel.Item0; // B
                }
            }

            // Chạy model
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("actual_input_1", inputTensor)
            };

            using var results = _session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();


            // MN3 thường có 2 output: [fake, real]
            float realScore = output[1];
            float fakeScore = output[0];

            // Nếu realScore cao hơn, xem như là thật
            bool isLive = realScore > 0.8;
            Console.WriteLine($"[AntiSpoof] Real={realScore:F3}, Fake={fakeScore:F3}, => {(isLive ? "LIVE" : "FAKE")}");

            return isLive;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
