using Microsoft.AspNetCore.Http;

namespace EviCRM.Server.ViewModels
{
    public class ProjectCreateModel
    {
        public int proj_id { get; set; }

        public string proj_desc { get; set; }
        
        public string proj_name { get; set; }  

        public string[] proj_included_tasks { get; set; }

        public DateTime proj_start { get; set; }
        public DateTime proj_end { get; set; }
        public string[] proj_members { get; set; }

        public string proj_short_desc { get; set; }

        public IFormFile[] proj_attachments { get; set; }

        public IFormFile proj_avatar_path { get; set; }

    }
}
