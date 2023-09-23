using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Services.IService;
using QlHui.App.Data.Utils;

namespace QlHui.App.Data.Services.ImplService
{
    internal class LsDongHuiService : BaseService, ILsDongHuiService
    {
        private readonly IUtils _utils;
        public LsDongHuiService(IUtils utils)
        {
            _utils = utils;
        }
        public IEnumerable<LichSuDongHuiDto> LayDanhSachLichSuDongHui(int huiVienThamGia)
        {
            var retResult = Enumerable.Empty<LichSuDongHuiDto>();
            if (huiVienThamGia > 0)
            {
                var entities = _connection.Table<LichSuDongHuiEntity>().Where(item => item.HuiVienThamGiaId == huiVienThamGia).AsEnumerable();
                if (entities.Any())
                {
                    retResult = _utils.TransformToDto<LichSuDongHuiDto, LichSuDongHuiEntity>(entities).ToList();
                }
            }
            return retResult;
        }
        public bool Insert(LichSuDongHuiDto input)
        {
            bool retResult = false;
            if (input != null)
            {
                var entity = _utils.TransformToEntity<LichSuDongHuiDto, LichSuDongHuiEntity>(input);
                var insertRs = _connection.Insert(entity);
                retResult = insertRs > 0;
            }
            return retResult;
        }
    }
}
