namespace QlHui.App.Data.Models.Dtos
{
    internal class HuiVienKetQuaTimKiemDto
    {
        public int HuiVienId { get; set; }
        public string MaHuiVien { get; set; }
        public string TenHuiVien { get; set; }
        public float? TongPhaiThu { get; set; }
        public float? TongPhaiTra { get; set; }
        public int? TrangThai { get; set; }
    }
}
