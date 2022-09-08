using CsvHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace EviCRM.Server.Core
{
    public class SystemCore
    {
        protected readonly string base_url = "https://localhost:7083";

        Sentinel sentinel = new Sentinel();

        public class SysLogStructure
        {
            public string dt_str { get; set; }
            public string page { get; set; }
            public string status { get; set; }
            public string body { get; set; }
            public string user { get; set; }
        }

       
        public enum LogLevels
        {
            Fatal,
            Error,
            Warning,
            Info,
            Debug,
        }

        public void Log(string control, string message, LogLevels log_level)
        {
            string log_body = "";

            switch(log_level)
            {
                case LogLevels.Fatal:
                    log_body += "< FATAL > : ";
                break;

                case LogLevels.Error:
                    log_body += "< ERROR > : ";
                    break;

                case LogLevels.Warning:
                    log_body += "< WARNING > : ";
                    break;

                case LogLevels.Info:
                    log_body += "< Info > : ";
                    break;

                case LogLevels.Debug:
                    log_body += "< Debug > : ";
                    break;
            }

            log_body += DateTime.Today.ToShortDateString();
            log_body += " " + DateTime.Now.ToShortTimeString();
            log_body += " | [ " + control + " ] : ( " + message + " )";

            Console.WriteLine(log_body);
            Debug.WriteLine(log_body);
        }

        public void Log(string instruction, string control, string message, LogLevels log_level)
        {
            string log_body = "";

            switch (log_level)
            {
                case LogLevels.Fatal:
                    log_body += "< FATAL > : ";
                    break;

                case LogLevels.Error:
                    log_body += "< ERROR > : ";
                    break;

                case LogLevels.Warning:
                    log_body += "< WARNING > : ";
                    break;

                case LogLevels.Info:
                    log_body += "< Info > : ";
                    break;

                case LogLevels.Debug:
                    log_body += "< Debug > : ";
                    break;
            }

            log_body += DateTime.Today.ToShortDateString();
            log_body += " " + DateTime.Now.ToShortTimeString();
            log_body += " | [ " + instruction + " ] : [ " + control + " ] : ( " + message + " )";

            Console.WriteLine(log_body);
            Debug.WriteLine(log_body);
        }

        public void BrowserConsoleLog(string message)
        {
                //JsRuntime.InvokeAsync<string>("console.log", message);
        }

        public string syslog_raw_read_by_date(IWebHostEnvironment iwebhost, DateTime dt, string user,bool supress_editing)
        {
            string str = "";

            try
            {
                if (System.IO.Directory.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user)))
                {
                    if (System.IO.File.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"))) //Ищем док за сегодня
                    {
                        try
                        {
                            return System.IO.File.ReadAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"));

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Произошла ошибка при открытии файла лога!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                            return str;
                        }

                    }
                    else
                    {
                        //Начало лога сегодня
                        //System.IO.File.Create(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"));
                        System.IO.File.WriteAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), "dt_str,page,status,body,user" + Environment.NewLine);
                        return "newfile";
                    }
                }
                else
                {
                    //Нет даже папки пользователя

                    try
                    {
                        Directory.CreateDirectory(Path.Combine(iwebhost.WebRootPath, "logs", user)); //Создаём папку пользователя
                        return "newdir";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при создании папки пользователя!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                        return str;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при попытке чтения логов!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                return str;
            }
        }

        public string syslog_raw_read_by_date(IWebHostEnvironment iwebhost, DateTime dt, string user)
        {
            string str = "";

            try
            {
                if (System.IO.Directory.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user)))
                {
                    if (System.IO.File.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"))) //Ищем док за сегодня
                    {
                        try
                        {
                            return System.IO.File.ReadAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"));

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Произошла ошибка при открытии файла лога!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                            return str;
                        }

                    }
                    else
                    {
                        //Начало лога сегодня
                        //System.IO.File.Create(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"));
                        System.IO.File.WriteAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), "dt_str,page,status,body,user" + Environment.NewLine);

                        return "newfile";
                    }
                }
                else
                {
                    //Нет даже папки пользователя

                    try
                    {
                        Directory.CreateDirectory(Path.Combine(iwebhost.WebRootPath, "logs", user)); //Создаём папку пользователя
                        return "newdir";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка при создании папки пользователя!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                        return str;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при попытке чтения логов!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                return str;
            }
        }

        bool syslog_raw_confirm_read(IWebHostEnvironment iwebhost, DateTime dt, string user)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user)))
                {
                    if (System.IO.File.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"))) //Ищем док за сегодня
                    {
                        try
                        {
                            return true;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Произошла ошибка при открытии файла лога!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                            return false;
                        }

                    }
                    else
                    {
                        //Начало лога сегодня
                        return false;
                    }
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при попытке чтения логов!" + Environment.NewLine + "Для " + sentinel.TripleDesEncrypt(user) + " за " + dt.ToShortDateString() + Environment.NewLine + ex.Message);
                return false;
            }
        }

        bool syslog_write_by_date(IWebHostEnvironment iwebhost, DateTime dt, string user, string new_data)
        {
            string str = "";

            if (System.IO.Directory.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user)))
            {
                if (System.IO.File.Exists(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log")))
                {
                    try
                    {
                        ///Лог найден
                        System.IO.File.AppendAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), new_data);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при записи отчёта в файл)");
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        //Лог не найден, создаём новый
                        System.IO.File.WriteAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), "dt_str,page,status,body,user" + Environment.NewLine);
                        System.IO.File.WriteAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), new_data);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при записи отчёта в файл для пользователя " + sentinel.TripleDesEncrypt(user));
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }

            }
            else
            {
                try
                {
                    //Не существует даже папки
                    System.IO.Directory.CreateDirectory(Path.Combine(iwebhost.WebRootPath, "logs", user));
                    System.IO.File.WriteAllText(Path.Combine(iwebhost.WebRootPath, "logs", user, "log_" + dt.ToShortDateString() + ".log"), "dt_str,page,status,body,user" + Environment.NewLine);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при создании папки под пользовательские логи для пользователя " + sentinel.TripleDesEncrypt(user));
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public void Syslog(IWebHostEnvironment iwebhost,string _user, string _control, string _message, LogLevels log_level)
        {
            SysLogStructure ls = new SysLogStructure();

            ls.dt_str = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();
           
            switch(log_level)
            {
                case LogLevels.Fatal:
                    ls.status = "fatal";
                    break;
                case LogLevels.Debug:
                    ls.status = "debug";
                    break;
                case LogLevels.Warning:
                    ls.status = "warning";
                    break;
                case LogLevels.Error:
                    ls.status = "error";
                    break;
                case LogLevels.Info:
                    ls.status = "info";
                    break;

                default:
                    ls.status = "fatal";
                    break;
            }

            ls.page = _control;
            ls.body = _message;
            ls.user = _user;

            string syslog_data = get_syslog_append_string(ls);
            bool status_writer = syslog_write_by_date(iwebhost,DateTime.Today, _user, syslog_data);

            if (status_writer)
            {
                Log("System Backend Core", "Выгрузка пользовательского лога для " + _user + " успешна!", LogLevels.Debug);
            }
            else
            {
                Log("System Backend Core", "Выгрузка пользовательского для " + _user + " провалилась!", LogLevels.Fatal);
            }
        }

        public void Syslog(IWebHostEnvironment iwebhost, string _user, string _control, string _message, LogLevels log_level,DateTime custom_date)
        {
            SysLogStructure ls = new SysLogStructure();

            ls.dt_str = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();

            switch (log_level)
            {
                case LogLevels.Fatal:
                    ls.status = "fatal";
                    break;
                case LogLevels.Debug:
                    ls.status = "debug";
                    break;
                case LogLevels.Warning:
                    ls.status = "warning";
                    break;
                case LogLevels.Error:
                    ls.status = "error";
                    break;
                case LogLevels.Info:
                    ls.status = "info";
                    break;

                default:
                    ls.status = "fatal";
                    break;
            }

            ls.page = _control;
            ls.body = _message;
            ls.user = _user;

            string syslog_data = get_syslog_append_string(ls);
            bool status_writer = syslog_write_by_date(iwebhost, custom_date, _user, syslog_data);

            if (status_writer)
            {
                Log("System Backend Core", "Выгрузка пользовательского лога для " + sentinel.TripleDesDecrypt(_user) + " успешна!", LogLevels.Debug);
            }
            else
            {
                Log("System Backend Core", "Выгрузка пользовательского для " + sentinel.TripleDesDecrypt(_user) + " провалилась!", LogLevels.Fatal);
            }
        }

        public IEnumerable<SysLogStructure> Syslog_GetLog_FromFile(string filepath)
        {
            IEnumerable<SysLogStructure> structure;
            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                structure = csv.GetRecords<SysLogStructure>();
            }
            return structure;
        }

        public List<SysLogStructure> Syslog_GetLog_FromFile(IWebHostEnvironment iwebhost, string userid,DateTime day)
        {
            IEnumerable<SysLogStructure> structure = null;
            List<SysLogStructure> structure_lst = new List<SysLogStructure>();

            if (syslog_raw_confirm_read(iwebhost,day, userid))
            {
                using (var reader = new StreamReader(Path.Combine(iwebhost.WebRootPath, "logs", userid, "log_" + day.ToShortDateString() + ".log")))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    structure = csv.GetRecords<SysLogStructure>();
                    foreach(var elem in structure)
                    {
                        structure_lst.Add(elem);
                    }
                }
               
            }

            if (structure_lst != null)
            {
                return structure_lst;
            }
            else
            {
                return null;
            }


        }

        public string get_syslog_append_string(SysLogStructure syslog_body)
        {
            var csv = new StringBuilder();
            csv.AppendLine(string.Join(",", syslog_body.dt_str, syslog_body.page, syslog_body.status, syslog_body.body, syslog_body.user));
            return csv.ToString();
        }

        public class Email_Message
        {
            public string email_subject { get; set; }

            public string email_body { get; set; }

            public List<string> email_whom { get; set; }

            public List<string> email_cc { get; set; }

            public List<string> email_bcc { get; set; }

            public MailPriority email_priority = MailPriority.High;

            public List<string> email_attachs { get; set; }

            public string smtp_login { get; set; }

            public string smtp_password { get; set; }

            public string smtp_host { get; set; }
            
            public int smtp_port { get; set; }

 

        }

        public async Task SendEmailviaSMTP(Email_Message msg, IWebHostEnvironment iwebhost)
        {
            var message = new MailMessage();

            if (msg.email_whom != null)
            {
                foreach (var elem in msg.email_whom)
                {
                    message.To.Add(elem);
                }
            }

            if (msg.email_cc != null)
            {
                foreach (var elem in msg.email_cc)
                {
                    message.CC.Add(elem);
                }
            }

            if (msg.email_bcc != null)
            {
                foreach (var elem in msg.email_bcc)
                {
                    message.Bcc.Add(elem);
                }
            }

            if (msg.email_subject != "" && msg.email_subject != null)
            {
                message.Subject = msg.email_subject;
            }
            else
            {
                message.Subject = "( Без темы )";
            }

            if (msg.email_body != "" && msg.email_body != null)
            {
                message.Body = msg.email_body;
            }
            else
            {
                message.Body = "( Письмо без текста )";
            }

            if (msg.email_attachs != null)
            {
                foreach (var elem in msg.email_attachs)
                {
                    if (File.Exists(Path.Combine(iwebhost.WebRootPath, "uploads", "email", elem)))
                    {
                        string file = Path.Combine(iwebhost.WebRootPath, "uploads", "email", elem);

                        Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(file);
                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                        disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                        message.Attachments.Add(data);
                    }
                }
            }

            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(msg.smtp_host,msg.smtp_port))
            {
                smtpClient.Credentials = new NetworkCredential(msg.smtp_login,msg.smtp_password);
                await smtpClient.SendMailAsync(message);
            }
        }

    }
}
