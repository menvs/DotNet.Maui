using qlhui.app.Data.DataAccess.Entities;
using QlHui.App.Data.Constant;
using QlHui.App.Data.Models.Dtos;
using QlHui.App.Data.Utils;
using System.Data.Common;
using System.Linq.Expressions;
using static QlHui.App.Data.Constant.Const;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Services
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
        (bool, List<string>) BoHui(HuiVienThamGiaDto huiVienThamGia, float soTienBoHui);
        void XoaData();

    }
    internal class QlHuiService : BaseService, IQlHuiService
    {
        private readonly IUtils _utils;
        public QlHuiService(IUtils utils)
        {
            _utils = utils;
        }
        public string TaoMaDayHui(DateTime ngayKhui, int loaiHui, float tien1C, int soNgayLoaiHui)
        {
            int index = 0;
            string ngayKhuiString = ngayKhui.ToString(DateTimeFormat.UIDateFormat);
            string dateTimePrefix = ngayKhui.ToString("ddTMMyy");
            string moneyPrefixString = tien1C.ToString();
            string loaiHuiFrefix = Enum.Parse(typeof(LoaiHui), loaiHui.ToString()).ToString();
            string maHuiTemp = "";
            if (moneyPrefixString.Length > 3)
            {
                moneyPrefixString = moneyPrefixString[..^3];
            }
            else
            {
                return maHuiTemp;
            }
            var queryData = _connection.Table<DayHuiEntity>().Where(item =>
                            item.NgayKhui == ngayKhuiString && item.LoaiHui == loaiHui
                            && item.TienMotChan == tien1C && item.SoNgayLoaiHui == soNgayLoaiHui);
            if (queryData != null)
            {
                do
                {
                    index++;
                    maHuiTemp = $"{dateTimePrefix}-{moneyPrefixString}/{soNgayLoaiHui}{loaiHuiFrefix}-DAY{index}";
                } while (queryData.Any(item => item.MaDayHui == maHuiTemp));
            }
            return maHuiTemp;
        }
        public bool Create(DayHuiDto input)
        {
            if (input == null) return false;
            var entity = _utils.TransformToEntity<DayHuiDto, DayHuiEntity>(input);
            var rs = _connection.Insert(entity);
            return rs > 0;
        }
        public bool Update(DayHuiDto input)
        {
            if (input == null || input.Id <= 0) return false;
            var entity = _utils.TransformToEntity<DayHuiDto, DayHuiEntity>(input);
            var rs = _connection.Update(entity);
            return rs > 0;
        }
        public SearchResult<DayHuiDto> Search(QLDayHuiSearchCriteria criteria, PanigationDto panigation)
        {
            var retData = new SearchResult<DayHuiDto>();
            var queryItems = _connection.Table<DayHuiEntity>().AsQueryable();
            Expression<Func<DayHuiEntity, bool>> expression = item => true;
            if (criteria.HienThiTatCaDayHuiCanBoHomNay)
            {
                string currentDate = DateTime.Now.Date.ToString(DateTimeFormat.UIDateFormat);
                queryItems = queryItems.AsEnumerable().Where(item =>
                            item.NgayBoHui != null && DateTime.ParseExact(item.NgayBoHui, DateTimeFormat.UIDateFormat, null).Date <= DateTime.Now.Date).AsQueryable();
            }
            if (criteria.NgayKhui.HasValue)
            {
                string ngayKhui = criteria.NgayKhui.Value.Date.ToString(DateTimeFormat.UIDateFormat);
                queryItems = queryItems.Where(item => ngayKhui.Equals(item.NgayKhui));
            }
            if (criteria.TienMotChan.HasValue)
            {
                queryItems = queryItems.Where(item => criteria.TienMotChan == item.TienMotChan);
            }
            if (criteria.MaDayHui.IsNotEmpty())
            {
                queryItems = queryItems.Where(item => item.MaDayHui != null
                                        && item.MaDayHui.Contains(criteria.MaDayHui.ToUpper()));
            }
            var tempItems = queryItems.OrderByDescending(item => item.Id);

            retData.TotalItem = tempItems.Count();
            var entityListItem = tempItems.Skip(panigation.Skip).Take(panigation.Take);
            if (entityListItem.Any())
            {
                retData.Data = _utils.TransformToDto<DayHuiDto, DayHuiEntity>(entityListItem).ToList();
            }
            return retData;
        }
        public bool DeleteDayHuiById(int id)
        {
            var returnValue = false;
            _connection.BeginTransaction();
            try
            {
                var tonTaiLichSuBoHui = _connection.Table<LichSuBoHuiEntity>().Any(item => item.DayHuiId == id);
                if (tonTaiLichSuBoHui)
                {
                    _connection.ExecuteScalar<LichSuBoHuiEntity>($"delete from LichSuBoHuiEntity where DayHuiId == '{id}'");
                }
                var huiVienThamGiaIds = _connection.Table<HuiVienThamGiaEntity>().Where(item => item.DayHuiId == id).Select(item => item.Id);
                if (huiVienThamGiaIds.Any())
                {
                    foreach (var hvThamId in huiVienThamGiaIds)
                    {
                        _connection.ExecuteScalar<LichSuDongHuiEntity>($"delete from LichSuDongHuiEntity where HuiVienThamGiaId == '{hvThamId}'");
                    }
                    _connection.ExecuteScalar<HuiVienThamGiaEntity>($"delete from HuiVienThamGiaEntity where DayHuiId == '{id}'");
                }

                var exsitedItem = _connection.Table<DayHuiEntity>().FirstOrDefault(item => item.Id == id);
                if (exsitedItem != null)
                {
                    var rs = _connection.Delete<DayHuiEntity>(exsitedItem.Id);
                    returnValue = rs > 0;
                }
            }
            catch (Exception)
            {
                returnValue = false;
                _connection.Rollback();
                throw;
            }

            if (returnValue)
            {
                _connection.Commit();
            }
            else
            {
                _connection.Rollback();
            }
            return returnValue;
        }
        public DayHuiDto GetById(int id)
        {
            var entity = _connection.Table<DayHuiEntity>().FirstOrDefault(item => item.Id == id);
            if (entity != null)
            {
                return _utils.TransformToDto<DayHuiDto, DayHuiEntity>(entity);
            }
            return null;
        }
        public bool DayHuiDaCoNguoiBoChua(int dayHuiId)
        {
            var dsNgThamGiaHui = _connection.Table<HuiVienThamGiaEntity>().Where(item => item.DayHuiId == dayHuiId);
            if (dsNgThamGiaHui != null && dsNgThamGiaHui.Any(item => item.SoCChet > 0))
            {
                return true;
            }
            return false;
        }
        public bool DoiNguoiThamGiaDayHui(HuiVienDto huiVienDoiThanh, int idHuiVienThamGia)
        {
            var huiVienDaThamGia = _connection.Table<HuiVienThamGiaEntity>()
                                .FirstOrDefault(hv => hv.Id == idHuiVienThamGia && hv.DaBoHui != true);

            if (huiVienDaThamGia != null)
            {
                var huiVienThamGiaMoi = huiVienDaThamGia;
                huiVienThamGiaMoi.HuiVienId = huiVienDoiThanh.Id;
                huiVienThamGiaMoi.TenHuiVien = huiVienDoiThanh.TenHuiVien;
                huiVienThamGiaMoi.MaHuiVien = huiVienDoiThanh.MaHuiVien;
                var updateRs = _connection.Update(huiVienThamGiaMoi);
                return updateRs > 0;
            }

            return false;
        }
        public (bool, List<string>) BoHui(HuiVienThamGiaDto huiVienThamGia, float soTienBoHui)
        {
            bool result = false;
            List<string> errorMessages = [];
            try
            {
                _connection.BeginTransaction();
                var danhSachHuiVienThamGia = _connection.Table<HuiVienThamGiaEntity>().Where(item => item.DayHuiId == huiVienThamGia.DayHuiId).ToList();
                var thongTinDayHui = _connection.Get<DayHuiEntity>(huiVienThamGia.DayHuiId);
                bool boHuiLanDau = thongTinDayHui?.DaCoNguoiBoHui == false;
                bool tienBoHuiLonHonTien1C = soTienBoHui > thongTinDayHui.TienMotChan;
                if (tienBoHuiLonHonTien1C)
                {
                    errorMessages.Add($"Không thể bỏ hụi vì số tiền bỏ hụi {soTienBoHui.GetMoneyFormat()} > Tiền 1 C {thongTinDayHui.TienMotChan.GetMoneyFormat()}");
                }
                var lichSuBoHui = _connection.Table<LichSuBoHuiEntity>().FirstOrDefault(item => item.KyBo == thongTinDayHui.KyBo && item.DayHuiId == thongTinDayHui.Id);
                if (lichSuBoHui != null)
                {
                    errorMessages.Add($"Kỳ hụi thứ {lichSuBoHui.KyBo} đã được {lichSuBoHui.NguoiBo} bỏ với số tiền {lichSuBoHui.SoTien.GetMoneyFormat()} ngày {lichSuBoHui.NgayBoHui}");
                }

                if (danhSachHuiVienThamGia.Count > 0 && errorMessages.Count == 0)
                {
                    var dsHuiVienThamGiaConLai = danhSachHuiVienThamGia.Where(item => item.Id != huiVienThamGia.Id);
                    int soCSongConLai = dsHuiVienThamGiaConLai.Sum(item => item.SoCSong);
                    int soCChetConLai = dsHuiVienThamGiaConLai.Sum(item => item.SoCChet);
                    float soTienChanSong = (thongTinDayHui.TienMotChan - soTienBoHui) * soCSongConLai;
                    float soTienChanChet = soCChetConLai * thongTinDayHui.TienMotChan;
                    var huiVienBoHui = danhSachHuiVienThamGia.FirstOrDefault(item => item.Id == huiVienThamGia.Id);
                    if (huiVienBoHui != null)
                    {
                        huiVienBoHui.SoTienBo = soTienBoHui;
                        huiVienBoHui.SoCSong = 0;
                        huiVienBoHui.SoCChet = 1;
                        huiVienBoHui.TrangThai = TrangThaiDongHui.CHUA_TRA.GetHashCode();
                        if (boHuiLanDau)
                        {
                            huiVienBoHui.TienLoi = huiVienBoHui.TienLoi.GetValueOrDefault() - soTienBoHui * soCSongConLai - thongTinDayHui.TienThao.GetValueOrDefault();
                            huiVienBoHui.PhaiTra = soTienChanSong + soTienChanChet - thongTinDayHui.TienThao.GetValueOrDefault();
                        }
                        else
                        {
                            huiVienBoHui.TienLoi = huiVienBoHui.TienLoi.GetValueOrDefault() - soTienBoHui * soCSongConLai;
                            huiVienBoHui.PhaiTra = soTienChanSong + soTienChanChet;
                        }

                        huiVienBoHui.DaBoHui = true;
                        huiVienBoHui.NgayBoHui = DateTime.Now.ToString(DateTimeFormat.UIDateFormat);

                        var updateRs = _connection.Update(huiVienBoHui);
                        if (updateRs > 0)
                        {
                            var capNhatDanhSachThamGia = dsHuiVienThamGiaConLai.Select(item =>
                            {

                                if (boHuiLanDau)
                                {
                                    item.TienLoi = soTienBoHui - thongTinDayHui.TienThao.GetValueOrDefault();
                                    item.KyHienTaiPhaiThu = thongTinDayHui.TienMotChan - soTienBoHui + thongTinDayHui.TienThao.GetValueOrDefault();
                                    item.TongPhaiThu = item.TongPhaiThu.GetValueOrDefault() + item.KyHienTaiPhaiThu.GetValueOrDefault();
                                }
                                else
                                {
                                    item.TienLoi = item.TienLoi.GetValueOrDefault() + soTienBoHui;
                                    float kyHienTaiPhaiThuTemp = 0;
                                    if (item.DaBoHui)
                                    {
                                        kyHienTaiPhaiThuTemp = thongTinDayHui.TienMotChan;
                                    }
                                    else
                                    {
                                        kyHienTaiPhaiThuTemp = thongTinDayHui.TienMotChan - soTienBoHui;

                                    }
                                    float tongPhaiThu = item.TongPhaiThu.GetValueOrDefault();
                                    float duNoPhaiThu = tongPhaiThu + kyHienTaiPhaiThuTemp;
                                    if (tongPhaiThu <= 0)
                                    {
                                        if (duNoPhaiThu > 0)
                                        {
                                            item.KyHienTaiPhaiThu = duNoPhaiThu;
                                        }
                                        else
                                        {
                                            item.KyHienTaiPhaiThu = 0;
                                        }
                                    }
                                    else
                                    {
                                        item.KyHienTaiPhaiThu = kyHienTaiPhaiThuTemp;
                                    }
                                    item.TongPhaiThu = tongPhaiThu + kyHienTaiPhaiThuTemp;
                                }

                                if ((item.TrangThai.HasValue == false
                                || item.TrangThai == (int)TrangThaiDongHui.DA_THU
                                || item.TrangThai == (int)TrangThaiDongHui.DA_TRA)
                                && item.KyHienTaiPhaiThu.GetValueOrDefault() > 0)
                                {
                                    item.TrangThai = (int)TrangThaiDongHui.CHUA_THU;
                                }
                                return item;
                            });

                            updateRs = _connection.UpdateAll(capNhatDanhSachThamGia);
                            if (updateRs > 0)
                            {
                                var lichSuDongHui = new LichSuBoHuiEntity()
                                {
                                    KyBo = thongTinDayHui.KyBo,
                                    DayHuiId = thongTinDayHui.Id,
                                    NgayBoHui = DateTime.Now.ToString(DateTimeFormat.UIDateFormat),
                                    NgayTao = DateTime.Now.ToString(DateTimeFormat.UIDateFormat),
                                    SoTien = soTienBoHui,
                                    NguoiBo = $"{huiVienThamGia.TenHuiVien}-{huiVienThamGia.MaHuiVien}",
                                    TongHot = huiVienBoHui.PhaiTra.GetValueOrDefault(),
                                    ChiTiet = $"Chốt {soTienBoHui.GetMoneyFormat()} \n" +
                                              $"Chết {soCChetConLai}x{thongTinDayHui.TienMotChan.GetMoneyFormat()} = {soTienChanChet.GetMoneyFormat()} \n" +
                                              $"Sống {soCSongConLai}x{(thongTinDayHui.TienMotChan - soTienBoHui).GetMoneyFormat()} = {soTienChanSong.GetMoneyFormat()} \n" +
                                               (boHuiLanDau ? $"Tiền thảo: {thongTinDayHui.TienThao.GetMoneyFormat()} \n" : "") +
                                              $"Tổng hốt {huiVienBoHui.PhaiTra.GetMoneyFormat()}"
                                };
                                var insertRs = _connection.Insert(lichSuDongHui);
                                if (thongTinDayHui.DaCoNguoiBoHui != true)
                                    thongTinDayHui.DaCoNguoiBoHui = true;
                                if (thongTinDayHui.KyBo < thongTinDayHui.TongSoChan)
                                {
                                    DateTime? ngayBoHui = DateTime.ParseExact(thongTinDayHui.NgayBoHui, DateTimeFormat.UIDateFormat, null);
                                    thongTinDayHui.NgayBoHui = ngayBoHui.TinhNgayBoHuiTiepTheo(thongTinDayHui.LoaiHui, thongTinDayHui.SoNgayLoaiHui).GetValueOrDefault().ToString(DateTimeFormat.UIDateFormat);
                                }
                                //else if (thongTinDayHui.KyBo == thongTinDayHui.TongSoChan)
                                //{
                                //    thongTinDayHui.DayHuiKetThuc = true;
                                //}

                                var updateDayHuiRs = _connection.Update(thongTinDayHui);
                                result = insertRs > 0 && updateDayHuiRs > 0;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _connection.Rollback();
                errorMessages.Add($"Lỗi hệ thống: {ex.Message}");
            }

            if (result) _connection.Commit();
            else _connection.Rollback();

            return (result, errorMessages);
        }

        public void XoaData()
        {
            DeleteAllTable();
        }
    }
}
