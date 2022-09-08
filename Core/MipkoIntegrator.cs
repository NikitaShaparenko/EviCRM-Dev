using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EviCRM.Server.Pages.Statistics.TimeAccounting.Statistics_WorkingHoursChart;

namespace EviCRM.Server.Core
{
    public class MipkoIntegrator
    {
        List<string> activity_start = new List<string>();
        List<string> activity_end = new List<string>();

        Decimal activity_total_seconds = 0;

        Decimal activity_total_minutes = 0;
        Decimal activity_total_hours = 0;

        List<string> application_start = new List<string>();
        List<string> application_name = new List<string>();
        List<string> application_end = new List<string>();

        List<string> daily_activity_date = new List<string>();
        List<string> daily_activity_start_time = new List<string>();
        List<string> daily_activity_end_time = new List<string>();

        List<string> clipboard_datetime = new List<string>();
        List<string> clipboard_application_path = new List<string>();
        List<string> clipboard_application_name = new List<string>();
        List<string> clipboard_application_desc = new List<string>();
        List<string> clipboard_application_data = new List<string>();

        List<string> fileoperation_datetime = new List<string>();
        List<string> fileoperation_application_path = new List<string>();
        List<string> fileoperation_application_name = new List<string>();
        List<string> fileoperation_application_desc = new List<string>();
        List<string> fileoperation_application_data = new List<string>();

        List<string> webpages_datetime = new List<string>();
        List<string> webpages_application_path = new List<string>();
        List<string> webpages_application_name = new List<string>();
        List<string> webpages_application_desc = new List<string>();
        List<string> webpages_application_data = new List<string>();

        List<string> SNS_datetime = new List<string>();
        List<string> SNS_application_path = new List<string>();
        List<string> SNS_application_name = new List<string>();
        List<string> SNS_application_desc = new List<string>();
        List<string> SNS_application_data = new List<string>();

        List<string> presskeys_datetime = new List<string>();
        List<string> presskeys_application_path = new List<string>();
        List<string> presskeys_application_name = new List<string>();
        List<string> presskeys_application_desc = new List<string>();
        List<string> presskeys_application_data = new List<string>();

        public List<string> AnalyzeMethodApplication(string filename)
        {
            List<string> activity_ = new List<string>();

            var path = filename; 
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();


                    if (fields[1] == "Активность программ")
                    {
                        if (fields[2] == @"C:\Windows\explorer.exe")
                        {
                            application_start.Add("Проводник");
                        }

                        if (fields[5] == "Завершение активности пользователя" || fields[5] == "Конец сессии" || fields[5] == "Заблокирован") activity_end.Add(fields[0]);
                        activity_.Add(fields[0] + " --- " + fields[5]);
                    }

                }
            }

            return activity_;

        }

        public decimal HowMuchHoursInWorkDay(int i)
        {
            return TimeDiffs(DateTime.Parse(daily_activity_start_time[i]), DateTime.Parse(daily_activity_end_time[i]));
        }

        public List<MiCommonModel> StatisticsFileOperation(string filename)
        {
            List<MiCommonModel> model_lst = new List<MiCommonModel>();
          

            var path = filename;
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    if (fields.Count() > 2)
                    {
                        if (fields[1] == "Операции с файлами")
                        {
                            fileoperation_datetime.Add(fields[0]);
                            fileoperation_application_path.Add(fields[2]);
                            fileoperation_application_name.Add(fields[3]);
                            fileoperation_application_desc.Add(fields[4]);
                            fileoperation_application_data.Add(fields[5]);
                        }
                    }

                }
            }

            for (int z = 0; z< fileoperation_datetime.Count;z++)
            {
                MiCommonModel model = new MiCommonModel();
                model.datetime = fileoperation_datetime[z];
               model.application_path =  fileoperation_application_path[z];
                model.application_name = fileoperation_application_name[z];
                model.appplication_desc = fileoperation_application_desc[z];
                model.appplication_data = fileoperation_application_data[z];
                model.category = StatisticsCat.files;
                model_lst.Add(model);
            }

            return model_lst;
        }

        public List<MiCommonModel> StatisticsPresskeys(string filename)
        {
            List<MiCommonModel> model_lst = new List<MiCommonModel>();
           

            var path = filename;
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    if (fields.Count() > 2)
                    {
                        if (fields[1] == "Нажатые клавиши")
                        {
                            presskeys_datetime.Add(fields[0]);
                            presskeys_application_path.Add(fields[2]);
                            presskeys_application_name.Add(fields[3]);
                            presskeys_application_desc.Add(fields[4]);
                            presskeys_application_data.Add(fields[5]);
                        }
                }

                }
            }

            for (int z = 0; z < presskeys_datetime.Count; z++)
            {
                MiCommonModel model = new MiCommonModel();
                model.datetime = presskeys_datetime[z];
                model.application_path = presskeys_application_path[z];
                model.application_name = presskeys_application_name[z];
                model.appplication_desc = presskeys_application_desc[z];
                model.appplication_data = presskeys_application_data[z];
                model.category = StatisticsCat.pressed_keys;
                model_lst.Add(model);
            }

            return model_lst;
        }

        public List<MiCommonModel> StatisticsWebPages(string filename)
        {
            List<MiCommonModel> model_lst = new List<MiCommonModel>();

            var path = filename; // Habeeb, "Dubai Media City, Dubai"
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

            if (fields.Count() > 2)
            { 
                if (fields[1] == "Посещенные веб-сайты")
                    {
                        webpages_datetime.Add(fields[0]);
                        webpages_application_path.Add(fields[2]);
                        webpages_application_name.Add(fields[3]);
                        webpages_application_desc.Add(fields[4]);
                        webpages_application_data.Add(fields[5]);
                    }

                }
        }
    }

            for (int z = 0; z < webpages_datetime.Count; z++)
            {
                MiCommonModel model = new MiCommonModel();
                model.datetime = webpages_datetime[z];
                model.application_path = webpages_application_path[z];
                model.application_name = webpages_application_name[z];
                model.appplication_desc = webpages_application_desc[z];
                model.appplication_data = webpages_application_data[z];
                model.category = StatisticsCat.web_pages;
                model_lst.Add(model);
            }

            return model_lst;
        }

        public List<MiCommonModel> StatisticsSNS(string filename)
        {
            List<MiCommonModel> model_lst = new List<MiCommonModel>();
            var path = filename; // Habeeb, "Dubai Media City, Dubai"
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

        while (!csvParser.EndOfData)
        {
            string[] fields = csvParser.ReadFields();

            if (fields.Count() > 2)
            {
                if (fields[1] == "Социальные сети")
                {
                    SNS_datetime.Add(fields[0]);
                    SNS_application_path.Add(fields[2]);
                    SNS_application_name.Add(fields[3]);
                    SNS_application_desc.Add(fields[4]);
                    SNS_application_data.Add(fields[5]);
                }

            }
        }
            }
            for (int z = 0; z < SNS_datetime.Count; z++)
            {
                MiCommonModel model = new MiCommonModel();
                model.datetime = SNS_datetime[z];
                model.application_path = SNS_application_path[z];
                model.application_name = SNS_application_name[z];
                model.appplication_desc = SNS_application_desc[z];
                model.appplication_data = SNS_application_data[z];
                model.category = StatisticsCat.SNS;
                model_lst.Add(model);
            }

            return model_lst;
        }

        public List<MiCommonModel> StatisticsClipboard(string filename)
        {
            List<MiCommonModel> model_lst = new List<MiCommonModel>();

            var path = filename; // Habeeb, "Dubai Media City, Dubai"
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

            if (fields.Count() > 2)
            {
                if (fields[1] == "Буфер обмена")
                {
                    clipboard_datetime.Add(fields[0]);
                    clipboard_application_path.Add(fields[2]);
                    clipboard_application_name.Add(fields[3]);
                    clipboard_application_desc.Add(fields[4]);
                    clipboard_application_data.Add(fields[5]);
                }
            }
                }
            }

            for (int z = 0; z < clipboard_datetime.Count; z++)
            {
                MiCommonModel model = new MiCommonModel();
                model.datetime = clipboard_datetime[z];
                model.application_path = clipboard_application_path[z];
                model.application_name = clipboard_application_name[z];
                model.appplication_desc = clipboard_application_desc[z];
                model.appplication_data = clipboard_application_data[z];
                model.category = StatisticsCat.clipboard;
                model_lst.Add(model);
            }

            return model_lst;
        }


        public decimal TimeDiffs(DateTime startTimestamp, DateTime stopTimestamp)
        {
            TimeSpan span = stopTimestamp - startTimestamp;
            var hours = span.TotalMilliseconds / 60 / 60 / 1000;
            var minutes = span.TotalMilliseconds / 60 / 1000;
            var seconds = span.TotalMilliseconds / 1000;

            decimal d_seconds = ((decimal)seconds);

            Console.WriteLine("Учитывается промежуток с " + startTimestamp.ToString() + " до " + stopTimestamp.ToString());

            return d_seconds;
        }

        public void AnalyzeMethodDailyHours()
        {
            //List<string> activity_ = new List<string>();

            daily_activity_start_time.Clear();

            string cur_dt = activity_start[0];
            DateTime cur_dt_date = DateTime.Parse(cur_dt);

            foreach (string act in activity_start)
            {
                DateTime dt = DateTime.Parse(act);

                if (dt.DayOfYear != cur_dt_date.DayOfYear)
                {
                    daily_activity_start_time.Add(dt.ToString());
                    cur_dt_date = dt;
                }
            }

            daily_activity_end_time.Clear();

            string end_cur_dt = activity_end[0];
            DateTime end_cur_dt_date = DateTime.Parse(end_cur_dt);

            //foreach (string act in activity_end)
            DateTime dt_cr = DateTime.Parse(activity_end[0]);
            DateTime dt_e = dt_cr;

            for (int i = 0; i < activity_end.Count; i++)
            {
                dt_cr = DateTime.Parse(activity_end[i]);
                if (dt_cr.DayOfYear != dt_e.DayOfYear && i > 0)
                {
                    daily_activity_end_time.Add(activity_end[i - 1]);
                    dt_e = dt_cr;
                }
                else
                {
                    // dt_e = dt_e;
                }
            }

        }

        public TerminalServer_AnalyzeMethodActivityModel AnalyzeMethodActivityByDate(string fileName, DateTime dt)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            TerminalServer_AnalyzeMethodActivityModel tsamam = new TerminalServer_AnalyzeMethodActivityModel();
            tsamam.activity_started = new List<DateTime>();
            tsamam.activity_ended = new List<DateTime>();
            tsamam.activity_body = new List<string>();



            var path = fileName;
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding("Windows-1251")))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                DateTime diff_dt1 = DateTime.Now;
                DateTime diff_dt2 = DateTime.Now;

                bool pair1 = false;
                bool pair2 = false;


                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    // Console.WriteLine("Количество столбцов: " + fields.Length);

                    if (DateTime.TryParse(fields[0], out DateTime dtn))
                    {
                        if (DateTime.Parse(fields[0]).Date == dt.Date && (DateTime.Parse(fields[0]).Hour>= 9 && DateTime.Parse(fields[0]).Hour <= 20))
                        {
                            if (fields[1] == "Активность пользователя")
                            {
                                //  if (fields[5] == "Начало активности пользователя" || fields[5] == "Активирован")
                                if (fields[5] == "Начало активности пользователя")
                                {

                                    //activity_start.Add(fields[0]);
                                    tsamam.activity_started.Add(DateTime.Parse(fields[0]));
                                    diff_dt1 = DateTime.Parse(fields[0]);
                                    pair1 = true;
                                }
                            }
                            if (fields.Length > 4)
                            { 
                            //if (fields[5] == "Завершение активности пользователя" || fields[5] == "Конец сессии" || fields[5] == "Заблокирован")
                            if (fields[5] == "Завершение активности пользователя")
                            {
                                if (pair1 == true)
                                {
                                    //activity_end.Add(fields[0]);
                                    tsamam.activity_ended.Add(DateTime.Parse(fields[0]));
                                    diff_dt2 = DateTime.Parse(fields[0]);
                                    pair2 = true;
                                    pair1 = false;

                                    activity_total_seconds += TimeDiffs(diff_dt1, diff_dt2);
                                    pair1 = false;
                                    pair2 = false;
                                    // activity_.Add(fields[0] + " --- " + fields[5]);

                                }

                            }
                            }
                            //if (pair1 && pair2)
                            //{

                            //}
                        }
                    }
                }
            }

            activity_total_minutes = activity_total_seconds / 60;
            activity_total_hours = activity_total_minutes / 60;

            decimal atm = Math.Round(activity_total_seconds / 60);
            decimal ath = Math.Round(activity_total_seconds / 60 / 60);

            decimal est_s = Math.Round(activity_total_seconds - (atm * 60) - (ath * 60 * 60));

            //Console.WriteLine("Всего отработал: " + str_h + " часов, " + str_m + " минут");

            if (tsamam.activity_started.Count > 0)
            {
                tsamam.tsamaam_start = tsamam.activity_started[0];
            }
            if (tsamam.activity_ended.Count > 0)
            {
                tsamam.tsamaam_end = tsamam.activity_ended[tsamam.activity_ended.Count-1];
                TimeSpan time = (tsamam.tsamaam_end - tsamam.tsamaam_start);
                tsamam.tsamaam_time = time;
            }
           
            return tsamam;
        }

        public List<string> AnalyzeMethodActivity(string fileName)
        {
            List<string> activity_ = new List<string>();

            var path = fileName; // Habeeb, "Dubai Media City, Dubai"
            using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

              
                csvParser.ReadLine();

                DateTime diff_dt1 = DateTime.Now;
                DateTime diff_dt2 = DateTime.Now;

                bool pair1 = false;
                bool pair2 = false;


                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    // Console.WriteLine("Количество столбцов: " + fields.Length);

                    if (fields[1] == "Активность пользователя")
                    {
                        //  if (fields[5] == "Начало активности пользователя" || fields[5] == "Активирован")
                        if (fields[5] == "Начало активности пользователя")
                        {

                            activity_start.Add(fields[0]);
                            diff_dt1 = DateTime.Parse(fields[0]);
                            pair1 = true;
                        }
                    }
                    //if (fields[5] == "Завершение активности пользователя" || fields[5] == "Конец сессии" || fields[5] == "Заблокирован")
                    if (fields[5] == "Завершение активности пользователя")
                    {
                        if (pair1 == true)
                        {
                            activity_end.Add(fields[0]);
                            diff_dt2 = DateTime.Parse(fields[0]);
                            pair2 = true;
                            pair1 = false;

                            activity_total_seconds += TimeDiffs(diff_dt1, diff_dt2);
                            pair1 = false;
                            pair2 = false;
                            activity_.Add(fields[0] + " --- " + fields[5]);

                        }

                    }
                    //if (pair1 && pair2)
                    //{

                    //}
                }
            }

            activity_total_minutes = activity_total_seconds / 60;
            activity_total_hours = activity_total_minutes / 60;

            decimal atm = Math.Round(activity_total_seconds / 60);
            decimal ath = Math.Round(activity_total_seconds / 60 / 60);

            decimal est_s = Math.Round(activity_total_seconds - (atm * 60) - (ath * 60 * 60));

            TimeSpan time = TimeSpan.FromSeconds((double)activity_total_seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str_h = time.ToString(@"hh");
            string str_m = time.ToString(@"mm");
            string str_s = time.ToString(@"ss\:fff");

            Console.WriteLine("Всего отработал: " + str_h + " часов, " + str_m + " минут");

            return activity_;
        }

    }

    public class TerminalServer_AnalyzeMethodActivityModel
        {
       public TimeSpan tsamaam_time { get; set; }
       public DateTime tsamaam_start { get; set; }
       public DateTime tsamaam_end { get; set; }

       public List<DateTime> activity_started { get; set; }
       public List<string> activity_body { get; set; }
      public  List<DateTime> activity_ended { get; set; }

        }
    }
