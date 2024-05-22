using System.ComponentModel;

namespace Sales.ViewModels.Users
{
    public class FilterVM
    {
        [DisplayName("Username: ")]
        public string Username { get; set; }
        [DisplayName("First name: ")]
        public string FirstName { get; set; }
        [DisplayName("Last name: ")]
        public string LastName { get; set; }
    }
}
