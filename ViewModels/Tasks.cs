using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace EviCRM.Server.ViewModels
{
    public class TasksCreateModel
    {

        //public int ID { get; set; }
        public string task_name { get; set; }
        public string task_description { get; set; }
        public string task_start_date { get; set; }
        public string task_end_date { get; set; }
        public List<IFormFile> task_attachments { get; set; }
        public string task_budget { get; set; }
        public string[] task_members { get; set; }

        public string[] task_proj { get; set; }
        public string task_author { get; set; }


    }

    public class TasksListModel
        {
        public string task_id { get; set; }
        public string user_id { get; set; }
        }

}
