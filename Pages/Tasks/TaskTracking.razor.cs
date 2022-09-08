using EviCRM.Server.Core;
using EviCRM.Server.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;
using System.Web.Mvc;

namespace EviCRM.Server.Pages.Tasks
{
	public class TaskTracking : ComponentBase
	{
        MySQL_Controller mysqlc = new MySQL_Controller();
        BackendController bc = new BackendController();

    }
}
