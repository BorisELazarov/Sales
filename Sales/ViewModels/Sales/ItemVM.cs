using System.ComponentModel;

namespace Sales.ViewModels.Sales
{
    public class ItemVM
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
