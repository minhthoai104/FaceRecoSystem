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
        public event EventHandler CloseRequested;

        private readonly PersonManager _personManager;
        private readonly FaceRecognition _fr;
        private readonly FaceDatabase _db;
        private CameraService _cameraService;
        private string _currentPersonName;

        // Constructor mặc định cho Designer
        public UpdatePersonControl()
        {
            InitializeComponent();
        }

        // Constructor chính được Form1 gọi
        public UpdatePersonControl(PersonManager personManager, FaceRecognition fr, FaceDatabase db)
        {
            InitializeComponent();
            _personManager = personManager;
            _fr = fr;
            _db = db;

            // --- Thiết lập ComboBox ---
            cbGender.Items.Clear();
            cbGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cbGender.DropDownStyle = ComboBoxStyle.DropDownList;

            // --- Style các nút ---
            StyleButton(btnUpdate, Color.FromArgb(39, 174, 96));
            StyleButton(btnCapture, Color.FromArgb(52, 152, 219));

            // THAY ĐỔI Ở ĐÂY: Xóa 3 nút cũ, thêm 1 nút mới
            StyleButton(btnDeleteImages, Color.FromArgb(231, 76, 60));

            // --- Gắn sự kiện nút Đóng/Back ---
            this.btnClose.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Phương thức public được Form1 gọi để tải dữ liệu của người dùng được chọn.
        /// </summary>
        public void LoadUserForUpdate(User user)
        {
            if (user == null)
            {
                MessageBox.Show("Không thể tải thông tin người dùng (user is null).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 1. Lưu tên (khóa chính) và điền thông tin
            _currentPersonName = user.FullName;
            txtName.Text = user.FullName;
            txtName.ReadOnly = true;
            txtAge.Text = user.Age.ToString();
            cbGender.SelectedItem = user.Gender;
            txtAddress.Text = user.Address;

            // 2. Tải 3 ảnh từ database
            picAngle1.Image = BytesToImage(user.FaceFront); // Chính diện
            picAngle2.Image = BytesToImage(user.FaceLeft);  // Góc trái
            picAngle3.Image = BytesToImage(user.FaceRight); // Góc phải

            lblStatus.Text = $"Đã tải thông tin cho: {user.FullName}";
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

        private async void btnCapture_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPersonName))
            {
                MessageBox.Show("Không thể chụp ảnh khi chưa tải thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cameraService == null)
            {
                _cameraService = new CameraService(_fr, _db);
            }
            
            lblStatus.Text = "📸 Chuẩn bị chụp 3 góc... Vui lòng làm theo hướng dẫn.";
            await Task.Delay(300);

            var faces = _personManager.CaptureThreeAngles(_currentPersonName);
            if (faces == null || faces.Count < 3)
            {
                lblStatus.Text = "❌ Chụp ảnh thất bại. Không đủ 3 góc.";
                MessageBox.Show("Không chụp đủ 3 góc, vui lòng thử lại!", "Lỗi Chụp Ảnh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            picAngle1.Image?.Dispose();
            picAngle2.Image?.Dispose();
            picAngle3.Image?.Dispose();

            picAngle1.Image = new Bitmap(BitmapConverter.ToBitmap(faces[0]));
            picAngle2.Image = new Bitmap(BitmapConverter.ToBitmap(faces[1]));
            picAngle3.Image = new Bitmap(BitmapConverter.ToBitmap(faces[2]));

            lblStatus.Text = "✅ Đã chụp đủ 3 góc! (Chưa lưu)";

            foreach (var face in faces)
            {
                face.Dispose();
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPersonName))
            {
                MessageBox.Show("Vui lòng tải thông tin nhân viên trước khi cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 16 || age > 60)
            {
                MessageBox.Show("Tuổi phải là số và nằm trong khoảng 16–60!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "💾 Đang cập nhật...";
            await Task.Delay(200);

            try
            {
                var user = new User
                {
                    FullName = _currentPersonName,
                    Age = age,
                    Gender = cbGender.Text,
                    Address = txtAddress.Text,
                    FaceFront = ImageToByteArray(picAngle1.Image),
                    FaceLeft = ImageToByteArray(picAngle2.Image),
                    FaceRight = ImageToByteArray(picAngle3.Image)
                };

                using (var mat1 = picAngle1.Image != null ? BitmapConverter.ToMat(To24bppRgb(new Bitmap(picAngle1.Image))) : null)
                using (var mat2 = picAngle1.Image != null ? BitmapConverter.ToMat(To24bppRgb(new Bitmap(picAngle1.Image))) : null)
                using (var mat3 = picAngle1.Image != null ? BitmapConverter.ToMat(To24bppRgb(new Bitmap(picAngle1.Image))) : null)
                {
                    user.FaceFrontEncoding = (mat1 != null) ? _db.GetFaceEncodingAsBytes(mat1) : null;
                    user.FaceLeftEncoding = (mat2 != null) ? _db.GetFaceEncodingAsBytes(mat2) : null;
                    user.FaceRightEncoding = (mat3 != null) ? _db.GetFaceEncodingAsBytes(mat3) : null;
                }

                _db.UpdateUser(user);

                lblStatus.Text = "✅ Cập nhật thành công!";
                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Lỗi cập nhật!";
                MessageBox.Show("⚠️ Lỗi khi cập nhật: " + ex.Message, "Lỗi chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region "Hàm hỗ trợ Chuyển đổi Ảnh"
        private Bitmap To24bppRgb(Bitmap src)
        {
            if (src.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                return src;

            Bitmap bmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            }
            return bmp;
        }

        private DrawingImage BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    return DrawingImage.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi BytesToImage: {ex.Message}");
                return null;
            }
        }

        private byte[] ImageToByteArray(DrawingImage img)
        {
            if (img == null) return null;
            try
            {
                using (var bmp = new Bitmap(img))
                {
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ImageToByteArray: {ex.Message}");
                return null;
            }
        }

        #endregion

        private void btnDeleteImages_Click(object sender, EventArgs e)
        {
            picAngle1.Image?.Dispose();
            picAngle1.Image = null;

            picAngle2.Image?.Dispose();
            picAngle2.Image = null;

            picAngle3.Image?.Dispose();
            picAngle3.Image = null;

            lblStatus.Text = "Đã xóa 3 ảnh. (Chưa lưu)";
        }
    }
}
