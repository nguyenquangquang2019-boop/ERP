using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ERP.DucAnh.DTO;

namespace ERP.DucAnh.DAL
{
    public class PhieuTraHangDAL
    {
        private readonly string connectionString;

        public PhieuTraHangDAL()
        {
            this.connectionString = DatabaseConfig.GetConnectionString();
        }

        public PhieuTraHangDAL(string connectionString)
        {
            this.connectionString = string.IsNullOrWhiteSpace(connectionString)
                ? DatabaseConfig.GetConnectionString()
                : connectionString;
        }

        // 1. Lấy toàn bộ danh sách phiếu trả hàng
        public List<PhieuTraHang> GetAll()
        {
            List<PhieuTraHang> list = new List<PhieuTraHang>();
            const string query = @"SELECT ID_PhieuTra, NgayTra, MaDVC, ID_CTYC, TrangThai
                                   FROM PhieuTraHang
                                   ORDER BY NgayTra DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhieuTraHang
                        {
                            ID_PhieuTra = reader["ID_PhieuTra"] != DBNull.Value ? reader["ID_PhieuTra"].ToString() : string.Empty,
                            NgayTra = reader["NgayTra"] != DBNull.Value ? Convert.ToDateTime(reader["NgayTra"]) : DateTime.MinValue,
                            MaDVC = reader["MaDVC"] != DBNull.Value ? reader["MaDVC"].ToString() : string.Empty,
                            ID_CTYC = reader["ID_CTYC"] != DBNull.Value ? reader["ID_CTYC"].ToString() : string.Empty,
                            TrangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // 2. Lấy phiếu trả hàng theo ID
        public PhieuTraHang GetByID(string id)
        {
            const string query = @"SELECT ID_PhieuTra, NgayTra, MaDVC, ID_CTYC, TrangThai
                                   FROM PhieuTraHang
                                   WHERE ID_PhieuTra = @ID_PhieuTra";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_PhieuTra", SqlDbType.NVarChar) { Value = id ?? string.Empty });
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PhieuTraHang
                        {
                            ID_PhieuTra = reader["ID_PhieuTra"] != DBNull.Value ? reader["ID_PhieuTra"].ToString() : string.Empty,
                            NgayTra = reader["NgayTra"] != DBNull.Value ? Convert.ToDateTime(reader["NgayTra"]) : DateTime.MinValue,
                            MaDVC = reader["MaDVC"] != DBNull.Value ? reader["MaDVC"].ToString() : string.Empty,
                            ID_CTYC = reader["ID_CTYC"] != DBNull.Value ? reader["ID_CTYC"].ToString() : string.Empty,
                            TrangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : string.Empty
                        };
                    }
                }
            }
            return null;
        }

        // 3. Thêm mới phiếu trả hàng
        public bool Insert(PhieuTraHang p)
        {
            const string query = @"INSERT INTO PhieuTraHang (ID_PhieuTra, NgayTra, MaDVC, ID_CTYC, TrangThai)
                                   VALUES (@ID_PhieuTra, @NgayTra, @MaDVC, @ID_CTYC, @TrangThai)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_PhieuTra", SqlDbType.NVarChar) { Value = (object)p.ID_PhieuTra ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@NgayTra", SqlDbType.DateTime) { Value = p.NgayTra != DateTime.MinValue ? p.NgayTra : DateTime.Now });
                command.Parameters.Add(new SqlParameter("@MaDVC", SqlDbType.NVarChar) { Value = (object)p.MaDVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ID_CTYC", SqlDbType.NVarChar) { Value = (object)p.ID_CTYC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = (object)p.TrangThai ?? DBNull.Value });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 4. Cập nhật phiếu trả hàng
        public bool Update(PhieuTraHang p)
        {
            const string query = @"UPDATE PhieuTraHang
                                   SET NgayTra = @NgayTra,
                                       MaDVC = @MaDVC,
                                       ID_CTYC = @ID_CTYC,
                                       TrangThai = @TrangThai
                                   WHERE ID_PhieuTra = @ID_PhieuTra";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_PhieuTra", SqlDbType.NVarChar) { Value = (object)p.ID_PhieuTra ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@NgayTra", SqlDbType.DateTime) { Value = p.NgayTra != DateTime.MinValue ? p.NgayTra : DateTime.Now });
                command.Parameters.Add(new SqlParameter("@MaDVC", SqlDbType.NVarChar) { Value = (object)p.MaDVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ID_CTYC", SqlDbType.NVarChar) { Value = (object)p.ID_CTYC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = (object)p.TrangThai ?? DBNull.Value });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 5. Xóa phiếu trả hàng theo ID_PhieuTra
        public bool Delete(string id)
        {
            const string query = "DELETE FROM PhieuTraHang WHERE ID_PhieuTra = @ID_PhieuTra";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_PhieuTra", SqlDbType.NVarChar) { Value = id ?? string.Empty });
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 6. Kiểm tra ID_CTYC đã tồn tại trong PhieuTraHang chưa (thu hồi trùng lặp)
        public bool CheckThuHoiDup(string idCTYC)
        {
            const string query = "SELECT COUNT(*) FROM PhieuTraHang WHERE ID_CTYC = @ID_CTYC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_CTYC", SqlDbType.NVarChar) { Value = idCTYC ?? string.Empty });
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        // 7. Tìm kiếm có JOIN với bảng DiemVanChuyen
        public List<PhieuTraHang> Search(string keyword, DateTime tuNgay, DateTime denNgay, string trangThai)
        {
            List<PhieuTraHang> list = new List<PhieuTraHang>();
            string kw = (keyword ?? string.Empty).Trim();
            string kwLike = "%" + kw + "%";

            // Đảm bảo lọc trọn vẹn từ 00:00:00 của tuNgay đến 23:59:59 của denNgay
            DateTime start = tuNgay.Date;
            DateTime end = denNgay.Date.AddDays(1).AddTicks(-1);

            const string query = @"
                SELECT PT.ID_PhieuTra, PT.NgayTra, PT.MaDVC, PT.ID_CTYC, PT.TrangThai
                FROM PhieuTraHang PT
                LEFT JOIN DiemVanChuyen DVC ON PT.MaDVC = DVC.MaDVC
                WHERE PT.NgayTra >= @TuNgay AND PT.NgayTra <= @DenNgay
                  AND (@Keyword = '' OR PT.ID_PhieuTra LIKE @KwLike OR DVC.TenDVC LIKE @KwLike)
                  AND (@TrangThai = '' OR @TrangThai = N'Tất cả trạng thái' OR PT.TrangThai = @TrangThai)
                ORDER BY PT.NgayTra DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@TuNgay", SqlDbType.DateTime) { Value = start });
                command.Parameters.Add(new SqlParameter("@DenNgay", SqlDbType.DateTime) { Value = end });
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.NVarChar) { Value = kw });
                command.Parameters.Add(new SqlParameter("@KwLike", SqlDbType.NVarChar) { Value = kwLike });
                command.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai ?? string.Empty });

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhieuTraHang
                        {
                            ID_PhieuTra = reader["ID_PhieuTra"] != DBNull.Value ? reader["ID_PhieuTra"].ToString() : string.Empty,
                            NgayTra = reader["NgayTra"] != DBNull.Value ? Convert.ToDateTime(reader["NgayTra"]) : DateTime.MinValue,
                            MaDVC = reader["MaDVC"] != DBNull.Value ? reader["MaDVC"].ToString() : string.Empty,
                            ID_CTYC = reader["ID_CTYC"] != DBNull.Value ? reader["ID_CTYC"].ToString() : string.Empty,
                            TrangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // 8. Lấy danh sách ID_CTYC từ bảng ChiTietYeuCau
        public List<string> GetDanhSachCTYC()
        {
            List<string> list = new List<string>();
            const string query = "SELECT ID_CTYC FROM ChiTietYeuCau ORDER BY ID_CTYC ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ID_CTYC"] != DBNull.Value)
                        {
                            list.Add(reader["ID_CTYC"].ToString());
                        }
                    }
                }
            }
            return list;
        }

        // 9. Lấy danh sách MaDVC từ bảng DiemVanChuyen
        public List<string> GetDanhSachMaDVC()
        {
            List<string> list = new List<string>();
            const string query = "SELECT MaDVC FROM DiemVanChuyen ORDER BY MaDVC ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["MaDVC"] != DBNull.Value)
                        {
                            list.Add(reader["MaDVC"].ToString());
                        }
                    }
                }
            }
            return list;
        }
    }
}
