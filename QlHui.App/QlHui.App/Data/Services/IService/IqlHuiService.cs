using QlHui.App.Data.Models.Dtos;

namespace QlHui.App.Data.Services.IService
{
    internal interface IQlHuiService
    {
        string TaoMaDayHui(DateTime ngayKhui, int loaiHui, float tien1C, int soNgayLoaiHui);
        bool Create(DayHuiDto input);
        bool Update(DayHuiDto input);
        SearchResult<DayHuiDto> Search(QLDayHuiSearchCriteria criteria, PanigationDto panigation);
        bool DeleteDayHuiById(int id);
        DayHuiDto GetById(int id);
        bool DayHuiDaCoNguoiBoChua(int dayHuiId);
        bool DoiNguoiThamGiaDayHui(HuiVienDto huiVienDoiThanh, int idHuiVienThamGia);
        bool BoHui(HuiVienThamGiaDto huiVienThamGia, float soTienBoHui);
        void XoaData();

    }
}
