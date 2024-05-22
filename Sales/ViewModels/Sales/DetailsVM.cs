using Sales.Enums;
using System.ComponentModel;

namespace Sales.ViewModels.Sales
{
    public class DetailsVM
    {
        public int Id { get; set; }
        [DisplayName("Status: ")]
        public Status Status { get; set; }
        [DisplayName("Date: ")]
        public DateTime Date { get; set; }
        public List<ItemVM> SaleProducts { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
