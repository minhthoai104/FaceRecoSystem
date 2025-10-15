using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingImage = System.Drawing.Image;

namespace FaceRecoSystem.controls
{
    public partial class UpdatePersonControl : UserControl
    {
        private readonly PersonManager _personManager;
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private CameraService _cameraService;
        private string _currentPersonName;

        private const string PH_NAME = "Họ tên";
        private const string PH_AGE = "Tuổi";
        private const string PH_ADDRESS = "Địa chỉ";

        public UpdatePersonControl(PersonManager personManager, FaceRecognition fr, FaceDatabase db)
        {
            InitializeComponent();

            _personManager = personManager;
            _fr = fr;
            _db = db;

            SetupPlaceholders();

            this.Load += UpdatePersonControl_Load;

            StyleButton(btnLoadInfo, Color.FromArgb(52, 152, 219));
            StyleButton(btnUpdate, Color.FromArgb(39, 174, 96));
            StyleButton(btnCapture, Color.FromArgb(52, 152, 219));
            StyleButton(btnDeleteAngle1, Color.FromArgb(231, 76, 60));
            StyleButton(btnDeleteAngle2, Color.FromArgb(231, 76, 60));
            StyleButton(btnDeleteAngle3, Color.FromArgb(231, 76, 60));
        }

        private void UpdatePersonControl_Load(object sender, EventArgs e)
        {
            rbCaptureNo.Checked = true;

            panelInfo.Paint += Panel_PaintRounded;
            panelFace.Paint += Panel_PaintRounded;

            ApplyHoverEffect(btnLoadInfo);
            ApplyHoverEffect(btnUpdate);
            ApplyHoverEffect(btnCapture);

            txtAge.TextChanged += TxtAge_TextChanged;
        }

        #region Designer-Safe Panel & Button Helpers

        private void Panel_PaintRounded(object sender, PaintEventArgs e)
        {
            if (sender is Panel p)
            {
                using (GraphicsPath path = CreateRoundedRect(p.ClientRectangle, 20))
                using (Pen pen = new Pen(Color.LightGray, 2))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void StyleButton(Button btn, Color bg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = bg;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(bg);
            btn.MouseLeave += (s, e) => btn.BackColor = bg;
        }

        private void ApplyHoverEffect(Button btn) => StyleButton(btn, btn.BackColor);

        #endregion

        #region Placeholders

        private void SetupPlaceholders()
        {
            ConfigurePlaceholder(txtName, PH_NAME);
            ConfigurePlaceholder(txtAge, PH_AGE);
            ConfigurePlaceholder(txtAddress, PH_ADDRESS);

            if (cbGender.Items.Count > 0 && string.IsNullOrEmpty(cbGender.Text))
                cbGender.SelectedIndex = 0;
        }

        private void ConfigurePlaceholder(TextBox box, string placeholder)
        {
            if (box == null) return;
            box.ForeColor = Color.Gray;
            if (string.IsNullOrWhiteSpace(box.Text))
                box.Text = placeholder;

            box.GotFocus += (s, e) =>
            {
                if (box.Text == placeholder)
                {
                    box.Text = "";
                    box.ForeColor = Color.Black;
                }
            };
            box.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = placeholder;
                    box.ForeColor = Color.Gray;
                }
            };
        }

        #endregion

        #region Load & Update Person

        private void btnLoadInfo_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (name == PH_NAME) name = "";

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên để tải thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            picAngle1.Image?.Dispose();
            picAngle2.Image?.Dispose();
            picAngle3.Image?.Dispose();
            picAngle1.Image = picAngle2.Image = picAngle3.Image = null;

            try
            {
                using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT FullName, Age, Gender, Address, FaceFront, FaceLeft, FaceRight
                                     FROM Users WHERE FullName = @name";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtAge.Text = reader["Age"].ToString();
                                cbGender.Text = reader["Gender"].ToString();
                                txtAddress.Text = reader["Address"].ToString();
                                _currentPersonName = reader["FullName"].ToString();

                                picAngle1.Image = ReadImage(reader["FaceFront"]);
                                picAngle2.Image = ReadImage(reader["FaceLeft"]);
                                picAngle3.Image = ReadImage(reader["FaceRight"]);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy người này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message);
            }
        }

        private System.Drawing.Image ReadImage(object dbValue)
        {
            try
            {
                if (dbValue == DBNull.Value || dbValue == null)
                    return null;

                byte[] bytes = dbValue as byte[];
                if (bytes == null || bytes.Length < 100)
                    return null;

                using (var ms = new MemoryStream(bytes))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ảnh bị lỗi hoặc không hợp lệ: " + ex.Message,
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            if (image == null)
                return null;

            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var bmp = new Bitmap(image))
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển ảnh sang byte[]: " + ex.Message);
                return null;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPersonName))
            {
                MessageBox.Show("Vui lòng tải thông tin trước!");
                return;
            }
            string ageText = (txtAge.Text == PH_AGE) ? "0" : txtAge.Text;

            if (!int.TryParse(ageText, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi là một con số hợp lệ!", "Lỗi Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (age < 16 || age > 60)
            {
                MessageBox.Show("Tuổi của nhân viên phải nằm trong khoảng từ 16 đến 60.", "Tuổi không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var updatedPerson = new User
            {
                FullName = txtName.Text == PH_NAME ? "" : txtName.Text,
                Age = age,
                Gender = cbGender.Text,
                Address = txtAddress.Text == PH_ADDRESS ? "" : txtAddress.Text,
                FaceFrontEncoding = ImageToByteArray(picAngle1.Image),
                FaceLeftEncoding = ImageToByteArray(picAngle2.Image),
                FaceRightEncoding = ImageToByteArray(picAngle3.Image)
            };

            bool success = _personManager.UpdatePerson(updatedPerson);

            MessageBox.Show(success ? "Cập nhật thành công!" : "Lỗi cập nhật!");
        }

        private void TxtAge_TextChanged(object sender, EventArgs e)
        {
            if (txtAge.Text == PH_AGE)
            {
                txtAge.ForeColor = Color.Gray;
                return;
            }

            if (int.TryParse(txtAge.Text, out int age))
            {
                if (age < 16 || age > 60)
                {
                    txtAge.ForeColor = Color.Red;
                }
                else
                {
                    txtAge.ForeColor = Color.Black;
                }
            }
            else if (!string.IsNullOrEmpty(txtAge.Text))
            {
                txtAge.ForeColor = Color.Red;
            }
            else
            {
                txtAge.ForeColor = Color.Black;
            }
        }

        #endregion

        #region Camera

        private void rbCaptureYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCaptureYes.Checked)
            {
                _cameraService = new CameraService(_fr, _db);
                _cameraService.StartCamera(picPreview);
            }
            else
            {
                _cameraService?.StopCamera();
            }
        }

        private async void btnCapture_Click(object sender, EventArgs e)
        {
            if (_cameraService == null)
            {
                MessageBox.Show("Camera chưa bật!");
                return;
            }

            PictureBox[] angles = { picAngle1, picAngle2, picAngle3 };
            string[] instructions = { "Nhìn thẳng vào camera...", "Quay trái...", "Quay phải..." };

            for (int i = 0; i < angles.Length; i++)
            {
                if (angles[i].Image == null)
                {
                    for (int j = 3; j >= 1; j--)
                    {
                        lblStatus.Text = $"{instructions[i]} {j}...";
                        await Task.Delay(1000);
                    }

                    angles[i].Image = _cameraService.CaptureFrame();
                    lblStatus.Text = $"{instructions[i]} - Đã chụp!";
                    await Task.Delay(500);
                }
            }

            lblStatus.Text = "Đã chụp đủ các góc còn thiếu!";
        }

        private void btnDeleteAngle1_Click(object sender, EventArgs e)
        {
            picAngle1.Image?.Dispose();
            picAngle1.Image = null;
            lblStatus.Text = "Góc chính diện đã xóa, bấm 'Chụp ảnh' để chụp lại";
        }

        private void btnDeleteAngle2_Click(object sender, EventArgs e)
        {
            picAngle2.Image?.Dispose();
            picAngle2.Image = null;
            lblStatus.Text = "Góc trái đã xóa, bấm 'Chụp ảnh' để chụp lại";
        }

        private void btnDeleteAngle3_Click(object sender, EventArgs e)
        {
            picAngle3.Image?.Dispose();
            picAngle3.Image = null;
            lblStatus.Text = "Góc phải đã xóa, bấm 'Chụp ảnh' để chụp lại";
        }

        #endregion
    }
}
