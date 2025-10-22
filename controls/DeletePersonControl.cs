using Emgu.CV;
using Emgu.CV.Structure;
using FaceRecoSystem.core;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FaceRecoSystem.controls
{
    public partial class DeletePersonControl : UserControl
    {
        private readonly PersonManager _personMgr;
        private VideoCapture _capture; private bool _isScanning = false;
        private string _recognizedName = null;

        public DeletePersonControl(PersonManager personMgr)
        {
            InitializeComponent();
            _personMgr = personMgr;
        }

        private void BtnDeleteByName_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên người cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ConfirmDeletion(name))
                return;

            try
            {
                bool deleted = _personMgr.DeletePersonByName(name);
                if (deleted)
                {
                    MessageBox.Show($"Đã xóa '{name}' thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Clear();
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy người có tên '{name}' trong cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnStartScan_Click(object sender, EventArgs e)
        {
            if (!_isScanning)
            {
                StartScanning();
            }
            else
            {
                StopScanning();
            }
        }

        private void StartScanning()
        {
            try
            {
                _capture = new VideoCapture(0);
                Application.Idle += ProcessFrame; btnStartScan.Text = "Dừng quét";
                lblScanStatus.Text = "Trạng thái: Đang quét...";
                btnDeleteByFace.Enabled = false;
                _isScanning = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động camera: " + ex.Message, "Lỗi Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopScanning()
        {
            Application.Idle -= ProcessFrame;
            _capture?.Dispose();
            _capture = null;
            btnStartScan.Text = "Bắt đầu quét";
            lblScanStatus.Text = "Trạng thái: Đã dừng";
            picCamera.Image = null;
            btnDeleteByFace.Enabled = false;
            _recognizedName = null;
            _isScanning = false;
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture == null) return;

            using (Mat frame = _capture.QueryFrame())
            {
                if (frame != null)
                {

                    if (!string.IsNullOrEmpty(_recognizedName))
                    {
                        lblScanStatus.Text = $"Đã nhận diện: {_recognizedName}";
                        btnDeleteByFace.Enabled = true;
                    }
                    else
                    {
                        lblScanStatus.Text = "Trạng thái: Đang quét...";
                        btnDeleteByFace.Enabled = false;
                    }
                    
                    picCamera.Image = frame.ToBitmap();
                }
            }
        }

        private void BtnDeleteByFace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_recognizedName)) return;
            if (!ConfirmDeletion(_recognizedName))
            {
                try
                {
                    _personMgr.DeletePersonByName(_recognizedName);
                    MessageBox.Show($"Đã xóa '{_recognizedName}' thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    StopScanning();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopScanning(); if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private bool ConfirmDeletion(string personName)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa '{personName}' không? Hành động này không thể hoàn tác.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}