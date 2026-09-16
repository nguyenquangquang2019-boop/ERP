using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERP.DucAnh.BLL;
using ERP.DucAnh.DTO;

namespace ERP
{
    public partial class QuanLyDonVanChuyen : Form
    {
        private readonly DonVanChuyenBLL bll = new DonVanChuyenBLL();

        // Các control nhập liệu và tác vụ
        public TextBox txtIDDonVC;
        public ComboBox cboBienSoXe;
        public ComboBox cboMaDVC;
        public ComboBox cboSanPham;
        public TextBox txtSoLuong;
        public DateTimePicker dtpThoiGianKhoiHanh;
        public ComboBox cboTrangThaiDon;
        public Button btnExport;
        private Panel pnlInputCard;

        public QuanLyDonVanChuyen()
        {
            InitializeComponent();
        }

        private void QuanLyDonVanChuyen_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            InitInputControls();
            KhoiTaoCotBang();

            // 1. Gọi BLL.LayDanhSach() → bind vào DataGridView
            LoadDataDonVanChuyen();

            // 2. Gọi BLL.LayDanhSachXeKhaDung() → nạp vào ComboBox biển số xe
            LoadDanhSachXeKhaDung();

            // 3. Gọi BLL.LayDanhSachDVC() → nạp vào ComboBox điểm vận chuyển
            LoadDanhSachDVC();

            // 4. Gọi BLL.LayDanhSachSanPham() → nạp vào ComboBox sản phẩm
            LoadDanhSachSanPham();

            // 5. Nạp ['Khởi tạo', 'Đang vận chuyển', 'Hoàn thành', 'Đã hủy'] vào ComboBox trạng thái
            LoadTrangThaiComboBoxes();
        }

        private void InitInputControls()
        {
            if (pnlInputCard != null) return;

            // Nút Xuất File trên thanh công cụ
            btnExport = new Button();
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BackColor = Color.FromArgb(108, 117, 125);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(370, 10);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(140, 35);
            btnExport.TabIndex = 9;
            btnExport.Text = "📥 Xuất File";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += new EventHandler(this.btnExport_Click);
            pnlActionTool.Controls.Add(btnExport);

            // Panel nhập liệu chi tiết
            pnlInputCard = new Panel();
            pnlInputCard.BackColor = Color.White;
            pnlInputCard.Dock = DockStyle.Top;
            pnlInputCard.Height = 105;
            pnlInputCard.Padding = new Padding(15, 10, 15, 10);

            // Hàng 1: Mã Đơn VC, Biển Số Xe, Điểm VC, Sản Phẩm
            Label lblID = new Label { Text = "Mã Đơn VC:", Location = new Point(15, 15), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            txtIDDonVC = new TextBox { Location = new Point(105, 12), Width = 110, Font = new Font("Segoe UI", 9F) };

            Label lblXe = new Label { Text = "Biển Số Xe:", Location = new Point(230, 15), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cboBienSoXe = new ComboBox { Location = new Point(315, 12), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            Label lblDVC = new Label { Text = "Điểm VC:", Location = new Point(470, 15), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cboMaDVC = new ComboBox { Location = new Point(545, 12), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            Label lblSP = new Label { Text = "Sản Phẩm:", Location = new Point(710, 15), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cboSanPham = new ComboBox { Location = new Point(790, 12), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            // Hàng 2: Số Lượng, Thời Gian KH, Trạng Thái, Nút Làm mới
            Label lblSL = new Label { Text = "Số Lượng:", Location = new Point(15, 55), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            txtSoLuong = new TextBox { Location = new Point(105, 52), Width = 110, Font = new Font("Segoe UI", 9F) };

            Label lblTG = new Label { Text = "TG Khởi Hành:", Location = new Point(230, 55), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            dtpThoiGianKhoiHanh = new DateTimePicker
            {
                Location = new Point(315, 52),
                Width = 140,
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm"
            };

            Label lblTT = new Label { Text = "Trạng Thái:", Location = new Point(470, 55), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cboTrangThaiDon = new ComboBox { Location = new Point(545, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            Button btnLamMoi = new Button
            {
                Text = "🧹 Làm Mới Form",
                Location = new Point(790, 50),
                Width = 150,
                Height = 30,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(230, 235, 245),
                FlatStyle = FlatStyle.Flat
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += (s, e) => ClearInputs();

            pnlInputCard.Controls.AddRange(new Control[] {
                lblID, txtIDDonVC,
                lblXe, cboBienSoXe,
                lblDVC, cboMaDVC,
                lblSP, cboSanPham,
                lblSL, txtSoLuong,
                lblTG, dtpThoiGianKhoiHanh,
                lblTT, cboTrangThaiDon,
                btnLamMoi
            });

            // Gắn vào pnlMainContent bên dưới pnlActionTool và phía trên dgvData
            pnlMainContent.Controls.Remove(dgvData);
            pnlMainContent.Controls.Add(dgvData);
            pnlMainContent.Controls.Add(pnlInputCard);

            // Gán sự kiện CellClick cho DataGridView
            dgvData.CellClick += new DataGridViewCellEventHandler(this.dgvData_CellClick);
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
            colID.FillWeight = 14;
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

            // 3. Mã Điểm Vận Chuyển (MaDVC)
            DataGridViewTextBoxColumn colMaDVC = new DataGridViewTextBoxColumn();
            colMaDVC.Name = "colMaDVC";
            colMaDVC.HeaderText = "MÃ ĐIỂM VC";
            colMaDVC.DataPropertyName = "MaDVC";
            colMaDVC.FillWeight = 16;
            colMaDVC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMaDVC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colMaDVC);

            // 4. Mã Sản Phẩm (ID_SP)
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
            colTrangThai.FillWeight = 14;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvData.Columns.Add(colTrangThai);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==========================================
        // TẢI DỮ LIỆU VÀ NẠP DANH MỤC
        // ==========================================

        private void LoadDataDonVanChuyen()
        {
            try
            {
                List<DonVanChuyen> list = bll.LayDanhSach();
                BindData(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách đơn vận chuyển: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindData(List<DonVanChuyen> list)
        {
            dgvData.DataSource = new BindingSource { DataSource = list };
        }

        private void LoadDanhSachXeKhaDung()
        {
            try
            {
                List<string> dsXe = bll.LayDanhSachXeKhaDung();
                cboBienSoXe.Items.Clear();
                foreach (string xe in dsXe)
                {
                    cboBienSoXe.Items.Add(xe);
                }
                if (cboBienSoXe.Items.Count > 0)
                {
                    cboBienSoXe.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách xe khả dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachDVC()
        {
            try
            {
                List<string> dsDVC = bll.LayDanhSachDVC();
                cboMaDVC.Items.Clear();
                foreach (string dvc in dsDVC)
                {
                    cboMaDVC.Items.Add(dvc);
                }
                if (cboMaDVC.Items.Count > 0)
                {
                    cboMaDVC.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách điểm vận chuyển: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachSanPham()
        {
            try
            {
                List<string> dsSP = bll.LayDanhSachSanPham();
                cboSanPham.Items.Clear();
                foreach (string sp in dsSP)
                {
                    cboSanPham.Items.Add(sp);
                }
                if (cboSanPham.Items.Count > 0)
                {
                    cboSanPham.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrangThaiComboBoxes()
        {
            string[] trangThais = { "Khởi tạo", "Đang vận chuyển", "Hoàn thành", "Đã hủy" };

            // ComboBox nhập liệu
            cboTrangThaiDon.Items.Clear();
            cboTrangThaiDon.Items.AddRange(trangThais);
            if (cboTrangThaiDon.Items.Count > 0)
            {
                cboTrangThaiDon.SelectedIndex = 0;
            }

            // ComboBox lọc
            cmbTrangThai.Items.Clear();
            cmbTrangThai.Items.Add("Tất cả trạng thái");
            cmbTrangThai.Items.AddRange(trangThais);
            cmbTrangThai.SelectedIndex = 0;
        }

        private void ClearInputs()
        {
            txtIDDonVC.Text = string.Empty;
            txtSoLuong.Text = string.Empty;
            dtpThoiGianKhoiHanh.Value = DateTime.Now;

            if (cboBienSoXe.Items.Count > 0) cboBienSoXe.SelectedIndex = 0;
            if (cboMaDVC.Items.Count > 0) cboMaDVC.SelectedIndex = 0;
            if (cboSanPham.Items.Count > 0) cboSanPham.SelectedIndex = 0;
            if (cboTrangThaiDon.Items.Count > 0) cboTrangThaiDon.SelectedIndex = 0;
        }

        // ==========================================
        // SỰ KIỆN CELLCLICK DATAGRIDVIEW
        // ==========================================

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvData.Rows.Count) return;

            DataGridViewRow row = dgvData.Rows[e.RowIndex];
            if (row == null) return;

            txtIDDonVC.Text = row.Cells["colID_DonVC"].Value?.ToString() ?? string.Empty;

            string bienSo = row.Cells["colBienSoXe"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(bienSo))
            {
                if (!cboBienSoXe.Items.Contains(bienSo))
                {
                    cboBienSoXe.Items.Add(bienSo);
                }
                cboBienSoXe.SelectedItem = bienSo;
            }

            string maDVC = row.Cells["colMaDVC"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(maDVC))
            {
                if (!cboMaDVC.Items.Contains(maDVC))
                {
                    cboMaDVC.Items.Add(maDVC);
                }
                cboMaDVC.SelectedItem = maDVC;
            }

            string sp = row.Cells["colID_SP"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(sp))
            {
                if (!cboSanPham.Items.Contains(sp))
                {
                    cboSanPham.Items.Add(sp);
                }
                cboSanPham.SelectedItem = sp;
            }

            txtSoLuong.Text = row.Cells["colSoLuongGiao"].Value?.ToString() ?? string.Empty;

            object dateVal = row.Cells["colThoiGianKhoiHanh"].Value;
            if (dateVal != null && DateTime.TryParse(dateVal.ToString(), out DateTime dt))
            {
                dtpThoiGianKhoiHanh.Value = dt;
            }
            else
            {
                dtpThoiGianKhoiHanh.Value = DateTime.Now;
            }

            string tt = row.Cells["colTrangThaiDon"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(tt))
            {
                cboTrangThaiDon.SelectedItem = tt;
            }
        }

        // ==========================================
        // CÁC NÚT HÀNH ĐỘNG CRUD
        // ==========================================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int soLuong = 0;
                if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong))
                {
                    MessageBox.Show("Số lượng giao phải là số nguyên dương hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                DonVanChuyen don = new DonVanChuyen
                {
                    ID_DonVC = txtIDDonVC.Text.Trim(),
                    BienSoXe = cboBienSoXe.Text.Trim(),
                    MaDVC = cboMaDVC.Text.Trim(),
                    ID_SP = cboSanPham.Text.Trim(),
                    SoLuongGiao = soLuong,
                    ThoiGianKhoiHanh = dtpThoiGianKhoiHanh.Value,
                    TrangThaiDon = string.IsNullOrEmpty(cboTrangThaiDon.Text.Trim()) ? "Khởi tạo" : cboTrangThaiDon.Text.Trim()
                };

                bll.ThemDon(don);
                MessageBox.Show("Thêm đơn vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataDonVanChuyen();
                LoadDanhSachXeKhaDung();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIDDonVC.Text))
                {
                    MessageBox.Show("Vui lòng chọn đơn vận chuyển cần sửa hoặc nhập mã đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuong = 0;
                if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong))
                {
                    MessageBox.Show("Số lượng giao phải là số nguyên dương hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                DonVanChuyen don = new DonVanChuyen
                {
                    ID_DonVC = txtIDDonVC.Text.Trim(),
                    BienSoXe = cboBienSoXe.Text.Trim(),
                    MaDVC = cboMaDVC.Text.Trim(),
                    ID_SP = cboSanPham.Text.Trim(),
                    SoLuongGiao = soLuong,
                    ThoiGianKhoiHanh = dtpThoiGianKhoiHanh.Value,
                    TrangThaiDon = cboTrangThaiDon.Text.Trim()
                };

                bll.SuaDon(don);
                MessageBox.Show("Cập nhật đơn vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataDonVanChuyen();
                LoadDanhSachXeKhaDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtIDDonVC.Text.Trim();
            if (string.IsNullOrEmpty(id) && dgvData.CurrentRow != null)
            {
                id = dgvData.CurrentRow.Cells["colID_DonVC"].Value?.ToString();
            }

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn đơn vận chuyển cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    bll.XoaDon(id);
                    MessageBox.Show("Xóa đơn vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataDonVanChuyen();
                    LoadDanhSachXeKhaDung();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string id = dgvData.CurrentRow.Cells["colID_DonVC"].Value?.ToString();
                string statusHienTai = dgvData.CurrentRow.Cells["colTrangThaiDon"].Value?.ToString();

                string statusMoi = "Đang vận chuyển";
                if (statusHienTai == "Khởi tạo") statusMoi = "Đang vận chuyển";
                else if (statusHienTai == "Đang vận chuyển") statusMoi = "Hoàn thành";
                else if (statusHienTai == "Hoàn thành") statusMoi = "Đã hủy";
                else if (statusHienTai == "Đã hủy") statusMoi = "Khởi tạo";

                try
                {
                    List<DonVanChuyen> all = bll.LayDanhSach();
                    DonVanChuyen don = all.Find(d => d.ID_DonVC == id);
                    if (don != null)
                    {
                        don.TrangThaiDon = statusMoi;
                        bll.SuaDon(don);
                        MessageBox.Show($"Đã cập nhật trạng thái Đơn VC [{id}] sang: [{statusMoi}]", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataDonVanChuyen();
                        LoadDanhSachXeKhaDung();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn vận chuyển cần cập nhật trạng thái!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            string keyword = txtSearch.Text.Trim();
            if (keyword == "🔍 Tìm kiếm theo Mã đơn, Mã xe, Địa điểm...") keyword = string.Empty;

            try
            {
                List<DonVanChuyen> list = string.IsNullOrEmpty(keyword)
                    ? bll.LayDanhSach()
                    : bll.TimKiem(keyword);

                if (cmbTrangThai.SelectedIndex > 0)
                {
                    string selectedStatus = cmbTrangThai.SelectedItem.ToString();
                    list = list.FindAll(d => d.TrangThaiDon == selectedStatus);
                }

                BindData(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        // NÚT XUẤT FILE [MỚI]
        // ==========================================

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                sfd.FileName = "DonVanChuyen_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Kiểm tra thư viện EPPlus: Project hiện chưa có NuGet EPPlus nên dùng CSV thay thế
                        StringBuilder sb = new StringBuilder();

                        // Tiêu đề cột
                        List<string> headers = new List<string>();
                        foreach (DataGridViewColumn col in dgvData.Columns)
                        {
                            if (col.Visible)
                            {
                                headers.Add("\"" + col.HeaderText.Replace("\"", "\"\"") + "\"");
                            }
                        }
                        sb.AppendLine(string.Join(",", headers));

                        // Dữ liệu dòng
                        foreach (DataGridViewRow row in dgvData.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                List<string> cells = new List<string>();
                                foreach (DataGridViewColumn col in dgvData.Columns)
                                {
                                    if (col.Visible)
                                    {
                                        object val = row.Cells[col.Index].Value;
                                        string strVal = val != null ? val.ToString() : "";
                                        cells.Add("\"" + strVal.Replace("\"", "\"\"") + "\"");
                                    }
                                }
                                sb.AppendLine(string.Join(",", cells));
                            }
                        }

                        // Ghi file kèm BOM UTF-8 để mở tiếng Việt không lỗi font
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xuất file thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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