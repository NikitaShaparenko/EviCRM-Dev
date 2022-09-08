namespace EviCRM.Server.ViewModels
{

    public class ToastNotification
    {
        string BodyText = "";

        ToastsTypes? ToastType;

        public ToastNotification(string text, ToastsTypes type)
            {
            BodyText = text;
            ToastType = type;
            }

       public string get_BodyText()
        {
            return BodyText;
        }

       public ToastsTypes get_ToastType()
        {
            if (!ToastType.HasValue)
            {
                return ToastsTypes.Primary;
            }
            else
            {
                return ToastType.Value;
            }
        }

        public enum ToastsTypes
        {
            Primary,
            Secondary,
            Info,
            Success,
            Warning,
            Danger,
            Light,
            Dark,
        } 
    }

    public class Markdown_Todo
    {
        public string idmarkdown_todo_list { get; set; }

        public string markdown_todo_taskid { get; set; }

        public bool markdown_todo_checked { get; set; }

        public string markdown_todo_name { get; set; }

        public string markdown_todo_cardid { get; set; }
    }

    public class Markdown_Card
    {
        public string idmarkdown_cards { get; set; }

        public string markdown_card_name { get; set; }
        public string markdown_card_description { get; set; }
       
        public DateTime markdown_card_date_start { get; set; }
        public DateTime markdown_card_date_end { get; set; }

        public string markdown_card_backcolor { get; set; }
        public string markdown_card_img_path { get; set; }

        public string markdown_card_taskid { get; set; }

        public string markdown_card_deskid { get; set; }

        public int desk_num { get; set; }
    }

    public class Markdown_Desk
    {
        public string idmarkdown_desks { get; set; }

        public string markdown_desk_name { get; set; }

        public string markdown_desk_personal_id { get; set; }

        public string markdown_desk_task_id { get; set; }

        public int desk_num { get; set; }
    }

    public class Markdown_Desk_TwoIntBindings
    {
        public int parameter { get; set; }
        public int index { get; set; }
    }
}
