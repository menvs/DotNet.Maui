using SQLite;

namespace qlhui.app.Data.DataAccess.Entities
{
    public class LichSuDongHuiEntity : BaseEntity
    {
        public string NgayDongHui { get; set; }
        public float? SoTien { get; set; }
        public int SoChanChet { get; set; }
        public int SoChanSong { get; set; }
        public float? TienPhaiThu { get; set; }
        public float? TienPhaiTra { get; set; }
        public string GhiChu { get; set; }
        [Indexed]
        public int HuiVienThamGiaId { get; set; }
        public int KyBo { get; set; }
        public int TrangThai { get; set; }
    }
}
