using FaceRecoSystem.core;
using System;
using System.Windows.Forms;

namespace FaceRecoSystem.controls
{
    public partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            InitializeComponent();
        }

        private void SettingControl_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            numHoldTime.Value = AppSettings.HoldTimeForAttendance;
            numCheckoutDelay.Value = AppSettings.MinTimeBetweenCheckInOut;
            numCheckinCooldown.Value = AppSettings.CooldownAfterCheckin;
            numResetDelay.Value = AppSettings.ResetInfoPanelDelay / 1000; // Chuyển từ ms sang s
        }

        private void SaveSettings()
        {
            AppSettings.HoldTimeForAttendance = (int)numHoldTime.Value;
            AppSettings.MinTimeBetweenCheckInOut = (int)numCheckoutDelay.Value;
            AppSettings.CooldownAfterCheckin = (int)numCheckinCooldown.Value;
            AppSettings.ResetInfoPanelDelay = (int)numResetDelay.Value * 1000; // Chuyển từ s sang ms

            AppSettings.Save();
        }

        private void SetDefaultValues()
        {
            numHoldTime.Value = AppSettings.DefaultHoldTime;
            numCheckoutDelay.Value = AppSettings.DefaultCheckoutDelay;
            numCheckinCooldown.Value = AppSettings.DefaultCheckinCooldown;
            numResetDelay.Value = AppSettings.DefaultResetInfoPanelDelay / 1000; // Chuyển từ ms sang s
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Đã lưu cài đặt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn khôi phục cài đặt mặc định không? Các thay đổi chưa lưu sẽ bị mất.",
                                         "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                SetDefaultValues();
            }
        }
    }
}