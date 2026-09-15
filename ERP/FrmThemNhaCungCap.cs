using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ERP
{
    public partial class FrmThemNhaCungCap : Form
    {
        // Chuỗi kết nối đến cơ sở dữ liệu ERP_BanHang_Full
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";

        public FrmThemNhaCungCap()
        {
            InitializeComponent();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra các trường dữ liệu bắt buộc (Not Null trong CSDL)
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtTenNCC.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin bắt buộc (Mã NCC, Tên NCC, Địa chỉ, SĐT)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sdt = txtSDT.Text.Trim();

            // 2. Kiểm tra số điện thoại phải đủ đúng 10 chữ số
            // Dùng Regex kiểm tra chuỗi chỉ bao gồm từ 0 đến 9 và có độ dài chính xác là 10
            if (!Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải bao gồm đúng 10 chữ số.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                txtSDT.SelectAll();
                return;
            }

            string idNcc = txtID.Text.Trim();
            string tenNcc = txtTenNCC.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string tenCN = txtTenCN.Text.Trim();
            string soHieuCN = txtSoHieuCN.Text.Trim();
            DateTime ngayCap = dtpNgayCap.Value;
            DateTime ngayHetHan = dtpNgayHetHan.Value;

            // 3. Câu lệnh SQL INSERT (Không còn FileScanCN)
            string query = @"INSERT INTO NhaCungCap 
                             (ID_NCC, TenNCC, DiaChi, SDT, TenChungNhan, SoHieuCN, NgayCap, NgayHetHan) 
                             VALUES 
                             (@ID_NCC, @TenNCC, @DiaChi, @SDT, @TenChungNhan, @SoHieuCN, @NgayCap, @NgayHetHan)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Truyền tham số an toàn chống SQL Injection
                    cmd.Parameters.AddWithValue("@ID_NCC", idNcc);
                    cmd.Parameters.AddWithValue("@TenNCC", tenNcc);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@SDT", sdt);

                    // Xử lý các giá trị có thể để trống (NULL)
                    cmd.Parameters.AddWithValue("@TenChungNhan", string.IsNullOrEmpty(tenCN) ? (object)DBNull.Value : tenCN);
                    cmd.Parameters.AddWithValue("@SoHieuCN", string.IsNullOrEmpty(soHieuCN) ? (object)DBNull.Value : soHieuCN);
                    cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCap.Checked ? (object)ngayCap : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgayHetHan", dtpNgayHetHan.Checked ? (object)ngayHetHan : DBNull.Value);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Lỗi trùng khóa chính (Primary Key)
                    {
                        MessageBox.Show($"Mã nhà cung cấp '{idNcc}' đã tồn tại trong hệ thống. Vui lòng nhập mã khác!", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}