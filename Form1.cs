using FaceLibrary;
using FaceRecognitionDotNet;
using FaceRecoSystem.controls;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Data.SqlClient;
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
        private SqlConnection _sqlConnection;
        private Button _activeButton;
        private Panel _activeBorderPanel;
        private PictureBox _cameraView;

        public Form1()
        {
            _sqlConnection = new SqlConnection(DatabaseHelper.ConnectionString);
            InitializeComponent();
            InitializeFaceSystem();
            InitButtonEvents();
        }

        private void InitializeFaceSystem()
        {
            string modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");

            if (!Directory.Exists(modelDir))
            {
                MessageBox.Show($"Không tìm thấy thư mục models.\nĐường dẫn đã kiểm tra: {modelDir}",
                                "Lỗi Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _fr = FaceRecognition.Create(modelDir);
                Thread.Sleep(200);

                _db = new FaceDatabase(_fr, DatabaseHelper.ConnectionString);
                _camService = new CameraService(_fr, _db); _personMgr = new PersonManager(_fr, _db);

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
                MessageBox.Show("Lỗi AccessViolationException — Dlib bị treo!\n" +
                                "Vui lòng kiểm tra lại file model .dat hoặc đảm bảo dự án đang chạy ở chế độ x64.",
                                "Lỗi Dlib", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định khi khởi tạo hệ thống:\n{ex.Message}",
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
            foreach (Control ctrl in panelMain.Controls)
            {
                if (ctrl is AttendanceControl attendance)
                {
                    attendance.StopCamera();
                    Console.WriteLine("[Form1] Đã dừng camera khi rời tab Attendance.");
                }
            }
            panelMain.Controls.Clear();

            if (_cameraView != null)
                _cameraView.Visible = false;

            UserControl page = null;

            switch (btn.Name)
            {
                case "btnAttendance":
                    var attendancePage = new AttendanceControl(DatabaseHelper.ConnectionString);
                    attendancePage.RegisterRequested += AttendanceControl_RegisterRequested;
                    page = attendancePage;
                    break;

                case "btnViewAttendanceList":
                    page = new AttendanceList(_db);
                    break;

                case "btnViewEmpList":
                    var personList = new PersonListControl();
                    personList.InitializeDatabase(_db);

                    personList.AddPersonRequested += PersonList_AddPersonRequested;
                    personList.UpdateUserRequested += PersonList_UpdateUserRequested;

                    page = personList;
                    break;

                case "btnSettings":
                    page = new SettingControl();
                    break;
            }

            if (page != null)
            {
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
            }
        }

        private void PersonList_AddPersonRequested(object sender, EventArgs e)
        {
            if (_activeButton != null)
            {
                var originalColor = Color.FromArgb(63, 114, 175);
                _activeButton.BackColor = originalColor;
                _activeButton.ForeColor = Color.White;
            }
            _activeButton = null;
            _activeBorderPanel.Visible = false;

            panelMain.Controls.Clear();
            var addPage = new AddPersonControl(_personMgr);
            addPage.Dock = DockStyle.Fill;

            addPage.CancelRequested += GoBackToPersonList;

            panelMain.Controls.Add(addPage);
        }

        private void PersonList_UpdateUserRequested(object sender, UserEventArgs e)
        {
            if (_activeButton != null)
            {
                var originalColor = Color.FromArgb(63, 114, 175);
                _activeButton.BackColor = originalColor;
                _activeButton.ForeColor = Color.White;
            }
            _activeButton = null;
            _activeBorderPanel.Visible = false;

            panelMain.Controls.Clear();
            var updatePage = new UpdatePersonControl(_personMgr, _fr, _db);
            updatePage.Dock = DockStyle.Fill;

            updatePage.CloseRequested += GoBackToPersonList;

            panelMain.Controls.Add(updatePage);

            updatePage.LoadUserForUpdate(e.SelectedUser);
        }

        private void AttendanceControl_RegisterRequested(object sender, EventArgs e)
        {
            if (sender is AttendanceControl attendance)
            {
                attendance.StopCamera();
                Console.WriteLine("[Form1] Camera đã tắt khi chuyển sang trang Đăng ký.");
            }
            if (_activeButton != null)
            {
                var originalColor = Color.FromArgb(63, 114, 175);
                _activeButton.BackColor = originalColor;
                _activeButton.ForeColor = Color.White;
            }
            _activeButton = null;
            _activeBorderPanel.Visible = false;

            panelMain.Controls.Clear();
            var addPage = new AddPersonControl(_personMgr);
            addPage.Dock = DockStyle.Fill;

            addPage.CancelRequested += GoBackToAttendance;
            panelMain.Controls.Add(addPage);
        }

        private void GoBackToAttendance(object sender, EventArgs e)
        {
            var btnAttendance = panelMenu.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnAttendance");

            if (btnAttendance != null)
            {
                ActivateButton(btnAttendance);
            }
            else
            {
                if (panelMenu.Controls.OfType<Button>().Any())
                {
                    ActivateButton(panelMenu.Controls.OfType<Button>().First());
                }
            }
        }

        private void GoBackToPersonList(object sender, EventArgs e)
        {
            var btnList = panelMenu.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnViewEmpList");

            if (btnList != null)
            {
                ActivateButton(btnList);
            }
            else
            {
                GoBackToAttendance(sender, e);
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