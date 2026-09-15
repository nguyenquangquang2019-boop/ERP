using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ERP
{
    public partial class QuanLyNhaCungCap : Form
    {
        // Chuỗi kết nối CSDL ERP_BanHang_Full
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
        private DataTable dtNCC;

        public QuanLyNhaCungCap()
        {
            InitializeComponent();
        }

        private void QuanLyNhaCungCap_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            KhoiTaoCotBang();
            LoadDataNhaCungCap();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Mã Nhà Cung Cấp
            DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
            colID.Name = "colID_NCC";
            colID.HeaderText = "MÃ NCC";
            colID.DataPropertyName = "ID_NCC";
            colID.FillWeight = 12;
            colID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colID.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colID);

            // 2. Tên Nhà Cung Cấp
            DataGridViewTextBoxColumn colTen = new DataGridViewTextBoxColumn();
            colTen.Name = "colTenNCC";
            colTen.HeaderText = "TÊN NHÀ CUNG CẤP";
            colTen.DataPropertyName = "TenNCC";
            colTen.FillWeight = 28;
            colTen.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTen);

            // 3. Số Điện Thoại
            DataGridViewTextBoxColumn colSDT = new DataGridViewTextBoxColumn();
            colSDT.Name = "colSDT";
            colSDT.HeaderText = "SỐ ĐIỆN THOẠI";
            colSDT.DataPropertyName = "SDT";
            colSDT.FillWeight = 15;
            colSDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSDT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colSDT);

            // 4. Địa Chỉ
            DataGridViewTextBoxColumn colDiaChi = new DataGridViewTextBoxColumn();
            colDiaChi.Name = "colDiaChi";
            colDiaChi.HeaderText = "ĐỊA CHỈ";
            colDiaChi.DataPropertyName = "DiaChi";
            colDiaChi.FillWeight = 30;
            colDiaChi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colDiaChi);

            // 5. Tên Chứng Nhận (Nếu có)
            DataGridViewTextBoxColumn colChungNhan = new DataGridViewTextBoxColumn();
            colChungNhan.Name = "colTenChungNhan";
            colChungNhan.HeaderText = "CHỨNG NHẬN";
            colChungNhan.DataPropertyName = "TenChungNhan";
            colChungNhan.FillWeight = 15;
            colChungNhan.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colChungNhan);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataNhaCungCap()
        {
<<<<<<< HEAD
            // Lấy đúng các cột có trong bảng NhaCungCap của bạn
            string query = @"SELECT ID_NCC, TenNCC, DiaChi, SDT, 
                            ISNULL(TenChungNhan, N'Không có') AS TenChungNhan, 
                            ISNULL(SoHieuCN, N'') AS SoHieuCN, 
                            NgayCap, NgayHetHan 
                     FROM NhaCungCap 
                     ORDER BY ID_NCC DESC";
=======
            string query = "SELECT ID_NCC, TenNCC, DiaChi, SDT, ISNULL(TenChungNhan, N'Không có') AS TenChungNhan FROM NhaCungCap ORDER BY ID_NCC DESC";
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtNCC = new DataTable();
                    da.Fill(dtNCC);

                    dgvData.DataSource = dtNCC;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu nhà cung cấp: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TÌM KIẾM VÀ LỌC DỮ LIỆU
        // ==========================================

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LocDuLieu();
        }

        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocDuLieu();
        }

        private void LocDuLieu()
        {
            if (dtNCC == null) return;

<<<<<<< HEAD
            // Lấy từ khóa tìm kiếm và chống lỗi cú pháp SQL injection cho DataView RowFilter
            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã, Tên nhà cung cấp, SĐT..." || string.IsNullOrEmpty(keyword))
            {
                keyword = "";
            }

            DataView dv = dtNCC.DefaultView;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Cho phép tìm kiếm linh hoạt trên Mã, Tên, SĐT, Địa chỉ hoặc Tên chứng nhận
                dv.RowFilter = $"ID_NCC LIKE '%{keyword}%' OR TenNCC LIKE '%{keyword}%' OR SDT LIKE '%{keyword}%' OR DiaChi LIKE '%{keyword}%' OR TenChungNhan LIKE '%{keyword}%'";
            }
            else
            {
                dv.RowFilter = ""; // Hiển thị toàn bộ nếu không nhập từ khóa
            }

=======
            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã, Tên nhà cung cấp, SĐT...") keyword = "";

            DataView dv = dtNCC.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (ID_NCC LIKE '%{keyword}%' OR TenNCC LIKE '%{keyword}%' OR SDT LIKE '%{keyword}%' OR DiaChi LIKE '%{keyword}%')";
            }

            dv.RowFilter = filter;
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
            dgvData.DataSource = dv;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm theo Mã, Tên nhà cung cấp, SĐT...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm theo Mã, Tên nhà cung cấp, SĐT...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // ==========================================
        // THÊM, SỬA, XÓA NHÀ CUNG CẤP
        // ==========================================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm Nhà cung cấp mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
<<<<<<< HEAD
            FrmThemNhaCungCap frmThem = new FrmThemNhaCungCap();

            // Nếu thêm dữ liệu thành công (DialogResult.OK), hệ thống sẽ tự động load lại bảng dữ liệu
            if (frmThem.ShowDialog() == DialogResult.OK)
            {
                LoadDataNhaCungCap();
            }
=======
            // Gọi Form Thêm NCC tại đây nếu có
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string idNCC = dgvData.CurrentRow.Cells["colID_NCC"].Value?.ToString();
<<<<<<< HEAD

                // Kiểm tra xem mã lấy ra có rỗng không trước khi mở form
                if (!string.IsNullOrEmpty(idNCC))
                {
                    // Gọi Form Sửa NCC và truyền vào idNCC
                    FrmSuaNhaCungCap frmSua = new FrmSuaNhaCungCap(idNCC);

                    // Nếu người dùng bấm lưu thành công (DialogResult.OK), load lại bảng dữ liệu
                    if (frmSua.ShowDialog() == DialogResult.OK)
                    {
                        LoadDataNhaCungCap(); // Thay tên hàm load dữ liệu danh sách NCC của bạn vào đây
                    }
                }
=======
                MessageBox.Show($"Chỉnh sửa nhà cung cấp: {idNCC}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Gọi Form Sửa NCC tại đây nếu có
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp cần chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            // 1. Kiểm tra xem người dùng đã chọn dòng nào trên bảng chưa
            if (dgvData.CurrentRow != null)
            {
                // Lấy mã NCC từ dòng đang chọn
                string idNCC = dgvData.CurrentRow.Cells["colID_NCC"].Value?.ToString();
                string tenNCC = dgvData.CurrentRow.Cells["colTenNCC"].Value?.ToString(); // Tên hiển thị để cảnh báo rõ hơn (nếu có cột này)

                // 2. Hiện hộp thoại xác nhận trước khi xóa
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhà cung cấp '{tenNCC} ({idNCC})' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
=======
            if (dgvData.CurrentRow != null)
            {
                string idNCC = dgvData.CurrentRow.Cells["colID_NCC"].Value?.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa Nhà cung cấp {idNCC}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
                    string query = "DELETE FROM NhaCungCap WHERE ID_NCC = @ID_NCC";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ID_NCC", idNCC);
<<<<<<< HEAD

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Gọi lại hàm load dữ liệu bảng của bạn ở đây (Thay 'LoadDanhSachNCC' bằng tên hàm thật của bạn)
                                LoadDataNhaCungCap();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhà cung cấp cần xóa trong cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (SqlException ex)
                        {
                            // Lỗi mã 547 thường do vướng khóa ngoại (Foreign Key) nếu nhà cung cấp này đã phát sinh giao dịch/hóa đơn
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Không thể xóa nhà cung cấp này vì đã phát sinh dữ liệu liên quan (như phiếu nhập hàng, sản phẩm,...).", "Lỗi ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
=======
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataNhaCungCap();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa nhà cung cấp này do có liên kết sản phẩm: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
                        }
                    }
                }
            }
            else
            {
<<<<<<< HEAD
                MessageBox.Show("Vui lòng chọn Nhà cung cấp cần xóa trên bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
=======
                MessageBox.Show("Vui lòng chọn Nhà cung cấp cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
            }
        }

        // ==========================================
        // ĐIỀU HƯỚNG SIDEBAR - PHÂN HỆ VẬN CHUYỂN
        // ==========================================

        private void btnQuanLyXe_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyXe frmXe = new QuanLyXe();
            frmXe.ShowDialog();
            this.Close();
        }

        private void btnQuanLyDiemVanChuyen_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyDiemVanChuyen frmDVC = new QuanLyDiemVanChuyen();
            frmDVC.ShowDialog();
            this.Close();
        }

        private void btnQuanLyTraHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyTraHang frmTraHang = new QuanLyTraHang();
            frmTraHang.ShowDialog();
            this.Close();
        }

        private void btnQuanLyDonVanChuyen_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyDonVanChuyen frmDonVC = new QuanLyDonVanChuyen();
            frmDonVC.ShowDialog();
            this.Close();
        }
    }
}