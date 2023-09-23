using QlHui.App.Data.Models.Dtos;

namespace QlHui.App.Data.Services.IService
{
    internal interface ILsDongHuiService
    {
        bool Insert(LichSuDongHuiDto input);
        IEnumerable<LichSuDongHuiDto> LayDanhSachLichSuDongHui(int huiVienThamGiaId);
    }
}
