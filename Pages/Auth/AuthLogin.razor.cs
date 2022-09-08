using EviCRM.Server.Core;
using Majorsoft.Blazor.Components.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace EviCRM.Server.Pages.Auth
{
    public partial class AuthLogin
    {
        string signin_login { get; set; }

        string signin_password { get; set; }

        string login_message { get; set; }

        string login_status { get; set; }
    }
}
