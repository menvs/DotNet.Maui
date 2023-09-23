using QlHui.App.Data.Models.Dtos;

namespace QlHui.App.Data.Services.IService
{
    internal interface IQlHuiVienService
    {
        HuiVienDto LayThongTinHuiVien(int huiVienId);
        HuiVienThamGiaDto LayThongTinHuiVienThamGia(int huiVienThamGiaId);
        IEnumerable<HuiVienDto> DsHuiVienTheoMaGanDung(string maHuiVIen);
        IEnumerable<HuiVienThamGiaDto> LayDanhSachHuiVienThamGiaDayHui(int dayHuiId);
        IEnumerable<TimKiemHuiVienAutocompleteDto> LayDanhSachHuiVienTheoTenGanDung(string tieuChiTimKiem);
        bool ThemDanhSachHuiVienThamGia(IEnumerable<HuiVienThamGiaDto> dtos);
        IEnumerable<HuiVienDto> ThemMoiDanhSachHuiVien(IEnumerable<HuiVienDto> dtos);
        bool XoaHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia);
        bool CapNhatHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia);
        SearchResult<HuiVienKetQuaTimKiemDto> TimKiemHuiVien(HuiVienTieuChiTimKiemDto tieuchiTimKiem, PanigationDto panigation);
        IEnumerable<TinhTienTheoHuiVienDto> LayDanhSachTinhTienTheoHuiVien(int huiVienId);
        bool TatToanTatCa(IList<TinhTienTheoHuiVienDto> dsHuiVien);
    }
}
