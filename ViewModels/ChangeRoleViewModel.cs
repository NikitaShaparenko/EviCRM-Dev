using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EviCRM.Server.ViewModels
{
    public class ChangeRoleViewModel
    {
        public int UserID { get; set; }
        public string UserLogin { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
