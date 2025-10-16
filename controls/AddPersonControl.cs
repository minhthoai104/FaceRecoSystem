using FaceRecoSystem.core;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class AddPersonControl : UserControl
    {
        private readonly PersonManager _personMgr;
        private List<Mat> _capturedImagePaths;
        public AddPersonControl(PersonManager personMgr)
        {
            InitializeComponent();
            SetupLayout();
            _personMgr = personMgr;
        }
        private string GenerateUserID()
        {
            string datePart = DateTime.Now.ToString("yyMMdd");
            string randomPart = new Random().Next(10, 99).ToString();
            return $"NV{datePart}{randomPart}";
        }
        private void SetupLayout()
        {
            this.Controls.Clear();
            var mainCenteringLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                AutoScroll = true
            };
            mainCenteringLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainCenteringLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mainCenteringLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.Controls.Add(mainCenteringLayout);
            var contentPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                MaximumSize = new System.Drawing.Size(800, 0),
                Padding = new Padding(20),
                BackColor = Color.White
            };
            mainCenteringLayout.Controls.Add(contentPanel, 1, 0);
            lblTitle = new Label
            {
                Text = "THÊM NHÂN VIÊN MỚI",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Width = 760,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 30),
            };
            contentPanel.Controls.Add(lblTitle);
            var infoGrid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true,
                MinimumSize = new System.Drawing.Size(660, 0),
                Margin = new Padding(50, 0, 50, 0)
            };
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            lblName = new Label { Text = "Họ và tên:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtName = new TextBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblName, 0, 0);
            infoGrid.Controls.Add(txtName, 1, 0);

            lblAge = new Label { Text = "Tuổi:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtAge = new TextBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblAge, 0, 1);
            infoGrid.Controls.Add(txtAge, 1, 1);
            txtAge.TextChanged += TxtAge_TextChanged;
            lblGender = new Label { Text = "Giới tính:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            cbGender = new ComboBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(5, 5, 0, 10) };
            cbGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            infoGrid.Controls.Add(lblGender, 0, 2);
            infoGrid.Controls.Add(cbGender, 1, 2);

            lblAddress = new Label { Text = "Địa chỉ:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtAddress = new TextBox { Font = new Font("Segoe UI", 12), Dock = DockStyle.Fill, Multiline = true, Height = 90, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblAddress, 0, 3);
            infoGrid.Controls.Add(txtAddress, 1, 3);

            contentPanel.Controls.Add(infoGrid);

            var faceSectionHeader = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 20, 0, 15)
            };
            faceSectionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            faceSectionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var lblFaceHeader = new Label
            {
                Text = "ẢNH KHUÔN MẶT",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            btnStartCapture = new Button
            {
                Text = "📸 Bắt đầu chụp",
                BackColor = Color.FromArgb(63, 114, 175),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new System.Drawing.Size(200, 40),
                Anchor = AnchorStyles.Right
            };
            btnStartCapture.FlatAppearance.BorderSize = 0;
            btnStartCapture.Click += btnStartCapture_Click;

            faceSectionHeader.Controls.Add(lblFaceHeader, 0, 0);
            faceSectionHeader.Controls.Add(btnStartCapture, 1, 0);

            contentPanel.Controls.Add(faceSectionHeader);

            var picturesPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Width = 760,
                Padding = new Padding(50, 0, 50, 0)
            };

            picFront = CreateFaceBox();
            lblFront = CreateFaceLabel("Chính diện");

            picLeft = CreateFaceBox();
            lblLeft = CreateFaceLabel("Nghiêng trái");

            picRight = CreateFaceBox();
            lblRight = CreateFaceLabel("Nghiêng phải");

            var faces = new (PictureBox pic, Label lbl)[]
            {
                (picFront, lblFront),
                (picLeft, lblLeft),
                (picRight, lblRight)
            };

            for (int i = 0; i < faces.Length; i++)
            {
                int index = i;
                var card = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    Size = new System.Drawing.Size(200, 230),
                    Margin = new Padding(10),
                    BackColor = Color.White,
                    Padding = new Padding(5)
                };

                var pic = faces[index].pic;
                pic.Size = new System.Drawing.Size(180, 180);
                pic.Margin = new Padding(10, 0, 10, 0);
                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Dock = DockStyle.Fill;

                var lbl = faces[index].lbl;
                lbl.Font = new Font("Segoe UI", 11);
                lbl.AutoSize = false;
                lbl.Width = 200;
                lbl.Height = 25;
                lbl.TextAlign = ContentAlignment.MiddleCenter;

                var btnDelete = new Button
                {
                    Text = "❌",
                    BackColor = Color.FromArgb(220, 50, 50),
                    ForeColor = Color.White,
                    Size = new System.Drawing.Size(28, 28),
                    FlatStyle = FlatStyle.Flat
                };
                btnDelete.FlatAppearance.BorderSize = 0;

                var picPanel = new Panel
                {
                    Size = new System.Drawing.Size(180, 180),
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                picPanel.Controls.Add(pic);
                picPanel.Controls.Add(btnDelete);

                btnDelete.Location = new System.Drawing.Point(picPanel.Width - btnDelete.Width - 4, 4);
                btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnDelete.BringToFront();

                pic.Click += (s, e) =>
                {
                    using (var ofd = new OpenFileDialog { Filter = "Ảnh (*.jpg;*.png)|*.jpg;*.png" })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                                {
                                    var temp = Image.FromStream(fs);
                                    var clone = new Bitmap(temp);
                                    temp.Dispose();

                                    pic.Image?.Dispose();
                                    pic.Image = clone;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Không thể mở ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                };

                btnDelete.Click += (s, e) =>
                {
                    if (pic.Image == null)
                    {
                        MessageBox.Show("Chưa có hình để xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        pic.Image.Dispose();
                        pic.Image = null;
                    }
                };

                card.Controls.Add(picPanel);
                card.Controls.Add(lbl);
                picturesPanel.Controls.Add(card);

                faces[index].pic.Click += (s, e) =>
                {
                    using (var ofd = new OpenFileDialog { Filter = "Ảnh (*.jpg;*.png)|*.jpg;*.png" })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            faces[index].pic.Image = Image.FromFile(ofd.FileName);
                        }
                    }
                };
            }
            contentPanel.Controls.Add(picturesPanel);

            btnSave = new Button
            {
                Text = "💾 Lưu thông tin",
                BackColor = Color.FromArgb(46, 160, 67),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new System.Drawing.Size(200, 45),
                Margin = new Padding(0, 30, 0, 20)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += btnSave_Click;

            var saveButtonPanel = new FlowLayoutPanel { Width = 760, AutoSize = true };
            saveButtonPanel.Controls.Add(btnSave);
            saveButtonPanel.Padding = new Padding((760 - btnSave.Width) / 2, 0, 0, 0);

            contentPanel.Controls.Add(saveButtonPanel);
        }

        private PictureBox CreateFaceBox()
        {
            return new PictureBox
            {
                Size = new System.Drawing.Size(150, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.WhiteSmoke
            };
        }

        private Label CreateFaceLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            };
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _capturedImagePaths = _personMgr.CaptureThreeAngles(name);

            if (_capturedImagePaths != null && _capturedImagePaths.Count == 3)
            {
                picFront.Image?.Dispose();
                picLeft.Image?.Dispose();
                picRight.Image?.Dispose();

                picFront.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[0]);
                picLeft.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[1]);
                picRight.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[2]);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string gender = cbGender.Text;
            string address = txtAddress.Text.Trim();

            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi là một con số hợp lệ!", "Lỗi Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (age < 16 || age > 60)
            {
                MessageBox.Show("Tuổi của nhân viên phải từ 16 đến 60.", "Tuổi không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_capturedImagePaths == null || _capturedImagePaths.Count < 3)
            {
                MessageBox.Show("Vui lòng chụp đủ 3 góc khuôn mặt trước khi lưu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string userId = GenerateUserID();

                byte[] frontVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[0]);
                byte[] leftVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[1]);
                byte[] rightVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[2]);

                if (frontVec == null || leftVec == null || rightVec == null)
                {
                    MessageBox.Show("Không thể trích xuất đặc trưng khuôn mặt từ ảnh. Vui lòng chụp lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2️⃣ Chuyển ảnh Mat sang byte[] để lưu gốc
                byte[] frontImg, leftImg, rightImg;
                using (var ms1 = new MemoryStream())
                using (var ms2 = new MemoryStream())
                using (var ms3 = new MemoryStream())
                {
                    var bmpFront = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[0]);
                    var bmpLeft = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[1]);
                    var bmpRight = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_capturedImagePaths[2]);

                    bmpFront.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bmpLeft.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bmpRight.Save(ms3, System.Drawing.Imaging.ImageFormat.Jpeg);

                    frontImg = ms1.ToArray();
                    leftImg = ms2.ToArray();
                    rightImg = ms3.ToArray();
                }

                // 3️⃣ Tạo đối tượng User
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

                // 4️⃣ Lưu vào DB
                bool result = _personMgr.AddNewUser(newUser, _capturedImagePaths);

                if (result)
                {
                    MessageBox.Show($"Đã lưu nhân viên {name} thành công!\nMã NV: {userId}",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset giao diện
                    txtName.Clear();
                    txtAge.Clear();
                    txtAddress.Clear();
                    cbGender.SelectedIndex = -1;

                    picFront.Image?.Dispose();
                    picLeft.Image?.Dispose();
                    picRight.Image?.Dispose();

                    _capturedImagePaths.ForEach(m => m.Dispose());
                    _capturedImagePaths = null;
                }
                else
                {
                    MessageBox.Show("Không thể lưu nhân viên. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtAge_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtAge.Text, out int age))
            {
                txtAge.ForeColor = (age < 16 || age > 60) ? Color.Red : Color.Black;
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
        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Contains("Front")) ClearPictureBoxImage(picFront);
            else if (btn.Name.Contains("Left")) ClearPictureBoxImage(picLeft);
            else if (btn.Name.Contains("Right")) ClearPictureBoxImage(picRight);
        }
        private void ClearPictureBoxImage(PictureBox pb)
        {
            if (pb?.Image != null)
            {
                pb.Image.Dispose();
                pb.Image = null;
            }
        }
        private void btnRefreshImages_Click(object sender, EventArgs e)
        {
            ClearAllFacePictures();
        }
        private void ClearAllFacePictures()
        {
            ClearPictureBoxImage(picFront);
            ClearPictureBoxImage(picLeft);
            ClearPictureBoxImage(picRight);
            _capturedImagePaths = null;
            MessageBox.Show("Đã xóa ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
