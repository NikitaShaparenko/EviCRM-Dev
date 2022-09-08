namespace EviCRM.Server.ViewModels
{
    public class AdminHealthModel
    {

        /*
         *  <th>№</th>
                         <th>Ссылка</th>
                         <th>HTTP код</th>
                         <th>Статус</th>
                         <th>Ответ на пинг</th>
                         <th>Действие</th>
         * 
         * 
         */
        public string ahm_num { get; set; }
        public string ahm_link { get; set; }
        public string ahm_http_code { get; set; }
        public string ahm_status { get; set; }
        public string ahm_ping_answer { get; set; }
        public string ahm_action { get; set; }

       public AdminHealthModel()
        {
            ahm_num = "";
            ahm_link = "";
            ahm_http_code = "";
            ahm_status = "";
            ahm_ping_answer = "";
            ahm_action = "";
        }


    }
}
