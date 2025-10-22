using System;
using System.IO;

namespace FaceRecoSystem
{
    public class User
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;

        public byte[] FaceFront { get; set; }
        public byte[] FaceLeft { get; set; }
        public byte[] FaceRight { get; set; }

        public byte[] FaceFrontEncoding { get; set; }
        public byte[] FaceLeftEncoding { get; set; }
        public byte[] FaceRightEncoding { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string CheckInImagePath { get; set; }
        public string CheckOutImagePath { get; set; }

        public int MinCheckOutTimeInMinutes;
        public int? CheckInCooldownInMinutes;
        public string FaceFrontPath { get; set; }

        // Helper: lưu byte[] thành file tạm và trả về path
        public static string SaveBytesToTempFile(byte[] bytes, string prefix = "face")
        {
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                string ext = ".jpg";
                string temp = Path.Combine(Path.GetTempPath(), $"{prefix}_{Guid.NewGuid():N}{ext}");
                File.WriteAllBytes(temp, bytes);
                return temp;
            }
            catch { return null; }
        }
    }
}