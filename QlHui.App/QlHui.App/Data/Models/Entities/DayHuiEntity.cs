namespace qlhui.app.Data.DataAccess.Entities
{
    public class DayHuiEntity : BaseEntity
    {
        public string NgayKhui { get; set; }
        public int LoaiHui { get; set; }
        public float TienMotChan { get; set; }
        public int KyBo { get; set; }
        public int TongSoChan { get; set; }
        public string MaDayHui { get; set; }
        public string NgayBoHui { get; set; }
        public bool DaCoNguoiBoHui { get; set; }
        public float? TienThao { get; set; }
        public int SoNgayLoaiHui { get; set; }
        //public bool DayHuiKetThuc { get; set; }
    }
}
