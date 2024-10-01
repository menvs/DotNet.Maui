using Newtonsoft.Json;
using System.Linq.Expressions;
using static QlHui.App.Data.Constant.Enums;

namespace QlHui.App.Data.Constant
{
    internal static class Extensions
    {
        public static string GetError(this Exception ex)
        {
            return $"Lỗi hệ thống!{ex.Message} \n {ex.InnerException?.Message}";

        }
        public static bool IsNotEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }
        public static string GetMoneyFormat(this float input) => input.ToString("#,##0");
        public static string GetMoneyFormat(this float? input, bool laySoAm = true)
        {
            var val = input.GetValueOrDefault();
            if (laySoAm == false)
            {
                if (val < 0)
                {
                    return "0";
                }
                else
                {
                    return val.GetMoneyFormat();
                }
            }
            else
            {
                return val.GetMoneyFormat();
            }
        }
        public static float? GetFloatNumber(this string stringValue)
        {
            float? floatValue = null;
            if (stringValue.IsNotEmpty())
            {
                var tryParseValue = float.Parse(stringValue.Replace(",", ""));
                floatValue = tryParseValue;
            }
            return floatValue;
        }
        public static DateTime? TinhNgayBoHuiTiepTheo(this DateTime? ngayKhui, int loaiHui, int soNgayLoaiHui)
        {
            DateTime? ngayBoHui = null;
            if (ngayKhui.HasValue && soNgayLoaiHui > 0)
            {
                if (loaiHui == LoaiHui.NGAY.GetHashCode())
                {
                    ngayBoHui = ngayKhui.Value.Date.AddDays(1 * soNgayLoaiHui);
                }
                else if (loaiHui == LoaiHui.TUAN.GetHashCode())
                {
                    ngayBoHui = ngayKhui.Value.Date.AddDays(7 * soNgayLoaiHui);
                }
                else if (loaiHui == LoaiHui.THANG.GetHashCode())
                {
                    ngayBoHui = ngayKhui.Value.Date.AddMonths(1 * soNgayLoaiHui);
                }
            }
            return ngayBoHui;
        }
        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                          Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
              (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                           Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
              (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
