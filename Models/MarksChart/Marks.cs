namespace EviCRM.Server.Models.MarksChart
{
    public class Marks
    {
        public string userID { get; set; }

        public int idmarks { get; set; }

        public int firstMark { get; set; }
 
        public int secondMark { get; set; }

        public string firstComment { get; set; }

        public string secondComment { get; set; }

        public DateTime date { get; set; }

        public bool isTwoMarks { get; set; }

        public List<string> firstAttachments { get; set; }

        public List<string> secondAttachments { get; set; }
    }
}
