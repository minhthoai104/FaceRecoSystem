using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class AddPersonControl : UserControl
    {
        public event EventHandler CancelRequested;
        private readonly PersonManager _personMgr;
        private List<Mat> _capturedImages;
        private static readonly Random _random = new Random();
        public AddPersonControl(PersonManager personMgr)
        {
            InitializeComponent();

            _personMgr = personMgr;
            _capturedImages = new List<Mat>();

            // Gắn sự kiện cho các nút từ giao diện
            this.btnStartCapture.Click += new System.EventHandler(this.btnStartCapture_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.txtAge.TextChanged += new System.EventHandler(this.TxtAge_TextChanged);
            this.btnDeleteImages.Click += new System.EventHandler(this.btnDeleteImages_Click); // <-- THÊM DÒNG NÀY
        }

        private string GenerateUserID()
        {
            string randomPart = _random.Next(99).ToString("D2");
            return $"NV{DateTime.Now:yyMMdd}{randomPart}";
        }

        private void btnDeleteImages_Click(object sender, EventArgs e)
        {
            if (_capturedImages == null || _capturedImages.Count == 0)
            {
                MessageBox.Show("Chưa có ảnh nào để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gọi hàm dọn dẹp ảnh đã có
            ClearAllFacePictures();
            MessageBox.Show("Đã xóa tất cả ảnh. Bạn có thể chụp lại.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hàm ClearAllFacePictures bạn đã có, đảm bảo nó tồn tại
        private void ClearAllFacePictures()
        {
            picThumbnailFront.Image?.Dispose();
            picThumbnailLeft.Image?.Dispose();
            picThumbnailRight.Image?.Dispose();
            picThumbnailFront.Image = null;
            picThumbnailLeft.Image = null;
            picThumbnailRight.Image = null;

            if (_capturedImages != null)
            {
                _capturedImages.ForEach(mat => mat.Dispose());
                _capturedImages.Clear();
            }
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập họ tên trước khi chụp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            // Gọi hàm chụp 3 góc mặt từ PersonManager (cửa sổ camera riêng sẽ hiện ra)
            var capturedMats = _personMgr.CaptureThreeAngles(name);

            if (capturedMats != null && capturedMats.Count == 3)
            {
                // Dọn dẹp ảnh cũ nếu có
                ClearAllFacePictures();
                _capturedImages = capturedMats;

                // Hiển thị ảnh đã chụp lên các thumbnail
                picThumbnailFront.Image = BitmapConverter.ToBitmap(_capturedImages[0]);
                picThumbnailLeft.Image = BitmapConverter.ToBitmap(_capturedImages[1]);
                picThumbnailRight.Image = BitmapConverter.ToBitmap(_capturedImages[2]);

                MessageBox.Show("Đã chụp thành công 3 góc mặt!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Quá trình chụp đã bị hủy hoặc thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string gender = cbGender.Text;
            string address = txtAddress.Text.Trim();

            // --- Validation (Kiểm tra dữ liệu) ---
            if (string.IsNullOrWhiteSpace(name) || cbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và Giới tính!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi là một con số hợp lệ!", "Lỗi Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (age < 18 || age > 60)
            {
                MessageBox.Show("Tuổi của nhân viên phải từ 18 đến 60.", "Tuổi không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_capturedImages == null || _capturedImages.Count < 3)
            {
                MessageBox.Show("Vui lòng chụp đủ 3 góc khuôn mặt trước khi lưu!", "Thiếu ảnh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Bắt đầu lưu ---
            try
            {
                string userId = GenerateUserID();

                byte[] frontVec = _personMgr.GetFaceEncodingAsBytes(_capturedImages[0]);
                byte[] leftVec = _personMgr.GetFaceEncodingAsBytes(_capturedImages[1]);
                byte[] rightVec = _personMgr.GetFaceEncodingAsBytes(_capturedImages[2]);

                if (frontVec == null || leftVec == null || rightVec == null)
                {
                    MessageBox.Show("Không thể trích xuất đặc trưng khuôn mặt từ một hoặc nhiều ảnh. Vui lòng chụp lại!", "Lỗi Encoding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] frontImg = _capturedImages[0].ToBytes(".jpg");
                byte[] leftImg = _capturedImages[1].ToBytes(".jpg");
                byte[] rightImg = _capturedImages[2].ToBytes(".jpg");

                var newUser = new User
                {
                    UserID = userId,
                    FullName = name,
                    Age = age,
                    Gender = gender,
                    Address = address,
                    FaceFront = frontImg,
                    FaceLeft = leftImg,
                    FaceRight = rightImg,
                    FaceFrontEncoding = frontVec,
                    FaceLeftEncoding = leftVec,
                    FaceRightEncoding = rightVec,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                bool result = _personMgr.AddNewUser(newUser, _capturedImages);

                if (result)
                {
                    MessageBox.Show($"Đã lưu nhân viên {name} thành công!\nMã NV: {userId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Không thể lưu nhân viên. Có lỗi xảy ra trong quá trình xử lý!", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nghiêm trọng khi lưu nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtName.Clear();
            txtAge.Clear();
            txtAddress.Clear();
            cbGender.SelectedIndex = -1;
            ClearAllFacePictures();
        }

        private void TxtAge_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtAge.Text, out int age))
            {
                txtAge.ForeColor = (age < 18 || age > 60) ? Color.Red : Color.Black;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtAge.Clear();
            txtAddress.Clear();
            cbGender.SelectedIndex = -1;
            ClearAllFacePictures();
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}