using Sales.Entities;
using Sales.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Sales.ViewModels.Products;

namespace Sales.ViewModels.Sales
{
    public class CreateVM
    {
        [DefaultValue(0)]
        public decimal TotalPrice { get; set; }
        public List<AddToCartVM> SaleProducts { get; set; }
    }
}
