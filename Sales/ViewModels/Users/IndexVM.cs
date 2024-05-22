using Sales.Entities;
using Sales.ViewModels.Shared;

namespace Sales.ViewModels.Users
{
    public class IndexVM
    {
        public List<User> Items { get; set; }
        public PagerVM Pager { get; set; }
        public FilterVM Filter { get; set; }
    }
}
