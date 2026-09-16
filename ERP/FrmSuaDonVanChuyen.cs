using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ERP.DucAnh.BLL;
using ERP.DucAnh.DTO;

namespace ERP
{
    public partial class FrmSuaDonVanChuyen : Form
    {
        private readonly DonVanChuyenBLL bll = new DonVanChuyenBLL();
        private readonly string idDonVCToEdit;

        public FrmSuaDonVanChuyen(string idDonVC)
        {
            InitializeComponent();
            idDonVCToEdit = idDonVC;
        }

        private void FrmSuaDonVanChuyen_Load(object sender, EventArgs e)
        {
            LoadDanhMuc();
            LoadThongTinDon();
        }

        private void LoadDanhMuc()
        {
            try
            {
                // 1. Điểm vận chuyển
                List<string> dsDVC = bll.LayDanhSachDVC();
                cboMaDVC.Items.Clear();
                foreach (string dvc in dsDVC)
                {
                    cboMaDVC.Items.Add(dvc);
                }

                // 2. Xe khả dụng
                List<string> dsXe = bll.LayDanhSachXeKhaDung();
                cboBienSoXe.Items.Clear();
                foreach (string xe in dsXe)
                {
                    cboBienSoXe.Items.Add(xe);
                }

                // 3. Sản phẩm
                List<string> dsSP = bll.LayDanhSachSanPham();
                cboSanPham.Items.Clear();
                foreach (string sp in dsSP)
                {
                    cboSanPham.Items.Add(sp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongTinDon()
        {
            try
            {
                DonVanChuyen don = bll.LayDonTheoID(idDonVCToEdit);
                if (don != null)
                {
                    txtIDDonVC.Text = don.ID_DonVC;

                    if (!cboMaDVC.Items.Contains(don.MaDVC)) cboMaDVC.Items.Add(don.MaDVC);
                    cboMaDVC.SelectedItem = don.MaDVC;

                    if (!cboBienSoXe.Items.Contains(don.BienSoXe)) cboBienSoXe.Items.Add(don.BienSoXe);
                    cboBienSoXe.SelectedItem = don.BienSoXe;

                    if (!cboSanPham.Items.Contains(don.ID_SP)) cboSanPham.Items.Add(don.ID_SP);
                    cboSanPham.SelectedItem = don.ID_SP;

                    txtSoLuongGiao.Text = don.SoLuongGiao.ToString();
                    if (don.ThoiGianKhoiHanh != DateTime.MinValue)
                    {
                        dtpThoiGianKhoiHanh.Value = don.ThoiGianKhoiHanh;
                    }

                    if (!cboTrangThaiDon.Items.Contains(don.TrangThaiDon)) cboTrangThaiDon.Items.Add(don.TrangThaiDon);
                    cboTrangThaiDon.SelectedItem = don.TrangThaiDon;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn vận chuyển cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maDVC = cboMaDVC.SelectedItem?.ToString();
            string bienSo = cboBienSoXe.SelectedItem?.ToString();
            string sanPham = cboSanPham.SelectedItem?.ToString();
            string strSoLuong = txtSoLuongGiao.Text.Trim();
            DateTime thoiGianGiao = dtpThoiGianKhoiHanh.Value;
            string trangThai = cboTrangThaiDon.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(maDVC))
            {
                MessageBox.Show("Vui lòng chọn Điểm giao nhận (Tuyến đường)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaDVC.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(bienSo))
            {
                MessageBox.Show("Vui lòng chọn Phương tiện / Đơn vị vận chuyển!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBienSoXe.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(sanPham))
            {
                MessageBox.Show("Vui lòng chọn Hàng hóa / Sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSanPham.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(strSoLuong) || !int.TryParse(strSoLuong, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng giao phải là số nguyên dương lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongGiao.Focus();
                txtSoLuongGiao.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(trangThai))
            {
                trangThai = "Khởi tạo";
            }

            try
            {
                DonVanChuyen don = new DonVanChuyen
                {
                    ID_DonVC = idDonVCToEdit,
                    MaDVC = maDVC,
                    BienSoXe = bienSo,
                    ID_SP = sanPham,
                    SoLuongGiao = soLuong,
                    ThoiGianKhoiHanh = thoiGianGiao,
                    TrangThaiDon = trangThai
                };

                bll.SuaDon(don);

                MessageBox.Show("Cập nhật đơn vận chuyển thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
