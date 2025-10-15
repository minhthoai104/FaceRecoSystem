using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecoSystem
{
    internal class Attendance
    {
        public string AttendanceID { get; set; }
        public string UserID { get; set; }
        public DateTime CheckInTime { get; set; }
        public System.Drawing.Image CheckInImage { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public System.Drawing.Image CheckOutImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
