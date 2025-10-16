using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Data.SqlClient;
using System.Drawing;
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

        public UpdatePersonControl(PersonManager personManager, FaceRecognition fr, FaceDatabase db)
        {
            InitializeComponent();
            _personManager = personManager;
            _fr = fr;
            _db = db;

            // Giới tính
            cbGender.Items.Clear();
            cbGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cbGender.DropDownStyle = ComboBoxStyle.DropDownList;

            StyleButton(btnLoadInfo, Color.FromArgb(52, 152, 219));
            StyleButton(btnUpdate, Color.FromArgb(39, 174, 96));
            StyleButton(btnCapture, Color.FromArgb(52, 152, 219));
            StyleButton(btnDeleteAngle1, Color.FromArgb(231, 76, 60));
            StyleButton(btnDeleteAngle2, Color.FromArgb(231, 76, 60));
            StyleButton(btnDeleteAngle3, Color.FromArgb(231, 76, 60));
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

        private void btnLoadInfo_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên để tải thông tin!", "Thông báo");
                return;
            }

            try
            {
                using var conn = new SqlConnection(DatabaseHelper.ConnectionString);
                conn.Open();
                string query = @"SELECT FullName, Age, Gender, Address, FaceFront, FaceLeft, FaceRight 
                                 FROM Users WHERE FullName = @name";
                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtAge.Text = reader["Age"].ToString();
                    cbGender.SelectedItem = reader["Gender"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                    _currentPersonName = reader["FullName"].ToString();

                    picAngle1.Image = ReadImage(reader["FaceFront"]);
                    picAngle2.Image = ReadImage(reader["FaceLeft"]);
                    picAngle3.Image = ReadImage(reader["FaceRight"]);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người này!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message);
            }
        }

        private DrawingImage ReadImage(object dbValue)
        {
            try
            {
                if (dbValue == DBNull.Value || dbValue == null) return null;
                var bytes = (byte[])dbValue;
                using var ms = new MemoryStream(bytes);
                using var tmp = DrawingImage.FromStream(ms);
                var bmp = new Bitmap(tmp.Width, tmp.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                using (var g = Graphics.FromImage(bmp))
                    g.DrawImage(tmp, 0, 0, bmp.Width, bmp.Height);
                return bmp;
            }
            catch
            {
                return null;
            }
        }

        private async void btnCapture_Click(object sender, EventArgs e)
        {
            if (_cameraService == null)
            {
                _cameraService = new CameraService(_fr, _db);
                _cameraService.StartCamera(null); // Không hiển thị preview
            }

            lblStatus.Text = "📸 Đang chụp ảnh...";
            await Task.Delay(300);

            var faces = _personManager.CaptureThreeAngles(txtName.Text.Trim());
            if (faces == null || faces.Count < 3)
            {
                lblStatus.Text = "❌ Không đủ ảnh!";
                MessageBox.Show("Không chụp đủ 3 góc, vui lòng thử lại!");
                return;
            }

            picAngle1.Image = new Bitmap(BitmapConverter.ToBitmap(faces[0]));
            picAngle2.Image = new Bitmap(BitmapConverter.ToBitmap(faces[1]));
            picAngle3.Image = new Bitmap(BitmapConverter.ToBitmap(faces[2]));

            lblStatus.Text = "✅ Đã chụp đủ 3 góc!";
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPersonName))
            {
                MessageBox.Show("Vui lòng tải thông tin trước!");
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 16 || age > 60)
            {
                MessageBox.Show("Tuổi phải nằm trong khoảng 16–60!");
                return;
            }

            lblStatus.Text = "💾 Đang cập nhật...";
            await Task.Delay(200);

            try
            {
                var user = new User
                {
                    FullName = txtName.Text.Trim(),
                    Age = age,
                    Gender = cbGender.Text,
                    Address = txtAddress.Text,
                    FaceFront = picAngle1.Image != null ? ImageToByteArray(new Bitmap(picAngle1.Image)) : null,
                    FaceLeft = picAngle2.Image != null ? ImageToByteArray(new Bitmap(picAngle2.Image)) : null,
                    FaceRight = picAngle3.Image != null ? ImageToByteArray(new Bitmap(picAngle3.Image)) : null
                };

                // Fix lỗi BMP 32bit -> ép sang BGR 3 kênh
                using var mat1 = picAngle1.Image != null ? BitmapConverter.ToMat(new Bitmap(picAngle1.Image)) : null;
                using var mat2 = picAngle2.Image != null ? BitmapConverter.ToMat(new Bitmap(picAngle2.Image)) : null;
                using var mat3 = picAngle3.Image != null ? BitmapConverter.ToMat(new Bitmap(picAngle3.Image)) : null;

                if (mat1 != null) user.FaceFrontEncoding = _personManager.GetFaceEncodingAsBytes(PersonManager.EnsureBgr(mat1));
                if (mat2 != null) user.FaceLeftEncoding = _personManager.GetFaceEncodingAsBytes(PersonManager.EnsureBgr(mat2));
                if (mat3 != null) user.FaceRightEncoding = _personManager.GetFaceEncodingAsBytes(PersonManager.EnsureBgr(mat3));

                bool ok = _personManager.Update(user);
                if (ok)
                {
                    lblStatus.Text = "✅ Cập nhật thành công!";
                    MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "❌ Thất bại!";
                    MessageBox.Show("Cập nhật thất bại! (Kiểm tra câu lệnh SQL)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Lỗi cập nhật: " + ex.ToString(), "Lỗi chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] ImageToByteArray(DrawingImage img)
        {
            if (img == null) return null;
            try
            {
                using (var clone = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                using (var g = Graphics.FromImage(clone))
                {
                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                    using (var ms = new MemoryStream())
                    {
                        clone.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển ảnh: " + ex.Message);
                return null;
            }
        }

        private void btnDeleteAngle1_Click(object sender, EventArgs e)
        {
            picAngle1.Image?.Dispose();
            picAngle1.Image = null;
            lblStatus.Text = "Đã xóa ảnh chính diện.";
        }

        private void btnDeleteAngle2_Click(object sender, EventArgs e)
        {
            picAngle2.Image?.Dispose();
            picAngle2.Image = null;
            lblStatus.Text = "Đã xóa ảnh góc trái.";
        }

        private void btnDeleteAngle3_Click(object sender, EventArgs e)
        {
            picAngle3.Image?.Dispose();
            picAngle3.Image = null;
            lblStatus.Text = "Đã xóa ảnh góc phải.";
        }
    }
}
