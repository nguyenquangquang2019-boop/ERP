using System;
using System.Collections.Generic;
using ERP.DucAnh.DAL;
using ERP.DucAnh.DTO;
using System.Data;

namespace ERP.DucAnh.BLL
{
    public class PhieuTraHangBLL
    {
        private readonly PhieuTraHangDAL dal;

        public PhieuTraHangBLL()
        {
            dal = new PhieuTraHangDAL();
        }

        public PhieuTraHangBLL(string connectionString)
        {
            dal = new PhieuTraHangDAL(connectionString);
        }

        public List<PhieuTraHang> GetAll()
        {
            return dal.GetAll();
        }

        public PhieuTraHang GetByID(string id)
        {
            return dal.GetByID(id);
        }

        public List<string> GetDanhSachCTYC()
        {
            return dal.GetDanhSachCTYC();
        }

        public List<string> GetDanhSachMaDVC()
        {
            return dal.GetDanhSachMaDVC();
        }

        public Dictionary<string, string> GetDanhSachDVC()
        {
            return dal.GetDanhSachDVC();
        }

        public List<string> GetDanhSachXeKhaDung()
        {
            return dal.GetDanhSachXeKhaDung();
        }

        public List<string> GetDanhSachLoaiXeRanh()
        {
            return dal.GetDanhSachLoaiXeRanh();
        }

        public DataTable GetAllPhuongTienRanh()
        {
            return dal.GetAllPhuongTienRanh();
        }

        public ThongTinYeuCau GetThongTinYeuCau(string idCTYC)
        {
            return dal.GetThongTinYeuCau(idCTYC);
        }

        // [Nghiệp vụ A2]: Lọc tìm kiếm và kiểm tra ngày kết thúc >= ngày bắt đầu
        public List<PhieuTraHang> TimKiem(string keyword, DateTime tuNgay, DateTime denNgay, string trangThai)
        {
            if (tuNgay > denNgay)
            {
                throw new Exception("Lỗi định dạng bộ lọc: Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");
            }
            return dal.Search(keyword, tuNgay, denNgay, trangThai);
        }

        // Thêm phiếu: Kiểm tra rỗng, kiểm tra trùng lặp thu hồi ID_CTYC
        public bool ThemPhieu(PhieuTraHang p)
        {
            if (p == null)
            {
                throw new Exception("Thông tin phiếu trả hàng không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.ID_PhieuTra))
            {
                throw new Exception("Mã phiếu trả không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.MaDVC))
            {
                throw new Exception("Mã điểm vận chuyển không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.ID_CTYC))
            {
                throw new Exception("Mã chi tiết yêu cầu không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.BienSoXe))
            {
                throw new Exception("Vui lòng chọn xe tải đi thu hồi!");
            }

            if (dal.GetByID(p.ID_PhieuTra.Trim()) != null)
            {
                throw new Exception($"Mã phiếu trả '{p.ID_PhieuTra}' đã tồn tại trong hệ thống!");
            }

            if (dal.CheckThuHoiDup(p.ID_CTYC.Trim()))
            {
                throw new Exception("Yêu cầu lỗi này đã được tạo phiếu thu hồi trước đó!");
            }

            if (string.IsNullOrWhiteSpace(p.TrangThai))
            {
                p.TrangThai = "Chờ xử lý";
            }

            if (p.NgayTra == DateTime.MinValue)
            {
                p.NgayTra = DateTime.Now;
            }

            return dal.Insert(p);
        }

        // Sửa phiếu: Phải query lấy trạng thái hiện tại. Nếu TrangThai != "Chờ xử lý" -> throw
        public bool SuaPhieu(PhieuTraHang p)
        {
            if (p == null)
            {
                throw new Exception("Thông tin phiếu trả hàng không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.ID_PhieuTra))
            {
                throw new Exception("Mã phiếu trả không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.MaDVC))
            {
                throw new Exception("Mã điểm vận chuyển không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.ID_CTYC))
            {
                throw new Exception("Mã chi tiết yêu cầu không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(p.BienSoXe))
            {
                throw new Exception("Vui lòng chọn xe tải đi thu hồi!");
            }

            PhieuTraHang hienTai = dal.GetByID(p.ID_PhieuTra.Trim());
            if (hienTai == null)
            {
                throw new Exception($"Không tìm thấy phiếu trả '{p.ID_PhieuTra}' trong hệ thống để cập nhật!");
            }

            if (hienTai.TrangThai != "Chờ xử lý")
            {
                throw new Exception("Chỉ được sửa/xóa phiếu khi đang ở trạng thái 'Chờ xử lý'!");
            }

            if (p.ID_CTYC.Trim() != hienTai.ID_CTYC && dal.CheckThuHoiDup(p.ID_CTYC.Trim()))
            {
                throw new Exception("Yêu cầu lỗi này đã được tạo phiếu thu hồi trước đó!");
            }

            if (string.IsNullOrWhiteSpace(p.TrangThai))
            {
                p.TrangThai = "Chờ xử lý";
            }

            return dal.Update(p);
        }

        // Xóa phiếu: Phải query lấy trạng thái hiện tại. Nếu TrangThai != "Chờ xử lý" -> throw
        public bool XoaPhieu(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Mã phiếu trả cần xóa không được để trống!");
            }

            PhieuTraHang hienTai = dal.GetByID(id.Trim());
            if (hienTai == null)
            {
                throw new Exception($"Không tìm thấy phiếu trả '{id}' trong hệ thống để xóa!");
            }

            if (hienTai.TrangThai != "Chờ xử lý")
            {
                throw new Exception("Chỉ được sửa/xóa phiếu khi đang ở trạng thái 'Chờ xử lý'!");
            }

            return dal.Delete(id.Trim());
        }
    }
}
