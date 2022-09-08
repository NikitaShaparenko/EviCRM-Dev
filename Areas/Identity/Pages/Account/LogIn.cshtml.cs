using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EviCRM.Server.Models.Auth;

namespace EviCRM.Server.Areas.Identity.Pages.Account
{
    public class Login : PageModel
    {
        [HttpPost]
        public void OnPost()
        {

        }

        [HttpPost]
        public void DoLogin(LoginPageModel lpm)
        {
         
        }
    }
}
