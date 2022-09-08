using CsvHelper;
using System.Globalization;

namespace EviCRM.Server.Pages.Admin
{
    public partial class AdminLoggin
    {
        EviCRM.Server.Core.SystemCore systemcore = new EviCRM.Server.Core.SystemCore();
        EviCRM.Server.Core.Sentinel sentinel = new EviCRM.Server.Core.Sentinel();

        public string getUsernamebyLogin(string login)
        {
            for (int i = 0; i < users_dt.Count; i++)
            {
                if (users_dt[i] == login)
                {
                    return fullname_dt[i];
                }
            }
            return "";
        }
       


        void OnChangeDate(string datetime_str)
        {
            DateTime dt = DateTime.Parse(datetime_str);
            dt_global = dt;
            journal_dt.Clear();
            StateHasChanged();

            if (dt > DateTime.Now)
            {
                Core.SystemCore.SysLogStructure ls = new Core.SystemCore.SysLogStructure
                {
                    dt_str = DateTime.Now.ToString(),
                    page = "Админ панель - Журналы",
                    status = "debug",
                    body = "„Если путешествия во времени возможны, то где же туристы из будущего?“ — Стивен Уильям Хокинг.",
                    user = selectedUser,
                };
                journal_dt.Add(ls);
                StateHasChanged();
                return;
            }

                string str = systemcore.syslog_raw_read_by_date(iwebhost,dt, selectedUser);

                switch (str)
                {
                    case "":
                        Core.SystemCore.SysLogStructure ls = new Core.SystemCore.SysLogStructure
                        {
                            dt_str = DateTime.Now.ToString(),
                            page = "Админ меню - Журнал",
                            status = "critical",
                            body = "Произошла ошибка при открытии файла",
                            user = selectedUser,
                        };
                        journal_dt.Add(ls);
                        StateHasChanged();
                        break;

                case "\r\n":
                    Core.SystemCore.SysLogStructure ls0 = new Core.SystemCore.SysLogStructure
                    {
                        dt_str = DateTime.Now.ToString(),
                        page = "Админ меню - Журнал",
                        status = "debug",
                        body = "Только что создался файл с логом, пользователь не проявлял активности ранее",
                        user = selectedUser,
                    };
                    journal_dt.Add(ls0);
                    StateHasChanged();
                    break;


                case "newfile":
                        Core.SystemCore.SysLogStructure ls1 = new Core.SystemCore.SysLogStructure
                        {
                            dt_str = DateTime.Now.ToString(),
                            page = "Админ меню - Журнал",
                            status = "debug",
                            body = "Только что создался файл с логом, пользователь не проявлял активности ранее",
                            user = selectedUser,
                        };
                        journal_dt.Add(ls1);
                        StateHasChanged();
                        break;

                    case "newdir":
                        Core.SystemCore.SysLogStructure ls2 = new Core.SystemCore.SysLogStructure
                        {
                            dt_str = DateTime.Now.ToString(),
                            page = "Админ меню - Журнал",
                            status = "debug",
                            body = "Только что создалась папка для логов пользователя, видимо он никогда не заходил в систему",
                            user = selectedUser,
                        };
                        journal_dt.Add(ls2);
                        StateHasChanged();
                        break;

                case ("dt_str,page,status,body,user\r\n"):
                    Core.SystemCore.SysLogStructure ls3 = new Core.SystemCore.SysLogStructure
                    {
                        dt_str = DateTime.Now.ToString(),
                        page = "Админ меню - Журнал",
                        status = "debug",
                        body = "Только что создался файл с логом, пользователь не проявлял активности ранее",
                        user = selectedUser,
                    };
                    journal_dt.Add(ls3);
                    StateHasChanged();
                    break;

                    default:

                    List<Core.SystemCore.SysLogStructure> events = systemcore.Syslog_GetLog_FromFile(iwebhost, selectedUser, dt);

                    if (events != null)
                    {
                        foreach (var elem in events)
                        {
                            journal_dt.Add(elem);
                        }
                    }
                    
                        break;
                }

            StateHasChanged();
        }
    }
}
