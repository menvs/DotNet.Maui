namespace QlHui.App.Data.Models.Dtos
{
    internal class TinhTienTheoHuiVienDto
    {
        public string MaDayHui { get; set; }
        public string NgayKhui { get; set; }
        public string Ky { get; set; }
        public float? Tien1C { get; set; }
        public float? TienBo { get; set; }
        public int SoCSong { get; set; }
        public int SoCChet { get; set; }
        public float? TienPhaiThu { get; set; }
        public float? TienPhaiTra { get; set; }
        public float? TienLoi { get; set; }
        public int? TrangThai { get; set; }
        public int HuiVienThamGiaId { get; set; }
    }
}
