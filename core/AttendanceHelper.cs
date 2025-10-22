using Dapper;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FaceRecoSystem.core
{
    public class AttendanceLog
    {
        public string AttendanceId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }

    public static class AttendanceHelper
    {
        // ================================
        // Cấu hình & biến toàn cục
        // ================================
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["FaceRecoDB"]?.ConnectionString ??
            "Server=LAPTOP-LGQDV4F7\\SQLEXPRESS;Database=FaceRecoDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Thời gian chờ (giây)
        private const int MinCheckInOutIntervalSec = 60;   // 1 phút giữa Check-In và Check-Out
        private const int MinReCheckInIntervalSec = 120;   // 2 phút giữa hai lần Check-In
        private static readonly Random _random = new Random();
        private static readonly ConcurrentDictionary<string, bool> _processingUsers = new();

        // Event log (UI có thể subscribe nếu cần)
        public static event Action<string> OnLog;

        private static void Log(string message)
        {
            Console.WriteLine($"[Attendance] {message}");
            OnLog?.Invoke(message);
        }

        // ================================
        // ProcessCheckInOrCheckOut (nhận byte[] ảnh JPG)
        // Trả về: "CHECKIN", "CHECKOUT", "WAIT:xx", "ERROR"
        // ================================
        public static async Task<string> ProcessCheckInOrCheckOut(string userId, byte[] frameBytes)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                Log("❌ userId trống hoặc null.");
                return "ERROR";
            }

            if (frameBytes == null || frameBytes.Length == 0)
            {
                Log("❌ Ảnh (frameBytes) rỗng hoặc null.");
                return "ERROR";
            }

            if (!_processingUsers.TryAdd(userId, true))
            {
                Log($"⏳ {userId} đang được xử lý, chờ vòng sau...");
                return "WAIT";
            }

            try
            {
                using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                // Tìm phiên đang mở (chưa checkout)
                string findOpenSessionSql = @"
                    SELECT TOP 1 AttendanceID, CheckInTime, CheckOutTime
                    FROM Attendance
                    WHERE UserID = @UserID 
                    AND CheckOutTime IS NULL
                    ORDER BY CheckInTime DESC;";

                var openSession = (await connection.QueryAsync<AttendanceLog>(
                    findOpenSessionSql, new { UserID = userId })).FirstOrDefault();

                if (openSession != null)
                {
                    var elapsed = DateTime.Now - openSession.CheckInTime;
                    if (elapsed.TotalSeconds < MinCheckInOutIntervalSec)
                    {
                        double wait = MinCheckInOutIntervalSec - elapsed.TotalSeconds;
                        Log($"⏳ {userId} cần đợi {wait:F0}s nữa mới được CHECK-OUT.");
                        return $"WAIT:{wait:F0}";
                    }

                    // Cập nhật CHECK-OUT
                    string updateSql = @"
                        UPDATE Attendance 
                        SET CheckOutTime = GETDATE(), 
                            CheckOutImage = @CheckOutImage,
                            UpdatedAt = GETDATE()
                        WHERE AttendanceID = @AttendanceID;";

                    int affected = await connection.ExecuteAsync(updateSql, new
                    {
                        AttendanceID = openSession.AttendanceId,
                        CheckOutImage = frameBytes
                    });

                    if (affected > 0)
                    {
                        Log($"✅ CHECK-OUT thành công cho {userId} (ID: {openSession.AttendanceId})");
                        return "CHECKOUT";
                    }

                    Log($"⚠ Không cập nhật được CHECK-OUT cho {userId}");
                    return "ERROR";
                }

                // Không có phiên mở -> kiểm tra lần CHECK-OUT gần nhất
                string findLastSessionSql = @"
                    SELECT TOP 1 AttendanceID, CheckInTime, CheckOutTime
                    FROM Attendance
                    WHERE UserID = @UserID
                    ORDER BY CheckInTime DESC;";

                var lastSession = (await connection.QueryAsync<AttendanceLog>(
                    findLastSessionSql, new { UserID = userId })).FirstOrDefault();

                if (lastSession?.CheckOutTime != null)
                {
                    var elapsed = DateTime.Now - lastSession.CheckOutTime.Value;
                    if (elapsed.TotalSeconds < MinReCheckInIntervalSec)
                    {
                        double wait = MinReCheckInIntervalSec - elapsed.TotalSeconds;
                        Log($"⏳ {userId} cần đợi {wait:F0}s nữa mới được CHECK-IN lại.");
                        return $"WAIT:{wait:F0}";
                    }
                }

                // Tạo phiên CHECK-IN mới
                string newAttendanceId = GenerateAttendanceId();

                string insertSql = @"
                    INSERT INTO Attendance (AttendanceID, UserID, CheckInTime, CheckInImage, CreatedAt, UpdatedAt)
                    VALUES (@AttendanceID, @UserID, GETDATE(), @CheckInImage, GETDATE(), GETDATE());";

                int inserted = await connection.ExecuteAsync(insertSql, new
                {
                    AttendanceID = newAttendanceId,
                    UserID = userId,
                    CheckInImage = frameBytes
                });

                if (inserted > 0)
                {
                    Log($"CHECK-IN thành công cho {userId} (ID: {newAttendanceId})");
                    return "CHECKIN";
                }

                Log($"Không lưu được CHECK-IN cho {userId}");
                return "ERROR";
            }
            catch (Exception ex)
            {
                Log($"Lỗi xử lý điểm danh ({userId}): {ex.Message}");
                return "ERROR";
            }
            finally
            {
                _processingUsers.TryRemove(userId, out _);
            }
        }

        public static async Task<string> GetCurrentStatus(string userId)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                string sql = @"
                    SELECT TOP 1 
                        CASE 
                            WHEN CheckOutTime IS NULL THEN 'IN'
                            ELSE 'OUT'
                        END AS Status
                    FROM Attendance
                    WHERE UserID = @UserID
                    ORDER BY CheckInTime DESC;";

                string status = await connection.QueryFirstOrDefaultAsync<string>(sql, new { UserID = userId });
                return status ?? "UNKNOWN";
            }
            catch (Exception ex)
            {
                Log($"⚠ Lỗi lấy trạng thái user ({userId}): {ex.Message}");
                return "UNKNOWN";
            }
        }

        private static string GenerateAttendanceId()
        {
            string randomPart = _random.Next(1000).ToString("D4");
            return $"{DateTime.Now:yyMMdd}{randomPart}";
        }
    }
}
