using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceRecoSystem
{
    public class ScrfdDetector : IDisposable
    {
        private readonly Net _net;
        private readonly int _inputWidth = 640;
        private readonly int _inputHeight = 640;
        private readonly float _confThreshold = 0.5f;
        private readonly float _nmsThreshold = 0.4f;

        private readonly int[] _strides = { 8, 16, 32, 64, 128 };
        private const int NumAnchors = 2;
        private readonly Dictionary<int, List<Point>> _anchorCenters = new();

        public ScrfdDetector(string modelPath)
        {
            _net = CvDnn.ReadNetFromOnnx(modelPath);
            if (_net == null || _net.Empty())
                throw new System.IO.FileNotFoundException("Không thể tải model SCRFD.", modelPath);

            GenerateAnchors();
        }

        private void GenerateAnchors()
        {
            foreach (var stride in _strides)
            {
                int featureMapWidth = _inputWidth / stride;
                int featureMapHeight = _inputHeight / stride;
                var centers = new List<Point>();
                for (int y = 0; y < featureMapHeight; y++)
                    for (int x = 0; x < featureMapWidth; x++)
                        for (int k = 0; k < NumAnchors; k++)
                            centers.Add(new Point(x, y));
                _anchorCenters[stride] = centers;
            }
        }

        private void PrintMatInfo(Mat m, string name)
        {
            try
            {
                Console.WriteLine($"[OUT] {name}: dims={m.Dims}, rows={m.Rows}, cols={m.Cols}, channels={m.Channels()}, total={m.Total()}, type={m.Type()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[OUT] {name}: cannot print info ({e.Message})");
            }
        }

        private float SafeGetFloat(Mat m, int flatIndex, int fallbackIndex = 0)
        {
            if (m == null || m.Empty() || m.Total() == 0) return 0f;
            long total = m.Total();
            if (flatIndex < 0) flatIndex = 0;
            if (flatIndex >= total) flatIndex = Math.Min((int)total - 1, fallbackIndex);

            try
            {
                if (m.Rows == 1 && m.Cols >= 1)
                    return m.At<float>(0, flatIndex);

                if (m.Cols == 1 && m.Rows >= 1)
                    return m.At<float>(flatIndex, 0);

                if (m.Dims >= 2)
                {
                    try
                    {
                        return m.At<float>(0, flatIndex);
                    }
                    catch { }
                }

                return m.At<float>(0, 0);
            }
            catch
            {
                try { return m.At<float>(0, 0); } catch { return 0f; }
            }
        }

        public Rect[] Detect(Mat frame)
        {
            if (frame == null || frame.Empty())
                return Array.Empty<Rect>();

            try
            {
                using var blob = CvDnn.BlobFromImage(frame, 1.0, new Size(_inputWidth, _inputHeight),
                    new Scalar(104, 117, 123), swapRB: false, crop: false);
                _net.SetInput(blob);

                var outNames = _net.GetUnconnectedOutLayersNames();
                Console.WriteLine("[SCRFD] model outputs count: " + outNames.Length);
                for (int i = 0; i < outNames.Length; i++)
                    Console.WriteLine($"[SCRFD] out[{i}] = {outNames[i]}");

                var outputs = new Mat[outNames.Length];
                var outputsList = new List<Mat>();
                for (int i = 0; i < outNames.Length; i++)
                {
                    try
                    {
                        Mat outMat = _net.Forward(outNames[i]);
                        outputsList.Add(outMat);
                        Console.WriteLine($"[SCRFD] Forwarded out[{i}] = {outNames[i]} -> mat added.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SCRFD] Forward fail for {outNames[i]}: {ex.Message}");
                        outputsList.Add(new Mat());
                    }
                }

                for (int i = 0; i < outputs.Length; i++)
                    PrintMatInfo(outputs[i], outNames[i]);

                int expectedAnchors = _anchorCenters.Sum(kvp => kvp.Value.Count);
                Console.WriteLine($"[SCRFD] expected anchors total = {expectedAnchors}");

                var allBoxes = new List<Rect>();
                var allConf = new List<float>();

                float scaleX = (float)frame.Width / _inputWidth;
                float scaleY = (float)frame.Height / _inputHeight;

                int outIdx = 0;
                foreach (var stride in _strides)
                {
                    if (outIdx + 1 >= outputs.Length)
                    {
                        Console.WriteLine($"[SCRFD] outputs exhausted at stride {stride}. outIdx={outIdx}, outputs.Length={outputs.Length}");
                        break;
                    }

                    Mat scoreMat = outputs[outIdx++];
                    Mat bboxMat = outputs[outIdx++];

                    PrintMatInfo(scoreMat, $"score (stride {stride})");
                    PrintMatInfo(bboxMat, $"bbox  (stride {stride})");

                    var anchorCenters = _anchorCenters[stride];
                    int anchorsCount = anchorCenters.Count;
                    Console.WriteLine($"[SCRFD] stride {stride} anchorsCount={anchorsCount}, scoreMat.total={scoreMat.Total()}, bboxMat.total={bboxMat.Total()}");

                    if (scoreMat.Total() < anchorsCount || bboxMat.Total() < anchorsCount * 4)
                    {
                        Console.WriteLine($"[SCRFD] WARNING: mismatch sizes at stride {stride}. scoreMat.Total={scoreMat.Total()} bboxMat.Total={bboxMat.Total()} expected anchors*4={anchorsCount * 4}");
                    }

                    int loopCount = Math.Min(anchorsCount, (int)scoreMat.Total());
                    for (int i = 0; i < loopCount; i++)
                    {
                        float conf = SafeGetFloat(scoreMat, i);
                        if (conf < _confThreshold) continue;

                        var anchor = anchorCenters[i];

                        float dx = 0, dy = 0, dw = 0, dh = 0;
                        try
                        {
                            if (bboxMat.Rows == 1 && bboxMat.Cols >= (i * 4 + 4))
                            {
                                dx = bboxMat.At<float>(0, i * 4 + 0) * stride;
                                dy = bboxMat.At<float>(0, i * 4 + 1) * stride;
                                dw = (float)Math.Exp(bboxMat.At<float>(0, i * 4 + 2)) * stride;
                                dh = (float)Math.Exp(bboxMat.At<float>(0, i * 4 + 3)) * stride;
                            }
                            else if (bboxMat.Channels() >= 4 && bboxMat.Total() >= anchorsCount)
                            {
                                dx = bboxMat.At<float>(i, 0) * stride;
                                dy = bboxMat.At<float>(i, 1) * stride;
                                dw = (float)Math.Exp(bboxMat.At<float>(i, 2)) * stride;
                                dh = (float)Math.Exp(bboxMat.At<float>(i, 3)) * stride;
                            }
                            else
                            {
                                dx = SafeGetFloat(bboxMat, i * 4 + 0) * stride;
                                dy = SafeGetFloat(bboxMat, i * 4 + 1) * stride;
                                dw = (float)Math.Exp(SafeGetFloat(bboxMat, i * 4 + 2)) * stride;
                                dh = (float)Math.Exp(SafeGetFloat(bboxMat, i * 4 + 3)) * stride;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"[SCRFD] bbox read error at stride {stride}, anchor {i}: {e.Message}");
                            continue;
                        }

                        float cx = anchor.X * stride + dx;
                        float cy = anchor.Y * stride + dy;

                        int left = (int)((cx - dw * 0.5f) * scaleX);
                        int top = (int)((cy - dh * 0.5f) * scaleY);
                        int width = (int)(dw * scaleX);
                        int height = (int)(dh * scaleY);

                        allBoxes.Add(new Rect(left, top, width, height));
                        allConf.Add(conf);
                    }
                }

                foreach (var m in outputs) m?.Dispose();

                if (allBoxes.Count == 0) return Array.Empty<Rect>();

                CvDnn.NMSBoxes(allBoxes, allConf, _confThreshold, _nmsThreshold, out int[] indices);
                var res = new List<Rect>();
                foreach (var i in indices)
                {
                    var box = allBoxes[i];
                    box.X = Math.Max(0, box.X);
                    box.Y = Math.Max(0, box.Y);
                    box.Width = Math.Min(frame.Width - box.X, box.Width);
                    box.Height = Math.Min(frame.Height - box.Y, box.Height);
                    if (box.Width > 10 && box.Height > 10) res.Add(box);
                }
                return res.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SCRFD] Detect error: {ex.Message}");
                return Array.Empty<Rect>();
            }
        }

        public void Dispose() => _net?.Dispose();
    }
}
