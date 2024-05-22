using Microsoft.AspNetCore.Mvc.Rendering;
using Sales.Entities;
using Sales.Enums;
using System.ComponentModel;

namespace Sales.ViewModels.Sales
{
    public class FilterVM
    {
        [DisplayName("Status: ")]
        public Status Status { get; set; }
        [DisplayName("Date: ")]
        public DateTime Date { get; set; }
        [DisplayName("User: ")]
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

    }
}
