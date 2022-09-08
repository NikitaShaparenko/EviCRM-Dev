namespace EviCRM.Server.Models
{
    public class PersonalTasksStatusCollection
    {
        List<PersonalTasksStatus> lst = new List<PersonalTasksStatus>();

        string task_id {get;set;}
    }

    public class PersonalTasksStatus
    {
        string user_id { get; set; }
        string task_id { get; set; }
        string personal_status { get; set; }
    }

}
