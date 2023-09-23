using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Services.IService;
using QlHui.App.Data.Utils;

namespace QlHui.App.Data.Services.ImplService
{
    internal class LsBoHuiService : BaseService, ILsBoHuiService
    {
        private readonly IUtils _utils;
        public LsBoHuiService(IUtils utils)
        {
            _utils = utils;

        }

        public IEnumerable<LichSuBoHuiDto> LayDanhSachLichSuBoHui(int dayHuiId)
        {
            var retResult = Enumerable.Empty<LichSuBoHuiDto>();
            if (dayHuiId > 0)
            {
                var entities = _connection.Table<LichSuBoHuiEntity>().Where(item => item.DayHuiId == dayHuiId).OrderByDescending(item => item.KyBo).AsEnumerable();
                if (entities.Any())
                {
                    retResult = _utils.TransformToDto<LichSuBoHuiDto, LichSuBoHuiEntity>(entities);
                }
            }
            return retResult;
        }
    }
}
