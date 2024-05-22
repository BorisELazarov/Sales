using System.ComponentModel.DataAnnotations;

namespace Sales.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
