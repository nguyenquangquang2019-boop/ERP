using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ERP.DucAnh.DAL;

namespace ERP
{
    public partial class FrmSuaNhaCungCap : Form
    {
        private string connectionString = DatabaseConfig.GetConnectionString();
        private string idNccToEdit;

        // Constructor nhận vào Mã NCC cần sửa từ form danh sách
        public FrmSuaNhaCungCap(string idNcc)
        {
            InitializeComponent();
            idNccToEdit = idNcc;
            txtID.Text = idNccToEdit;
            txtID.ReadOnly = true; // Khóa mã NCC không cho sửa khóa chính
        }

        private void FrmSuaNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadThongTinNhaCungCap();
        }

        // Đổ dữ liệu cũ của nhà cung cấp lên các control để chỉnh sửa
        private void LoadThongTinNhaCungCap()
        {
            string query = "SELECT * FROM NhaCungCap WHERE ID_NCC = @ID_NCC";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID_NCC", idNccToEdit);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTenNCC.Text = reader["TenNCC"].ToString();
                            txtDiaChi.Text = reader["DiaChi"].ToString();
                            txtSDT.Text = reader["SDT"].ToString();
                            txtTenCN.Text = reader["TenChungNhan"] != DBNull.Value ? reader["TenChungNhan"].ToString() : "";
                            txtSoHieuCN.Text = reader["SoHieuCN"] != DBNull.Value ? reader["SoHieuCN"].ToString() : "";

                            if (reader["NgayCap"] != DBNull.Value)
                            {
                                dtpNgayCap.Checked = true;
                                dtpNgayCap.Value = Convert.ToDateTime(reader["NgayCap"]);
                            }
                            else
                            {
                                dtpNgayCap.Checked = false;
                            }

                            if (reader["NgayHetHan"] != DBNull.Value)
                            {
                                dtpNgayHetHan.Checked = true;
                                dtpNgayHetHan.Value = Convert.ToDateTime(reader["NgayHetHan"]);
                            }
                            else
                            {
                                dtpNgayHetHan.Checked = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thông tin nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra các trường dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin bắt buộc (Tên NCC, Địa chỉ, SĐT)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sdt = txtSDT.Text.Trim();

            // 2. Kiểm tra số điện thoại phải đúng 10 chữ số
            if (!Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải bao gồm đúng 10 chữ số.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                txtSDT.SelectAll();
                return;
            }

            string tenNcc = txtTenNCC.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string tenCN = txtTenCN.Text.Trim();
            string soHieuCN = txtSoHieuCN.Text.Trim();
            DateTime ngayCap = dtpNgayCap.Value;
            DateTime ngayHetHan = dtpNgayHetHan.Value;

            // 3. Câu lệnh SQL UPDATE
            string query = @"UPDATE NhaCungCap SET 
                             TenNCC = @TenNCC, 
                             DiaChi = @DiaChi, 
                             SDT = @SDT, 
                             TenChungNhan = @TenChungNhan, 
                             SoHieuCN = @SoHieuCN, 
                             NgayCap = @NgayCap, 
                             NgayHetHan = @NgayHetHan 
                             WHERE ID_NCC = @ID_NCC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID_NCC", idNccToEdit);
                    cmd.Parameters.AddWithValue("@TenNCC", tenNcc);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    cmd.Parameters.AddWithValue("@TenChungNhan", string.IsNullOrEmpty(tenCN) ? (object)DBNull.Value : tenCN);
                    cmd.Parameters.AddWithValue("@SoHieuCN", string.IsNullOrEmpty(soHieuCN) ? (object)DBNull.Value : soHieuCN);
                    cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCap.Checked ? (object)ngayCap : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgayHetHan", dtpNgayHetHan.Checked ? (object)ngayHetHan : DBNull.Value);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}