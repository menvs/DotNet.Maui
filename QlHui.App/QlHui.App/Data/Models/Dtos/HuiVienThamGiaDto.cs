using QlHui.App.Data.Constant;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Models.Dtos
{
    internal class HuiVienThamGiaDto : BaseDto
    {
        public int HuiVienId { get; set; }
        public int DayHuiId { get; set; }
        public string TenHuiVien { get; set; }
        public string MaHuiVien { get; set; }
        public string GhiChu { get; set; }
        public int? TrangThai { get; set; }
        public int SoCSong { get; set; }
        public int SoCChet { get; set; }
        public float? TongDaDong { get; set; }
        public float? TongPhaiThu { get; set; }
        public float? PhaiTra { get; set; }
        public float? KyHienTaiPhaiThu { get; set; }
        public int SoCThamGia { get; set; }
        public bool DaBoHui { get; set; }
        public float? SoTienBo { get; set; }
        public float? TienLoi { get; set; }
        public float? TienDaTra { get; set; }
        public HuiVienThamGiaDto()
        {
            SoCThamGia = 1;
            SoCSong = 0;
            SoCChet = 0;
        }
        public HuiVienThamGiaDto ThuTien(float? soTien = null)
        {
            var tempItem = this.DeepCopy();
            float soTienThuTemp = soTien.HasValue ? soTien.Value : tempItem.KyHienTaiPhaiThu.GetValueOrDefault();
            tempItem.TongDaDong = tempItem.TongDaDong.GetValueOrDefault() + soTienThuTemp;
            tempItem.TongPhaiThu = tempItem.TongPhaiThu.GetValueOrDefault() - soTienThuTemp;

            float kyHienTaiPhaiThuTemp = tempItem.KyHienTaiPhaiThu.GetValueOrDefault() - soTienThuTemp;
            if (kyHienTaiPhaiThuTemp < 0)
            {
                tempItem.KyHienTaiPhaiThu = 0;
            }
            else
            {
                tempItem.KyHienTaiPhaiThu = kyHienTaiPhaiThuTemp;
            }
            if (tempItem.KyHienTaiPhaiThu == 0)
                tempItem.TrangThai = TrangThaiDongHui.DA_THU.GetHashCode();
            return tempItem;
        }
        public HuiVienThamGiaDto TraTien()
        {
            var tempItem = this.DeepCopy();
            float tienTra = tempItem.PhaiTra.GetValueOrDefault() - tempItem.TongPhaiThu.GetValueOrDefault();
            if (tempItem.TongPhaiThu.GetValueOrDefault() > 0)
            {
                tempItem = tempItem.ThuTien(tempItem.TongPhaiThu);
            }
            tempItem.TienDaTra = tempItem.TienDaTra.GetValueOrDefault() + tienTra;
            tempItem.PhaiTra = 0;
            tempItem.TrangThai = TrangThaiDongHui.DA_TRA.GetHashCode();
            return tempItem;
        }
    }

}
