namespace EviCRM.Server.Models.Email
{
    public class EmailIncomingModel
    {
        public string email_date { get; set; }
        public string email_subject { get; set; }
        public string email_from { get; set; }

        public string email_body { get; set; }

        public string email_sender { get; set; }
        public string[] email_to { get; set; }

        public string email_cc { get; set; }
        public string email_bcc { get; set; }

        public string email_replyto {get;set;}


    }
}
