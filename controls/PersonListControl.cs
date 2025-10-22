using FaceRecoSystem.core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class PersonListControl : UserControl
    {
        public event EventHandler AddPersonRequested;
        public event EventHandler<UserEventArgs> UpdateUserRequested;

        private FaceDatabase _db;
        private List<User> _allUsers;

        private const string SearchPlaceholder = "Tìm kiếm theo tên hoặc ID...";
        private readonly Color _placeholderColor = Color.Gray;
        private readonly Color _textColor = Color.Black;

        // Thêm 2 biến để giữ icon
        private Image _editIcon;
        private Image _deleteIcon;

        public PersonListControl()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null, dgvUsers, new object[] { true });
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupSearchBoxPlaceholder();
        }

        public void InitializeDatabase(FaceDatabase db)
        {
            _db = db;
            SetupDataGridView();
            RefreshData();
        }

        public void RefreshData()
        {
            if (_db == null) return;
            try
            {
                SetupSearchBoxPlaceholder();
                dgvUsers.DataSource = null;
                _allUsers = _db.GetAllUsers();
                dgvUsers.DataSource = _allUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            // --- Style Header (vẫn giữ màu tối) ---
            var hBackColor = Color.FromArgb(35, 40, 55);
            var hForeColor = Color.White;

            // --- Style Bảng (Light Theme) ---
            var pBackColor = Color.White; // Nền trắng (cho dòng 1, 3, 5...)
            var pAlternateBackColor = Color.FromArgb(249, 249, 249); // Nền xen kẽ (trắng xám rất nhạt)
            var pForeColor = Color.Black; // Chữ đen
            var gridColor = Color.FromArgb(220, 220, 220); // Lưới xám nhạt

            var pSelectionBackColor = Color.FromArgb(0, 120, 215); // Hover xanh
            var pSelectionForeColor = Color.White; // Chữ trắng khi hover

            // --- Áp dụng Style ---
            dgvUsers.BackgroundColor = pBackColor;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.GridColor = gridColor;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.EnableHeadersVisualStyles = false;

            // Header (Giữ nguyên)
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = hBackColor;
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = hForeColor;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 45;
            dgvUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvUsers.DefaultCellStyle.BackColor = pBackColor; // Nền trắng
            dgvUsers.DefaultCellStyle.ForeColor = pForeColor; // Chữ đen
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvUsers.DefaultCellStyle.SelectionBackColor = pSelectionBackColor; // Hover xanh
            dgvUsers.DefaultCellStyle.SelectionForeColor = pSelectionForeColor; // Chữ trắng
            dgvUsers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Style Dòng Xen Kẽ (Alternating) - Dòng "Trần Minh Thoại"
            // Sửa lại dòng này để dùng màu sáng
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = pAlternateBackColor; // Nền xen kẽ (sáng)
            dgvUsers.AlternatingRowsDefaultCellStyle.ForeColor = pForeColor; // Chữ đen
            dgvUsers.AlternatingRowsDefaultCellStyle.SelectionBackColor = pSelectionBackColor; // Hover xanh
            dgvUsers.AlternatingRowsDefaultCellStyle.SelectionForeColor = pSelectionForeColor; // Chữ trắng

            // --- KẾT THÚC SỬA ---

            dgvUsers.RowTemplate.Height = 50;
            dgvUsers.Columns.Clear();
            dgvUsers.AutoGenerateColumns = false;

            // --- Tải Icon (giữ nguyên) ---
            try
            {
                string iconFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icons");
                string editIconPath = Path.Combine(iconFolder, "ic_edit.png");
                string deleteIconPath = Path.Combine(iconFolder, "ic_delete.png");

                if (File.Exists(editIconPath))
                    _editIcon = Image.FromFile(editIconPath);

                if (File.Exists(deleteIconPath))
                    _deleteIcon = Image.FromFile(deleteIconPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Không thể tải icon: {ex.Message}");
            }

            // --- Thêm Cột (giữ nguyên) ---
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "HỌ TÊN", Name = "colUser", DataPropertyName = "FullName", FillWeight = 35, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) } });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID NHÂN VIÊN", Name = "colId", DataPropertyName = "UserID", FillWeight = 25 });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TUỔI", Name = "colAge", DataPropertyName = "Age", FillWeight = 10 });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "GIỚI TÍNH", Name = "colGender", DataPropertyName = "Gender", FillWeight = 15 });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ĐỊA CHỈ", Name = "colAddress", DataPropertyName = "Address", FillWeight = 35, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) } });

            var actionColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "HÀNH ĐỘNG",
                Name = "colActions",
                FillWeight = 24
            };
            dgvUsers.Columns.Add(actionColumn);

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --- Đăng ký sự kiện (giữ nguyên) ---
            dgvUsers.CellPainting += DgvUsers_CellPainting;
            dgvUsers.CellClick += DgvUsers_CellClick;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddPersonRequested?.Invoke(this, EventArgs.Empty);
        }

        // THÊM PHƯƠNG THỨC NÀY để vẽ icon
        private void DgvUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Chỉ vẽ cột "colActions" và không vẽ header
            if (e.RowIndex < 0 || e.ColumnIndex != dgvUsers.Columns["colActions"].Index)
                return;

            // Không vẽ nếu không tải được icon
            if (_editIcon == null || _deleteIcon == null) return;

            // 1. Vẽ nền (để xử lý khi dòng được chọn)
            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground);

            // 2. Tính toán vị trí 2 icon
            int iconSize = 20; // Kích thước mong muốn của icon
            int paddingVertical = (e.CellBounds.Height - iconSize) / 2; // Căn giữa theo chiều dọc
            int paddingHorizontal = 10; // Khoảng cách giữa 2 icon

            // Tổng chiều rộng của 2 icon và khoảng cách
            int totalWidth = (iconSize * 2) + paddingHorizontal;
            // Vị trí X bắt đầu để căn giữa 2 icon trong cell
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;

            // 3. Xác định vùng vẽ cho từng icon
            Rectangle editRect = new Rectangle(
                startX,
                e.CellBounds.Top + paddingVertical,
                iconSize,
                iconSize);

            Rectangle deleteRect = new Rectangle(
                startX + iconSize + paddingHorizontal, // Nằm bên phải icon Sửa
                e.CellBounds.Top + paddingVertical,
                iconSize,
                iconSize);

            // 4. Vẽ 2 icon
            e.Graphics.DrawImage(_editIcon, editRect);
            e.Graphics.DrawImage(_deleteIcon, deleteRect);

            // 5. Báo cho DataGridView biết là ta đã tự vẽ xong
            e.Handled = true;
        }

        // CẬP NHẬT PHƯƠNG THỨC NÀY
        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _db == null) return;
            var user = dgvUsers.Rows[e.RowIndex].DataBoundItem as User;
            if (user == null) return;

            string colName = dgvUsers.Columns[e.ColumnIndex].Name;

            // Chỉ xử lý khi click vào cột "colActions"
            if (colName == "colActions")
            {
                // Lấy vị trí click (so với DataGridView)
                Point clickPos = dgvUsers.PointToClient(Cursor.Position);

                // Lấy vùng hiển thị của cell
                Rectangle cellBounds = dgvUsers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Tính toán lại vị trí 2 icon (giống hệt trong CellPainting)
                int iconSize = 20;
                int paddingVertical = (cellBounds.Height - iconSize) / 2;
                int paddingHorizontal = 10;
                int totalWidth = (iconSize * 2) + paddingHorizontal;
                int startX = cellBounds.Left + (cellBounds.Width - totalWidth) / 2;

                Rectangle editRect = new Rectangle(startX, cellBounds.Top + paddingVertical, iconSize, iconSize);
                Rectangle deleteRect = new Rectangle(startX + iconSize + paddingHorizontal, cellBounds.Top + paddingVertical, iconSize, iconSize);

                // Kiểm tra xem click trúng icon nào
                if (editRect.Contains(clickPos))
                {
                    // --- Click trúng icon SỬA ---
                    UpdateUserRequested?.Invoke(this, new UserEventArgs(user));
                }
                else if (deleteRect.Contains(clickPos))
                {
                    // --- Click trúng icon XÓA ---
                    var confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên '{user.FullName}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        // SỬA LỖI: Phương thức DeleteUser của bạn nhận "FullName", không phải "UserID"
                        _db.DeleteUser(user.FullName);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                }
            }
        }

        #region Search Box Placeholder Logic
        private void SetupSearchBoxPlaceholder()
        {
            txtSearch.Text = SearchPlaceholder;
            txtSearch.ForeColor = _placeholderColor;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == SearchPlaceholder)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = _textColor;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                SetupSearchBoxPlaceholder();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_allUsers == null || txtSearch.Text == SearchPlaceholder) return;

            string searchText = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                dgvUsers.DataSource = _allUsers;
            }
            else
            {
                var filteredList = _allUsers.Where(u =>
                    (u.FullName?.ToLower().Contains(searchText) ?? false) ||
                    (u.UserID?.ToLower().Contains(searchText) ?? false)
                ).ToList();
                dgvUsers.DataSource = filteredList;
            }
        }
        #endregion
    }

    public class UserEventArgs : EventArgs
    {
        public User SelectedUser { get; }
        public UserEventArgs(User user) => SelectedUser = user;
    }
}