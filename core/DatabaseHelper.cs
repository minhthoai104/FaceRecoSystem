using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecoSystem.core
{
    internal class DatabaseHelper
    {
        public static string ConnectionString { get; } =
            "Server=LAPTOP-LGQDV4F7\\SQLEXPRESS;Database=FaceRecoDB;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
