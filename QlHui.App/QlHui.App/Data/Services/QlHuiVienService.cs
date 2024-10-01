using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Utils;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Services
{
    internal interface IQlHuiVienService
    {
        HuiVienDto LayThongTinHuiVien(int huiVienId);
        HuiVienThamGiaDto LayThongTinHuiVienThamGia(int huiVienThamGiaId);
        IList<HuiVienDto> DsHuiVienTheoMaGanDung(string maHuiVIen);
        IList<HuiVienThamGiaDto> LayDanhSachHuiVienThamGiaDayHui(int dayHuiId);
        IList<TimKiemHuiVienAutocompleteDto> LayDanhSachHuiVienTheoTenGanDung(string tieuChiTimKiem);
        bool ThemDanhSachHuiVienThamGia(IList<HuiVienThamGiaDto> dtos);
        IList<HuiVienDto> ThemMoiDanhSachHuiVien(IList<HuiVienDto> dtos);
        bool XoaHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia);
        bool CapNhatHuiVienThamGia(HuiVienThamGiaDto huiVienThamGia);
        SearchResult<HuiVienKetQuaTimKiemDto> TimKiemHuiVien(HuiVienTieuChiTimKiemDto tieuchiTimKiem, PanigationDto panigation);
        IList<TinhTienTheoHuiVienDto> LayDanhSachTinhTienTheoHuiVien(int huiVienId);
        bool TatToanTatCa(IList<TinhTienTheoHuiVienDto> dsHuiVien);
    }

    internal class QlHuiVienService : BaseService, IQlHuiVienService
    {
        private readonly IUtils _utils;
        public QlHuiVienService(IUtils utils)
        {
            _utils = utils;
        }
        public IList<HuiVienDto> ThemMoiDanhSachHuiVien(IList<HuiVienDto> dtos)
        {
            IList<HuiVienDto> retItem = [];
            if (dtos != null && dtos.Any())
            {
                var entities = _utils.TransformToEntity<HuiVienDto, HuiVienEntity>(dtos);
                var insertRs = _connection.InsertAll(entities, true);
                if (insertRs > 0)
                {
                    var danhSachMaHuiVien = dtos.Select(item => item.MaHuiVien);
                    entities = _connection.Table<HuiVienEntity>()
                                .Where(item => danhSachMaHuiVien.Contains(item.MaHuiVien)).ToList();

                    retItem = _utils.TransformToDto<HuiVienDto, HuiVienEntity>(entities).ToList();
                }
            }
            return retItem;
        }
        public IList<HuiVienThamGiaDto> LayDanhSachHuiVienThamGiaDayHui(int dayHuiId)
        {
            IList<HuiVienThamGiaDto> retData = [];
            if (dayHuiId > 0)
            {
                var entityItems = _connection.Table<HuiVienThamGiaEntity>().Where(hv => hv.DayHuiId == dayHuiId).ToList();
                if (entityItems.Count>0)
                {
                    retData = _utils.TransformToDto<HuiVienThamGiaDto, HuiVienThamGiaEntity>(entityItems);
                }
            }
            return retData;
        }
        public bool ThemDanhSachHuiVienThamGia(IList<HuiVienThamGiaDto> dtos)
        {
            if (dtos != null && dtos.Count>0)
            {
                var entities = _utils.TransformToEntity<HuiVienThamGiaDto, HuiVienThamGiaEntity>(dtos);
                var insertRs = _connection.InsertAll(entities, true);
                return insertRs > 0;
            }
            return false;
        }
        public IList<HuiVienDto> DsHuiVienTheoMaGanDung(string maHuiVIen)
        {
            IList<HuiVienDto> dsHuiVien = [];
            if (string.IsNullOrEmpty(maHuiVIen) == false)
            {
                string upper = maHuiVIen.ToUpper();
                var dsHuiVienEntity = _connection.Table<HuiVienEntity>().Where(item => item.MaHuiVien.Contains(upper)).ToList();
                if (dsHuiVienEntity.Count>0)
                {
                    dsHuiVien = _utils.TransformToDto<HuiVienDto, HuiVienEntity>(dsHuiVienEntity);
                }
            }
            return dsHuiVien;
        }
        public IList<TimKiemHuiVienAutocompleteDto> LayDanhSachHuiVienTheoTenGanDung(string tieuChiTimKiem)
        {
            IList<TimKiemHuiVienAutocompleteDto> retData = [];
            if (string.IsNullOrEmpty(tieuChiTimKiem)) return retData;
            tieuChiTimKiem = tieuChiTimKiem.Trim();
            var queryData = _connection.Table<HuiVienEntity>()
                                        .Where(item => item.TenHuiVien.ToLower().Contains(tieuChiTimKiem.ToLower()))
                                        .Take(10).ToList();
            if (queryData != null && queryData.Count>0)
            {
                retData = queryData.Select(item =>
                {
                    var newItem = new TimKiemHuiVienAutocompleteDto
                    {
                        HuiVienId = item.Id,
                        TenHuiVien = item.TenHuiVien,
                        MaHuiVien = item.MaHuiVien,
                        DisplayName = $"{item.TenHuiVien}-{item.MaHuiVien}"
                    };
                    return newItem;
                }).ToList();
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
                if (!string.IsNullOrEmpty(tieuchiTimKiem.MaHuiVien))
                {
                    dsHuiVien = dsHuiVien.Where(item => item.MaHuiVien != null
                                                && item.MaHuiVien.ToUpper().Contains(tieuchiTimKiem.MaHuiVien.ToUpper()));
                }

                if (!string.IsNullOrEmpty(tieuchiTimKiem.TenHuiVien))
                {
                    dsHuiVien = dsHuiVien.Where(item => item.TenHuiVien != null
                                                && item.TenHuiVien.ToUpper().Contains(tieuchiTimKiem.TenHuiVien.ToUpper()));
                }

                var dshuiVienThamGia = _connection.Table<HuiVienThamGiaEntity>();
                const int ttDaThu = (int)TrangThaiDongHui.DA_THU;
                const int ttChuaThu = (int)TrangThaiDongHui.CHUA_THU;
                const int ttChuaTra = (int)TrangThaiDongHui.CHUA_TRA;

                var queryData = dsHuiVien.Join(dshuiVienThamGia.DefaultIfEmpty(),
                        hv => hv.Id, hvThamGia => hvThamGia?.HuiVienId,
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

                        var dsDuNoItems = group.Where(item => item.TrangThai != null && item.TrangThai != ttDaThu)
                                                .Select(duNo => new { duNo.TongPhaiThu, duNo.PhaiTra }).ToList();
                        if (dsDuNoItems.Count > 0)
                        {
                            float tongPhaiThuTemp = dsDuNoItems.Sum(item => item.TongPhaiThu.GetValueOrDefault());
                            float tongPhaiTraTemp = dsDuNoItems.Sum(item => item.PhaiTra.GetValueOrDefault());
                            float duNo = tongPhaiTraTemp - tongPhaiThuTemp;

                            int status = ttDaThu;
                            if (duNo > 0)
                            {
                                status = ttChuaTra;
                                retItem.TongPhaiTra = duNo;
                                retItem.TongPhaiThu = 0;
                            }
                            else if (duNo < 0)
                            {
                                status = ttChuaThu;
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
                    });

                if (tieuchiTimKiem.CanThuTien || tieuchiTimKiem.CanTraTien)
                {
                    if (tieuchiTimKiem.CanThuTien && tieuchiTimKiem.CanTraTien)
                    {
                        queryData = queryData.Where(item => item.TrangThai == ttChuaThu || item.TrangThai == ttChuaTra);
                    }
                    else if (tieuchiTimKiem.CanThuTien)
                    {
                        queryData = queryData.Where(item => item.TrangThai == ttChuaThu);
                    }
                    else
                    {
                        queryData = queryData.Where(item => item.TrangThai == ttChuaTra);
                    }
                }

                retResult.TotalItem = queryData.Count();
                retResult.Data = queryData.OrderByDescending(t => t.TenHuiVien).Skip(panigation.Skip).Take(panigation.Take).ToList();
            }
            return retResult;
        }

        public IList<TinhTienTheoHuiVienDto> LayDanhSachTinhTienTheoHuiVien(int huiVienId)
        {
            IList<TinhTienTheoHuiVienDto> retResult = [];
            if (huiVienId > 0)
            {
                int chuaThu = (int)TrangThaiDongHui.CHUA_THU;
                int chuaTra = (int)TrangThaiDongHui.CHUA_TRA;

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
                    }).ToList();

                if (dsHuiVienThamGia.Count > 0)
                {
                    var dsIdDayHui = dsHuiVienThamGia.Select(item => item.DayHuiId).Distinct();

                    var dsDayHui = _connection.Table<DayHuiEntity>().Where(item => dsIdDayHui.Contains(item.Id)).ToList();
                    if (dsDayHui.Count > 0)
                    {
                        var tempdsDayHui = dsDayHui.Select(item =>
                         {
                             var demKyBo = _connection.Table<HuiVienThamGiaEntity>().Count(i => i.DayHuiId == item.Id && i.SoCSong != 1);
                             return new
                             {
                                 item.MaDayHui,
                                 item.NgayKhui,
                                 KyBo = demKyBo,
                                 item.TongSoChan,
                                 item.TienMotChan,
                                 item.Id
                             };
                         });
                        retResult = tempdsDayHui.Join(dsHuiVienThamGia,
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
                           }).ToList();
                    }
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
                if (dsHuiVien.Count>0)
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
