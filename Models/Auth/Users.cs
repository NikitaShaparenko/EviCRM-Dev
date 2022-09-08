namespace EviCRM.Server.Models.Auth
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public bool RememberPassword { get; set; }
    }
}
