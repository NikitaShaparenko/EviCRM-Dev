using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EviCRM.Server.Models
{
    public class CompanyDates
    {
        public int Sr { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
    }
}