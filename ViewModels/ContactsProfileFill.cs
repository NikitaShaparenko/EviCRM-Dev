using Microsoft.AspNetCore.Http;

namespace EviCRM.Server.ViewModels
{
    public class ContactsProfileFillModel
    {
        public string fud_lastname { get; set; }
        public string fud_division { get; set; }
        public string fud_phone { get; set; }
        public string fud_position { get; set; }
        public string fud_city { get; set; }
        public string fud_bio { get; set; }
        public string fud_telegram_token { get; set; }

        public string fud_author { get; set; }
        public IFormFile fud_avatar { get; set; }

        public string[] fud_skills { get; set; }

    }
}
