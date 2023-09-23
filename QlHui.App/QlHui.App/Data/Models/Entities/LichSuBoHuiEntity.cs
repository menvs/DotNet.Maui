using SQLite;

namespace qlhui.app.Data.DataAccess.Entities
{
    public class LichSuBoHuiEntity : BaseEntity
    {
        public int KyBo { get; set; }
        public string NgayBoHui { get; set; }
        public float SoTien { get; set; }
        public string NguoiBo { get; set; }
        public float TongHot { get; set; }
        public string ChiTiet { get; set; }
        [Indexed]
        public int DayHuiId { get; set; }
    }
}
