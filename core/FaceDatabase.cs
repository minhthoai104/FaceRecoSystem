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

        // ===== Helper: Convert Functions =====
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

        // ===== Public CRUD =====
        public List<User> GetAllPersons()
        {
            var persons = new List<User>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Users WHERE IsActive = 1", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                persons.Add(MapReaderToUser(reader));
            return persons;
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

            // cập nhật cache
            _info[user.FullName] = user;
            _known[user.FullName] = new List<double[]>
            {
                BytesToDoubleArray(user.FaceFrontEncoding),
                BytesToDoubleArray(user.FaceLeftEncoding),
                BytesToDoubleArray(user.FaceRightEncoding)
            };
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
            if (faces == null || faces.Count < 3)
                throw new Exception("Cần đủ 3 ảnh: front, left, right");

            user.FaceFront = MatToBytes(faces[0]);
            user.FaceLeft = MatToBytes(faces[1]);
            user.FaceRight = MatToBytes(faces[2]);

            user.FaceFrontEncoding = GetFaceEncodingAsBytes(faces[0]);
            user.FaceLeftEncoding = GetFaceEncodingAsBytes(faces[1]);
            user.FaceRightEncoding = GetFaceEncodingAsBytes(faces[2]);

            if (user.FaceFrontEncoding == null || user.FaceLeftEncoding == null || user.FaceRightEncoding == null)
                throw new Exception($"Không thể trích xuất encoding cho {user.FullName}");

            InsertUser(user);
        }

        public byte[] GetFaceEncodingAsBytes(Mat face)
        {
            using var img = face.ToFaceRecognitionImage();
            using var enc = _fr.FaceEncodings(img).FirstOrDefault();
            if (enc == null) return null;
            var arr = enc.GetRawEncoding();
            return DoubleArrayToBytes(arr);
        }

        // ===== Face Matching =====
        public (string name, double confidence)? FindClosestMatch(FaceEncoding encoding)
        {
            if (encoding == null || _known.Count == 0)
                return null;

            double[] targetEnc = encoding.GetRawEncoding();
            const double TOLERANCE = 0.45;

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

            return null;
        }

        // ===== Load from SQL =====
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
        }

        // ===== Helper =====
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

        public void Dispose()
        {
            _known.Clear();
            _info.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
