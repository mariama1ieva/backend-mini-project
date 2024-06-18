using System.ComponentModel.DataAnnotations;

namespace fruit_backend_project.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username or Email is wrong")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage = "Password is wrong")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
