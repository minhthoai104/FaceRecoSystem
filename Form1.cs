using FaceRecognitionDotNet;
using FaceRecoSystem.controls;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FaceRecoSystem
{
    public partial class Form1 : Form
    {
        private FaceRecognition _fr;
        private FaceDatabase _db;
        private CameraService _camService;
        private PersonManager _personMgr;
        private CascadeClassifier _faceDetector;

        private Button _activeButton;
        private Panel _activeBorderPanel;
        private PictureBox _cameraView;

        public Form1()
        {
            InitializeComponent();
            InitializeFaceSystem();
            InitButtonEvents();
        }

        private void InitializeFaceSystem()
        {
            // use the app base models folder (assumes models placed next to exe in "models")
            string modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");

            if (!Directory.Exists(modelDir))
            {
                MessageBox.Show($"❌ Không tìm thấy thư mục models.\nĐường dẫn đã kiểm tra: {modelDir}",
                                "Lỗi Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Create FaceRecognition (dlib) -- argument is models folder
                _fr = FaceRecognition.Create(modelDir);
                Thread.Sleep(200);

                _db = new FaceDatabase(_fr, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "faces_db"), DatabaseHelper.ConnectionString);
                _camService = new CameraService(_fr, _db); // optional: if used elsewhere
                _personMgr = new PersonManager(_fr, _db);

                // camera view setup (if you want to show raw camera elsewhere)
                _cameraView = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Visible = false
                };
                panelMain.Controls.Add(_cameraView);
            }
            catch (AccessViolationException)
            {
                MessageBox.Show("💥 Lỗi AccessViolationException — Dlib bị treo!\n" +
                                "Vui lòng kiểm tra lại file model .dat hoặc đảm bảo dự án đang chạy ở chế độ x64.",
                                "Lỗi Dlib", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"💥 Lỗi không xác định khi khởi tạo hệ thống:\n{ex.Message}",
                                "Lỗi Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitButtonEvents()
        {
            var buttons = panelMenu.Controls.OfType<Button>().ToList();
            var originalColor = Color.FromArgb(63, 114, 175);

            foreach (var btn in buttons)
            {
                btn.MouseEnter += (s, e) =>
                {
                    var currentBtn = (Button)s;
                    currentBtn.BackColor = Color.White;
                    currentBtn.ForeColor = originalColor;
                };

                btn.MouseLeave += (s, e) =>
                {
                    var currentBtn = (Button)s;
                    if (currentBtn != _activeButton)
                    {
                        currentBtn.BackColor = originalColor;
                        currentBtn.ForeColor = Color.White;
                    }
                };

                btn.Click += (s, e) => ActivateButton((Button)s);
            }

            _activeBorderPanel = new Panel
            {
                Size = new System.Drawing.Size(5, 60),
                BackColor = originalColor,
                Visible = false
            };
            panelMenu.Controls.Add(_activeBorderPanel);
        }

        private void ActivateButton(Button btn)
        {
            var originalColor = Color.FromArgb(63, 114, 175);

            if (_activeButton != null)
            {
                _activeButton.BackColor = originalColor;
                _activeButton.ForeColor = Color.White;
            }

            _activeButton = btn;
            _activeButton.BackColor = Color.White;
            _activeButton.ForeColor = originalColor;

            _activeBorderPanel.Visible = true;
            _activeBorderPanel.Location = new System.Drawing.Point(0, btn.Top);
            _activeBorderPanel.BringToFront();

            LoadPage(btn);
        }

        private void LoadPage(Button btn)
        {
            panelMain.Controls.Clear();
            _cameraView.Visible = false;

            UserControl page = null;

            switch (btn.Name)
            {
                case "btnAttendance":
                    page = new AttendanceControl(_fr, _db);
                    break;

                case "btnViewList":
                    page = new PersonListControl();
                    break;

                case "btnAddPerson":
                    page = new AddPersonControl(_personMgr);
                    break;

                case "btnUpdatePerson":
                    page = new UpdatePersonControl(_personMgr, _fr, _db);
                    break;

                case "btnDeletePerson":
                    page = new DeletePersonControl(_personMgr);
                    break;
            }

            if (page != null)
            {
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
            }
        }

        private void FaceReco_Load(object sender, EventArgs e)
        {
            ActivateButton(btnAttendance);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _camService?.StopCamera();
        }
    }
}
