using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sales.ViewModels.Products
{
    public class CreateVM
    {
        [DisplayName("Name: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string Name { get; set; }
        [DisplayName("Description: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string Description { get; set; }

        [DisplayName("Quantity: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public int Quantity { get; set; }

        [DisplayName("Price per unit: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public decimal PricePerUnit { get; set; }
    }
}
