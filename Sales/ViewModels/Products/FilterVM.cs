using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModels.Products
{
    public class FilterVM
    {
        [DisplayName("Name: ")]
        public string Name { get; set; }
        [DisplayName("Quantity: ")]
        public int Quantity { get; set; }
        [DisplayName("Price per unit: ")]
        public decimal PricePerUnit { get; set; }
    }
}
