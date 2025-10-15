using FaceRecognitionDotNet;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FaceRecoSystem
{
    public class FaceDatabase : IDisposable
    {
        private readonly FaceRecognition _fr;
        private readonly string _dbPath;
        private readonly string _connectionString;
        private readonly Dictionary<string, List<FaceEncoding>> _known = new();
        private readonly Dictionary<string, User> _info = new();

        public IReadOnlyDictionary<string, List<FaceEncoding>> Known => _known;

        public FaceDatabase(FaceRecognition fr, string dbPath, string connectionString)
        {
            _fr = fr ?? throw new ArgumentNullException(nameof(fr));
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Directory.CreateDirectory(_dbPath);

            LoadFromSql();
        }

        private string GenerateId() => DateTime.Now.ToString("MMddHHmmss");

        // Lấy tất cả Users từ SQL
        public List<User> GetAllPersons()
        {
            var persons = new List<User>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                persons.Add(new User
                {
                    UserID = reader["UserID"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Age = reader["Age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Age"]),
                    Gender = reader["Gender"]?.ToString(),
                    Address = reader["Address"]?.ToString(),
                    FaceFrontPath = reader["FaceFront"]?.ToString(),
                    FaceLeftPath = reader["FaceLeft"]?.ToString(),
                    FaceRightPath = reader["FaceRight"]?.ToString()
                });
            }

            return persons;
        }

        public void Save(string name, int age, string gender, string address, List<Mat> faces)
        {
            if (faces == null || faces.Count < 3)
                throw new ArgumentException("Cần 3 ảnh mặt: Front, Left, Right");

            string id = GenerateId();
            string folder = Path.Combine(_dbPath, $"{name}_{id}");
            Directory.CreateDirectory(folder);

            string faceFrontPath = Path.Combine(folder, $"{name}_front.jpg");
            string faceLeftPath = Path.Combine(folder, $"{name}_left.jpg");
            string faceRightPath = Path.Combine(folder, $"{name}_right.jpg");

            Cv2.ImWrite(faceFrontPath, faces[0]);
            Cv2.ImWrite(faceLeftPath, faces[1]);
            Cv2.ImWrite(faceRightPath, faces[2]);

            var encList = new List<FaceEncoding>();

            foreach (string path in new[] { faceFrontPath, faceLeftPath, faceRightPath })
            {
                using var img = FaceRecognition.LoadImageFile(path);
                var enc = _fr.FaceEncodings(img).FirstOrDefault();
                if (enc != null)
                    encList.Add(enc);
            }

            if (encList.Count == 0)
                throw new Exception("Không tạo được encoding cho " + name);

            _known[name] = encList;
            _info[name] = new User
            {
                UserID = id,
                FullName = name,
                Age = age,
                Gender = gender,
                Address = address,
                FaceFrontPath = faceFrontPath,
                FaceLeftPath = faceLeftPath,
                FaceRightPath = faceRightPath
            };

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                INSERT INTO Users (UserID, FullName, Age, Gender, Address, FaceFront, FaceLeft, FaceRight, CreatedAt, UpdatedAt)
                VALUES (@UserID, @FullName, @Age, @Gender, @Address, @FaceFront, @FaceLeft, @FaceRight, GETDATE(), GETDATE())";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserID", id);
            cmd.Parameters.AddWithValue("@FullName", name);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@FaceFront", faceFrontPath);
            cmd.Parameters.AddWithValue("@FaceLeft", faceLeftPath);
            cmd.Parameters.AddWithValue("@FaceRight", faceRightPath);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"[DB] ✅ Saved {name} với 3 góc mặt vào SQL");
        }


        // Load database from SQL and produce encodings for available images (front/left/right)
        private void LoadFromSql()
        {
            _known.Clear();
            _info.Clear();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var u = new User
                {
                    UserID = reader["UserID"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Gender = reader["Gender"].ToString(),
                    Address = reader["Address"].ToString(),
                    FaceFrontPath = reader["FaceFront"]?.ToString(),
                    FaceLeftPath = reader["FaceLeft"]?.ToString(),
                    FaceRightPath = reader["FaceRight"]?.ToString()
                };

                _info[u.FullName] = u;

                var encList = new List<FaceEncoding>();
                foreach (string path in new[] { u.FaceFrontPath, u.FaceLeftPath, u.FaceRightPath })
                {
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        using var img = FaceRecognition.LoadImageFile(path);
                        var enc = _fr.FaceEncodings(img).FirstOrDefault();
                        if (enc != null)
                            encList.Add(enc);
                    }
                }

                if (encList.Count > 0)
                    _known[u.FullName] = encList;
            }

            Console.WriteLine($"[DB] ✅ Loaded {_info.Count} users với vector 3 góc mặt");
        }
        public (string name, double confidence)? FindClosestMatch(FaceEncoding encoding)
        {
            const double TOLERANCE = 0.45;
            string bestName = null;
            double bestDistance = double.MaxValue;

            foreach (var kvp in _known)
            {
                foreach (var enc in kvp.Value)
                {
                    double dist = FaceRecognition.FaceDistance(enc, encoding);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestName = kvp.Key;
                    }
                }
            }

            if (bestName != null && bestDistance < TOLERANCE)
            {
                double confidence = (1.0 - bestDistance) * 100;
                return (bestName, confidence);
            }

            return null;
        }

        public string FacesPath => _dbPath;

        public User? GetPersonByName(string name)
        {
            if (_info.ContainsKey(name)) return _info[name];

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Users WHERE FullName = @FullName";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FullName", name);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserID = reader["UserID"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Age = reader["Age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Age"]),
                    Gender = reader["Gender"]?.ToString(),
                    Address = reader["Address"]?.ToString(),
                    FaceFrontPath = reader["FaceFront"]?.ToString(),
                    FaceLeftPath = reader["FaceLeft"]?.ToString(),
                    FaceRightPath = reader["FaceRight"]?.ToString()
                };
            }

            return null;
        }

        public void UpdateEncoding(string name, FaceEncoding newEncoding)
        {
            if (newEncoding == null) return;

            if (!_known.ContainsKey(name))
                _known[name] = new List<FaceEncoding>();

            _known[name].Clear();
            _known[name].Add(newEncoding);

            Console.WriteLine($"[DB] ✅ Updated encoding for {name}");
        }

        public void SavePerson(User person)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"UPDATE Users SET 
                    FullName = @FullName,
                    Age = @Age,
                    Gender = @Gender,
                    Address = @Address,
                    UpdatedAt = GETDATE()
                   WHERE UserID = @UserID";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserID", person.UserID);
            cmd.Parameters.AddWithValue("@FullName", person.FullName);
            cmd.Parameters.AddWithValue("@Age", person.Age);
            cmd.Parameters.AddWithValue("@Gender", person.Gender ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", person.Address ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();

            _info[person.FullName] = person;

            Console.WriteLine($"[DB] ✅ Updated info for {person.FullName}");
        }

        public void Delete(string name)
        {
            _info.Remove(name);
            _known.Remove(name);

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "DELETE FROM Users WHERE FullName = @FullName";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FullName", name);
            cmd.ExecuteNonQuery();

            var folder = Directory.GetDirectories(_dbPath)
                .FirstOrDefault(d => Path.GetFileName(d).StartsWith(name + "_"));
            if (folder != null)
                Directory.Delete(folder, true);

            Console.WriteLine($"[DB] 🗑️ Deleted {name}");
        }

        public bool TryGetInfo(string name, out User info)
        {
            if (_info.ContainsKey(name))
            {
                info = _info[name];
                return true;
            }

            info = null;
            return false;
        }

        public void PrintDebugInfo()
        {
            Console.WriteLine("[DB] Known persons summary:");
            foreach (var kv in _known)
                Console.WriteLine($"  - {kv.Key}: {kv.Value.Count} encodings");
        }

        public void Dispose()
        {
            foreach (var encList in _known.Values)
                foreach (var enc in encList)
                    enc.Dispose();
            _known.Clear();
            GC.Collect();
        }
    }
}
