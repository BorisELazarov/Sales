using Sales.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Entities
{
    public class Sale:BaseEntity
    {
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
