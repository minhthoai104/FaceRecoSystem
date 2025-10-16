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

        public Recognizer(IReadOnlyDictionary<string, List<double[]>> known, double tolerance = 0.45)
        {
            _known = known ?? new Dictionary<string, List<double[]>>();
            _tolerance = tolerance;
        }

        public (string name, double confidence) Recognize(FaceEncoding encoding)
        {
            if (encoding == null || _known.Count == 0)
                return ("Unknown", 0);

            double[] targetEnc = encoding.GetRawEncoding();
            string bestName = "Unknown";
            double bestDistance = double.MaxValue;

            foreach (var kvp in _known)
            {
                foreach (var knownEnc in kvp.Value)
                {
                    double dist = ComputeFaceDistance(knownEnc, targetEnc);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestName = kvp.Key;
                    }
                }
            }

            if (bestDistance < _tolerance)
            {
                double confidence = (1.0 - bestDistance) * 100.0;
                return (bestName, Math.Round(confidence, 2));
            }

            return ("Unknown", 0);
        }

        private static double ComputeFaceDistance(double[] enc1, double[] enc2)
        {
            if (enc1 == null || enc2 == null || enc1.Length != enc2.Length)
                return double.MaxValue;

            double sum = 0;
            for (int i = 0; i < enc1.Length; i++)
            {
                double diff = enc1[i] - enc2[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum / enc1.Length);
        }
    }
}
