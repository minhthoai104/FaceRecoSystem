using Dapper;
using FaceRecognitionDotNet;
using FaceRecoSystem.core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FaceRecoSystem
{
    public class FaceDatabase : IDisposable
    {
        private readonly string _connectionString;
        private readonly FaceRecognition _fr;
        private readonly Dictionary<string, List<double[]>> _known = new();
        private readonly Dictionary<string, User> _info = new();
        public IReadOnlyDictionary<string, List<double[]>> Known => _known;
        public IReadOnlyDictionary<string, User> Info => _info;

        public FaceDatabase(FaceRecognition fr, string connectionString)
        {
            _connectionString = connectionString;
            _fr = fr;
            LoadFromSql();
        }

        public static byte[] DoubleArrayToBytes(double[] arr)
        {
            if (arr == null) return null;
            var bytes = new byte[arr.Length * sizeof(double)];
            Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static double[] BytesToDoubleArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            var arr = new double[bytes.Length / sizeof(double)];
            Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
            return arr;
        }

        public static byte[] MatToBytes(Mat mat)
        {
            if (mat == null || mat.Empty()) return null;
            return mat.ToBytes(".jpg");
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT UserID, FullName, Age, Gender, Address, FaceFront, FaceLeft, FaceRight FROM Users", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserID = reader["UserID"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Gender = reader["Gender"].ToString(),
                    Address = reader["Address"].ToString(),
                    FaceFront = reader["FaceFront"] == DBNull.Value ? null : (byte[])reader["FaceFront"],
                    FaceLeft = reader["FaceLeft"] == DBNull.Value ? null : (byte[])reader["FaceLeft"],
                    FaceRight = reader["FaceRight"] == DBNull.Value ? null : (byte[])reader["FaceRight"]
                });
            }
            return users;
        }

        public User? GetPersonByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Users WHERE FullName = @FullName", conn);
            cmd.Parameters.AddWithValue("@FullName", name);
            using var reader = cmd.ExecuteReader();

            return reader.Read() ? MapReaderToUser(reader) : null;
        }

        public void InsertUser(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                INSERT INTO Users (
                    UserID, FullName, Age, Gender, Address,
                    FaceFront, FaceLeft, FaceRight,
                    FaceFrontEncoding, FaceLeftEncoding, FaceRightEncoding,
                    IsActive, CreatedAt, UpdatedAt
                )
                VALUES (
                    @UserID, @FullName, @Age, @Gender, @Address,
                    @FaceFront, @FaceLeft, @FaceRight,
                    @FaceFrontEnc, @FaceLeftEnc, @FaceRightEnc,
                    1, GETDATE(), GETDATE()
                )";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserID", user.UserID);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Age", user.Age);
            cmd.Parameters.AddWithValue("@Gender", (object?)user.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)user.Address ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@FaceFront", (object?)user.FaceFront ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceLeft", (object?)user.FaceLeft ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceRight", (object?)user.FaceRight ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@FaceFrontEnc", (object?)user.FaceFrontEncoding ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceLeftEnc", (object?)user.FaceLeftEncoding ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceRightEnc", (object?)user.FaceRightEncoding ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            _info[user.FullName] = user;
            _known[user.FullName] = new List<double[]>
            {
                BytesToDoubleArray(user.FaceFrontEncoding),
                BytesToDoubleArray(user.FaceLeftEncoding),
                BytesToDoubleArray(user.FaceRightEncoding)
            };
            LoadFromSql();
        }

        public void UpdateUser(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                UPDATE Users SET 
                    Age=@Age, Gender=@Gender, Address=@Address,
                    FaceFront=@FaceFront, FaceLeft=@FaceLeft, FaceRight=@FaceRight,
                    FaceFrontEncoding=@FaceFrontEnc, FaceLeftEncoding=@FaceLeftEnc, FaceRightEncoding=@FaceRightEnc,
                    UpdatedAt=GETDATE()
                WHERE FullName=@FullName";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Age", user.Age);
            cmd.Parameters.AddWithValue("@Gender", (object?)user.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)user.Address ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@FaceFront", (object?)user.FaceFront ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceLeft", (object?)user.FaceLeft ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceRight", (object?)user.FaceRight ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@FaceFrontEnc", (object?)user.FaceFrontEncoding ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceLeftEnc", (object?)user.FaceLeftEncoding ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceRightEnc", (object?)user.FaceRightEncoding ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            _info[user.FullName] = user;
            _known[user.FullName] = new List<double[]>
            {
                BytesToDoubleArray(user.FaceFrontEncoding),
                BytesToDoubleArray(user.FaceLeftEncoding),
                BytesToDoubleArray(user.FaceRightEncoding)
            };
            LoadFromSql();
        }

        public void DeleteUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE Users SET IsActive = 0, UpdatedAt = GETDATE() WHERE FullName = @FullName";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FullName", name);
            cmd.ExecuteNonQuery();

            if (_info.ContainsKey(name))
                _info[name].IsActive = false;
            _known.Remove(name);
        }

        public void SaveFaceImagesAndEncodings(User user, List<Mat> faces)
        {
            try
            {
                Console.WriteLine("[DB] Bắt đầu lưu thông tin người dùng...");
                InsertUser(user);
                Console.WriteLine("[DB] ✅ Đã chèn người dùng vào bảng");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] ❌ Lỗi khi lưu vào DB: {ex.Message}");
                throw; // ném ra để AddNewUser bắt được
            }
        }


        public byte[] GetFaceEncodingAsBytes(Mat face)
        {
            using var img = face.ToFaceRecognitionImage();
            using var enc = _fr.FaceEncodings(img).FirstOrDefault();
            if (enc == null) return null;

            double[] raw = enc.GetRawEncoding();

            return DoubleArrayToBytes(raw);
        }


        public static float[] DoubleToFloat(double[] src)
        {
            if (src == null) return null;
            float[] result = new float[src.Length];
            for (int i = 0; i < src.Length; i++)
                result[i] = (float)src[i];
            return result;
        }

        public (string name, double confidence)? FindClosestMatch(FaceEncoding encoding)
        {
            if (encoding == null || _known.Count == 0)
                return null;

            double[] targetEnc = Array.ConvertAll(encoding.GetRawEncoding(), x => (double)x);
            const double TOLERANCE = 0.7;

            string bestName = null;
            double bestDistance = double.MaxValue;

            foreach (var kvp in _known)
            {
                foreach (var knownEnc in kvp.Value)
                {
                    double dist = ComputeFaceDistance(knownEnc, targetEnc);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestName = kvp.Key;
                    }
                }
            }

            if (bestName != null && bestDistance < TOLERANCE)
            {
                double confidence = (1.0 - bestDistance) * 100.0;
                return (bestName, Math.Round(confidence, 2));
            }

            Console.WriteLine($"[FindClosestMatch] ❌ Không khớp (best={bestName ?? "null"}, dist={bestDistance:F3})");
            return null;
        }

        private void LoadFromSql()
        {
            _known.Clear();
            _info.Clear();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Users WHERE IsActive = 1", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var user = MapReaderToUser(reader);
                _info[user.FullName] = user;

                var encs = new List<double[]>();
                foreach (string field in new[] { "FaceFrontEncoding", "FaceLeftEncoding", "FaceRightEncoding" })
                {
                    if (reader[field] != DBNull.Value)
                    {
                        var bytes = (byte[])reader[field];
                        var arr = BytesToDoubleArray(bytes);
                        if (arr != null)
                            encs.Add(arr);
                    }
                }
                if (encs.Count > 0)
                    _known[user.FullName] = encs;
            }

            Console.WriteLine($"[LoadFromSql] ✅ Đã tải {_known.Count} người từ DB");
            Console.WriteLine($"Loaded {_known.Count} users.");
            foreach (var kvp in _known)
            {
                Console.WriteLine($"User: {kvp.Key}, Encodings: {kvp.Value.Count}, First vector len: {kvp.Value[0]?.Length}");
            }

        }

        private static double ComputeFaceDistance(double[] enc1, double[] enc2)
        {
            if (enc1 == null || enc2 == null || enc1.Length != enc2.Length)
                return double.MaxValue;

            double sum = 0;
            for (int i = 0; i < enc1.Length; i++)
            {
                double diff = enc1[i] - enc2[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum / enc1.Length);
        }

        public bool TryGetInfo(string name, out User info)
            => _info.TryGetValue(name, out info);

        private static User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                UserID = reader["UserID"].ToString(),
                FullName = reader["FullName"].ToString(),
                Age = reader["Age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Age"]),
                Gender = reader["Gender"]?.ToString(),
                Address = reader["Address"]?.ToString(),
                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                FaceFront = reader["FaceFront"] == DBNull.Value ? null : (byte[])reader["FaceFront"],
                FaceLeft = reader["FaceLeft"] == DBNull.Value ? null : (byte[])reader["FaceLeft"],
                FaceRight = reader["FaceRight"] == DBNull.Value ? null : (byte[])reader["FaceRight"]
            };
        }

        public async void MarkAttendance(string userName, Mat frame)
        {
            if (!TryGetInfo(userName, out var user))
            {
                Console.WriteLine($"[Attendance] Không tìm thấy thông tin người: {userName}");
                return;
            }

            try
            {
                byte[] imageBytes = frame.ToBytes(".jpg");

                string result = await AttendanceHelper.ProcessCheckInOrCheckOut(user.UserID, imageBytes);

                if (result == "CHECKIN" || result == "CHECKOUT")
                {
                    Console.WriteLine($"[Attendance] Ghi nhận thành công cho {userName}: {result}");
                }
                else if (result.StartsWith("ERROR"))
                {
                    Console.WriteLine($"[Attendance] Lỗi ghi công cho {userName}: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Attendance] Lỗi hệ thống khi ghi công cho {userName}: {ex.Message}");
            }
        }

        public List<Attendance> GetAllAttendance()
        {
            const string sql = @"
                SELECT a.AttendanceID, a.UserID, a.CheckInTime, a.CheckOutTime,
                       a.CheckInImage, a.CheckOutImage,
                       u.FullName, u.Age, u.Gender, u.Address
                FROM Attendance a
                INNER JOIN Users u ON a.UserID = u.UserID
                ORDER BY a.CheckInTime DESC;";

            using (var conn = new SqlConnection(_connectionString))
            {
                var list = conn.Query(sql).Select(row => new Attendance
                {
                    AttendanceID = row.AttendanceID,
                    UserID = row.UserID,
                    CheckInTime = row.CheckInTime,
                    CheckOutTime = row.CheckOutTime,
                    CheckInImage = row.CheckInImage == null ? null : (byte[])row.CheckInImage,
                    CheckOutImage = row.CheckOutImage == null ? null : (byte[])row.CheckOutImage,
                    UserInfo = new User
                    {
                        UserID = row.UserID,
                        FullName = row.FullName,
                        Age = row.Age,
                        Gender = row.Gender,
                        Address = row.Address
                    }
                }).ToList();

                Console.WriteLine($"[GetAllAttendance] Lấy được {list.Count} bản ghi từ DB");

                foreach (var att in list)
                    Console.WriteLine($"  -> {att.UserInfo.FullName} ({att.CheckInTime:dd/MM HH:mm})");

                return list;
            }
        }

        private System.Drawing.Image ByteArrayToImage(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private static System.Drawing.Image BytesToImage(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return System.Drawing.Image.FromStream(ms);
        }

        public void Dispose()
        {
            _known.Clear();
            _info.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
