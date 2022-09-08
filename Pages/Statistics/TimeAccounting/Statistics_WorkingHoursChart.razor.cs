using Blazorise.Charts;
using EviCRM.Server.Core;
using EviCRM.Server.Models;
using EviCRM.Server.Pages.Charts;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace EviCRM.Server.Pages.Statistics.TimeAccounting
{
    public partial class Statistics_WorkingHoursChart
    {

        List<StatisticsCat> CatToView = new List<StatisticsCat>();

       public void SetViewCat(StatisticsCat sc, ChangeEventArgs cea)
        {
            if (bool.Parse(cea.Value.ToString()))
            {
                //checked
                CatToView.Add(sc);
                StateHasChanged();
            }
            else
            {
                if (CatToView.Contains(sc))
                {
                    int ind = CatToView.IndexOf(sc);
                    CatToView.RemoveAt(ind);
         
                }
                StateHasChanged();
                //unchecked
            }
        }

        public enum StatisticsCat
        {
            pressed_keys,
            web_pages,
            SNS,
            clipboard,
            files,
        }
        bool personal { get; set; }

        List<MiCommonModel> mi_StatisticsPresskeys = new List<MiCommonModel>();
        List<MiCommonModel> mi_StatisticsWebPages = new List<MiCommonModel>();
        List<MiCommonModel> mi_StatisticsSNS = new List<MiCommonModel>();
        List<MiCommonModel> mi_StatisticsClipboard = new List<MiCommonModel>();
        List<MiCommonModel> mi_StatisticsFileOperation = new List<MiCommonModel>();
        List<MiCommonModel> mi_common = new List<MiCommonModel>();
        List<List<MiCommonModel>> mi_commonlst = new List<List<MiCommonModel>>();

        public List<MiCommonModel> getStatCommonModel(List<MiCommonModel> keys, List<MiCommonModel> web, List<MiCommonModel> sns, List<MiCommonModel> clipboard, List<MiCommonModel> files)
        {
            List<MiCommonModel> mcm = new List<MiCommonModel>();

            foreach(var elem in keys)
            {
                mcm.Add(elem);
            }
            foreach (var elem in web)
            {
                mcm.Add(elem);
            }
            foreach (var elem in sns)
            {
                mcm.Add(elem);
            }
            foreach (var elem in clipboard)
            {
                mcm.Add(elem);
            }
            foreach (var elem in files)
            {
                mcm.Add(elem);
            }

            mcm = mcm.OrderBy(x => DateTime.Parse(x.datetime)).ToList();

            return mcm;
        }

        public void getStatisticsPressedKeys(string filename)
        {
            mi_StatisticsPresskeys = mi.StatisticsPresskeys(filename);
            mi_StatisticsWebPages = mi.StatisticsWebPages(filename);
            mi_StatisticsSNS = mi.StatisticsSNS(filename);
            mi_StatisticsClipboard = mi.StatisticsClipboard(filename);
            mi_StatisticsFileOperation = mi.StatisticsFileOperation(filename);
        }

        public void ShowGraphActivityFull()
        {
            graphData2 = data_series_collection;
            modalGraphOpened = true;
            StateHasChanged();
        }

        public string genPathToThinClientStatsByUserID(int user_array_id, DateTime dt_n)
        {
            string str = "";

            if (Directory.Exists(Path.Combine(_env.WebRootPath, "thinclient")))
            {

            }
            else
            {
                Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "thinclient"));
            }

            string thinc = thinclient_dt[user_array_id];

            bool isFirstServer = true;

            if (thinc.Contains("rendev_"))
            {
                isFirstServer = true;
                thinc = thinc.Replace("rendev_", "");
            }
            else
            {
                isFirstServer = false;
                thinc = thinc.Replace("renovationdev_", "");
            }

            string path_base = Path.Combine(_env.WebRootPath, "thinclient");
            string full_path = path_base;

            if (thinc != "")
            {

                if (isFirstServer)
                {
                    string day_str = dt_n.Day.ToString();
                    string month_str = dt_n.Month.ToString();
                    string year_str = dt_n.Year.ToString();

                    if (day_str.Length < 2)
                    {
                        day_str = "0" + dt_n.Day;
                    }

                    if (month_str.Length < 2)
                    {
                        month_str = "0" + dt_n.Month;
                    }

                    if (year_str.Length < 2)
                    {
                        year_str = "0" + dt_n.Year;
                    }



                    full_path = Path.Combine(_env.WebRootPath, "thinclient", "RENDEV", thinc, (day_str + "-" + month_str + "-" + year_str) + ".csv");

                }
                else
                {

                    string day_str = dt_n.Day.ToString();
                    string month_str = dt_n.Month.ToString();
                    string year_str = dt_n.Year.ToString();

                    if (day_str.Length < 2)
                    {
                        day_str = "0" + dt_n.Day;
                    }

                    if (month_str.Length < 2)
                    {
                        month_str = "0" + dt_n.Month;
                    }

                    if (year_str.Length < 2)
                    {
                        year_str = "0" + dt_n.Year;
                    }

                    full_path = Path.Combine(_env.WebRootPath, "thinclient", "RENOVATIONDEV", thinc, (day_str + "-" + month_str + "-" + year_str) + ".csv");
                }

                if (File.Exists(full_path))
                {
                    str = full_path;
                }
            }
            return str;
        }

        LineChartOptions ChartOptionsGenerator()
        {
            LineChartOptions lineChartOptions = new()
            {
                Locale = "ru",
                Plugins = new ChartPlugins
                {
                    Legend = new ChartLegend
                    {
                        Display = false,
                    }
                },


                Parsing = new ChartParsing
                {
                    XAxisKey = "sector",
                    YAxisKey = "count",
                },



                Scales = new ChartScales
                {

                    X = new ChartAxis
                    {

                        Type = "time",
                        Time = new ChartAxisTime
                        {
                            DisplayFormat = new ChartAxisTimeDisplayFormat
                            {
                                Second = "HH:mm:ss",
                                Minute = "HH:mm",
                                Hour = "HH",
                            },
                        },

                    },
                    Y = new ChartAxis
                    {
                        Min = 0,
                        Max = users_max_series + 1,
                        //Ticks = new ChartAxisTicks
                        //{
                        //    StepSize = 0.5,
                        //}
                    },

                },
            };

            return lineChartOptions;
        }

        public static double users_max_series = 5.0;

        public int getFindMaxInListInt(List<int> arr)
        {
            int max = int.MinValue;

            foreach(var elem in arr)
            {
                if (elem > max)
                {
                    max = elem;
                }
            }
            return max;
        }

        public double getFindMaxInListDouble(List<double> arr)
        {
            double max = double.MinValue;

            foreach (var elem in arr)
            {
                if (elem > max)
                {
                    max = elem;
                }
            }
            return max;
        }

        public string UTF8_Supressor(string cp1251_encoded)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] win1251bytes = win1251.GetBytes(cp1251_encoded);
            byte[] utf8bytes = Encoding.Convert(win1251, utf8, win1251bytes);

            //string utf8_str = utf8.GetString(utf8bytes);
            string utf8_str = ConvertWin1251ToUTF8(cp1251_encoded);
            return utf8_str;



        }

        readonly static System.Text.Encoding WINDOWS1251 = Encoding.GetEncoding(1251);
        readonly static System.Text.Encoding UTF8 = Encoding.UTF8;

        static string ConvertWin1251ToUTF8(string inString)
        {
            return UTF8.GetString(WINDOWS1251.GetBytes(inString));
        }

        public async Task ChangeAcitivityInterop(List<int> arr_cnts)
        {
            mi_commonlst.Clear();
            mi_common.Clear();
            uavm.Clear();
            graphData.Clear();
            mi_commonlst.Clear();
            DateTime dt = activity_date;
            //int arr_cnt = getArrCntByLogin(session_cookie_data);

            table_activity_page_workdaily = "";
            table_activity_page_workdaily_duration = "";
            data_series_collection.Clear();
            foreach (int p in arr_cnts)
            {

                string log_path = genPathToThinClientStatsByUserID(p, dt);

                if (log_path != "")
                {

                    List<MiCommonModel> mi_StatisticsPresskeys = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsWebPages = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsSNS = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsClipboard = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsFileOperation = new List<MiCommonModel>();

                    mi_StatisticsPresskeys = mi.StatisticsPresskeys(log_path);
                    mi_StatisticsWebPages = mi.StatisticsWebPages(log_path);
                    mi_StatisticsClipboard  = mi.StatisticsClipboard(log_path);
                    mi_StatisticsFileOperation = mi.StatisticsFileOperation(log_path);
                    mi_StatisticsSNS = mi.StatisticsSNS(log_path);

                    List<MiCommonModel> mi_common = new List<MiCommonModel>();
                    mi_common = getStatCommonModel(mi_StatisticsPresskeys, mi_StatisticsWebPages, mi_StatisticsSNS, mi_StatisticsClipboard, mi_StatisticsFileOperation);
                    mi_commonlst.Add(mi_common);
                  
                }



                if ((log_path) != "")
            {
                TerminalServer_AnalyzeMethodActivityModel tsamam = mi.AnalyzeMethodActivityByDate(log_path, dt);

                if (tsamam.tsamaam_start.Year != 1)
                {

                    string str_h = tsamam.tsamaam_time.ToString(@"hh");
                    string str_m = tsamam.tsamaam_time.ToString(@"mm");
                        //string str_s = tsamam.tsamaam_time.ToString(@"ss\:fff");

                        //a_str += tsamam.tsamaam_start.ToShortTimeString() + " / ";
                        //a_str += tsamam.tsamaam_end.ToShortTimeString();
                        //a_str += "<br> (" + str_h + " ч " + str_m + " мин)";

                        UserActivityViewModel uavm_elem = new UserActivityViewModel()
                        {
                            username = fullname_dt[p],
                            workdaily = "c " + tsamam.tsamaam_start.ToShortTimeString() + " до " + tsamam.tsamaam_end.ToShortTimeString(),
                            workdaily_duration = "Длительность: " + str_h + " ч " + str_m + " мин",

                        };
                        uavm.Add(uavm_elem);


                    for (int tsamam_al_cnt = 0; tsamam_al_cnt < tsamam.activity_started.Count; tsamam_al_cnt++)
                    {
                        DateTime dt_start_activity = tsamam.activity_started[tsamam_al_cnt];
                        DateTime dt_end_activity = tsamam.activity_ended[tsamam_al_cnt];

                        List<WatcherEvent> data_collection = new List<WatcherEvent>();

                        int h_sa = dt_start_activity.Hour;
                        int m_sa = dt_start_activity.Minute;

                        int h_ea = dt_end_activity.Hour;
                        int m_ea = dt_end_activity.Minute;

                        List<ChartTimeModel> ctm_lst = new List<ChartTimeModel>();

                        if (h_sa == h_ea)
                        {
                            //Меньше часа в наборе
                            for (int m = m_sa; m < m_ea; m++)
                            {
                                ChartTimeModel ctm = new ChartTimeModel();



                                ctm.hour = h_sa;
                                ctm.minute = m;
                                ctm_lst.Add(ctm);
                            }
                        }
                        else if (h_ea > h_sa)
                        {
                            //Больше часа в наборе

                            //Сколько не хватает до часа в большую сторону
                            int start_min_dev = 60 - m_sa;

                            for (int z = 0; z < start_min_dev; z++)
                            {
                                ChartTimeModel ctm = new ChartTimeModel();


                                ctm.hour = h_sa;
                                ctm.minute = m_sa + z;
                                ctm_lst.Add(ctm);
                            }

                            //Сколько между ними часов
                            int how_much_hours = h_ea - h_sa;

                            how_much_hours -= 1;

                            for (int az = 1; az < how_much_hours; az++)
                            {
                                for (int bz = 0; bz < 60; bz++)
                                {
                                    ChartTimeModel ctm = new ChartTimeModel();

                                    ctm.hour = h_sa + 1 + az;
                                    ctm.minute = bz;
                                    ctm_lst.Add(ctm);
                                }
                            }

                            //Добираем последние крупицы времени
                            for (int cp = 0; cp < m_ea; cp++)
                            {
                                ChartTimeModel ctm = new ChartTimeModel();


                                ctm.hour = h_sa + 1 + how_much_hours + 1;
                                ctm.minute = cp;
                                ctm_lst.Add(ctm);
                            }
                        }

                        foreach (var elem in ctm_lst)
                        {

                            string h_str = "";
                            string m_str = "";

                            if (elem.hour < 10)
                            {
                                h_str += "0" + elem.hour.ToString();
                            }
                            else
                            {
                                h_str += elem.hour.ToString();
                            }

                            if (elem.minute < 10)
                            {
                                m_str += "0" + elem.minute.ToString();
                            }
                            else
                            {
                                m_str += elem.minute.ToString();
                            }

                                users_series_max.Add(0.5 * (p + 1));
                                WatcherEvent we = new WatcherEvent { Sector = h_str + ":" + m_str, Count = 0.5*(p+1) };
                            data_collection.Add(we);
                        }



                        LineChartDataset<WatcherEvent> lcd_we_new = new LineChartDataset<WatcherEvent>();

                        lcd_we_new.Label = "Активность пользователя " + fullname_dt[p];
                        lcd_we_new.Data = data_collection;
                        lcd_we_new.BackgroundColor = backgroundColors[p]; // line chart can only have one color
                        lcd_we_new.BorderColor = borderColors[1];
                        lcd_we_new.Fill = true;
                        lcd_we_new.PointRadius = 1.0f;
                        lcd_we_new.BorderWidth = 0;
                        lcd_we_new.PointBorderColor = Enumerable.Repeat(borderColors.First(), 6).ToList();
                        lcd_we_new.CubicInterpolationMode = "monotone";

                        data_series_collection.Add(lcd_we_new);

                         //partial activity laoder




                        ctm_lst.Clear();
                    }

                }

            }
            }
            
            foreach(var elem in data_series_collection)
            {
                graphData.Add(elem);
            }

            users_max_series = getFindMaxInListDouble(users_series_max);
            lineChartOptionsInterop = ChartOptionsGenerator();

            graph_vis = false;
            StateHasChanged();
            await Task.Delay(120);
            
            graph_vis = true;
            StateHasChanged();

        }

    }
}
