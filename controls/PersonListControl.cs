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
        private readonly FaceDatabase _db;

        public PersonListControl(FaceDatabase db)
        {
            InitializeComponent();
            _db = db;
        }

        public PersonListControl()
        {
        }

        private void PersonListControl_Load(object sender, EventArgs e)
        {
            LoadPersonList();
        }

        private void LoadPersonList()
        {
            listView1.Items.Clear();

            try
            {
                var persons = _db.GetAllPersons();
                foreach (var p in persons)
                {
                    var item = new ListViewItem(p.FullName);
                    item.SubItems.Add(p.Age.ToString());
                    item.SubItems.Add(p.Gender);
                    item.SubItems.Add(p.Address);
                    item.SubItems.Add(p.CheckInTime?.ToString("HH:mm:ss dd/MM/yyyy") ?? "-");
                    item.SubItems.Add(p.CheckOutTime?.ToString("HH:mm:ss dd/MM/yyyy") ?? "-");
                    item.Tag = p;
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading person list: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var selected = listView1.SelectedItems[0];
            var person = selected.Tag as User;
            if (person == null) return;

            lblName.Text = person.FullName;
            lblGender.Text = person.Gender;
            lblAddress.Text = person.Address;
            lblAge.Text = person.Age.ToString();
            picCheckIn.Image = LoadImageSafe(person.CheckInImagePath);
            picCheckOut.Image = LoadImageSafe(person.CheckOutImagePath);
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
    }
}
