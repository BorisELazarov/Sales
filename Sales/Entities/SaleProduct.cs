using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Entities
{
    public class SaleProduct:BaseEntity
    {
        public int SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product  { get; set; }
        public int Quantity { get; set; }
    }
}
