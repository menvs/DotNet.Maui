namespace QlHui.App.Data.Models.Dtos
{
    internal class SearchResult<T>
    {
        public int TotalItem { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
