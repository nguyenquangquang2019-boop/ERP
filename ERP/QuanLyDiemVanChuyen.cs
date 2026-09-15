using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ERP
{
    public partial class QuanLyDiemVanChuyen : Form
    {
        // Chuỗi kết nối CSDL SQL Server ERP_BanHang_Full
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
        private DataTable dtDiemVC;

        public QuanLyDiemVanChuyen()
        {
            InitializeComponent();
        }

        private void QuanLyDiemVanChuyen_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            KhoiTaoCotBang();
            LoadDataDiemVanChuyen();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Mã Điểm Vận Chuyển (MaDVC)
            DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
            colID.Name = "colMaDVC";
            colID.HeaderText = "MÃ ĐIỂM VC";
            colID.DataPropertyName = "MaDVC";
            colID.FillWeight = 15;
            colID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colID.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colID);

            // 2. Tên Điểm Vận Chuyển (TenDVC)
            DataGridViewTextBoxColumn colTen = new DataGridViewTextBoxColumn();
            colTen.Name = "colTenDVC";
            colTen.HeaderText = "TÊN ĐIỂM VẬN CHUYỂN";
            colTen.DataPropertyName = "TenDVC";
            colTen.FillWeight = 25;
            colTen.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTen);

            // 3. Địa Chỉ Chi Tiết (DiaChiDVC)
            DataGridViewTextBoxColumn colDiaChi = new DataGridViewTextBoxColumn();
            colDiaChi.Name = "colDiaChiDVC";
            colDiaChi.HeaderText = "ĐỊA CHỈ CHI TIẾT";
            colDiaChi.DataPropertyName = "DiaChiDVC";
            colDiaChi.FillWeight = 30;
            colDiaChi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colDiaChi);

            // 4. Số Điện Thoại (SDT_DVC)
            DataGridViewTextBoxColumn colSDT = new DataGridViewTextBoxColumn();
            colSDT.Name = "colSDT_DVC";
            colSDT.HeaderText = "SỐ ĐIỆN THOẠI";
            colSDT.DataPropertyName = "SDT_DVC";
            colSDT.FillWeight = 15;
            colSDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSDT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colSDT);

            // 5. Người Đại Diện (TenNguoiDaiDien)
            DataGridViewTextBoxColumn colNguoiDD = new DataGridViewTextBoxColumn();
            colNguoiDD.Name = "colTenNguoiDaiDien";
            colNguoiDD.HeaderText = "NGƯỜI ĐẠI DIỆN";
            colNguoiDD.DataPropertyName = "TenNguoiDaiDien";
            colNguoiDD.FillWeight = 15;
            colNguoiDD.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colNguoiDD);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataDiemVanChuyen()
        {
            // Truy vấn đúng tên 5 cột trong bảng DiemVanChuyen
            string query = "SELECT MaDVC, TenDVC, DiaChiDVC, SDT_DVC, TenNguoiDaiDien FROM DiemVanChuyen ORDER BY MaDVC DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtDiemVC = new DataTable();
                    da.Fill(dtDiemVC);

                    dgvData.DataSource = dtDiemVC;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu điểm vận chuyển: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dtDiemVC == null) return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã điểm, Tên điểm, Địa chỉ...") keyword = "";

            DataView dv = dtDiemVC.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (MaDVC LIKE '%{keyword}%' OR TenDVC LIKE '%{keyword}%' OR DiaChiDVC LIKE '%{keyword}%' OR SDT_DVC LIKE '%{keyword}%' OR TenNguoiDaiDien LIKE '%{keyword}%')";
            }

            dv.RowFilter = filter;
            dgvData.DataSource = dv;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm theo Mã điểm, Tên điểm, Địa chỉ...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm theo Mã điểm, Tên điểm, Địa chỉ...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // ==========================================
        // THÊM, SỬA, XÓA ĐIỂM VẬN CHUYỂN
        // ==========================================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở Form thêm Điểm vận chuyển mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
<<<<<<< HEAD
            FrmThemDiemVanChuyen frmThem = new FrmThemDiemVanChuyen();

            // Nếu thêm dữ liệu thành công (DialogResult.OK), hệ thống sẽ tự động load lại bảng dữ liệu
            if (frmThem.ShowDialog() == DialogResult.OK)
            {
                LoadDataDiemVanChuyen();
            }
=======
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string maDVC = dgvData.CurrentRow.Cells["colMaDVC"].Value?.ToString();
<<<<<<< HEAD

                if (!string.IsNullOrEmpty(maDVC))
                {
                    // Gọi Form Sửa Điểm Vận Chuyển
                    FrmSuaDiemVanChuyen frmSua = new FrmSuaDiemVanChuyen(maDVC);

                    // Nếu bấm Lưu thành công, load lại danh sách
                    if (frmSua.ShowDialog() == DialogResult.OK)
                    {
                        // Thay 'LoadDanhSachDVC()' bằng tên hàm load dữ liệu bảng của bạn (ví dụ: LoadData(), HienThi(),...)
                        LoadDataDiemVanChuyen();
                    }
                }
=======
                MessageBox.Show($"Chỉnh sửa điểm vận chuyển: {maDVC}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Điểm vận chuyển cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            // 1. Kiểm tra xem người dùng đã chọn dòng nào trên bảng chưa
            if (dgvData.CurrentRow != null)
            {
                // Lấy Mã DVC và Tên DVC từ dòng đang chọn (kiểm tra lại tên cột cho khớp với thiết kế của bạn)
                string maDVC = dgvData.CurrentRow.Cells["colMaDVC"].Value?.ToString();
                string tenDVC = dgvData.CurrentRow.Cells["colTenDVC"].Value?.ToString();

                // 2. Hiển thị hộp thoại xác nhận trước khi xóa
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa điểm vận chuyển '{tenDVC} ({maDVC})' không?",
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
                string maDVC = dgvData.CurrentRow.Cells["colMaDVC"].Value?.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa Điểm vận chuyển [{maDVC}]?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
                    string query = "DELETE FROM DiemVanChuyen WHERE MaDVC = @MaDVC";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@MaDVC", maDVC);
<<<<<<< HEAD

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa điểm vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Gọi lại hàm load dữ liệu bảng của bạn ở đây (Thay 'LoadDanhSachDVC' bằng tên hàm thực tế của bạn)
                                LoadDataDiemVanChuyen();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy điểm vận chuyển cần xóa trong cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (SqlException ex)
                        {
                            // Lỗi mã 547: Bị vướng khóa ngoại (Foreign Key) nếu điểm vận chuyển này đã được sử dụng ở bảng khác (như hóa đơn, phiếu vận chuyển,...)
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Không thể xóa điểm vận chuyển này vì đã phát sinh dữ liệu liên quan trong hệ thống.", "Lỗi ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                            MessageBox.Show("Xóa điểm vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataDiemVanChuyen();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa điểm này do đang được sử dụng ở đơn vận chuyển: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
>>>>>>> eed68e83fb15d88b2b64622e9a22619d58157bd4
                        }
                    }
                }
            }
            else
            {
<<<<<<< HEAD
                MessageBox.Show("Vui lòng chọn Điểm vận chuyển cần xóa trên bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
=======
                MessageBox.Show("Vui lòng chọn Điểm vận chuyển cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnQuanLyNhaCungCap_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyNhaCungCap frmNCC = new QuanLyNhaCungCap();
            frmNCC.ShowDialog();
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