using System.Configuration;

namespace FaceRecoSystem.core
{
    public static class AppSettings
    {
        // Giá trị mặc định
        public const int DefaultHoldTime = 3; // giây
        public const int DefaultCheckoutDelay = 240; // phút
        public const int DefaultCheckinCooldown = 10; // phút
        public const int DefaultResetInfoPanelDelay = 5000; // mili-giây

        // Các thuộc tính tĩnh để truy cập
        public static int HoldTimeForAttendance { get; set; } = DefaultHoldTime;
        public static int MinTimeBetweenCheckInOut { get; set; } = DefaultCheckoutDelay;
        public static int CooldownAfterCheckin { get; set; } = DefaultCheckinCooldown;
        public static int ResetInfoPanelDelay { get; set; } = DefaultResetInfoPanelDelay;

        // Tải cài đặt từ file config của ứng dụng
        public static void Load()
        {
            HoldTimeForAttendance = GetConfigValue("HoldTimeForAttendance", DefaultHoldTime);
            MinTimeBetweenCheckInOut = GetConfigValue("MinTimeBetweenCheckInOut", DefaultCheckoutDelay);
            CooldownAfterCheckin = GetConfigValue("CooldownAfterCheckin", DefaultCheckinCooldown);
            ResetInfoPanelDelay = GetConfigValue("ResetInfoPanelDelay", DefaultResetInfoPanelDelay);
        }

        // Lưu cài đặt vào file config
        public static void Save()
        {
            SetConfigValue("HoldTimeForAttendance", HoldTimeForAttendance.ToString());
            SetConfigValue("MinTimeBetweenCheckInOut", MinTimeBetweenCheckInOut.ToString());
            SetConfigValue("CooldownAfterCheckin", CooldownAfterCheckin.ToString());
            SetConfigValue("ResetInfoPanelDelay", ResetInfoPanelDelay.ToString());
        }

        // Helper methods
        private static int GetConfigValue(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        private static void SetConfigValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}