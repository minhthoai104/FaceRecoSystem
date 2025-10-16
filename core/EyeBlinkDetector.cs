using FaceRecognitionDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = OpenCvSharp.Point;
using CvPoint = OpenCvSharp.Point;

namespace FaceRecoSystem
{
    internal class EyeBlinkDetector
    {
        private const double EyeARThreshold = 0.22;
        private const int EyeARConsecutiveFrames = 2;

        private int _counter = 0;

        private double CalculateEar(IEnumerable<CvPoint> eyeLandmarks)
        {
            var eyePoints = eyeLandmarks.ToList();
            if (eyePoints.Count != 6)
                return 0.0;

            double a = GetDistance(eyePoints[1], eyePoints[5]);
            double b = GetDistance(eyePoints[2], eyePoints[4]);
            double c = GetDistance(eyePoints[0], eyePoints[3]);

            return (a + b) / (2.0 * c);
        }

        private double GetDistance(CvPoint p1, CvPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public bool Detect(IDictionary<FacePart, IEnumerable<FaceRecognitionDotNet.Point>> landmarks)
        {
            try
            {
                var leftEye = landmarks[FacePart.LeftEye]
                    .Select(p => new CvPoint((int)p.X, (int)p.Y));
                var rightEye = landmarks[FacePart.RightEye]
                    .Select(p => new CvPoint((int)p.X, (int)p.Y));

                double leftEar = CalculateEar(leftEye);
                double rightEar = CalculateEar(rightEye);
                double ear = (leftEar + rightEar) / 2.0;

                if (ear < EyeARThreshold)
                {
                    _counter++;
                }
                else
                {
                    if (_counter >= EyeARConsecutiveFrames)
                    {
                        _counter = 0;
                        return true;
                    }
                    _counter = 0;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}