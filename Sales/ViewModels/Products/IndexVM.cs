using Sales.Entities;
using Sales.ViewModels.Shared;

namespace Sales.ViewModels.Products
{
    public class IndexVM
    {
        public List<Product> Items { get; set; }
        public PagerVM Pager { get; set; }
        public FilterVM Filter { get; set; }
    }
}
