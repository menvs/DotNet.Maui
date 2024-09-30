using Microsoft.Extensions.Configuration;

namespace QlHui.App.Data.Models.Config
{
    public class OwnerConfigModel
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;  
        public string BankName { get; set; } = string.Empty;
   
        public OwnerConfigModel() {
            Name = "Lê Thị Hà";
            PhoneNumber = "0935 368 050";
            BankAccountNumber = "196904363979";
            BankName = "Techcombank";
        }
    }
}
