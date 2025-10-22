using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class AttendanceList : UserControl
    {
        private readonly FaceDatabase _db;

        public AttendanceList(FaceDatabase db)
        {
            InitializeComponent();
            _db = db;
            this.Load += AttendanceList_Load;
        }

        private void AttendanceList_Load(object sender, EventArgs e)
        {
            SetupListView();
            LoadAttendanceList();
        }
        private void SetupListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Họ tên", 150);
                listView1.Columns.Add("Mã nhân viên", 100);
                listView1.Columns.Add("Giờ vào", 150);
                listView1.Columns.Add("Giờ ra", 150);
            }
        }
        private void LoadAttendanceList()
        {
            listView1.Items.Clear();

            try
            {
                var list = _db.GetAllAttendance();
                Console.WriteLine($"[AttendanceList] Load {list.Count} bản ghi");

                foreach (var att in list)
                {
                    var u = att.UserInfo;
                    var item = new ListViewItem(u.FullName);
                    item.SubItems.Add(u.UserID);
                    item.SubItems.Add(att.CheckInTime.ToString("HH:mm:ss dd/MM/yyyy"));
                    item.SubItems.Add(att.CheckOutTime?.ToString("HH:mm:ss dd/MM/yyyy") ?? "-");
                    item.Tag = att;
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AttendanceList] Lỗi load dữ liệu: {ex}");
            }
        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var selected = listView1.SelectedItems[0];
            var att = selected.Tag as Attendance;
            if (att == null) return;

            var u = att.UserInfo;
            lblName.Text = "Họ và tên: " + u.FullName;
            lblGender.Text = "Giới tính: " + u.Gender;
            lblAge.Text = "Tuổi: " + u.Age.ToString();
            lblAddress.Text = "Địa chỉ: " + u.Address;

            picCheckIn.Image = BytesToImage(att.CheckInImage);
            picCheckOut.Image = BytesToImage(att.CheckOutImage);

        }
        private Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private Image LoadImageSafe(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    return Image.FromFile(path);
            }
            catch { }
            return null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAttendanceList();
        }
    }
}
