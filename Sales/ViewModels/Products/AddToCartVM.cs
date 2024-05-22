using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sales.ViewModels.Products
{
    public class AddToCartVM
    {
        public int Id { get; set; }
        [DisplayName("Name: ")]
        public string Name { get; set; }
        [DisplayName("Description: ")]
        public string Description { get; set; }

        [DisplayName("Quantity: ")]
        public int Quantity { get; set; }

        [DisplayName("Price per unit: ")]
        public decimal PricePerUnit { get; set; }
        [DisplayName("Units wanted: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public int Units { get; set; }
    }
}
