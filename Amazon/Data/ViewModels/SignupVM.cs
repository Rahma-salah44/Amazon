using System.ComponentModel.DataAnnotations;
using static Amazon.Data.Enums.Enums;

namespace Amazon.Data.ViewModels
{
    public class SignupVM
    {
        [Display(Name ="Full Name")]
        [Required(ErrorMessage ="Full Name is Required")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "UserName  is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public UserType UserType { get; set; } =UserType.Client ;

        [Display(Name = "Confirm Password")]

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConfiremPassword { get; set; }

    }
}
