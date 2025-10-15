using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class CameraControl : UserControl
    {
        private readonly CameraService _camService;
        private Thread _cameraThread;
        private bool _running;

        public CameraControl(CameraService camService)
        {
            _camService = camService ?? throw new ArgumentNullException(nameof(camService));
            InitializeComponent();
        }

        public void StartCamera()
        {
            if (_running) return;

            _running = true;
            _cameraThread = new Thread(() =>
            {
                while (_running)
                {
                    try
                    {
                        using (Bitmap frame = _camService.CaptureFrame())
                        {
                            if (frame == null)
                                continue;
                            pictureBox.Invoke((Action)(() =>
                            {
                                pictureBox.Image?.Dispose();
                                pictureBox.Image = (Bitmap)frame.Clone();
                            }));
                        }

                        Thread.Sleep(30);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Camera thread error: " + ex.Message);
                    }
                }
            })
            {
                IsBackground = true
            };

            _cameraThread.Start();
        }

        public void StopCamera()
        {
            _running = false;

            if (_cameraThread != null && _cameraThread.IsAlive)
            {
                _cameraThread.Join(300);
                _cameraThread = null;
            }

            // Dọn dẹp ảnh hiển thị
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopCamera();
            base.OnHandleDestroyed(e);
        }
    }
}
