using QlHui.App.Data.Models.Dtos;

namespace QlHui.App.Data.Services.IService
{
    internal interface ILsBoHuiService
    {
        IEnumerable<LichSuBoHuiDto> LayDanhSachLichSuBoHui(int dayHuiId);
    }
}
