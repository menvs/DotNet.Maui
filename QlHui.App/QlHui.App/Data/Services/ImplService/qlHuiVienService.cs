using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Services.IService;
using QlHui.App.Data.Utils;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Services.ImplService
{
    internal class QlHuiVienService : BaseService, IQlHuiVienService
    {
        private readonly IUtils _utils;
        public QlHuiVienService(IUtils utils)
        {
            _utils = utils;
        }
        public IEnumerable<HuiVienDto> ThemMoiDanhSachHuiVien(IEnumerable<HuiVienDto> dtos)
        {
            var retItem = Enumerable.Empty<HuiVienDto>();
            if (dtos != null && dtos.Any())
            {
                var entities = _utils.TransformToEntity<HuiVienDto, HuiVienEntity>(dtos);
                var insertRs = _connection.InsertAll(entities, true);
                if (insertRs > 0)
                {
                    var danhSachMaHuiVien = dtos.Select(item => item.MaHuiVien);
                    entities = _connection.Table<HuiVienEntity>().Where(item => danhSachMaHuiVien.Contains(item.MaHuiVien)).AsEnumerable();
                    retItem = _utils.TransformToDto<HuiVienDto, HuiVienEntity>(entities);
                }
            }
            return retItem;
        }
        public IEnumerable<HuiVienThamGiaDto> LayDanhSachHuiVienThamGiaDayHui(int dayHuiId)
        {
            var retData = Enumerable.Empty<HuiVienThamGiaDto>();
            if (dayHuiId > 0)
            {
                var entityItems = _connection.Table<HuiVienThamGiaEntity>().Where(hv => hv.DayHuiId == dayHuiId).AsEnumerable();
                if (entityItems.Any())
                {
                    retData = _utils.TransformToDto<HuiVienThamGiaDto, HuiVienThamGiaEntity>(entityItems);
                }
            }
            return retData;
        }
        public bool ThemDanhSachHuiVienThamGia(IEnumerable<HuiVienThamGiaDto> dtos)
        {
            if (dtos != null && dtos.Any())
            {
                var entities = _utils.TransformToEntity<HuiVienThamGiaDto, HuiVienThamGiaEntity>(dtos);
                if (entities.Any())
                {
                    var insertRs = _connection.InsertAll(entities, true);
                    return insertRs > 0;
                }
            }
            return false;
        }
        public IEnumerable<HuiVienDto> DsHuiVienTheoMaGanDung(string maHuiVIen)
        {
            var dsHuiVien = Enumerable.Empty<HuiVienDto>();
            if (string.IsNullOrEmpty(maHuiVIen) == false)
            {
                string upper = maHuiVIen.ToUpper();
                var dsHuiVienEntity = _connection.Table<HuiVienEntity>().Where(item => item.MaHuiVien.Contains(upper)).AsEnumerable();
                if (dsHuiVienEntity.Any())
                {
                    dsHuiVien = _utils.TransformToDto<HuiVienDto, HuiVienEntity>(dsHuiVienEntity);
                }
            }
            return dsHuiVien;
        }
        public IEnumerable<TimKiemHuiVienAutocompleteDto> LayDanhSachHuiVienTheoTenGanDung(string tieuChiTimKiem)
        {
            var retData = Enumerable.Empty<TimKiemHuiVienAutocompleteDto>();
            if (string.IsNullOrEmpty(tieuChiTimKiem)) return retData;
            tieuChiTimKiem = tieuChiTimKiem.Trim();
            //string query = $"SELECT Id,TenHuiVien,MaHuiVien FROM HuiVienEntity WHERE upper(TenHuiVien) like '%'";
            //var tempList = _connection.DeferredQuery<HuiVienEntity>(query, tieuChiTimKiem.Trim().ToUpper());
            var tempList = _connection.Table<HuiVienEntity>().AsEnumerable();
            var queryData = tempList.Where(item => item.TenHuiVien.Contains(tieuChiTimKiem, StringComparison.CurrentCultureIgnoreCase));
            if (queryData != null && queryData.Any())
            {
                retData = queryData.Take(10).Select(item =>
                {
                    var newItem = new TimKiemHuiVienAutocompleteDto
                    {
                        HuiVienId = item.Id,
                        TenHuiVien = item.TenHuiVien,
                        MaHuiVien = item.MaHuiVien,
                        DisplayName = $"{item.TenHuiVien}-{item.MaHuiVien}"
                    };
                    return newItem;
                });
            }
            return retData;
        }
        public bool XoaHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia)
        {
            if (huiVienThamGia != null)
            {
                var entity = _utils.TransformToEntity<HuiVienThamGiaDto, HuiVienThamGiaEntity>(huiVienThamGia);
                var rs = _connection.Delete(entity);
                return rs > 0;
            }
            return false;
        }
        public bool CapNhatHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia)
        {
            if (huiVienThamGia != null && huiVienThamGia.Id > 0)
            {
                var entity = _utils.TransformToEntity<HuiVienThamGiaDto, HuiVienThamGiaEntity>(huiVienThamGia);
                var rs = _connection.Update(entity);
                return rs > 0;
            }
            return false;
        }
        public HuiVienDto LayThongTinHuiVien(int huiVienId)
        {
            if (huiVienId > 0)
            {
                var entity = _connection.Table<HuiVienEntity>().FirstOrDefault(item => item.Id == huiVienId);
                if (entity != null)
                {
                    return _utils.TransformToDto<HuiVienDto, HuiVienEntity>(entity);
                }
            }
            return null;
        }

        public HuiVienThamGiaDto LayThongTinHuiVienThamGia(int huiVienThamGiaId)
        {
            var entity = _connection.Get<HuiVienThamGiaEntity>(huiVienThamGiaId);
            if (entity != null)
            {
                return _utils.TransformToDto<HuiVienThamGiaDto, HuiVienThamGiaEntity>(entity);
            }
            return null;
        }

        public SearchResult<HuiVienKetQuaTimKiemDto> TimKiemHuiVien(HuiVienTieuChiTimKiemDto tieuchiTimKiem, PanigationDto panigation)
        {
            SearchResult<HuiVienKetQuaTimKiemDto> retResult = new();
            if (tieuchiTimKiem != null)
            {
                var dsHuiVien = _connection.Table<HuiVienEntity>();
                var dshuiVienThamGia = _connection.Table<HuiVienThamGiaEntity>();

                var queryData = dsHuiVien.Join(dshuiVienThamGia.DefaultIfEmpty(),
                        hv => hv.Id, hvThamGia => hvThamGia.HuiVienId,
                        (hv, hvThamGia) => new
                        {
                            hv.MaHuiVien,
                            hv.TenHuiVien,
                            HuiVienId = hv.Id,
                            hvThamGia?.TrangThai,
                            hvThamGia?.TongPhaiThu,
                            hvThamGia?.PhaiTra,
                        })
                    .GroupBy(item => $"{item.MaHuiVien}-{item.HuiVienId}").Select(group =>
                    {
                        var firstItem = group.FirstOrDefault();
                        var retItem = new HuiVienKetQuaTimKiemDto()
                        {
                            HuiVienId = firstItem.HuiVienId,
                            TenHuiVien = firstItem?.TenHuiVien,
                            MaHuiVien = firstItem?.MaHuiVien,
                        };

                        var dsDuNoItems = group.Where(item => item.TrangThai != null);
                        if (dsDuNoItems.Any())
                        {
                            dsDuNoItems = dsDuNoItems.Where(item => item.TrangThai != TrangThaiDongHui.DA_THU.GetHashCode());

                            float tongPhaiThuTemp = dsDuNoItems.Sum(item => item.TongPhaiThu.GetValueOrDefault());
                            float tongPhaiTraTemp = dsDuNoItems.Sum(item => item.PhaiTra.GetValueOrDefault());
                            float duNo = tongPhaiTraTemp - tongPhaiThuTemp;

                            int status = TrangThaiDongHui.DA_THU.GetHashCode();
                            if (duNo > 0)
                            {
                                status = TrangThaiDongHui.CHUA_TRA.GetHashCode();
                                retItem.TongPhaiTra = duNo;
                                retItem.TongPhaiThu = 0;
                            }
                            else if (duNo < 0)
                            {
                                status = TrangThaiDongHui.CHUA_THU.GetHashCode();
                                retItem.TongPhaiThu = -duNo;
                                retItem.TongPhaiTra = 0;
                            }
                            retItem.TrangThai = status;
                        }
                        else
                        {
                            retItem.TrangThai = null;
                            retItem.TongPhaiTra = 0;
                            retItem.TongPhaiThu = 0;
                        }

                        return retItem;
                    }).AsEnumerable();

                if (queryData.Any())
                {
                    if (string.IsNullOrEmpty(tieuchiTimKiem.MaHuiVien) == false)
                    {
                        queryData = queryData.Where(item => item.MaHuiVien != null && item.MaHuiVien.Contains(tieuchiTimKiem.MaHuiVien, StringComparison.CurrentCultureIgnoreCase));
                    }
                    if (string.IsNullOrEmpty(tieuchiTimKiem.TenHuiVien) == false)
                    {
                        queryData = queryData.Where(item => item.TenHuiVien != null && item.TenHuiVien.Contains(tieuchiTimKiem.TenHuiVien, StringComparison.CurrentCultureIgnoreCase));
                    }
                    if (tieuchiTimKiem.CanThuTien || tieuchiTimKiem.CanTraTien)
                    {
                        if (tieuchiTimKiem.CanThuTien && tieuchiTimKiem.CanTraTien)
                        {
                            queryData = queryData.Where(item => item.TrangThai == TrangThaiDongHui.CHUA_THU.GetHashCode() || item.TrangThai == TrangThaiDongHui.CHUA_TRA.GetHashCode());
                        }
                        else if (tieuchiTimKiem.CanThuTien)
                        {
                            queryData = queryData.Where(item => item.TrangThai == TrangThaiDongHui.CHUA_THU.GetHashCode());
                        }
                        else
                        {
                            queryData = queryData.Where(item => item.TrangThai == TrangThaiDongHui.CHUA_TRA.GetHashCode());
                        }
                    }
                    retResult.TotalItem = queryData.Count();
                    retResult.Data = queryData.Skip(panigation.Skip).Take(panigation.Take);
                }
            }
            return retResult;
        }

        public IEnumerable<TinhTienTheoHuiVienDto> LayDanhSachTinhTienTheoHuiVien(int huiVienId)
        {
            var retResult = Enumerable.Empty<TinhTienTheoHuiVienDto>();
            if (huiVienId > 0)
            {
                int chuaThu = TrangThaiDongHui.CHUA_THU.GetHashCode();
                int chuaTra = TrangThaiDongHui.CHUA_TRA.GetHashCode();

                var dsHuiVienThamGia = _connection.Table<HuiVienThamGiaEntity>()
                    .Where(item => item.HuiVienId == huiVienId && (item.TrangThai == chuaThu || item.TrangThai == chuaTra))
                    .Select(item => new
                    {
                        item.DayHuiId,
                        item.SoCSong,
                        item.SoCChet,
                        item.TongPhaiThu,
                        item.PhaiTra,
                        item.TienLoi,
                        item.TrangThai,
                        item.Id
                    });

                if (dsHuiVienThamGia.Any())
                {
                    var dsIdDayHui = dsHuiVienThamGia.Select(item => item.DayHuiId).Distinct();

                    var dsDayHui = _connection.Table<DayHuiEntity>().Where(item => dsIdDayHui.Contains(item.Id)).Select(item =>
                    {
                        var tempItem = _connection.Table<HuiVienThamGiaEntity>().Where(i => i.DayHuiId == item.Id && i.SoCSong != 1).Select(z => z.Id).Count();
                        return new
                        {
                            item.MaDayHui,
                            item.NgayKhui,
                            KyBo = tempItem,
                            item.TongSoChan,
                            item.TienMotChan,
                            item.Id
                        };
                    });

                    retResult = dsDayHui.Join(dsHuiVienThamGia,
                        dayHui => dayHui.Id,
                        huiVienThamGia => huiVienThamGia.DayHuiId,
                        (dayHui, huiVienThamGia) =>
                    new TinhTienTheoHuiVienDto()
                    {
                        MaDayHui = dayHui.MaDayHui,
                        NgayKhui = dayHui.NgayKhui,
                        Ky = $"{dayHui.KyBo}/{dayHui.TongSoChan}",
                        Tien1C = dayHui.TienMotChan,
                        SoCSong = huiVienThamGia.SoCSong,
                        SoCChet = huiVienThamGia.SoCChet,
                        TienPhaiThu = huiVienThamGia.TongPhaiThu,
                        TienPhaiTra = huiVienThamGia.PhaiTra,
                        TienLoi = huiVienThamGia.TienLoi,
                        TrangThai = huiVienThamGia.TrangThai,
                        HuiVienThamGiaId = huiVienThamGia.Id
                    }).AsEnumerable();
                }
            }
            return retResult;
        }

        public bool TatToanTatCa(IList<TinhTienTheoHuiVienDto> dsHuiVien)
        {
            bool retResult = false;
            try
            {
                _connection.BeginTransaction();
                if (dsHuiVien != null && dsHuiVien.Any())
                {
                    var dsLichSuDongHuiMoi = new List<LichSuDongHuiDto>();
                    var dsHuiVienThamGiaUpdate = new List<HuiVienThamGiaDto>();
                    foreach (var hv in dsHuiVien)
                    {
                        var hvThamGia = _connection.Get<HuiVienThamGiaEntity>(hv.HuiVienThamGiaId);
                        int kyBo = int.Parse(hv.Ky.Split("/").First());
                        var hvThamGiaDtos = _utils.TransformToDto<HuiVienThamGiaDto, HuiVienThamGiaEntity>(hvThamGia);
                        if (hvThamGia.TrangThai == TrangThaiDongHui.CHUA_THU.GetHashCode())
                        {
                            float soTien = hv.TienPhaiThu.GetValueOrDefault() - hv.TienPhaiTra.GetValueOrDefault();
                            hvThamGiaDtos = hvThamGiaDtos.ThuTien(soTien);
                            dsLichSuDongHuiMoi.Add(new LichSuDongHuiDto().LayLsThuTien(hvThamGiaDtos, kyBo, soTien));
                        }
                        else
                        {
                            // float soTien = hv.TienPhaiTra.GetValueOrDefault() - hv.TienPhaiThu.GetValueOrDefault();
                            hvThamGiaDtos = hvThamGiaDtos.TraTien();
                            dsLichSuDongHuiMoi.Add(new LichSuDongHuiDto().LayLsTraTien(hvThamGiaDtos, kyBo));
                        }
                        dsHuiVienThamGiaUpdate.Add(hvThamGiaDtos);
                    }

                    var updateListEntities = _utils.TransformToEntity<HuiVienThamGiaDto, HuiVienThamGiaEntity>(dsHuiVienThamGiaUpdate);
                    var updateRs = _connection.UpdateAll(updateListEntities);

                    var historyEntity = _utils.TransformToEntity<LichSuDongHuiDto, LichSuDongHuiEntity>(dsLichSuDongHuiMoi);
                    var insertHistoryRs = _connection.InsertAll(historyEntity);

                    retResult = updateRs > 0 && insertHistoryRs > 0;
                }
            }
            catch (Exception)
            {
                _connection.Rollback();
                throw;
            }

            if (retResult)
                _connection.Commit();
            else
                _connection.Rollback();

            return retResult;
        }

    }
}
