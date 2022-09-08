using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.ComponentModel;
using EviCRM.Server.Models;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace EviCRM.Server.Calendar
{

    public class Calendar : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

}
