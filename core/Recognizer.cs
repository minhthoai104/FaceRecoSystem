using FaceRecognitionDotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceRecoSystem
{
    public class Recognizer
    {
        private readonly IReadOnlyDictionary<string, List<double[]>> _known;
        private readonly double _tolerance;

        public Recognizer(IReadOnlyDictionary<string, List<double[]>> known, double tolerance = 0.6)
        {
            _known = known ?? new Dictionary<string, List<double[]>>();
            _tolerance = tolerance;

            // Chuẩn hóa trước tất cả embedding known (L2 normalize)
            foreach (var key in _known.Keys.ToList())
            {
                var list = _known[key];
                for (int i = 0; i < list.Count; i++)
                    list[i] = Normalize(list[i]);
            }
        }

        /// <summary>
        /// Nhận diện khuôn mặt: trả về (name, distance, cosine, confidence)
        /// </summary>
        public (string name, double distance, double cosine, double confidence) Recognize(FaceEncoding encoding)
        {
            if (encoding == null || _known.Count == 0)
                return ("Unknown", double.MaxValue, 0, 0);

            double[] targetEnc = Normalize(encoding.GetRawEncoding());

            string bestName = "Unknown";
            double bestDist = double.MaxValue;
            double bestCos = -1;

            foreach (var kvp in _known)
            {
                foreach (var knownEnc in kvp.Value)
                {
                    double dist = Euclidean(knownEnc, targetEnc);
                    double cos = Cosine(knownEnc, targetEnc);

                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestCos = cos;
                        bestName = kvp.Key;
                    }
                }
            }

            // --- Tính confidence ---
            double confidence = DistanceToConfidence(bestDist);

            // --- In ra log để debug ---
            Console.WriteLine($"[FaceMatch] → Name={bestName,-15} | Dist={bestDist:F4} | Cos={bestCos:F4} | Conf={confidence:F1}%");

            if (bestDist < _tolerance)
                return (bestName, bestDist, bestCos, confidence);

            return ("Unknown", bestDist, bestCos, confidence);
        }
        private static double[] Normalize(double[] v)
        {
            double norm = Math.Sqrt(v.Sum(x => x * x));
            if (norm == 0) return v;
            return v.Select(x => x / norm).ToArray();
        }

        private static double Euclidean(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                double d = a[i] - b[i];
                sum += d * d;
            }
            return Math.Sqrt(sum);
        }

        private static double Cosine(double[] a, double[] b)
        {
            double dot = 0;
            for (int i = 0; i < a.Length; i++)
                dot += a[i] * b[i];
            return dot; // Đã normalize nên dot chính là cosine similarity
        }

        /// <summary>
        /// Chuyển khoảng cách thành độ tin cậy (confidence) 0–100%
        /// </summary>
        private static double DistanceToConfidence(double dist)
        {
            // Dạng logistic — mượt và phản ánh độ tin cậy thực tế
            double k = 15;      // Độ dốc (cao hơn = nhạy hơn)
            double mid = 0.45;  // Điểm giữa (confidence = 50%)

            double conf = 100 / (1 + Math.Exp(k * (dist - mid)));

            if (conf < 0) conf = 0;
            if (conf > 100) conf = 100;
            return Math.Round(conf, 2);
        }
    }
}
