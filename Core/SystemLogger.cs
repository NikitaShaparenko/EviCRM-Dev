namespace EviCRM.Server.Core
{
    public class SystemLogger
    {
        string userID { get; set; }
        string webroot { get; set; }
        public SystemLogger(string _webroot)
        {
            webroot = _webroot;
        }

       
    }
}
