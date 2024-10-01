namespace QlHui.App.Data.Models.Dtos
{
    internal class LichSuBoHuiDto : BaseDto
    {
        public int KyBo { get; set; }
        public DateTime? NgayBoHui { get; set; }
        public float? SoTien { get; set; }
        public string NguoiBo { get; set; }
        public float? TongHot { get; set; }
        public string ChiTiet { get; set; }
        public int DayHuiId { get; set; }
    }
}
