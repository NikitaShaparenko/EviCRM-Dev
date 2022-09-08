using Microsoft.AspNetCore.Components;
using EviCRM.Server.Core;
using EviCRM.Server.Models;
using Majorsoft.Blazor.Components.Notifications;
using Microsoft.AspNetCore.SignalR.Client;


using Append.Blazor.Notifications;
using Majorsoft.Blazor.Extensions.BrowserStorage;
using Majorsoft.Blazor.Components.GdprConsent;
using Majorsoft.Blazor.Components;
using EviCRM.Server.Pages.Charts;
using Blazorise.Charts;
using Microsoft.JSInterop;
using EviCRM.Server.Models.Statistics;
using EviCRM.Server.Pages.Charts.ChartListInterop;

namespace EviCRM.Server.Pages
{
    public partial class Index
    {
        LineChartViewModule lcvm;
        LineChartViewModuleFullScreenrazor lcvm_modal = new LineChartViewModuleFullScreenrazor();

        private HubConnection? hubConnection_chat; //Хаб чата
        private HubConnection? hubConnection_mail; //Хаб почты
        private HubConnection? hubConnection_task; //Хаб задачника
        private HubConnection? hubConnection_general; //Хаб общий

        private List<string> messages_chat = new List<string>();
        private List<string> messages_mail = new List<string>();
        private List<string> messages_task = new List<string>();
        private List<string> messages_general = new List<string>();

        private List<string> lst_messages_chat = new List<string>();
        private List<string> lst_messages_mail = new List<string>();
        private List<string> lst_messages_task = new List<string>();
        private List<string> lst_messages_general = new List<string>();

        List<SortingModel> lst_sortingmodel = new List<SortingModel>();
        List<SortingModel> lst_sm_sorted = new List<SortingModel>();

        BackendController bc = new BackendController();
        Sentinel sentinel = new Sentinel();
        SystemCore systemcore = new SystemCore();

        private string? userInput;
        private string? messageInput;

        bool modalGraphOpened { get; set; }
        DateTime activity_date { get; set; }

        string table_activity_page_workdaily { get; set; }
        string table_activity_page_workdaily_duration { get; set; }

        public async Task ChangeActivityDate(string value)
        {
            if (DateTime.TryParse(value, out DateTime s))
            {
                activity_date = DateTime.Parse(value); 
                activity_date = DateTime.Parse(value);
                await ChangeAcitivityInterop();
                StateHasChanged();
            }
        }

        LineChartOptions lineChartOptionsInterop = new LineChartOptions();

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
                        Max = 1 + 1,
                        //Ticks = new ChartAxisTicks
                        //{
                        //    StepSize = 0.5,
                        //}
                    },

                },
            };

            return lineChartOptions;
        }

        public void ShowGraphActivityFull()
        {
            graphData2 = data_series_collection;
            modalGraphOpened = true;
            StateHasChanged();
        }

        List<string> isInCast(string username)
        {
            List<string> lstr = new List<string>();
            List<string> buffer_decoder = new List<string>();
            for (int zo = 0; zo < tasks_members_dt.Count; zo++)
            {
                buffer_decoder = bc.getMultipleStringDecodingString(tasks_members_dt[zo]);
                foreach (string bd in buffer_decoder)
                {
                    if (bd == username)
                    {
                        lstr.Add(tasks_id_dt[zo]);
                    }
                }
            }
            return lstr;
        }
        List<string> isInAuthors(string username)
        {
            List<string> lstr = new List<string>();
            List<string> buffer_decoder = new List<string>();
            for (int zo = 0; zo < tasks_authors_dt.Count; zo++)
            {

                if (tasks_authors_dt[zo] == username)
                {
                    lstr.Add(tasks_id_dt[zo]);
                }
            }
            return lstr;
        }

        public async Task SignalR_Chat_Handler(string user, string message)
        {
            bool isTabActive = false;

            if (notify_toasts)
            {
                await ToastsNotifications_ShowCustomToast_Primary("Новое сообщение!" + Environment.NewLine + "'" + user + "' написал: " + message);
            }
            if (notify_browser)
            {
                //await NotificationShow("Новое сообщение в чате!", Environment.NewLine + "'" + user + "' написал: " + message);
            }
            if (notify_Telegram)
            {
                //await TelegramNotificator(user, message);
            }
        }

        public async Task TelegramNotificator(string login, string message)
        {
            List<string> cmd_bn = new List<string>();
            List<string> cmd_bv = new List<string>();

            cmd_bn.Add("cmd");
            cmd_bv.Add("telegram_direct");

            cmd_bn.Add("login");
            cmd_bv.Add(login);

            cmd_bn.Add("message");
            cmd_bv.Add(Base64Encode("message"));
            List<string> chatID_collections = new List<string>();

            string telegram_status_cmd = bc.backend_GetLinkBuilder(cmd_bn, cmd_bv);
            string telegram_status = await bc.backend_PostAsync("http://localhost:9254/get/", telegram_status_cmd);

        }

        private async Task NotificationShow(string n_title, string n_description)
        {
            //await _notificationService.CreateAsync(n_title, n_description);
            //path = "images/github.png"
        }

        public async Task SignalR_Mail_Handler(string user, string message)
        {

        }

        public async Task SignalR_Task_Handler(string user, string message)
        {

        }

        public async Task SignalR_General_Handler(string user, string message)
        {

        }


        #region Toast Notifications

        private async Task ToastsNotifications_ShowAllToast()
        {
            foreach (var item in Enum.GetValues<NotificationTypes>())
            {
                _toastService.ShowToast($@"<strong>Toast оповещения:</strong> Это оповещение типа {item}", item);
            }
        }

        private Guid _lastToastId;

        private async Task ToastsNotifications_ShowCustomToast_Primary(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Primary);
        }
        private async Task ToastsNotifications_ShowCustomToast_Secondary(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Secondary);
        }
        private async Task ToastsNotifications_ShowCustomToast_Info(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Info);
        }
        private async Task ToastsNotifications_ShowCustomToast_Success(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Success);
        }
        private async Task ToastsNotifications_ShowCustomToast_Warning(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Warning);
        }
        private async Task ToastsNotifications_ShowCustomToast_Danger(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Danger);
        }
        private async Task ToastsNotifications_ShowCustomToast_Light(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Light);
        }
        private async Task ToastsNotifications_ShowCustomToast_Dark(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Dark);
        }
        private async Task ToastsNotifications_ShowCustomToast(string toastText, NotificationStyles ns, NotificationTypes ntype)
        {
            _lastToastId = _toastService.ShowToast(new ToastSettings()
            {
                Content = builder => builder.AddMarkupContent(0, toastText),
                NotificationStyle = ns,
                Type = ntype,
                AutoCloseInSec = _toastAutoCloseInSec,
                ShadowEffect = _toastShadowEffect,
                ShowCloseButton = _toastShowCloseButton,
                ShowCloseCountdownProgress = _toastShowCountdownProgress,
                ShowIcon = _toastShowIcon
            });
        }

        #endregion

        MySQL_Controller mysqlc = new MySQL_Controller();

        bool isAdmin { get; set; }
        DateTime dt_now { get; set; }

        string current_date_time { get; set; }

        string cookie_role { get; set; }
        string cookie_data { get; set; }

        string weekd_color { get; set; }

        //EviCRM.Server.Core.BackendController bc = new EviCRM.Server.Core.BackendController();

        HubConnection connection_chat;
        HubConnection connection_mail;
        HubConnection connection_task;
        HubConnection connection_general;
        string Message = "Очень длинное тестовое сообщение";
        IList<string> messages = new List<string>();

        bool notify_toasts = true;
        bool notify_browser = true;
        bool notify_Telegram = true;

        bool ready { get; set; }

        private string _toastText = $@"<strong>Toast:</strong> This is a(n) {NotificationTypes.Primary} notification";
        private bool _toastShowIcon = true;
        private bool _toastShowCloseButton = true;
        private bool _toastShowCountdownProgress = true;
        private uint _toastAutoCloseInSec = 5;
        private uint _toastShadowEffect = 5;
        private NotificationStyles _toastStyle;
        private NotificationTypes _toastTypeLevel;

        private ElementReference log1;
        private string _toastLog = "";

        string current_user_id { get; set; }

        List<string> thinclient_dt = new List<string>();

        List<string> tasks_id_dt = new List<string>();
        List<string> tasks_name_dt = new List<string>();
        List<string> tasks_budgets_dt = new List<string>();
        List<string> tasks_members_dt = new List<string>();
        List<string> tasks_status_dt = new List<string>();
        List<string> tasks_personal_status_dt = new List<string>();
        List<string> tasks_startdate_dt = new List<string>();
        List<string> tasks_enddate_dt = new List<string>();
        List<string> tasks_authors_dt = new List<string>();

        List<string> username_is_incast = new List<string>();
        List<string> username_is_inauthors = new List<string>();
        List<string> users_avatars_dt = new List<string>();
        List<string> users_dt = new List<string>();

        List<string> marks_idmarks = new List<string>();
        List<string> marks_user_id = new List<string>();
        List<string> marks_date = new List<string>();
        List<string> marks_first_mark = new List<string>();
        List<string> marks_second_mark = new List<string>();
        List<string> marks_isTwoMarks = new List<string>();

        List<string> id_dt = new List<string>();
        List<string> fullname_dt = new List<string>();

        string session_cookie_data { get; set; }
        string session_role { get; set; }

        List<LineChartDataset<WatcherEvent>> graphData = new List<LineChartDataset<WatcherEvent>>();
        List<LineChartDataset<WatcherEvent>> graphData2 = new List<LineChartDataset<WatcherEvent>>();

        [Inject]
        IJSRuntime iJS { get; set; }
        enum table_activity_pages
        {
            marks,
            tasks,
            tasks_board,
            time_hours,
            time_heatmap,
            time_radar,
        };

        table_activity_pages tableactivity_active_tab;

        private async Task Debug_SendTo(string user, string message)
        {
            if (connection_chat is not null)
            {
                await connection_chat.SendAsync("SendMessage", user, message);
            }
            if (connection_mail is not null)
            {
                await connection_mail.SendAsync("SendMessage", user, message);
            }
            if (connection_task is not null)
            {
                await connection_task.SendAsync("SendMessage", user, message);
            }
            if (connection_general is not null)
            {
                await connection_general.SendAsync("SendMessage", user, message);
            }
        }
        private async Task Debug_SendAll(string message)
        {
            if (hubConnection_chat is not null)
            {
                await hubConnection_chat.SendAsync("broadcastMessage", message);
                await hubConnection_chat.InvokeAsync("SendAll", "Blazor Client", Message);
            }
            if (hubConnection_mail is not null)
            {
                await hubConnection_mail.SendAsync("broadcastMessage", message);
                await hubConnection_mail.InvokeAsync("SendAll", "Blazor Client", Message);
            }
            if (hubConnection_task is not null)
            {
                await hubConnection_task.SendAsync("broadcastMessage", message);
                await hubConnection_task.InvokeAsync("SendAll", "Blazor Client", Message);
            }
            if (hubConnection_general is not null)
            {
                await hubConnection_general.SendAsync("broadcastMessage", message);
                await hubConnection_general.InvokeAsync("SendAll", "Blazor Client", Message);
            }
        }

        public bool IsConnected_chat =>
            connection_chat?.State == HubConnectionState.Connected;

        public bool IsConnected_mail =>
          connection_mail?.State == HubConnectionState.Connected;

        public bool IsConnected_task =>
          connection_task?.State == HubConnectionState.Connected;

        public bool IsConnected_general =>
          connection_general?.State == HubConnectionState.Connected;

        public int getArrCntByLogin(string login)
        {
            for (int i = 0; i < users_dt.Count; i++)
            {
                if (users_dt[i] == login)
                {
                    return i;
                }
            }
            return 0;
        }
        List<LineChartDataset<WatcherEvent>> data_series_collection = new List<LineChartDataset<WatcherEvent>>();

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

            foreach (var elem in keys)
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




        public void getStatisticsCore(string filename)
        {
            mi_StatisticsPresskeys = mi.StatisticsPresskeys(filename);
            mi_StatisticsWebPages = mi.StatisticsWebPages(filename);
            mi_StatisticsSNS = mi.StatisticsSNS(filename);
            mi_StatisticsClipboard = mi.StatisticsClipboard(filename);
            mi_StatisticsFileOperation = mi.StatisticsFileOperation(filename);
        }

        async Task getRadarData()
        {
            RadarChartDataset<int> rcd_int = new RadarChartDataset<int>();

            int rcd_stat_web = mi_StatisticsWebPages.Count;
            int rcd_stat_sns = mi_StatisticsSNS.Count;
            int rcd_stat_clipboard = mi_StatisticsClipboard.Count;
            int rcd_stat_files = mi_StatisticsFileOperation.Count;

            rcd_int.Data = new List<int>();
            rcd_int.Data.Add(rcd_stat_web);
            rcd_int.Data.Add(rcd_stat_sns);
            rcd_int.Data.Add(rcd_stat_clipboard);
            rcd_int.Data.Add(rcd_stat_files);

            rcd_int.Label = "Радар активности";

            List<string> lbls = new List<string>();
            lbls.Add("Веб-страницы");
            lbls.Add("Социальные сети");
            lbls.Add("Буфер обмена");
            lbls.Add("Работа с файлами");

            radar_lbls = lbls;
            radar_lst = rcd_int;
            radar_visupdate = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(200);
            radar_visupdate = true;
            await InvokeAsync(StateHasChanged);

        }

        public async Task ChangeAcitivityInterop()
        {
            DateTime dt = activity_date;
            int arr_cnt = getArrCntByLogin(session_cookie_data);


            string log_path = genPathToThinClientStatsByUserID(arr_cnt, dt);

            data_series_collection.Clear();

            if ((log_path != ""))
            {
                getStatisticsCore(log_path);
                getRadarData();
            }

            mi_commonlst.Clear();
            mi_common.Clear();
      
            graphData.Clear();
            mi_commonlst.Clear();
            //int arr_cnt = getArrCntByLogin(session_cookie_data);

            //table_activity_page_workdaily = "";
            //table_activity_page_workdaily_duration = "";
            data_series_collection.Clear();
           
                if (log_path != "")
                {

                    List<MiCommonModel> mi_StatisticsPresskeys = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsWebPages = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsSNS = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsClipboard = new List<MiCommonModel>();
                    List<MiCommonModel> mi_StatisticsFileOperation = new List<MiCommonModel>();

                    mi_StatisticsPresskeys = mi.StatisticsPresskeys(log_path);
                    mi_StatisticsWebPages = mi.StatisticsWebPages(log_path);
                    mi_StatisticsClipboard = mi.StatisticsClipboard(log_path);
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
                    
                    table_activity_page_workdaily = "c " + tsamam.tsamaam_start.ToShortTimeString() + " до " + tsamam.tsamaam_end.ToShortTimeString();
                    table_activity_page_workdaily_duration = str_h + " ч " + str_m + " мин";

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
                            for (int p = 0; p < m_ea; p++)
                            {
                                ChartTimeModel ctm = new ChartTimeModel();


                                ctm.hour = h_sa + 1 + how_much_hours + 1;
                                ctm.minute = p;
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


                            WatcherEvent we = new WatcherEvent { Sector = h_str + ":" + m_str, Count = 0.5 };
                            data_collection.Add(we);
                        }



                        LineChartDataset<WatcherEvent> lcd_we_new = new LineChartDataset<WatcherEvent>();

                        lcd_we_new.Label = "Активность пользователя";
                        lcd_we_new.Data = data_collection;
                        lcd_we_new.BackgroundColor = backgroundColors[1]; // line chart can only have one color
                        lcd_we_new.BorderColor = borderColors[1];
                        lcd_we_new.Fill = true;
                        lcd_we_new.PointRadius = 1.0f;
                        lcd_we_new.BorderWidth = 0;
                        lcd_we_new.PointBorderColor = Enumerable.Repeat(borderColors.First(), 6).ToList();
                        lcd_we_new.CubicInterpolationMode = "monotone";

                        data_series_collection.Add(lcd_we_new);
                        lineChartOptionsInterop = ChartOptionsGenerator();
                        ctm_lst.Clear();
                    }

                }

            }

            await lcvm.UpdateGraph(data_series_collection);
        }


        private List<string> backgroundColors = new() { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        private List<string> borderColors = new() { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

        public void OnCloseModalGraphFullScreenHander(bool closed)
        {
            modalGraphOpened = false;
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

        public string genPathToThinClientStatsByUserID(int user_array_id)
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

            if (isFirstServer)
            {
                full_path = Path.Combine(_env.WebRootPath, "thinclient", "RENDEV", thinc, thinc + ".csv");

            }
            else
            {
                full_path = Path.Combine(_env.WebRootPath, "thinclient", "RENOVATIONDEV", thinc + ".csv");
            }

            if (File.Exists(full_path))
            {
                str = full_path;
            }

            return str;
        }

        public string getUserDataByIDAndDay(int i, DateTime dt_ud)
        {
            string a_str = "";

            //i - user array id
            //days - number of days in month

            string log_path = genPathToThinClientStatsByUserID(i);

            if ((log_path) != "")
            {
                TerminalServer_AnalyzeMethodActivityModel tsamam = mi.AnalyzeMethodActivityByDate(log_path, dt_ud);

                if (tsamam.tsamaam_start.Year != 1)
                {

                    string str_h = tsamam.tsamaam_time.ToString(@"hh");
                    string str_m = tsamam.tsamaam_time.ToString(@"mm");
                    //string str_s = tsamam.tsamaam_time.ToString(@"ss\:fff");

                    a_str += tsamam.tsamaam_start.ToShortTimeString() + " / ";
                    a_str += tsamam.tsamaam_end.ToShortTimeString();
                    a_str += "(" + str_h + " ч " + str_m + " мин)";
                }
            }



            return a_str;
        }

        public TerminalServer_AnalyzeMethodActivityModel getUserData(DateTime dt_ud, int i)
        {
            TerminalServer_AnalyzeMethodActivityModel tsamam = new TerminalServer_AnalyzeMethodActivityModel();
            string a_str = "";

            string log_path = genPathToThinClientStatsByUserID(i);

            if ((log_path) != "")
            {
                tsamam = mi.AnalyzeMethodActivityByDate(log_path, dt_ud);

                return tsamam;

            }

            return tsamam;

        }


        MipkoIntegrator mi = new MipkoIntegrator();
        MipkoIntegratorCore mic = new MipkoIntegratorCore();



        public async ValueTask DisposeAllAsync()
        {
            if (connection_chat is not null)
            {
                await connection_chat.DisposeAsync();
            }
            if (connection_mail is not null)
            {
                await connection_mail.DisposeAsync();
            }
            if (connection_task is not null)
            {
                await connection_task.DisposeAsync();
            }
            if (connection_general is not null)
            {
                await connection_general.DisposeAsync();
            }
        }

        public async Task DevInterpeter()
        {
            await Debug_SendAll("test");

        }

        bool radar_visupdate { get; set; }
        List<string> radar_lbls = new List<string>();
       RadarChartDataset<int> radar_lst = new RadarChartDataset<int>();
        protected override async Task OnInitializedAsync()
        {
            ready = false;
            modalGraphOpened = false;
            radar_visupdate = true;

            tasks_id_dt = await mysqlc.getListTaskIDAsync();
            tasks_name_dt = await mysqlc.getListTaskNameAsync();
            tasks_budgets_dt = await mysqlc.getListTaskBudgetAsync();
            tasks_members_dt = await mysqlc.getListTaskMembersAsync();
            tasks_status_dt = await mysqlc.getListTaskStatusAsync();
            tasks_startdate_dt = await mysqlc.getListTaskStartDateAsync();
            tasks_enddate_dt = await mysqlc.getListTaskEndDateAsync();
            tasks_authors_dt = await mysqlc.getListTasksAuthorAsync();
            tasks_personal_status_dt = await mysqlc.getListTaskPersonalStatusAsync();
            thinclient_dt = await mysqlc.getListUsersThinClientAsync();

            users_dt = await mysqlc.getListUsersLoginAsync();
            id_dt = await mysqlc.getListUsersIDAsync();
            fullname_dt = await mysqlc.getListUsersFullnameAsync();

            marks_idmarks = await mysqlc.getListMarksIDAsync();
            marks_user_id = await mysqlc.getListMarksUserIDAsync();
            marks_date = await mysqlc.getListMarksDateAsync();
            marks_first_mark = await mysqlc.getListMarksFirstMarkAsync();
            marks_second_mark = await mysqlc.getListMarksSecondMarkAsync();
            marks_isTwoMarks = await mysqlc.getListMarksIsTwoMarksAsync();

            session_cookie_data = authStateProvider.GetAuthenticationStateAsync().Result.User.Identity.Name;
           
            session_role = await mysqlc.MySql_ContextAsyncL(mysqlc.getProjectUsersRolesByIdentityName(session_cookie_data));

            current_user_id = getUserArrayIDByUsername(session_cookie_data);

            if (session_role == "admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
            username_is_incast = isInCast(session_cookie_data);
            username_is_inauthors = isInAuthors(session_cookie_data);

            users_avatars_dt = mysqlc.getAvatarPaths(users_dt);
            lst_sortingmodel.Clear();

            for (int cnt = 0; cnt < tasks_id_dt.Count; cnt++)
            {

                if (isAdmin)
                {
                    SortingModel sm = new SortingModel();
                    sm.task_id = tasks_id_dt[cnt];
                    sm.task_start = DateTime.Parse(tasks_startdate_dt[cnt]);
                    sm.task_end = DateTime.Parse(tasks_enddate_dt[cnt]);
                    sm.task_body = tasks_name_dt[cnt];
                    lst_sortingmodel.Add(sm);
                }
                else
                {
                    if (username_is_incast.Contains(tasks_id_dt[cnt]))
                    {
                        SortingModel sm = new SortingModel();
                        sm.task_id = tasks_id_dt[cnt];
                        sm.task_start = DateTime.Parse(tasks_startdate_dt[cnt]);
                        sm.task_end = DateTime.Parse(tasks_enddate_dt[cnt]);
                        sm.task_body = tasks_name_dt[cnt];
                        lst_sortingmodel.Add(sm);
                    }
                }
            }
            lst_sm_sorted = SortingTaskModels(lst_sortingmodel);

            DateTime dt1 = DateTime.Now.AddDays(-1);
            activity_date = dt1;

            int p = 0;
            if (users_dt != null)
            {
                p = users_dt.Count;
            }

            for (int i = 0; i < id_dt.Count; i++)
            {

            }

            LoadStatisticsTasksData();
            PersonalDataLoad();

            tasks_current = personal_stat.tasks_status_pending;
            tasks_delayed = personal_stat.tasks_status_delayed;
            tasks_planned = personal_stat.tasks_status_approved + personal_stat.tasks_status_waiting;
            tasks_failed = personal_stat.tasks_status_failed;

            //========================

            isJobDescriptionOpened = false;
            isUrgentEventsOpened = false;

            ready = true;
            StateHasChanged();
        }

        public double statistics_tasks_getAverage()
        {
            List<double> avg_arr = new List<double>();

            double average = 0.0;

            foreach (var elem in statistics_arr)
            {
                avg_arr.Add(elem.get_totaltasks_Mark() + elem.get_personaltasks_Mark());
            }

            foreach (double elem in avg_arr)
            {
                average += elem;
            }

            return average / avg_arr.Count;

        }

        public void PersonalDataLoad()
        {
            int arr_num = -1;

            string user_login = session_cookie_data;

            for (int i = 0; i<users_dt.Count;i++)
            {
                if (users_dt[i] == user_login)
                {
                    arr_num = i;
                }
            }

            if (arr_num != -1)
            { 
            if (statistics_arr != null)
            {
                foreach (var elem in statistics_arr)
                {
                    if (elem.user_arr_id == arr_num)
                    {
                        personal_stat = elem;
                    }
                }
            }
            }

            StateHasChanged();
        }

        public void LoadStatisticsTasksData()
        {
            statistics_arr.Clear();

            for (int i = 0; i < users_dt.Count; i++)
            {
                StatisticsChartElement sc = new StatisticsChartElement();

                //Данные пользователя
                sc.user_arr_id = i;
                sc.user_fullname = fullname_dt[i];

                ChartOptions pieOptions = new ChartOptions
                {
                    Responsive = true,
                    Plugins = new ChartPlugins
                    {
                        Legend = new ChartLegend
                        {
                            Display = false,
                        },

                        Title = new ChartTitle
                        {
                            Display = false,
                            Text = i.ToString(),
                        }
                    },
                };

                sc.pieOption = pieOptions;

                ChartOptions barOptions = new ChartOptions
                {
                    Responsive = true,
                    Plugins = new ChartPlugins
                    {
                        Legend = new ChartLegend
                        {
                            Display = false,
                        },

                        Title = new ChartTitle
                        {
                            Display = false,
                            Text = i.ToString(),
                        }
                    },
                };

                sc.barOption = barOptions;


               
                sc.barDatasetLabels = new string[] { "Ожидает подтверждения", "Подтверждено", "Выполняется", "Просрочена", "Выполнена", "Отменена", "Провалена" };
                sc.pieDatasetLabels = new string[] { "Ожидает подтверждения", "Подтверждено", "Выполняется", "Просрочена", "Выполнена", "Отменена", "Провалена" };

                sc.personal_barDatasetLabels = new string[] { "Ожидает подтверждения", "Подтверждено", "Выполняется", "Просрочена", "Выполнена", "Отменена", "Провалена" };
                sc.personal_pieDatasetLabels = new string[] { "Ожидает подтверждения", "Подтверждено", "Выполняется", "Просрочена", "Выполнена", "Отменена", "Провалена" };

                for (int z = 0; z < tasks_id_dt.Count; z++)
                {
                    if (isInCast(users_dt[i]).Contains(tasks_id_dt[z]) || isInAuthors(users_dt[i]).Contains(tasks_id_dt[z]))
                    {

                        personal_status = bc.getMultipleStringDecodingString(tasks_personal_status_dt[z]); //разбились на пары
                        personal_status_arg1 = bc.getMultArgs_DeCombine_Args1(personal_status); //шапки пар
                        personal_status_arg2 = bc.getMultArgs_DeCombine_Args2(personal_status); // значения пар
                        int personal_status_arr_id = getPersonalStatusArrayIDByUsername(users_dt[i]);

                        if (personal_status_arg2.Count > personal_status_arr_id)
                        {

                            switch (personal_status_arg2[personal_status_arr_id])
                            {
                                case "waiting":
                                    sc.tasks_personal_status_waiting += 1;
                                    break;

                                case "approved":
                                    sc.tasks_personal_status_approved += 1;
                                    break;

                                case "pending":
                                    sc.tasks_personal_status_pending += 1;
                                    break;

                                case "delayed":
                                    sc.tasks_personal_status_delayed += 1;
                                    break;

                                case "completed":
                                    sc.tasks_personal_status_completed += 1;
                                    break;

                                case "canceled":
                                    sc.tasks_personal_status_canceled += 1;
                                    break;

                                case "failed":
                                    sc.tasks_personal_status_failed += 1;
                                    break;
                            }
                        }


                        if (tasks_status_dt.Count > z)
                        {

                            switch (tasks_status_dt[z])
                            {
                                case "waiting":
                                    sc.tasks_status_waiting += 1;
                                    break;

                                case "approved":
                                    sc.tasks_status_approved += 1;
                                    break;

                                case "pending":
                                    sc.tasks_status_pending += 1;
                                    break;

                                case "delayed":
                                    sc.tasks_status_delayed += 1;
                                    break;

                                case "completed":
                                    sc.tasks_status_completed += 1;
                                    break;

                                case "canceled":
                                    sc.tasks_status_canceled += 1;
                                    break;

                                case "failed":
                                    sc.tasks_status_failed += 1;
                                    break;


                            }
                        }
                    }
                }

                PieChartDataset<int> pcd = new PieChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                           ChartColor.FromRgba(108, 117, 125, 255),
                       ChartColor.FromRgba(23,162,184,255),
                       ChartColor.FromRgba(40,167,69, 255),
                       ChartColor.FromRgba(220,53,69, 255),
                       ChartColor.FromRgba(128,158,11, 255),
                       ChartColor.FromRgba(249,58,37, 255),
                       ChartColor.FromRgba(249,58,37, 255),

                    },
                    Label = "График статусов задачи",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                pcd.Data.Add(sc.tasks_status_waiting);
                pcd.Data.Add(sc.tasks_status_approved);
                pcd.Data.Add(sc.tasks_status_pending);
                pcd.Data.Add(sc.tasks_status_delayed);
                pcd.Data.Add(sc.tasks_status_completed);
                pcd.Data.Add(sc.tasks_status_canceled);
                pcd.Data.Add(sc.tasks_status_failed);



                sc.pieDatasetList = pcd;

                BarChartDataset<int> bcd = new BarChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                          ChartColor.FromRgba(108, 117, 125, 255),
                       ChartColor.FromRgba(23,162,184,255),
                       ChartColor.FromRgba(40,167,69, 255),
                       ChartColor.FromRgba(220,53,69, 255),
                       ChartColor.FromRgba(128,158,11, 255),
                       ChartColor.FromRgba(249,58,37, 255),
                       ChartColor.FromRgba(249,58,37, 255),

                    },
                    Label = "График статусов задачи",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                bcd.Data.Add(sc.tasks_status_waiting);
                bcd.Data.Add(sc.tasks_status_approved);
                bcd.Data.Add(sc.tasks_status_pending);
                bcd.Data.Add(sc.tasks_status_delayed);
                bcd.Data.Add(sc.tasks_status_completed);
                bcd.Data.Add(sc.tasks_status_canceled);
                bcd.Data.Add(sc.tasks_status_failed);

                sc.barDatasetList = bcd;

                PieChartDataset<int> pcd_personal = new PieChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {


                       ChartColor.FromRgba(108, 117, 125, 255),
                       ChartColor.FromRgba(23,162,184,255),
                       ChartColor.FromRgba(40,167,69, 255),
                       ChartColor.FromRgba(220,53,69, 255),
                       ChartColor.FromRgba(128,158,11, 255),
                       ChartColor.FromRgba(249,58,37, 255),
                       ChartColor.FromRgba(249,58,37, 255),


                    },
                    Label = "График статусов задачи",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                pcd_personal.Data.Add(sc.tasks_personal_status_waiting);
                pcd_personal.Data.Add(sc.tasks_personal_status_approved);
                pcd_personal.Data.Add(sc.tasks_personal_status_pending);
                pcd_personal.Data.Add(sc.tasks_personal_status_delayed);
                pcd_personal.Data.Add(sc.tasks_personal_status_completed);
                pcd_personal.Data.Add(sc.tasks_personal_status_canceled);
                pcd_personal.Data.Add(sc.tasks_personal_status_failed);

                sc.personal_pieDatasetList = pcd_personal;

                BarChartDataset<int> bcd_personal = new BarChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                         ChartColor.FromRgba(108, 117, 125, 255),
                       ChartColor.FromRgba(23,162,184,255),
                       ChartColor.FromRgba(40,167,69, 255),
                       ChartColor.FromRgba(220,53,69, 255),
                       ChartColor.FromRgba(128,158,11, 255),
                       ChartColor.FromRgba(249,58,37, 255),
                       ChartColor.FromRgba(249,58,37, 255),

                    },
                    Label = "График статусов задачи",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                bcd_personal.Data.Add(sc.tasks_personal_status_waiting);
                bcd_personal.Data.Add(sc.tasks_personal_status_approved);
                bcd_personal.Data.Add(sc.tasks_personal_status_pending);
                bcd_personal.Data.Add(sc.tasks_personal_status_delayed);
                bcd_personal.Data.Add(sc.tasks_personal_status_completed);
                bcd_personal.Data.Add(sc.tasks_personal_status_canceled);
                bcd_personal.Data.Add(sc.tasks_personal_status_failed);

                sc.personal_barDatasetList = bcd_personal;

                statistics_arr.Add(sc);
            }


        }

        public int getPersonalStatusArrayIDByUsername(string username)
        {
            for (int i = 0; i < personal_status_arg1.Count; i++)
            {
                if (personal_status_arg1[i] == username)
                {
                    return i;
                }
            }
            return 0;
        }

        public string getUserArrayIDByUsername(string username)
        {
            for (int i = 0; i<id_dt.Count;i++)
            {
                if (users_dt[i] == username)
                {
                    return i.ToString();
                }
            }

            return "";
        }

        public void JobDescriptionHandlerModule()
        {

            if (isJobDescriptionOpened)
            {
                CloseJobDescriptionModule();
            }
            else
            {
                OpenJobDescriptionModule();
            }
            StateHasChanged();

        }

        public void CloseJobDescriptionModule()
        {
            isJobDescriptionOpened = false;
            StateHasChanged();
        }

        public void OpenJobDescriptionModule()
        {
            isJobDescriptionOpened = true;
            StateHasChanged();
        }

        bool isUrgentEventsOpened { get; set; }

        public void UrgentEventsHandlerModule()
        {

            if (isUrgentEventsOpened)
            {
                CloseUrgentEventsHandlerModule();
            }
            else
            {
                OpenUrgentEventsHandlerModule();
            }


        }

        public void CloseUrgentEventsHandlerModule()
        {
            isUrgentEventsOpened = false;
            StateHasChanged();
        }

        public void OpenUrgentEventsHandlerModule()
        {
            isUrgentEventsOpened = true;
            StateHasChanged();
        }

        public void tableactivity_setmark()
        {
            tableactivity_active_tab = table_activity_pages.marks;
            StateHasChanged();
        }

        public void tableactivity_settasks()
        {
            tableactivity_active_tab = table_activity_pages.tasks;
            StateHasChanged();
        }

        public void tableactivity_settasks_board()
        {
            tableactivity_active_tab = table_activity_pages.tasks_board;
            StateHasChanged();
        }

        public void tableactivity_sethours()
        {
            tableactivity_active_tab = table_activity_pages.time_hours;
            StateHasChanged();
        }

        public void tableactivity_setheatmap()
        {
            tableactivity_active_tab = table_activity_pages.time_heatmap;
            StateHasChanged();
        }

        public void tableactivity_setradar()
        {
            tableactivity_active_tab = table_activity_pages.time_radar;
            StateHasChanged();
        }

        bool isJobDescriptionOpened { get; set; }

        List<SortingModel> SortingTaskModels(List<SortingModel> source)
        {
            List<SortingModel> destination = new List<SortingModel>();
            destination = source;

            destination.Sort(delegate (SortingModel us1, SortingModel us2)
            { return us1.task_end.CompareTo(us2.task_end); });

            return destination;
        }

        Task OnBroadcastMessage(string name, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                messages.Add(name + " : " + message);
                StateHasChanged();
            }
            return Task.CompletedTask;
        }

        async Task SendMessage()
        {
            await hubConnection_general.InvokeAsync("SendAll", "Blazor Client", Message);
            Message = "";
        }

        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                string[] jsArgs = new string[] { "placeholder", "pdf", Guid.NewGuid().GetHashCode().ToString(), "Образец должностной инструкции", "https://evicrm.store/uploads/users/", "word", };
                await TestJSInterop();
                await iJS.InvokeVoidAsync("job_description_testloader");
                new Timer(DateTimeCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }

        async Task TestJSInterop()
        {
            await iJS.InvokeVoidAsync("console.log", "Тест JS Interop в браузере");
        }

        private void DateTimeCallback(object state)
        {
            Thread.Sleep(10);
            current_date_time = getCurrentDateTime();
            InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            dt_now = DateTime.Now;
            current_date_time = getCurrentDateTime();
            tableactivity_active_tab = table_activity_pages.tasks_board;
            StateHasChanged();
        }

        string getCurrentDateTime()
        {
            dt_now = DateTime.Now;
            string str = "";

            switch (dt_now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    str += "пн, ";
                    weekd_color = "red";
                    break;

                case DayOfWeek.Tuesday:
                    str += "вт, ";
                    weekd_color = "yellow";
                    break;
                case DayOfWeek.Wednesday:
                    str += "ср, ";
                    weekd_color = "pink";
                    break;
                case DayOfWeek.Thursday:
                    str += "чт, ";
                    weekd_color = "green";
                    break;
                case DayOfWeek.Friday:
                    str += "пт, ";
                    weekd_color = "orange";
                    break;
                case DayOfWeek.Saturday:
                    str += "сб, ";
                    weekd_color = "blue";
                    break;
                case DayOfWeek.Sunday:
                    str += "вс, ";
                    weekd_color = "purple";
                    break;
                default:
                    str += "?, ";
                    weekd_color = "black";
                    break;
            }


            //Формат даты-времени:      пт, 27.05.2022 20:49

            str += dt_now.ToShortDateString();

            if (dt_now.TimeOfDay.Hours < 10)
            {
                str += "  0" + dt_now.TimeOfDay.Hours + ":";
            }
            else
            {
                str += "  " + dt_now.TimeOfDay.Hours + ":";
            }

            if (dt_now.TimeOfDay.Minutes < 10)
            {
                str += "0" + dt_now.TimeOfDay.Minutes + ":";
            }
            else
            {
                str += dt_now.TimeOfDay.Minutes + ":";
            }

            if (dt_now.TimeOfDay.Seconds < 10)
            {
                str += "0" + dt_now.TimeOfDay.Seconds;
            }
            else
            {
                str += dt_now.TimeOfDay.Seconds;
            }

            return str;



        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        
       }


        }
}