using SQLite;

namespace qlhui.app.Data.DataAccess.Entities
{
    public class HuiVienThamGiaEntity : BaseEntity
    {
        [Indexed]
        public int HuiVienId { get; set; }
        [Indexed]
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
        public bool DaBoHui { get; set; }
        public float? SoTienBo { get; set; }
        public float? TienLoi { get; set; }
        public float? TienDaTra { get; set; }
        public string NgayBoHui { get; set; }
    }
}
