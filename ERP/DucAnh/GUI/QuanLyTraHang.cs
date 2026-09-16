using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ERP.DucAnh.BLL;
using ERP.DucAnh.DTO;

namespace ERP
{
    public partial class QuanLyTraHang : Form
    {
        private readonly PhieuTraHangBLL bll = new PhieuTraHangBLL();

        // Các control nhập liệu và nút thao tác (tạo động trên code-behind, không can thiệp .Designer.cs)
        private TextBox txtID_PhieuTra;
        private DateTimePicker dtpNgayTra;
        private ComboBox cboMaDVC;
        private ComboBox cboID_CTYC;
        private ComboBox cboTrangThaiInput;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        // Bộ lọc ngày & tìm kiếm
        private DateTimePicker dtpTuNgay;
        private DateTimePicker dtpDenNgay;
        private Button btnTimKiem;

        public QuanLyTraHang()
        {
            InitializeComponent();
            KhoiTaoGiaoDienNhapLieu();
        }

        private void KhoiTaoGiaoDienNhapLieu()
        {
            // Mở rộng pnlActionTool để chứa Form nhập liệu và Bộ lọc
            pnlActionTool.Height = 185;

            // 1. Cập nhật Tiêu đề và Nút thao tác CRUD trên dòng 1
            lblTitle.Text = "Quản lý trả hàng (Phiếu thu hồi)";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // Nút Thêm
            btnThem = new Button
            {
                Text = "➕ Thêm phiếu",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(115, 32),
                Location = new Point(520, 5),
                Cursor = Cursors.Hand
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += btnThem_Click;

            // Nút Sửa
            btnSua = new Button
            {
                Text = "✏ Sửa phiếu",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(105, 32),
                Location = new Point(645, 5),
                Cursor = Cursors.Hand
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += btnSua_Click;

            // Nút Xóa
            btnXoa = new Button
            {
                Text = "🗑 Xóa phiếu",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(105, 32),
                Location = new Point(760, 5),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += btnXoa_Click;

            // Nút Làm mới
            btnLamMoi = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 32),
                Location = new Point(875, 5),
                Cursor = Cursors.Hand
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += (s, e) => { ResetForm(); LoadDataPhieuTraHang(); };

            // 2. Dòng 2: Form nhập liệu
            Label lblIDPT = new Label { Text = "Mã phiếu trả (*):", Location = new Point(0, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            txtID_PhieuTra = new TextBox { Location = new Point(0, 68), Size = new Size(160, 27), Font = new Font("Segoe UI", 9F) };

            Label lblNgayTra = new Label { Text = "Ngày trả:", Location = new Point(175, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            dtpNgayTra = new DateTimePicker
            {
                Location = new Point(175, 68),
                Size = new Size(175, 27),
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm"
            };

            Label lblMaDVC = new Label { Text = "Điểm vận chuyển (*):", Location = new Point(365, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            cboMaDVC = new ComboBox { Location = new Point(365, 68), Size = new Size(180, 27), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            Label lblCTYC = new Label { Text = "Chi tiết yêu cầu (*):", Location = new Point(560, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            cboID_CTYC = new ComboBox { Location = new Point(560, 68), Size = new Size(185, 27), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            Label lblTrangThaiInput = new Label { Text = "Trạng thái:", Location = new Point(760, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            cboTrangThaiInput = new ComboBox { Location = new Point(760, 68), Size = new Size(215, 27), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };

            // 3. Dòng 3: Bộ lọc và Tìm kiếm
            Label lblTuNgay = new Label { Text = "Từ ngày:", Location = new Point(330, 115), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            dtpTuNgay = new DateTimePicker
            {
                Location = new Point(330, 138),
                Size = new Size(140, 27),
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-1)
            };

            Label lblDenNgay = new Label { Text = "Đến ngày:", Location = new Point(485, 115), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            dtpDenNgay = new DateTimePicker
            {
                Location = new Point(485, 138),
                Size = new Size(140, 27),
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(1)
            };

            Label lblLocTrangThai = new Label { Text = "Lọc trạng thái:", Location = new Point(640, 115), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };

            // Điều chỉnh lại vị trí ô txtSearch và cmbTrangThai từ Designer
            txtSearch.Location = new Point(0, 138);
            txtSearch.Size = new Size(315, 27);
            txtSearch.Text = "🔍 Tìm kiếm theo Mã phiếu trả, Tên ĐVC...";

            cmbTrangThai.Location = new Point(640, 138);
            cmbTrangThai.Size = new Size(185, 27);

            btnTimKiem = new Button
            {
                Text = "🔍 Tìm kiếm",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(125, 30),
                Location = new Point(850, 136),
                Cursor = Cursors.Hand
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += btnTimKiem_Click;

            // Ẩn btnApprove khỏi tầm nhìn hoặc tái sử dụng làm nút tìm kiếm
            btnApprove.Visible = false;

            // Thêm các control vào pnlActionTool
            pnlActionTool.Controls.Add(btnThem);
            pnlActionTool.Controls.Add(btnSua);
            pnlActionTool.Controls.Add(btnXoa);
            pnlActionTool.Controls.Add(btnLamMoi);

            pnlActionTool.Controls.Add(lblIDPT);
            pnlActionTool.Controls.Add(txtID_PhieuTra);
            pnlActionTool.Controls.Add(lblNgayTra);
            pnlActionTool.Controls.Add(dtpNgayTra);
            pnlActionTool.Controls.Add(lblMaDVC);
            pnlActionTool.Controls.Add(cboMaDVC);
            pnlActionTool.Controls.Add(lblCTYC);
            pnlActionTool.Controls.Add(cboID_CTYC);
            pnlActionTool.Controls.Add(lblTrangThaiInput);
            pnlActionTool.Controls.Add(cboTrangThaiInput);

            pnlActionTool.Controls.Add(lblTuNgay);
            pnlActionTool.Controls.Add(dtpTuNgay);
            pnlActionTool.Controls.Add(lblDenNgay);
            pnlActionTool.Controls.Add(dtpDenNgay);
            pnlActionTool.Controls.Add(lblLocTrangThai);
            pnlActionTool.Controls.Add(btnTimKiem);

            // Đăng ký sự kiện CellClick cho dgvData
            dgvData.CellClick += dgvData_CellClick;
        }

        // ==========================================
        // 1. SỰ KIỆN FORM_LOAD
        // ==========================================
        private void QuanLyTraHang_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            KhoiTaoCotBang();

            // Đổ data vào ComboBox ID_CTYC, ComboBox MaDVC, ComboBox Trạng thái
            LoadComboBoxData();

            // Load DataGridView ban đầu
            LoadDataPhieuTraHang();
        }

        private void KhoiTaoCotBang()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;

            // 1. Mã Phiếu Trả (ID_PhieuTra)
            DataGridViewTextBoxColumn colIDPT = new DataGridViewTextBoxColumn();
            colIDPT.Name = "colID_PhieuTra";
            colIDPT.HeaderText = "MÃ PHIẾU TRẢ";
            colIDPT.DataPropertyName = "ID_PhieuTra";
            colIDPT.FillWeight = 16;
            colIDPT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDPT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDPT.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colIDPT.DefaultCellStyle.ForeColor = Color.FromArgb(13, 110, 253);
            dgvData.Columns.Add(colIDPT);

            // 2. Ngày Trả (NgayTra)
            DataGridViewTextBoxColumn colNgayTra = new DataGridViewTextBoxColumn();
            colNgayTra.Name = "colNgayTra";
            colNgayTra.HeaderText = "NGÀY TRẢ";
            colNgayTra.DataPropertyName = "NgayTra";
            colNgayTra.FillWeight = 20;
            colNgayTra.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgayTra.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgayTra.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvData.Columns.Add(colNgayTra);

            // 3. Mã Điểm Vận Chuyển (MaDVC)
            DataGridViewTextBoxColumn colMaDVC = new DataGridViewTextBoxColumn();
            colMaDVC.Name = "colMaDVC";
            colMaDVC.HeaderText = "MÃ ĐIỂM VC";
            colMaDVC.DataPropertyName = "MaDVC";
            colMaDVC.FillWeight = 18;
            colMaDVC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMaDVC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colMaDVC);

            // 4. Mã Chi Tiết Yêu Cầu (ID_CTYC)
            DataGridViewTextBoxColumn colIDCTYC = new DataGridViewTextBoxColumn();
            colIDCTYC.Name = "colID_CTYC";
            colIDCTYC.HeaderText = "MÃ CHI TIẾT YC";
            colIDCTYC.DataPropertyName = "ID_CTYC";
            colIDCTYC.FillWeight = 20;
            colIDCTYC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIDCTYC.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(colIDCTYC);

            // 5. Trạng Thái (TrangThai)
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.Name = "colTrangThai";
            colTrangThai.HeaderText = "TRẠNG THÁI";
            colTrangThai.DataPropertyName = "TrangThai";
            colTrangThai.FillWeight = 18;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvData.Columns.Add(colTrangThai);

            dgvData.AllowUserToResizeColumns = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadComboBoxData()
        {
            try
            {
                // 1. Đổ data vào ComboBox ID_CTYC
                cboID_CTYC.Items.Clear();
                List<string> dsCTYC = bll.GetDanhSachCTYC();
                foreach (string ctyc in dsCTYC)
                {
                    cboID_CTYC.Items.Add(ctyc);
                }
                if (cboID_CTYC.Items.Count > 0) cboID_CTYC.SelectedIndex = 0;

                // 2. Đổ data vào ComboBox MaDVC
                cboMaDVC.Items.Clear();
                List<string> dsDVC = bll.GetDanhSachMaDVC();
                foreach (string dvc in dsDVC)
                {
                    cboMaDVC.Items.Add(dvc);
                }
                if (cboMaDVC.Items.Count > 0) cboMaDVC.SelectedIndex = 0;

                // 3. Đổ data vào ComboBox Trạng thái (Chờ xử lý, Đang thu hồi, Đã nhập kho, Đã hủy)
                string[] dsTrangThai = { "Chờ xử lý", "Đang thu hồi", "Đã nhập kho", "Đã hủy" };

                cboTrangThaiInput.Items.Clear();
                cboTrangThaiInput.Items.AddRange(dsTrangThai);
                cboTrangThaiInput.SelectedIndex = 0;

                // Đổ data vào ComboBox bộ lọc trạng thái
                cmbTrangThai.Items.Clear();
                cmbTrangThai.Items.Add("Tất cả trạng thái");
                cmbTrangThai.Items.AddRange(dsTrangThai);
                cmbTrangThai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataPhieuTraHang()
        {
            try
            {
                List<PhieuTraHang> list = bll.GetAll();
                BindData(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu phiếu trả hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindData(List<PhieuTraHang> list)
        {
            dgvData.DataSource = null;
            dgvData.DataSource = list;

            // Định dạng màu sắc trạng thái
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["colTrangThai"].Value != null)
                {
                    string tt = row.Cells["colTrangThai"].Value.ToString();
                    if (tt == "Chờ xử lý")
                    {
                        row.Cells["colTrangThai"].Style.ForeColor = Color.FromArgb(255, 152, 0); // Cam
                    }
                    else if (tt == "Đang thu hồi")
                    {
                        row.Cells["colTrangThai"].Style.ForeColor = Color.FromArgb(13, 110, 253); // Xanh dương
                    }
                    else if (tt == "Đã nhập kho")
                    {
                        row.Cells["colTrangThai"].Style.ForeColor = Color.FromArgb(40, 167, 69); // Xanh lá
                    }
                    else if (tt == "Đã hủy")
                    {
                        row.Cells["colTrangThai"].Style.ForeColor = Color.FromArgb(220, 53, 69); // Đỏ
                    }
                }
            }
        }

        // ==========================================
        // 2. SỰ KIỆN CELLCLICK: FILL THÔNG TIN FORM NHẬP LIỆU
        // ==========================================
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvData.Rows.Count)
            {
                DataGridViewRow row = dgvData.Rows[e.RowIndex];

                txtID_PhieuTra.Text = row.Cells["colID_PhieuTra"].Value?.ToString() ?? "";

                if (row.Cells["colNgayTra"].Value != null && DateTime.TryParse(row.Cells["colNgayTra"].Value.ToString(), out DateTime ngay))
                {
                    dtpNgayTra.Value = ngay;
                }

                string maDVC = row.Cells["colMaDVC"].Value?.ToString() ?? "";
                if (!cboMaDVC.Items.Contains(maDVC) && !string.IsNullOrEmpty(maDVC))
                {
                    cboMaDVC.Items.Add(maDVC);
                }
                cboMaDVC.SelectedItem = maDVC;

                string idCTYC = row.Cells["colID_CTYC"].Value?.ToString() ?? "";
                if (!cboID_CTYC.Items.Contains(idCTYC) && !string.IsNullOrEmpty(idCTYC))
                {
                    cboID_CTYC.Items.Add(idCTYC);
                }
                cboID_CTYC.SelectedItem = idCTYC;

                string tt = row.Cells["colTrangThai"].Value?.ToString() ?? "";
                if (!cboTrangThaiInput.Items.Contains(tt) && !string.IsNullOrEmpty(tt))
                {
                    cboTrangThaiInput.Items.Add(tt);
                }
                cboTrangThaiInput.SelectedItem = tt;
            }
        }

        // ==========================================
        // 3. SỰ KIỆN CLICK TÌM KIẾM
        // ==========================================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                if (keyword == "🔍 Tìm kiếm theo Mã phiếu trả, Tên ĐVC..." || keyword == "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...")
                {
                    keyword = "";
                }

                DateTime tuNgay = dtpTuNgay.Value;
                DateTime denNgay = dtpDenNgay.Value;
                string trangThai = cmbTrangThai.SelectedItem?.ToString() ?? "";

                var list = bll.TimKiem(keyword, tuNgay, denNgay, trangThai);
                if (list.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả phù hợp");
                    dgvData.DataSource = null;
                }
                else
                {
                    BindData(list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ==========================================
        // 4. SỰ KIỆN THÊM / SỬA / XÓA
        // ==========================================

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuTraHang p = new PhieuTraHang
                {
                    ID_PhieuTra = txtID_PhieuTra.Text.Trim(),
                    NgayTra = dtpNgayTra.Value,
                    MaDVC = cboMaDVC.SelectedItem?.ToString() ?? cboMaDVC.Text.Trim(),
                    ID_CTYC = cboID_CTYC.SelectedItem?.ToString() ?? cboID_CTYC.Text.Trim(),
                    TrangThai = cboTrangThaiInput.SelectedItem?.ToString() ?? "Chờ xử lý"
                };

                bll.ThemPhieu(p);
                MessageBox.Show("Thêm phiếu trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadDataPhieuTraHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuTraHang p = new PhieuTraHang
                {
                    ID_PhieuTra = txtID_PhieuTra.Text.Trim(),
                    NgayTra = dtpNgayTra.Value,
                    MaDVC = cboMaDVC.SelectedItem?.ToString() ?? cboMaDVC.Text.Trim(),
                    ID_CTYC = cboID_CTYC.SelectedItem?.ToString() ?? cboID_CTYC.Text.Trim(),
                    TrangThai = cboTrangThaiInput.SelectedItem?.ToString() ?? "Chờ xử lý"
                };

                bll.SuaPhieu(p);
                MessageBox.Show("Cập nhật phiếu trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadDataPhieuTraHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txtID_PhieuTra.Text.Trim();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã phiếu trả cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu trả [{id}]?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    bll.XoaPhieu(id);
                    MessageBox.Show("Xóa phiếu trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    LoadDataPhieuTraHang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtID_PhieuTra.Clear();
            dtpNgayTra.Value = DateTime.Now;
            if (cboMaDVC.Items.Count > 0) cboMaDVC.SelectedIndex = 0;
            if (cboID_CTYC.Items.Count > 0) cboID_CTYC.SelectedIndex = 0;
            cboTrangThaiInput.SelectedIndex = 0; // "Chờ xử lý"
            txtID_PhieuTra.Focus();
        }

        // ==========================================
        // CÁC SỰ KIỆN TƯƠNG THÍCH VỚI .DESIGNER.CS
        // ==========================================
        private void btnApprove_Click(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Tự động tìm kiếm nhanh khi người dùng gõ từ khóa
            if (txtSearch.Text != "🔍 Tìm kiếm theo Mã phiếu trả, Tên ĐVC..." &&
                txtSearch.Text != "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...")
            {
                btnTimKiem_Click(sender, e);
            }
        }

        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "🔍 Tìm kiếm theo Mã phiếu trả, Tên ĐVC..." ||
                txtSearch.Text == "🔍 Tìm kiếm theo Mã phiếu trả, Mã YC, Khách hàng...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "🔍 Tìm kiếm theo Mã phiếu trả, Tên ĐVC...";
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