using System;

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
    }
}