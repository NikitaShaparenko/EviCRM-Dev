namespace EviCRM.Server.Models
{
    public class CalendarGraphic
    {
        public string Code { get; set; }
        public string Color { get; set; }
        public string ForeColor { get; set; }

    }

    public class CalendarCallbackModel
    {
        public string calendar_id { get; set; }

        public BlazorCalendar.Models.Tasks tasks = new BlazorCalendar.Models.Tasks();
        public bool isNotify { get; set; }

        public bool isInOffice { get; set; }

        public List<string> lst_attachs { get; set; }

    }

}
