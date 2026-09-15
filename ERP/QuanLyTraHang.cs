using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ERP
{
    public partial class QuanLyTraHang : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
        private DataTable dtPhieuTra;

        public QuanLyTraHang()
        {
            InitializeComponent();
        }

        private void QuanLyTraHang_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            KhoiTaoCotBang();
            LoadDataPhieuTraHang();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Mã Phiếu Trả
            DataGridViewTextBoxColumn colIDPT = new DataGridViewTextBoxColumn();
            colIDPT.Name = "colID_PhieuTra";
            colIDPT.HeaderText = "MÃ PHIẾU TRẢ";
            colIDPT.DataPropertyName = "ID_PhieuTra";
            colIDPT.FillWeight = 12;
            colIDPT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDPT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDPT.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colIDPT.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colIDPT);

            // 2. Mã Yêu Cầu (Bán Hàng)
            DataGridViewTextBoxColumn colIDYC = new DataGridViewTextBoxColumn();
            colIDYC.Name = "colID_YC";
            colIDYC.HeaderText = "MÃ YC BÁN HÀNG";
            colIDYC.DataPropertyName = "ID_YC";
            colIDYC.FillWeight = 14;
            colIDYC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDYC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colIDYC);

            // 3. Tên Sản Phẩm Lỗi
            DataGridViewTextBoxColumn colTenHang = new DataGridViewTextBoxColumn();
            colTenHang.Name = "colTenHang";
            colTenHang.HeaderText = "SẢN PHẨM THU HỒI";
            colTenHang.DataPropertyName = "TenHang";
            colTenHang.FillWeight = 24;
            colTenHang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTenHang);

            // 4. Số Lượng
            DataGridViewTextBoxColumn colSoLuong = new DataGridViewTextBoxColumn();
            colSoLuong.Name = "colSoLuong";
            colSoLuong.HeaderText = "SL LỖI";
            colSoLuong.DataPropertyName = "SoLuong";
            colSoLuong.FillWeight = 10;
            colSoLuong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSoLuong.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colSoLuong);

            // 5. Khách Hàng Yêu Cầu
            DataGridViewTextBoxColumn colTenKH = new DataGridViewTextBoxColumn();
            colTenKH.Name = "colTenDoanhNghiep";
            colTenKH.HeaderText = "KHÁCH HÀNG";
            colTenKH.DataPropertyName = "TenDoanhNghiep";
            colTenKH.FillWeight = 22;
            colTenKH.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTenKH);

            // 6. Trạng Thái Bên Bán Hàng Nắm
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.Name = "colTrangThai";
            colTrangThai.HeaderText = "TRẠNG THÁI PHÂN HỆ BH";
            colTrangThai.DataPropertyName = "TrangThai";
            colTrangThai.FillWeight = 18;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvData.Columns.Add(colTrangThai);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataPhieuTraHang()
        {
            // Query lấy dữ liệu yêu cầu từ phân hệ Bán Hàng đã đẩy sang PhieuTraHang
            string query = @"
                SELECT 
                    PT.ID_PhieuTra,
                    YC.ID_YC,
                    HH.TenHang,
                    CTYC.SoLuong,
                    KH.TenDoanhNghiep,
                    YC.TrangThai
                FROM PhieuTraHang PT
                INNER JOIN ChiTietYeuCau CTYC ON PT.ID_CTYC = CTYC.ID_CTYC
                INNER JOIN YeuCauSauBanHang YC ON CTYC.ID_YC = YC.ID_YC
                INNER JOIN SanPham SP ON CTYC.ID_SP = SP.ID_SP
                INNER JOIN HangHoa HH ON SP.MaHang = HH.MaHang
                INNER JOIN KhachHang KH ON YC.ID_KH = KH.ID_KH
                ORDER BY PT.ID_PhieuTra DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtPhieuTra = new DataTable();
                    da.Fill(dtPhieuTra);

                    dgvData.DataSource = dtPhieuTra;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải phiếu trả hàng: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // XÁC NHẬN TIẾP NHẬN XỬ LÝ VẬN CHUYỂN
        // ==========================================
        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu trả hàng cần xác nhận!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maYC = dgvData.CurrentRow.Cells["colID_YC"].Value?.ToString();
            string trangThaiHienTai = dgvData.CurrentRow.Cells["colTrangThai"].Value?.ToString();

            if (trangThaiHienTai == "Đã nhận xử lý vận chuyển")
            {
                MessageBox.Show("Yêu cầu trả hàng này đã được xác nhận tiếp nhận vận chuyển trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show(
                $"Xác nhận tiếp nhận xử lý vận chuyển cho yêu cầu [{maYC}]?\nTrạng thái này sẽ ngay lập tức cập nhật báo về cho Phân hệ Bán Hàng.",
                "Xác nhận chuyển giao",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr == DialogResult.Yes)
            {
                // Cập nhật trạng thái YeuCauSauBanHang thành 'Đã nhận xử lý vận chuyển'
                string updateQuery = "UPDATE YeuCauSauBanHang SET TrangThai = N'Đã nhận xử lý vận chuyển' WHERE ID_YC = @ID_YC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@ID_YC", maYC);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Đã xác nhận thành công! Phân hệ Bán hàng đã ghi nhận trạng thái: [Đã nhận xử lý vận chuyển] cho mã YC {maYC}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDataPhieuTraHang();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            if (dtPhieuTra == null) return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...") keyword = "";

            DataView dv = dtPhieuTra.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (ID_PhieuTra LIKE '%{keyword}%' OR ID_YC LIKE '%{keyword}%' OR TenHang LIKE '%{keyword}%' OR TenDoanhNghiep LIKE '%{keyword}%')";
            }

            if (cmbTrangThai.SelectedIndex > 0)
            {
                string selectedStatus = cmbTrangThai.SelectedItem.ToString();
                if (selectedStatus == "Đang xử lý (Chờ nhận)")
                {
                    filter += " AND TrangThai = 'Đang xử lý'";
                }
                else if (selectedStatus == "Đã nhận xử lý vận chuyển")
                {
                    filter += " AND TrangThai = 'Đã nhận xử lý vận chuyển'";
                }
            }

            dv.RowFilter = filter;
            dgvData.DataSource = dv;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // ==========================================
        // ĐIỀU HƯỚNG SIDEBAR
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

        private void btnQuanLyDiemVanChuyen_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyDiemVanChuyen frmDVC = new QuanLyDiemVanChuyen();
            frmDVC.ShowDialog();
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