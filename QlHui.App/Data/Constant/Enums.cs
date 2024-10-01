using QlHui.App.Data.Models.Dtos;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Constant
{
    internal static class Enums
    {
        public enum LoaiHui
        {
            NGAY,
            TUAN,
            THANG
        }
        public enum FormMode
        {
            EDIT,
            VIEW,
        }
        public enum TrangThaiDongHui
        {
            CHUA_THU,
            DA_THU,
            CHUA_TRA,
            DA_TRA
        }
    }
    internal static class Const
    {
        public static List<LoaiHuiDto> DanhSachLoaiHui = new()
        {
            new LoaiHuiDto()
            {
                Value = LoaiHui.NGAY.GetHashCode(),
                DisplayName = "Ngày"
            },
            new LoaiHuiDto()
            {
                Value = LoaiHui.TUAN.GetHashCode(),
                DisplayName = "Tuần"
            },
            new LoaiHuiDto()
            {
                Value = LoaiHui.THANG.GetHashCode(),
                DisplayName = "Tháng"
            }
        };
        public static class DateTimeFormat
        {
            public static string UTCDateTime = "dd-MM-yyyyTHH:mm:ssZ";
            public static string UIDateFormat = "dd/MM/yyyy";
        }
        public static class NumericFormat
        {
            public static string MoneyFormat = "#,##0";
        }
        public const string EmptyTable = "Không có dữ liệu";
        public const string SummaryTable = "Hiển thị trang {0} của {1} (Tổng {2} dòng)";
    }
}
