using OpenCvSharp;
using System;
using System.IO;

namespace FaceRecoSystem.core
{
    public class HaarDetector : IDisposable
    {
        private readonly CascadeClassifier _faceCascade;

        public HaarDetector(string cascadePath)
        {
            if (!System.IO.File.Exists(cascadePath))
                throw new FileNotFoundException("Không tìm thấy file cascade: " + cascadePath);

            _faceCascade = new CascadeClassifier(cascadePath);
        }

        public Rect[] Detect(Mat frame)
        {
            if (frame == null || frame.Empty())
                return Array.Empty<Rect>();

            using var gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            var faces = _faceCascade.DetectMultiScale(
                gray,
                scaleFactor: 1.1,
                minNeighbors: 5,
                flags: HaarDetectionTypes.ScaleImage,
                minSize: new OpenCvSharp.Size(60, 60)
            );

            return faces;
        }

        public void Dispose() => _faceCascade?.Dispose();
    }
}
