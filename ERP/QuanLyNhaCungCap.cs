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
            string query = "SELECT ID_NCC, TenNCC, DiaChi, SDT, ISNULL(TenChungNhan, N'Không có') AS TenChungNhan FROM NhaCungCap ORDER BY ID_NCC DESC";

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

            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (keyword == "🔍 Tìm kiếm theo Mã, Tên nhà cung cấp, SĐT...") keyword = "";

            DataView dv = dtNCC.DefaultView;
            string filter = "1=1";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND (ID_NCC LIKE '%{keyword}%' OR TenNCC LIKE '%{keyword}%' OR SDT LIKE '%{keyword}%' OR DiaChi LIKE '%{keyword}%')";
            }

            dv.RowFilter = filter;
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
            // Gọi Form Thêm NCC tại đây nếu có
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string idNCC = dgvData.CurrentRow.Cells["colID_NCC"].Value?.ToString();
                MessageBox.Show($"Chỉnh sửa nhà cung cấp: {idNCC}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Gọi Form Sửa NCC tại đây nếu có
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp cần chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string idNCC = dgvData.CurrentRow.Cells["colID_NCC"].Value?.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa Nhà cung cấp {idNCC}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string query = "DELETE FROM NhaCungCap WHERE ID_NCC = @ID_NCC";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ID_NCC", idNCC);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataNhaCungCap();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa nhà cung cấp này do có liên kết sản phẩm: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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