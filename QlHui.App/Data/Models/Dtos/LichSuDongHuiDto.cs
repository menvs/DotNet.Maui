using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Models.Dtos
{
    internal class LichSuDongHuiDto : BaseDto
    {
        public DateTime? NgayDongHui { get; set; }
        public float? SoTien { get; set; }
        public int SoChanChet { get; set; }
        public int SoChanSong { get; set; }
        public float? TienPhaiThu { get; set; }
        public float? TienPhaiTra { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public int HuiVienThamGiaId { get; set; }
        public int KyBo { get; set; }
        public LichSuDongHuiDto LayLsTraTien(HuiVienThamGiaDto dto, int kyBo)
        {
            NgayDongHui = DateTime.Now;
            TienPhaiTra = dto.TienDaTra;
            SoTien = dto.SoTienBo;
            SoChanSong = dto.SoCSong;
            SoChanChet = dto.SoCChet;
            TrangThai = TrangThaiDongHui.DA_TRA.GetHashCode();
            KyBo = kyBo;
            HuiVienThamGiaId = dto.Id;
            NgayTao = DateTime.Now;
            return this;
        }
        public LichSuDongHuiDto LayLsThuTien(HuiVienThamGiaDto dto, int kyBo, float? tienDong)
        {
            NgayDongHui = DateTime.Now;
            TienPhaiThu = tienDong;
            SoTien = dto.SoTienBo;
            SoChanSong = dto.SoCSong;
            SoChanChet = dto.SoCChet;
            TrangThai = TrangThaiDongHui.DA_THU.GetHashCode();
            KyBo = kyBo;
            HuiVienThamGiaId = dto.Id;
            NgayTao = DateTime.Now;
            return this;
        }
    }
}
