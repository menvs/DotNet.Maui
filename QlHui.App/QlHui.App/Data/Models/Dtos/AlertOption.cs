using Radzen;

namespace QlHui.App.Data.Models.Dtos
{
    internal class AlertOption
    {
        public string Message { get; set; }
        public AlertStyle Style { get; set; }
        public bool IsShowAlert { get; set; }

        public async Task ShowAlertSuccess(string message, bool isAutoClose = true)
        {
            Message = message;
            Style = AlertStyle.Success;
            if (IsShowAlert == false)
            {
                IsShowAlert = true;
            }
            if (isAutoClose)
            {
                await CloseAsync();
            }
        }
        public async Task ShowAlertError(string message, bool isAutoClose = true)
        {
            Message = message;
            Style = AlertStyle.Danger;
            if (IsShowAlert == false)
            {
                IsShowAlert = true;
            }
            if (isAutoClose)
            {
                await CloseAsync();
            }
        }
        public async Task ShowAlertInfo(string message, bool isAutoClose = true)
        {
            Message = message;
            Style = AlertStyle.Info;
            if (IsShowAlert == false)
            {
                IsShowAlert = true;
            }
            if (isAutoClose)
            {
                await CloseAsync();
            }
        }
        public async Task CloseAsync()
        {
            await Task.Delay(2000);
            IsShowAlert = false;
            Message = string.Empty;
        }
    }
}
