using System.ComponentModel.DataAnnotations;

namespace StudentsUnion.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите свой пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
