using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class AddPersonControl : UserControl
    {
        private readonly PersonManager _personMgr;
        private string[] _capturedImagePaths;
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
            // Xóa hết các control cũ để dựng lại từ đầu
            this.Controls.Clear();
            //this.BackColor = Color.FromArgb(245, 245, 245);

            // ===== 1. Bố cục chính để CĂN GIỮA toàn bộ nội dung =====
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

            // ===== 2. Panel chính chứa TOÀN BỘ nội dung =====
            var contentPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                MaximumSize = new Size(800, 0),
                Padding = new Padding(20),
                BackColor = Color.White
            };
            mainCenteringLayout.Controls.Add(contentPanel, 1, 0);

            // ===== 3. Tiêu đề chính =====
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


            // ===== 4. Vùng nhập thông tin cá nhân =====
            var infoGrid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 4,
                // *** THAY ĐỔI CUỐI CÙNG: Dùng AutoSize và MinimumSize ***
                AutoSize = true,                  // Cho phép tự tính chiều cao
                MinimumSize = new Size(660, 0),   // Nhưng chiều ngang không được co lại
                Margin = new Padding(50, 0, 50, 0)
            };
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // -- Dòng 1: Họ và tên --
            lblName = new Label { Text = "Họ và tên:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtName = new TextBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblName, 0, 0);
            infoGrid.Controls.Add(txtName, 1, 0);

            // -- Dòng 2: Tuổi --
            lblAge = new Label { Text = "Tuổi:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtAge = new TextBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblAge, 0, 1);
            infoGrid.Controls.Add(txtAge, 1, 1);
            txtAge.TextChanged += TxtAge_TextChanged;
            // -- Dòng 3: Giới tính --
            lblGender = new Label { Text = "Giới tính:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            cbGender = new ComboBox { Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(5, 5, 0, 10) };
            cbGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            infoGrid.Controls.Add(lblGender, 0, 2);
            infoGrid.Controls.Add(cbGender, 1, 2);

            // -- Dòng 4: Địa chỉ --
            lblAddress = new Label { Text = "Địa chỉ:", Font = new Font("Segoe UI", 12), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            txtAddress = new TextBox { Font = new Font("Segoe UI", 12), Dock = DockStyle.Fill, Multiline = true, Height = 90, Margin = new Padding(5, 5, 0, 10) };
            infoGrid.Controls.Add(lblAddress, 0, 3);
            infoGrid.Controls.Add(txtAddress, 1, 3);

            contentPanel.Controls.Add(infoGrid);

            // ===== 5. Vùng ảnh khuôn mặt =====
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
                Size = new Size(200, 40),
                Anchor = AnchorStyles.Right
            };
            btnStartCapture.FlatAppearance.BorderSize = 0;
            btnStartCapture.Click += btnStartCapture_Click;

            faceSectionHeader.Controls.Add(lblFaceHeader, 0, 0);
            faceSectionHeader.Controls.Add(btnStartCapture, 1, 0);

            contentPanel.Controls.Add(faceSectionHeader);

            // -- Vùng chứa 3 ảnh --
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
                    Size = new Size(200, 230),
                    Margin = new Padding(10),
                    BackColor = Color.White,
                    Padding = new Padding(5)
                };

                var pic = faces[index].pic;
                pic.Size = new Size(180, 180);
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
                    Size = new Size(28, 28),
                    FlatStyle = FlatStyle.Flat
                };
                btnDelete.FlatAppearance.BorderSize = 0;

                var picPanel = new Panel
                {
                    Size = new Size(180, 180),
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                picPanel.Controls.Add(pic);
                picPanel.Controls.Add(btnDelete);

                btnDelete.Location = new Point(picPanel.Width - btnDelete.Width - 4, 4);
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

            // ===== 6. Nút LƯU cuối cùng =====
            btnSave = new Button
            {
                Text = "💾 Lưu thông tin",
                BackColor = Color.FromArgb(46, 160, 67),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
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
                Size = new Size(150, 150),
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

        private Button CreateDeleteButton(PictureBox targetPic)
        {
            var btn = new Button
            {
                Text = "❌",
                BackColor = Color.LightGray,
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat
            };
            btn.Click += (s, e) =>
            {
                targetPic.Image?.Dispose();
                targetPic.Image = null;
            };
            return btn;
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            if (txtName == null || txtAge == null || txtAddress == null || cbGender == null)
            {
                MessageBox.Show("Các control chưa được khởi tạo!");
                return;
            }
            if (_personMgr == null)
            {
                MessageBox.Show("_personMgr chưa được khởi tạo!");
                return;
            }
            if (picFront == null || picLeft == null || picRight == null)
            {
                MessageBox.Show("Các PictureBox chưa được khởi tạo!");
                return;
            }

            string name = txtName.Text.Trim();
            int.TryParse(txtAge.Text, out int age);
            string gender = cbGender.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _capturedImagePaths = _personMgr.CaptureFacesOnly(name, age, gender, address, true);

            if (_capturedImagePaths != null && _capturedImagePaths.Length == 3)
            {
                picFront.Image = Image.FromFile(_capturedImagePaths[0]);
                picLeft.Image = Image.FromFile(_capturedImagePaths[1]);
                picRight.Image = Image.FromFile(_capturedImagePaths[2]);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string gender = cbGender.Text;
            string address = txtAddress.Text.Trim();

            // === BẮT ĐẦU KHỐI LỆNH KIỂM TRA TUỔI ===

            // 1. Kiểm tra xem người dùng có nhập đúng là số không
            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi là một con số hợp lệ!",
                                "Lỗi Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng lại, không thực hiện tiếp
            }

            // 2. Nếu đúng là số, kiểm tra khoảng giá trị (16-60)
            if (age < 16 || age > 60)
            {
                MessageBox.Show("Tuổi của nhân viên phải nằm trong khoảng từ 16 đến 60.",
                                "Tuổi không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng lại, không thực hiện tiếp
            }

            // === KẾT THÚC KHỐI LỆNH KIỂM TRA TUỔI ===

            // Kiểm tra các thông tin bắt buộc khác
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_capturedImagePaths == null || _capturedImagePaths.Length < 3)
            {
                MessageBox.Show("Vui lòng chụp đủ 3 góc khuôn mặt trước khi lưu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1️⃣ Sinh mã nhân viên
                string userId = GenerateUserID();

                // 2️⃣ Lấy vector khuôn mặt
                byte[] frontVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[0]);
                byte[] leftVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[1]);
                byte[] rightVec = _personMgr.GetFaceEncodingAsBytes(_capturedImagePaths[2]);

                if (frontVec == null || leftVec == null || rightVec == null)
                {
                    MessageBox.Show("Không thể trích xuất đặc trưng khuôn mặt từ ảnh. Vui lòng chụp lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3️⃣ Lưu nhân viên vào SQL (sử dụng biến 'age' đã được kiểm tra ở trên)
                bool result = _personMgr.SavePersonToDatabase(userId, name, age, gender, address, frontVec, leftVec, rightVec);

                // 4️⃣ Thông báo kết quả
                if (result)
                {
                    MessageBox.Show($"✅ Đã lưu nhân viên {name} thành công!\nMã NV: {userId}",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form
                    txtName.Clear();
                    txtAge.Clear();
                    txtAddress.Clear();
                    cbGender.SelectedIndex = -1;
                    // Giả sử bạn có 3 PictureBox tên là picFront, picLeft, picRight
                    if (picFront != null) picFront.Image = null;
                    if (picLeft != null) picLeft.Image = null;
                    if (picRight != null) picRight.Image = null;
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
                txtAge.ForeColor = Color.Red; // Báo lỗi nếu nhập không phải là số
            }
            else
            {
                txtAge.ForeColor = Color.Black; // Trả về màu đen nếu ô trống
            }
        }
        private void btnDeleteFront_Click(object sender, EventArgs e)
        {
            picFront.Image?.Dispose();
            picFront.Image = null;
        }

        private void btnDeleteLeft_Click(object sender, EventArgs e)
        {
            picLeft.Image?.Dispose();
            picLeft.Image = null;
        }

        private void btnDeleteRight_Click(object sender, EventArgs e)
        {
            picRight.Image?.Dispose();
            picRight.Image = null;
        }

    }
}
