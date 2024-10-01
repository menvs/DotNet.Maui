namespace QlHui.App.Data.Models.Dtos
{
    internal class PanigationDto
    {
        public int Skip { get; set; }
        public int Take { get; set; }

        public PanigationDto()
        {
            Skip = 0;
            Take = 10;
        }
        public PanigationDto(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}
