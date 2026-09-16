using System;
using System.Collections.Generic;
using System.Data;
using ERP.DucAnh.DAL;
using ERP.DucAnh.DTO;

namespace ERP.DucAnh.BLL
{
    public class DonVanChuyenBLL
    {
        private readonly DonVanChuyenDAL dal;

        public DonVanChuyenBLL()
        {
            dal = new DonVanChuyenDAL();
        }

        public DonVanChuyenBLL(string connectionString)
        {
            dal = new DonVanChuyenDAL(connectionString);
        }

        public List<DonVanChuyen> LayDanhSach()
        {
            return dal.GetAll();
        }

        public DonVanChuyen LayDonTheoID(string id)
        {
            return dal.GetByID(id);
        }

        public List<string> LayDanhSachXeKhaDung()
        {
            return dal.GetDanhSachXeKhaDung();
        }

        public List<string> LayDanhSachDVC()
        {
            return dal.GetDanhSachDVC();
        }

        public List<string> LayDanhSachSanPham()
        {
            return dal.GetDanhSachSanPham();
        }

        public bool ThemDon(DonVanChuyen don)
        {
            if (don == null)
                throw new ArgumentNullException(nameof(don), "Dữ liệu đơn vận chuyển không được để trống.");

            if (string.IsNullOrWhiteSpace(don.ID_DonVC))
                throw new ArgumentException("Mã đơn vận chuyển không được để trống.");

            if (dal.IsExist(don.ID_DonVC))
                throw new ArgumentException($"Mã đơn vận chuyển '{don.ID_DonVC}' đã tồn tại trong hệ thống.");

            if (string.IsNullOrWhiteSpace(don.BienSoXe))
                throw new ArgumentException("Biển số xe không được để trống.");

            if (string.IsNullOrWhiteSpace(don.MaDVC))
                throw new ArgumentException("Mã điểm vận chuyển không được để trống.");

            if (string.IsNullOrWhiteSpace(don.ID_SP))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            if (don.SoLuongGiao <= 0)
                throw new ArgumentException("Số lượng giao phải lớn hơn 0.");

            if (string.IsNullOrWhiteSpace(don.TrangThaiDon))
                don.TrangThaiDon = "Khởi tạo";

            bool success = dal.Insert(don);
            if (!success)
                throw new Exception("Thêm đơn vận chuyển thất bại. Vui lòng thử lại!");
            return success;
        }

        public bool SuaDon(DonVanChuyen don)
        {
            if (don == null)
                throw new ArgumentNullException(nameof(don), "Dữ liệu đơn vận chuyển không được để trống.");

            if (string.IsNullOrWhiteSpace(don.ID_DonVC))
                throw new ArgumentException("Mã đơn vận chuyển không được để trống.");

            if (!dal.IsExist(don.ID_DonVC))
                throw new ArgumentException($"Không tìm thấy đơn vận chuyển mã '{don.ID_DonVC}' để cập nhật.");

            if (string.IsNullOrWhiteSpace(don.BienSoXe))
                throw new ArgumentException("Biển số xe không được để trống.");

            if (string.IsNullOrWhiteSpace(don.MaDVC))
                throw new ArgumentException("Mã điểm vận chuyển không được để trống.");

            if (string.IsNullOrWhiteSpace(don.ID_SP))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            if (don.SoLuongGiao <= 0)
                throw new ArgumentException("Số lượng giao phải lớn hơn 0.");

            bool success = dal.Update(don);
            if (!success)
                throw new Exception("Cập nhật đơn vận chuyển thất bại. Vui lòng thử lại!");
            return success;
        }

        public bool XoaDon(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã đơn vận chuyển cần xóa không được để trống.");

            if (!dal.IsExist(id))
                throw new ArgumentException($"Không tìm thấy đơn vận chuyển mã '{id}' để xóa.");

            bool success = dal.Delete(id);
            if (!success)
                throw new Exception("Xóa đơn vận chuyển thất bại. Vui lòng thử lại!");
            return success;
        }

        public List<DonVanChuyen> TimKiem(string keyword)
        {
            return dal.Search(keyword);
        }

        public DataTable GetTatCaDonVanChuyen()
        {
            return dal.GetTatCaDonVanChuyen();
        }
    }
}
