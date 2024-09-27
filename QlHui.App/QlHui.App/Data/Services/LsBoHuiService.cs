using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Utils;

namespace QlHui.App.Data.Services
{
    internal interface ILsBoHuiService
    {
        IList<LichSuBoHuiDto> LayDanhSachLichSuBoHui(int dayHuiId);
    }
    internal class LsBoHuiService : BaseService, ILsBoHuiService
    {
        private readonly IUtils _utils;
        public LsBoHuiService(IUtils utils)
        {
            _utils = utils;

        }

        public IList<LichSuBoHuiDto> LayDanhSachLichSuBoHui(int dayHuiId)
        {
            IList<LichSuBoHuiDto> retResult = [];
            if (dayHuiId > 0)
            {
                var entities = _connection.Table<LichSuBoHuiEntity>()
                                .Where(item => item.DayHuiId == dayHuiId)
                                .OrderByDescending(item => item.KyBo).ToList();
                if (entities.Count>0)
                {
                    retResult = _utils.TransformToDto<LichSuBoHuiDto, LichSuBoHuiEntity>(entities);
                }
            }
            return retResult;
        }
    }
}
