using FaceRecognitionDotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceRecoSystem
{
    internal class Recognizer
    {
        private readonly IReadOnlyDictionary<string, List<FaceEncoding>> _db;
        private readonly double _tolerance;

        public Recognizer(IReadOnlyDictionary<string, List<FaceEncoding>> db, double tolerance = 0.4)
        {
            _db = db;
            _tolerance = tolerance;
        }

        public (string name, double confidence) Recognize(FaceEncoding unknownEncoding)
        {
            if (!_db.Any() || unknownEncoding == null)
            {
                return ("Unknown", 0);
            }

            string bestMatchName = "Unknown";
            double bestMatchDistance = double.MaxValue;

            foreach (var personEntry in _db)
            {
                string personName = personEntry.Key;
                List<FaceEncoding> knownEncodings = personEntry.Value;

                double distance = knownEncodings
                    .Select(knownEnc => FaceRecognition.FaceDistance(knownEnc, unknownEncoding))
                    .Min();

                if (distance < bestMatchDistance)
                {
                    bestMatchDistance = distance;
                    bestMatchName = personName;
                }
            }
            
            if (bestMatchDistance <= _tolerance)
            {
                double normalizedDistance = bestMatchDistance / _tolerance;
                double confidence = (1.0 - Math.Pow(normalizedDistance, 4)) * 100.0;
                
                return (bestMatchName, confidence);
            }

            return ("Unknown", 0);
        }
    }
}