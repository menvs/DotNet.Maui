using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Utils;

namespace QlHui.App.Data.Services
{
    internal interface ILsDongHuiService
    {
        bool Insert(LichSuDongHuiDto input);
        IList<LichSuDongHuiDto> LayDanhSachLichSuDongHui(int huiVienThamGiaId);
    }
    internal class LsDongHuiService : BaseService, ILsDongHuiService
    {
        private readonly IUtils _utils;
        public LsDongHuiService(IUtils utils)
        {
            _utils = utils;
        }
        public IList<LichSuDongHuiDto> LayDanhSachLichSuDongHui(int huiVienThamGia)
        {
            List<LichSuDongHuiDto> retResult = [];
            if (huiVienThamGia > 0)
            {
                var entities = _connection.Table<LichSuDongHuiEntity>().Where(item => item.HuiVienThamGiaId == huiVienThamGia).ToList();
                if (entities.Count > 0)
                {
                    retResult = _utils.TransformToDto<LichSuDongHuiDto, LichSuDongHuiEntity>(entities);
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
