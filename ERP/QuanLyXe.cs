using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ERP
{
    public partial class QuanLyXe : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ERP_BanHang_Full;Integrated Security=True";
        private DataTable dtXe;

        public QuanLyXe()
        {
            InitializeComponent();
        }

        private void QuanLyXe_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            if (cboVehicleType.Items.Count > 0) cboVehicleType.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;

            KhoiTaoCotBang();
            LoadDataXe();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Biển Số Xe (Khóa chính BienSoXe)
            DataGridViewTextBoxColumn colBienSo = new DataGridViewTextBoxColumn();
            colBienSo.Name = "colBienSoXe";
            colBienSo.HeaderText = "BIỂN SỐ XE";
            colBienSo.DataPropertyName = "BienSoXe";
            colBienSo.FillWeight = 15;
            colBienSo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colBienSo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colBienSo.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colBienSo.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colBienSo);

            // 2. Loại Xe (LoaiXe)
            DataGridViewTextBoxColumn colLoaiXe = new DataGridViewTextBoxColumn();
            colLoaiXe.Name = "colLoaiXe";
            colLoaiXe.HeaderText = "LOẠI XE";
            colLoaiXe.DataPropertyName = "LoaiXe";
            colLoaiXe.FillWeight = 18;
            colLoaiXe.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colLoaiXe);

            // 3. Tải Trọng (TaiTrong)
            DataGridViewTextBoxColumn colTaiTrong = new DataGridViewTextBoxColumn();
            colTaiTrong.Name = "colTaiTrong";
            colTaiTrong.HeaderText = "TẢI TRỌNG (TẤN)";
            colTaiTrong.DataPropertyName = "TaiTrong";
            colTaiTrong.FillWeight = 15;
            colTaiTrong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTaiTrong.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTaiTrong.DefaultCellStyle.Format = "N2";
            dgvData.Columns.Add(colTaiTrong);

            // 4. Tên Tài Xế (TenTaiXe)
            DataGridViewTextBoxColumn colTaiXe = new DataGridViewTextBoxColumn();
            colTaiXe.Name = "colTenTaiXe";
            colTaiXe.HeaderText = "TÊN TÀI XẾ";
            colTaiXe.DataPropertyName = "TenTaiXe";
            colTaiXe.FillWeight = 22;
            colTaiXe.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvData.Columns.Add(colTaiXe);

            // 5. Trạng Thái Xe (TrangThaiXe)
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.Name = "colTrangThaiXe";
            colTrangThai.HeaderText = "TRẠNG THÁI";
            colTrangThai.DataPropertyName = "TrangThaiXe";
            colTrangThai.FillWeight = 18;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvData.Columns.Add(colTrangThai);

            // 6. Kích Hoạt (KichHoat - Hiển thị dạng CheckBox hoặc Text)
            DataGridViewCheckBoxColumn colKichHoat = new DataGridViewCheckBoxColumn();
            colKichHoat.Name = "colKichHoat";
            colKichHoat.HeaderText = "KÍCH HOẠT";
            colKichHoat.DataPropertyName = "KichHoat";
            colKichHoat.FillWeight = 12;
            colKichHoat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colKichHoat.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colKichHoat);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataXe()
        {
            // Truy vấn lấy dữ liệu chính xác từ bảng PhuongTien
            string query = "SELECT BienSoXe, LoaiXe, TaiTrong, TenTaiXe, TrangThaiXe, KichHoat FROM PhuongTien ORDER BY BienSoXe ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtXe = new DataTable();
                    da.Fill(dtXe);

                    dgvData.DataSource = dtXe;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách phương tiện: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void cboVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocDuLieu();
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocDuLieu();
        }

        private void LocDuLieu()
        {
            if (dtXe == null) return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm chủ xe, biển số, SĐT..." || keyword == "🔍 Tìm kiếm phương tiện, tài xế...") keyword = "";

            DataView dv = dtXe.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (BienSoXe LIKE '%{keyword}%' OR LoaiXe LIKE '%{keyword}%' OR TenTaiXe LIKE '%{keyword}%')";
            }

            // Lọc theo loại xe nếu ComboBox chọn loại xe cụ thể
            if (cboVehicleType.SelectedIndex > 0)
            {
                string selectedType = cboVehicleType.SelectedItem.ToString();
                filter += $" AND LoaiXe = '{selectedType}'";
            }

            // Lọc theo trạng thái xe
            if (cboStatus.SelectedIndex > 0)
            {
                string selectedStatus = cboStatus.SelectedItem.ToString();
                filter += $" AND TrangThaiXe = '{selectedStatus}'";
            }

            dv.RowFilter = filter;
            dgvData.DataSource = dv;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm chủ xe, biển số, SĐT..." || txtSearch.Text == "🔍 Tìm kiếm phương tiện, tài xế...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm phương tiện, tài xế...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // ==========================================
        // THÊM, SỬA, XÓA PHƯƠNG TIỆN
        // ==========================================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở Form thêm Phương tiện mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string bienSo = dgvData.CurrentRow.Cells["colBienSoXe"].Value?.ToString();
                MessageBox.Show($"Chỉnh sửa thông tin phương tiện biển số: {bienSo}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn xe cần chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string bienSo = dgvData.CurrentRow.Cells["colBienSoXe"].Value?.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa phương tiện biển số [{bienSo}]?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // Truy vấn xóa từ bảng PhuongTien
                    string query = "DELETE FROM PhuongTien WHERE BienSoXe = @BienSoXe";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@BienSoXe", bienSo);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa phương tiện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataXe();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa phương tiện này do đang gắn liền với các Đơn vận chuyển: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn xe cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ==========================================
        // ĐIỀU HƯỚNG SIDEBAR
        // ==========================================

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

        private void btnQuanLyDonVanChuyen_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyDonVanChuyen frmDonVC = new QuanLyDonVanChuyen();
            frmDonVC.ShowDialog();
            this.Close();
        }
    }
}