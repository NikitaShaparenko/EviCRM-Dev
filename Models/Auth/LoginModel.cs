using System.ComponentModel.DataAnnotations;

namespace EviCRM.Server.Models.Auth
{
    public class LoginModel
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public bool RememberPassword { get; set; }

        public int? RoleID { get; set; }

        public Role Role { get; set; }
    };

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role()
        {
            //Users = new List<User>();
        }
    }
}