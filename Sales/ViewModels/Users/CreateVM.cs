using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sales.ViewModels.Users
{
    public class CreateVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string Password { get; set; }

        [DisplayName("First name: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string FirstName { get; set; }

        [DisplayName("Last name: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public string LastName { get; set; }
    }
}
