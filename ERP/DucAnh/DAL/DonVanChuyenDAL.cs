using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ERP.DucAnh.DTO;

namespace ERP.DucAnh.DAL
{
    public static class DatabaseConfig
    {
        public const string ConnectionString =
            @"Data Source=DESKTOP-JMQN698\SQLEXPRESS;Initial Catalog=ERP_BanHang;Integrated Security=True;TrustServerCertificate=True";

        public static string GetConnectionString()
        {
            try
            {
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["ERP_BanHang"];
                if (setting != null && !string.IsNullOrEmpty(setting.ConnectionString))
                {
                    return setting.ConnectionString;
                }
                if (ConfigurationManager.ConnectionStrings.Count > 0 && ConfigurationManager.ConnectionStrings[0] != null)
                {
                    return ConfigurationManager.ConnectionStrings[0].ConnectionString;
                }
            }
            catch
            {
                // Dự phòng nếu không đọc được ConfigurationManager
            }
            return ConnectionString;
        }
    }

    public class DonVanChuyenDAL
    {
        private readonly string connectionString;

        public DonVanChuyenDAL()
        {
            this.connectionString = DatabaseConfig.GetConnectionString();
        }

        public DonVanChuyenDAL(string connectionString)
        {
            this.connectionString = string.IsNullOrEmpty(connectionString)
                ? DatabaseConfig.GetConnectionString()
                : connectionString;
        }

        // 1. Lấy toàn bộ danh sách đơn vận chuyển
        public List<DonVanChuyen> GetAll()
        {
            List<DonVanChuyen> list = new List<DonVanChuyen>();
            const string query = @"SELECT ID_DonVC, BienSoXe, MaDVC, ID_SP, SoLuongGiao, ThoiGianKhoiHanh, TrangThaiDon
                                   FROM DonVanChuyen
                                   ORDER BY ThoiGianKhoiHanh DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DonVanChuyen
                        {
                            ID_DonVC = reader["ID_DonVC"] != DBNull.Value ? reader["ID_DonVC"].ToString() : string.Empty,
                            BienSoXe = reader["BienSoXe"] != DBNull.Value ? reader["BienSoXe"].ToString() : string.Empty,
                            MaDVC = reader["MaDVC"] != DBNull.Value ? reader["MaDVC"].ToString() : string.Empty,
                            ID_SP = reader["ID_SP"] != DBNull.Value ? reader["ID_SP"].ToString() : string.Empty,
                            SoLuongGiao = reader["SoLuongGiao"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongGiao"]) : 0,
                            ThoiGianKhoiHanh = reader["ThoiGianKhoiHanh"] != DBNull.Value ? Convert.ToDateTime(reader["ThoiGianKhoiHanh"]) : DateTime.MinValue,
                            TrangThaiDon = reader["TrangThaiDon"] != DBNull.Value ? reader["TrangThaiDon"].ToString() : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // 2. Thêm mới đơn vận chuyển
        public bool Insert(DonVanChuyen don)
        {
            const string query = @"INSERT INTO DonVanChuyen
                                    (ID_DonVC, BienSoXe, MaDVC, ID_SP, SoLuongGiao, ThoiGianKhoiHanh, TrangThaiDon)
                                   VALUES
                                    (@ID_DonVC, @BienSoXe, @MaDVC, @ID_SP, @SoLuongGiao, @ThoiGianKhoiHanh, @TrangThaiDon)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_DonVC", SqlDbType.NVarChar) { Value = (object)don.ID_DonVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BienSoXe", SqlDbType.NVarChar) { Value = (object)don.BienSoXe ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@MaDVC", SqlDbType.NVarChar) { Value = (object)don.MaDVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ID_SP", SqlDbType.NVarChar) { Value = (object)don.ID_SP ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@SoLuongGiao", SqlDbType.Int) { Value = don.SoLuongGiao });
                command.Parameters.Add(new SqlParameter("@ThoiGianKhoiHanh", SqlDbType.DateTime) { Value = don.ThoiGianKhoiHanh });
                command.Parameters.Add(new SqlParameter("@TrangThaiDon", SqlDbType.NVarChar) { Value = (object)don.TrangThaiDon ?? DBNull.Value });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 3. Cập nhật đơn vận chuyển
        public bool Update(DonVanChuyen don)
        {
            const string query = @"UPDATE DonVanChuyen
                                   SET BienSoXe = @BienSoXe,
                                       MaDVC = @MaDVC,
                                       ID_SP = @ID_SP,
                                       SoLuongGiao = @SoLuongGiao,
                                       ThoiGianKhoiHanh = @ThoiGianKhoiHanh,
                                       TrangThaiDon = @TrangThaiDon
                                   WHERE ID_DonVC = @ID_DonVC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_DonVC", SqlDbType.NVarChar) { Value = (object)don.ID_DonVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BienSoXe", SqlDbType.NVarChar) { Value = (object)don.BienSoXe ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@MaDVC", SqlDbType.NVarChar) { Value = (object)don.MaDVC ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ID_SP", SqlDbType.NVarChar) { Value = (object)don.ID_SP ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@SoLuongGiao", SqlDbType.Int) { Value = don.SoLuongGiao });
                command.Parameters.Add(new SqlParameter("@ThoiGianKhoiHanh", SqlDbType.DateTime) { Value = don.ThoiGianKhoiHanh });
                command.Parameters.Add(new SqlParameter("@TrangThaiDon", SqlDbType.NVarChar) { Value = (object)don.TrangThaiDon ?? DBNull.Value });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 4. Xóa đơn vận chuyển theo ID_DonVC
        public bool Delete(string id)
        {
            const string query = "DELETE FROM DonVanChuyen WHERE ID_DonVC = @ID_DonVC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_DonVC", SqlDbType.NVarChar) { Value = (object)id ?? DBNull.Value });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // 5. Tìm kiếm theo ID_DonVC, BienSoXe hoặc MaDVC (dùng LIKE)
        public List<DonVanChuyen> Search(string keyword)
        {
            List<DonVanChuyen> list = new List<DonVanChuyen>();
            const string query = @"SELECT ID_DonVC, BienSoXe, MaDVC, ID_SP, SoLuongGiao, ThoiGianKhoiHanh, TrangThaiDon
                                   FROM DonVanChuyen
                                   WHERE ID_DonVC LIKE @Keyword
                                      OR BienSoXe LIKE @Keyword
                                      OR MaDVC LIKE @Keyword
                                   ORDER BY ThoiGianKhoiHanh DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                string searchPattern = "%" + (keyword ?? string.Empty).Trim() + "%";
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.NVarChar) { Value = searchPattern });

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DonVanChuyen
                        {
                            ID_DonVC = reader["ID_DonVC"] != DBNull.Value ? reader["ID_DonVC"].ToString() : string.Empty,
                            BienSoXe = reader["BienSoXe"] != DBNull.Value ? reader["BienSoXe"].ToString() : string.Empty,
                            MaDVC = reader["MaDVC"] != DBNull.Value ? reader["MaDVC"].ToString() : string.Empty,
                            ID_SP = reader["ID_SP"] != DBNull.Value ? reader["ID_SP"].ToString() : string.Empty,
                            SoLuongGiao = reader["SoLuongGiao"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongGiao"]) : 0,
                            ThoiGianKhoiHanh = reader["ThoiGianKhoiHanh"] != DBNull.Value ? Convert.ToDateTime(reader["ThoiGianKhoiHanh"]) : DateTime.MinValue,
                            TrangThaiDon = reader["TrangThaiDon"] != DBNull.Value ? reader["TrangThaiDon"].ToString() : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // 6. Kiểm tra ID_DonVC đã tồn tại chưa (phục vụ kiểm tra A2)
        public bool IsExist(string id)
        {
            const string query = "SELECT COUNT(1) FROM DonVanChuyen WHERE ID_DonVC = @ID_DonVC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(new SqlParameter("@ID_DonVC", SqlDbType.NVarChar) { Value = (object)id ?? DBNull.Value });

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        // 7. Lấy danh sách BienSoXe từ bảng PhuongTien có KichHoat = 1 VÀ TrangThaiXe = 'Sẵn sàng'
        //    VÀ không nằm trong đơn vận chuyển nào đang có TrangThaiDon = 'Đang vận chuyển'
        public List<string> GetDanhSachXeKhaDung()
        {
            List<string> list = new List<string>();
            const string query = @"SELECT BienSoXe
                                   FROM PhuongTien
                                   WHERE KichHoat = 1
                                     AND TrangThaiXe = N'Sẵn sàng'
                                     AND BienSoXe NOT IN (
                                         SELECT BienSoXe
                                         FROM DonVanChuyen
                                         WHERE TrangThaiDon = N'Đang vận chuyển'
                                           AND BienSoXe IS NOT NULL
                                     )
                                   ORDER BY BienSoXe ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["BienSoXe"] != DBNull.Value)
                        {
                            list.Add(reader["BienSoXe"].ToString());
                        }
                    }
                }
            }
            return list;
        }

        // 8. Lấy danh sách MaDVC từ bảng DiemVanChuyen
        public List<string> GetDanhSachDVC()
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

        // 9. Lấy danh sách ID_SP từ bảng SanPham
        public List<string> GetDanhSachSanPham()
        {
            List<string> list = new List<string>();
            const string query = "SELECT ID_SP FROM SanPham ORDER BY ID_SP ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ID_SP"] != DBNull.Value)
                        {
                            list.Add(reader["ID_SP"].ToString());
                        }
                    }
                }
            }
            return list;
        }

        // Các hàm phụ trợ tương thích ngược với BLL hiện tại
        public DataTable GetTatCaDonVanChuyen()
        {
            const string query = @"
                SELECT DVC.ID_DonVC, DVC.BienSoXe, DVC.MaDVC,
                       ISNULL(DMC.TenDVC, DVC.MaDVC) AS TenDVC,
                       DVC.ID_SP, DVC.SoLuongGiao, DVC.ThoiGianKhoiHanh,
                       DVC.TrangThaiDon
                FROM DonVanChuyen DVC
                LEFT JOIN DiemVanChuyen DMC ON DVC.MaDVC = DMC.MaDVC
                ORDER BY DVC.ThoiGianKhoiHanh DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                DataTable result = new DataTable();
                adapter.Fill(result);
                return result;
            }
        }

        public void ThemDon(DonVanChuyen don)
        {
            Insert(don);
        }
    }
}
