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
        private const double EyeARThreshold = 0.22; // ngưỡng nhắm mắt
        private const int EyeARConsecutiveFrames = 2; // số frame nhắm liên tiếp

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

        /// <summary>
        /// Phát hiện chớp mắt từ các điểm mốc khuôn mặt (landmarks)
        /// </summary>
        public bool Detect(IDictionary<FacePart, IEnumerable<FaceRecognitionDotNet.Point>> landmarks)
        {
            try
            {
                // Lấy mốc mắt trái/phải và chuyển sang kiểu OpenCV Point
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
                        return true; // 👁️ chớp mắt hoàn tất
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