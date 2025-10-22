using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecoSystem
{
    public class Attendance
    {
        public string AttendanceID { get; set; }
        public string UserID { get; set; }
        public DateTime CheckInTime { get; set; }
        public byte[] CheckInImage { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public byte[] CheckOutImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User UserInfo { get; set; }
    }
}
