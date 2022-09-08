using System.ComponentModel.DataAnnotations;

namespace EviCRM.Server.ViewModels
{
    public class RegisterModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указана электронная почта")]
        public string Email { get; set; }
        public bool RememberPassword { get; set; }
    }
}
