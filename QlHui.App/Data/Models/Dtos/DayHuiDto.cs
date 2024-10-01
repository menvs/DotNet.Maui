namespace QlHui.App.Data.Models.Dtos
{
    internal class DayHuiDto : BaseDto
    {
        public DateTime? NgayKhui { get; set; }
        public int? LoaiHui { get; set; }
        public float? TienMotChan { get; set; }
        public float? TienThao { get; set; }
        public int KyBo { get; set; }
        public int TongSoChan { get; set; }
        public string MaDayHui { get; set; }
        public DateTime? NgayBoHui { get; set; }
        public bool DaCoNguoiBoHui { get; set; }
        public int SoNgayLoaiHui { get; set; }
        //public bool DayHuiKetThuc { get; set; }
        public DayHuiDto()
        {
            NgayKhui = DateTime.Now;
            SoNgayLoaiHui = 1;
        }
    }
}
