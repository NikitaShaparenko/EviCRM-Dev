namespace EviCRM.Server.Models
{
    public class SortingModel
    {
        public string task_id { get; set; }
        public DateTime task_start { get; set; }
        public DateTime task_end { get; set; }
        public string task_body { get; set; }
        public string task_proj { get; set; }
    }
}
