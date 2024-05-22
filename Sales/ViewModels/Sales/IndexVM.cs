using Sales.Entities;
using Sales.ViewModels.Shared;

namespace Sales.ViewModels.Sales
{
    public class IndexVM
    {
        public List<Sale> Items { get; set; }
        public PagerVM Pager { get; set; }
        public FilterVM Filter { get; set; }

    }
}
