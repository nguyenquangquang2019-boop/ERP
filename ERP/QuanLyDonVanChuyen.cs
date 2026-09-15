using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ERP
{
    public partial class QuanLyDonVanChuyen : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
        private DataTable dtDonVC;

        public QuanLyDonVanChuyen()
        {
            InitializeComponent();
        }

        private void QuanLyDonVanChuyen_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            if (cmbTrangThai.Items.Count > 0)
            {
                cmbTrangThai.SelectedIndex = 0;
            }

            KhoiTaoCotBang();
            LoadDataDonVanChuyen();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Mã Đơn Vận Chuyển (ID_DonVC)
            DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
            colID.Name = "colID_DonVC";
            colID.HeaderText = "MÃ ĐƠN VC";
            colID.DataPropertyName = "ID_DonVC";
            colID.FillWeight = 12;
            colID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colID.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colID.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colID);

            // 2. Biển Số Xe (BienSoXe)
            DataGridViewTextBoxColumn colXe = new DataGridViewTextBoxColumn();
            colXe.Name = "colBienSoXe";
            colXe.HeaderText = "BIỂN SỐ XE";
            colXe.DataPropertyName = "BienSoXe";
            colXe.FillWeight = 14;
            colXe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colXe.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colXe);

            // 3. Tên Điểm Vận Chuyển (TenDVC - JOIN từ DiemVanChuyen)
            DataGridViewTextBoxColumn colTenDVC = new DataGridViewTextBoxColumn();
            colTenDVC.Name = "colTenDVC";
            colTenDVC.HeaderText = "ĐỊA ĐIỂM GIAO HÀNG";
            colTenDVC.DataPropertyName = "TenDVC";
            colTenDVC.FillWeight = 25;
            colTenDVC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTenDVC);

            // 4. Mã Sản Phẩm (ID_SP - chuẩn theo Schema CSDL)
            DataGridViewTextBoxColumn colSP = new DataGridViewTextBoxColumn();
            colSP.Name = "colID_SP";
            colSP.HeaderText = "MÃ SẢN PHẨM";
            colSP.DataPropertyName = "ID_SP";
            colSP.FillWeight = 14;
            colSP.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSP.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colSP);

            // 5. Số Lượng Giao (SoLuongGiao)
            DataGridViewTextBoxColumn colSL = new DataGridViewTextBoxColumn();
            colSL.Name = "colSoLuongGiao";
            colSL.HeaderText = "SL GIAO";
            colSL.DataPropertyName = "SoLuongGiao";
            colSL.FillWeight = 10;
            colSL.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSL.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colSL);

            // 6. Thời Gian Khởi Hành (ThoiGianKhoiHanh)
            DataGridViewTextBoxColumn colNgay = new DataGridViewTextBoxColumn();
            colNgay.Name = "colThoiGianKhoiHanh";
            colNgay.HeaderText = "THỜI GIAN KHỞI HÀNH";
            colNgay.DataPropertyName = "ThoiGianKhoiHanh";
            colNgay.FillWeight = 18;
            colNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgay.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvData.Columns.Add(colNgay);

            // 7. Trạng Thái Đơn (TrangThaiDon)
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.Name = "colTrangThaiDon";
            colTrangThai.HeaderText = "TRẠNG THÁI";
            colTrangThai.DataPropertyName = "TrangThaiDon";
            colTrangThai.FillWeight = 15;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvData.Columns.Add(colTrangThai);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataDonVanChuyen()
        {
            // Truy vấn lấy dữ liệu chuẩn theo đúng tên cột trong CREATE TABLE DonVanChuyen
            string query = @"
                SELECT 
                    DVC.ID_DonVC,
                    DVC.BienSoXe,
                    DVC.MaDVC,
                    ISNULL(DMC.TenDVC, DVC.MaDVC) AS TenDVC,
                    DVC.ID_SP,
                    DVC.SoLuongGiao,
                    DVC.ThoiGianKhoiHanh,
                    DVC.TrangThaiDon
                FROM DonVanChuyen DVC
                LEFT JOIN DiemVanChuyen DMC ON DVC.MaDVC = DMC.MaDVC
                ORDER BY DVC.ThoiGianKhoiHanh DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtDonVC = new DataTable();
                    da.Fill(dtDonVC);

                    dgvData.DataSource = dtDonVC;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách đơn vận chuyển: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dtDonVC == null) return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã đơn, Mã xe, Địa điểm...") keyword = "";

            DataView dv = dtDonVC.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (ID_DonVC LIKE '%{keyword}%' OR BienSoXe LIKE '%{keyword}%' OR TenDVC LIKE '%{keyword}%' OR ID_SP LIKE '%{keyword}%')";
            }

            if (cmbTrangThai.SelectedIndex > 0)
            {
                string selectedStatus = cmbTrangThai.SelectedItem.ToString();
                filter += $" AND TrangThaiDon = '{selectedStatus}'";
            }

            dv.RowFilter = filter;
            dgvData.DataSource = dv;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm theo Mã đơn, Mã xe, Địa điểm...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm theo Mã đơn, Mã xe, Địa điểm...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // ==========================================
        // CÁC NÚT HÀNH ĐỘNG
        // ==========================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở màn hình tạo Đơn vận chuyển mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string id = dgvData.CurrentRow.Cells["colID_DonVC"].Value?.ToString();
                MessageBox.Show($"Chỉnh sửa đơn vận chuyển mã: {id}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn vận chuyển cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string id = dgvData.CurrentRow.Cells["colID_DonVC"].Value?.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa đơn vận chuyển [{id}]?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string query = "DELETE FROM DonVanChuyen WHERE ID_DonVC = @ID_DonVC";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ID_DonVC", id);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataDonVanChuyen();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa đơn vận chuyển này: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn vận chuyển cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string id = dgvData.CurrentRow.Cells["colID_DonVC"].Value?.ToString();
                string statusHienTai = dgvData.CurrentRow.Cells["colTrangThaiDon"].Value?.ToString();

                // Chuyển đổi trạng thái theo CHECK constraint: Khởi tạo -> Đang vận chuyển -> Hoàn thành
                string statusMoi = "Đang vận chuyển";
                if (statusHienTai == "Khởi tạo") statusMoi = "Đang vận chuyển";
                else if (statusHienTai == "Đang vận chuyển") statusMoi = "Hoàn thành";
                else if (statusHienTai == "Hoàn thành") statusMoi = "Khởi tạo";

                string query = "UPDATE DonVanChuyen SET TrangThaiDon = @TrangThaiDon WHERE ID_DonVC = @ID_DonVC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@TrangThaiDon", statusMoi);
                        cmd.Parameters.AddWithValue("@ID_DonVC", id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Đã cập nhật trạng thái Đơn VC [{id}] sang: [{statusMoi}]", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataDonVanChuyen();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn vận chuyển cần cập nhật trạng thái!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnQuanLyTraHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyTraHang frmTraHang = new QuanLyTraHang();
            frmTraHang.ShowDialog();
            this.Close();
        }
    }
}