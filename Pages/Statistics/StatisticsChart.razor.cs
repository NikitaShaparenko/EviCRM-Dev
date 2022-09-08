namespace EviCRM.Server.Pages.Statistics
{
    using EviCRM.Server.Core;
    using EviCRM.Server.Models;
    using Majorsoft.Blazor.Components.Notifications;
    using Microsoft.AspNetCore.SignalR.Client;
    using Majorsoft.Blazor.Extensions.BrowserStorage;
    using Majorsoft.Blazor.Components.GdprConsent;
    using Majorsoft.Blazor.Components;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using Microsoft.AspNetCore.Components.Authorization;

    using Blazorise.Charts;
    using EviCRM.Server.Models.Statistics;
    using ChartJs.Blazor.Util;
    using EviCRM.Server.Pages.Charts.ChartListInterop;

    public partial class StatisticsChart
    {

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        NavigationManager UriHelper { get; set; }

        [Inject]
        ILocalStorageService _localStorageService { get; set; }

        [Inject]
        ISessionStorageService _sessionStorageService { get; set; }

        [Inject]
        IToastService _toastService { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IWebHostEnvironment _env { get; set; }

        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        private void OnModalPieBarClose(bool accepted)
        {
            isModalPieOpen = false;
            StateHasChanged();

        }

        private void OpenModalPieBarDialog()
        {
            isModalPieOpen = true;
            StateHasChanged();
        }

        public void PieBarClickInterpreter(int i)
        {
            //if (modalPieBarAllow)
            //{
            //    modal_pie_config = _config[i];
            //    modal_user = fullname_dt[i];
            //    OpenPieBarModalDialog();
            //}
        }

        public async Task isOnlyWithMarksHandler(ChangeEventArgs cea)
        {
            isOnlyWithMarks = bool.Parse(cea.Value.ToString());

            InvokeAsync(StateHasChanged);
        
        }

        public async Task topnumber_changed(ChangeEventArgs cea)
        {
            TopNumber = int.Parse(cea.Value.ToString());
            ModelSort(true);
            InvokeAsync(StateHasChanged);
        }

        public async Task isTopChange(ChangeEventArgs cea)
        {
            isTop = bool.Parse(cea.Value.ToString());
            ModelSort(true);
            InvokeAsync(StateHasChanged);

        }

        public List<StatisticsViewModel_Pie> SortByIncreaseAvgValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderBy(x => x.getAverage(true)).ThenBy(x => x.user_fullname).ToList();
        }
        public List<StatisticsViewModel_Pie> SortByDecreaseAvgValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderByDescending(x => x.getAverage(true)).ThenBy(x => x.user_fullname).ToList();
        }

        public List<StatisticsViewModel_Pie> SortByIncreaseNameValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderBy(x => x.user_fullname).ToList();
        }
        public List<StatisticsViewModel_Pie> SortByDecreaseNameValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderByDescending(x => x.user_fullname).ToList();
        }

        bool isForwardOrder = true;

        public void ModelSort()
        {
            List<StatisticsViewModel_Pie> svmp_main_dubler = new List<StatisticsViewModel_Pie>();
        
            foreach(var elem in svmp_main)
            {
                svmp_main_dubler.Add(elem);
            }

            isForwardOrder = !isForwardOrder;

            svmp_main.Clear();
            StateHasChanged();
            if (isForwardOrder)
            {
                svmp_main = SortByIncreaseAvgValue(svmp_main_dubler);
            }
            else
            {
                svmp_main = SortByDecreaseAvgValue(svmp_main_dubler);
            }
            StateHasChanged();
        }

        public void ModelSort(string field)
        {
            List<StatisticsViewModel_Pie> svmp_main_dubler = new List<StatisticsViewModel_Pie>();

            foreach (var elem in svmp_main)
            {
                svmp_main_dubler.Add(elem);
            }

            isForwardOrder = !isForwardOrder;

            svmp_main.Clear();
            StateHasChanged();
            if (isForwardOrder)
            {
                svmp_main = SortByIncreaseNameValue(svmp_main_dubler);
            }
            else
            {
                svmp_main = SortByDecreaseNameValue(svmp_main_dubler);
            }
            StateHasChanged();
        }

        public void ModelSort(bool isForceForward)
        {
            List<StatisticsViewModel_Pie> svmp_main_dubler = new List<StatisticsViewModel_Pie>();

            foreach (var elem in svmp_main)
            {
                svmp_main_dubler.Add(elem);
            }
           
            svmp_main.Clear();
            StateHasChanged();
            svmp_main = SortByDecreaseAvgValue(svmp_main_dubler);
           
            StateHasChanged();
        }
        public async Task DateStartChangeHandler(ChangeEventArgs e)
        {
            DateTime dt_start2 = DateTime.Parse(e.Value.ToString());

            if (dt_start2.Date == DateTime.Now.Date && dt_end.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start2, dt_end);
            }
            getCompanyEvaluations();
            InvokeAsync(StateHasChanged);
        }

        public async Task DateEndChangeHandler(ChangeEventArgs e)
        {
            DateTime dt_end2 = DateTime.Parse(e.Value.ToString());
            //dt_end = DateTime.Parse(e.Value.ToString());

            if (dt_start.Date == DateTime.Now.Date && dt_end2.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start, dt_end2);
            }
            await InvokeAsync(StateHasChanged);
        }

        bool isModalPieOpen { get; set; }
        bool modalAllow { get; set; }

        bool personal { get; set; }

        DateTime dt_start { get; set; }

        DateTime dt_end { get; set; }
        public Task ClickEventExcelSave()
        {
            return Task.FromResult<object>(null);
        }

        public enum StatWindow
        {
            personal,
            common
        };

        public enum InterpolationCats
        {
            disabled,
            enabled,
            stepbefore,
            stepafter,
            stepmiddle
        };

        public bool pageLoaded { get; set; }

        public string InterpolationCatsString { get; set; }
     
        public async Task InterpolationCatsStringHandler(ChangeEventArgs cea)
        {
            string str = "";
            InterpolationCatsString = cea.Value.ToString();
            if (dt_start.Date == DateTime.Now.Date && dt_end.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start, dt_end);
            }
            await InvokeAsync(StateHasChanged);
        }
            

        public string StatWindowString { get; set; }
        public string StatWindowString2 { get; set; }

        public string isPieString
        {
            get
            {
                if (isPie)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            set
            {
                if (value == "True")
                {
                    isPie = true;
                    StateHasChanged();
                }
                else
                {
                    isPie = false;
                    StateHasChanged();
                }
            }
            
        }

        public bool isTop { get; set; }

        public bool isOnlyWithMarks { get; set; }

        public int TopNumber { get; set; }

        public int svp_index { get; set; }

        public List<StatisticsViewModel_Pie> svmp_main = new List<StatisticsViewModel_Pie>();


        private MySQL_Controller mysqlc = new MySQL_Controller();
        public static BackendController bc = new BackendController();

        //public ContactsUserModel cum = new ContactsUserModel();
        //public List<ContactsUserModel> cum_dt = new List<ContactsUserModel>();

        double avg_company { get; set; }

        double academic_perfomace_company { get; set; }

        double quality_of_work_company { get; set; }

        double perfomance_company { get; set; }

        bool isPie { get; set; }
        public string modal_user { get; set; }

        public int num { get; set; }

        List<string> users_dt = new List<string>();
        List<string> email_dt = new List<string>();
        List<string> passwords_dt = new List<string>();
        List<string> id_dt = new List<string>();
        List<string> fullname_dt = new List<string>();
        List<string> position_dt = new List<string>();
        List<string> skills_dt = new List<string>();
        List<string> avatarpath_dt = new List<string>();
        List<string> projects_val_dt = new List<string>();

        List<string> marks_idmarks = new List<string>();
        List<string> marks_user_id = new List<string>();
        List<string> marks_date = new List<string>();
        List<string> marks_first_mark = new List<string>();
        List<string> marks_second_mark = new List<string>();
        List<string> marks_first_description = new List<string>();
        List<string> marks_second_description = new List<string>();
        List<string> marks_isTwoMarks = new List<string>();

        #region Marks

        double getAverage(int i, bool isOverload)
        {
            double a = 0;

            int one = get_All_1MarksByUserID(id_dt[i]);
            int two = get_All_2MarksByUserID(id_dt[i]);
            int three = get_All_3MarksByUserID(id_dt[i]);
            int four = get_All_4MarksByUserID(id_dt[i]);
            int five = get_All_5MarksByUserID(id_dt[i]);

            if ((one + two + three + four + five) != 0)
            {
                a = (double)(one + two + three + four + five);
                double b = (double)(one * 1 + two * 2 + 3 * three + 4 * four + 5 * five);
                a = (double)(b / a);
            }
            else
            {
                a = 0;
            }

            return a;
        }

        async Task<double> getCompanyEvaluations()
        {

            //academic_perfomace_company = 0.0;
            //avg_company = 0.0;
            //quality_of_work_company = 0.0;
            //perfomance_company = 0.0;

            List<double> avgs = new List<double>();
            double avg = 0;
            foreach (var elem in svmp_main)
            {
                if (!(elem.marks1 == 0 && elem.marks2 == 0 && elem.marks3 == 0 && elem.marks4 == 0 && elem.marks5 == 0))
                {
                    avgs.Add(Double.Parse(elem.getAverage()));
                }
            }

            avg = 0;
            for (int z = 0; z < avgs.Count; z++)
            {
                avg += avgs[z];
            }

            if (avgs.Count != 0)
            {
                avg_company = (double)((double)avg / avgs.Count);
            }

            double t_v = 0;

            foreach (var elem in svmp_main)
            {
                if (!(elem.marks1 == 0 && elem.marks2 == 0 && elem.marks3 == 0 && elem.marks4 == 0 && elem.marks5 == 0))
                {
                    t_v += elem.marks3 + elem.marks4 + elem.marks5;
                }
            }

            t_v = (double)(t_v) / svmp_main.Count;
            academic_perfomace_company = t_v;

            t_v = 0;

            foreach (var elem in svmp_main)
            {
                if (!(elem.marks1 == 0 && elem.marks2 == 0 && elem.marks3 == 0 && elem.marks4 == 0 && elem.marks5 == 0))
                {
                    t_v += elem.marks4 + elem.marks5;
                }
            }
            t_v = (double)(t_v) / svmp_main.Count;
            quality_of_work_company = t_v;

            t_v = 0;
            double t_v2 = 0;
            double t_v3 = 0;

            foreach (var elem in svmp_main)
            {
                if (!(elem.marks1 == 0 && elem.marks2 == 0 && elem.marks3 == 0 && elem.marks4 == 0 && elem.marks5 == 0))
                {
                    t_v += elem.marks5 + elem.marks4 * 0.64 + elem.marks3 * 0.36 + elem.marks2 * 0.16 + elem.marks1 * 0.08;
                }
            }
            perfomance_company = t_v / svmp_main.Count;
            return 0;
        }

        int get_All_1Marks()
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "1")
                {
                    marks++;
                }
                if (marks_second_mark[i] == "1")
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_2Marks()
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "2")
                {
                    marks++;
                }
                if (marks_second_mark[i] == "2")
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_3Marks()
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "3")
                {
                    marks++;
                }
                if (marks_second_mark[i] == "3")
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_4Marks()
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "4")
                {
                    marks++;
                }
                if (marks_second_mark[i] == "4")
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_5Marks()
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "5")
                {
                    marks++;
                }
                if (marks_second_mark[i] == "5")
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_1MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "1" && marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (marks_second_mark[i] == "1" && marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_2MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "2" && marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (marks_second_mark[i] == "2" && marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_3MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "3" && marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (marks_second_mark[i] == "3" && marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_4MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "4" && marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (marks_second_mark[i] == "4" && marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_5MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_first_mark[i] == "5" && marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (marks_second_mark[i] == "5" && marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        #endregion

        #region Calcs
        public void PrimaryCalcs()
        {
            int i = 0;
            foreach (var elem in svmp_main)
            {
                List<BarChartDataset<int>> lbcd = new List<BarChartDataset<int>>();


                get_All_1MarksByUserID(id_dt[elem.user_arr_id]);
                get_All_2MarksByUserID(id_dt[elem.user_arr_id]);
                get_All_3MarksByUserID(id_dt[elem.user_arr_id]);
                get_All_4MarksByUserID(id_dt[elem.user_arr_id]);
                get_All_5MarksByUserID(id_dt[elem.user_arr_id]);
                PrimaryChartsConfig(i, get_All_1MarksByUserID(id_dt[i]), get_All_2MarksByUserID(id_dt[i]), get_All_3MarksByUserID(id_dt[i]), get_All_4MarksByUserID(id_dt[i]), get_All_5MarksByUserID(id_dt[i]));
            }
        }

        public void SecondaryCalcs(int j)
        {
            List<double> lst_average = new List<double>();
            List<string> lst_dates = new List<string>();

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_user_id[i] == users_dt[j])
                {
                    if (marks_isTwoMarks[i] == "True")
                    {
                        int a = int.Parse(marks_first_mark[i]);
                        int b = int.Parse(marks_second_mark[i]);

                        double avg = (double)(((double)a + (double)b) / 2);
                        lst_average.Add(avg);
                        lst_dates.Add(marks_date[i]);
                        //2 оценки
                    }
                    else
                    {
                        //1 оценка, нет среднего
                        lst_average.Add(double.Parse(marks_first_mark[i]));
                        lst_dates.Add(marks_date[i]);
                    }
                }
            }

            SecondaryChartsConfig(j, lst_average, lst_dates);
        }

        public void SecondaryCalcs()
        {

            List<double> lst_average = new List<double>();
            List<string> lst_dates = new List<string>();
            List<string> lst_userID = new List<string>();

            for (int i = 0; i < marks_idmarks.Count; i++)
            {
                if (marks_isTwoMarks[i] == "True")
                {
                    int a = int.Parse(marks_first_mark[i]);
                    int b = int.Parse(marks_second_mark[i]);

                    double avg = (double)(((double)a + (double)b) / 2);
                    lst_average.Add(avg);
                    DateTime dt = DateTime.Parse(marks_date[i]);
                    lst_dates.Add(dt.ToShortDateString());
                    lst_userID.Add(marks_user_id[i]);
                    //2 оценки
                }
                else
                {
                    //1 оценка, нет среднего
                    lst_average.Add(double.Parse(marks_first_mark[i]));
                    lst_dates.Add(marks_date[i]);
                    lst_userID.Add(marks_user_id[i]);
                }
            }

            List<double> lst_average2 = new List<double>();
            List<string> lst_dates2 = new List<string>();


            for (int i = 0; i < id_dt.Count; i++)
            {
                string userID = id_dt[i];
                lst_average2.Clear();
                lst_dates2.Clear();
                for (int z = 0; z < lst_userID.Count; z++)
                {
                    if (lst_userID[z] == userID)
                    {
                        lst_average2.Add(lst_average[z]);
                        lst_dates2.Add(lst_dates[z]);
                    }

                }
                SecondaryChartsConfig(int.Parse(userID), lst_average2, lst_dates2);

            }

        }

        public void SeparatorStep2(List<string> users_IDs, List<double> avgs, List<string> title_dates)
        {

        }

        #endregion

        public void topnumber_changed()
        {

        }
        private List<string> borderColors = new() { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };
        private Dictionary<int, PieChartListInteropModule> pieComponents = new Dictionary<int, PieChartListInteropModule>();
        private Dictionary<int, BarChartListInteropModule> barComponents = new Dictionary<int, BarChartListInteropModule>();
        public async Task LoadDataList()
        {
            svmp_main.Clear();
            pieComponents.Clear();
            barComponents.Clear();
            StateHasChanged();

            for (int i = 0; i < users_dt.Count; i++)
            {
                pieComponents.Add(i, new PieChartListInteropModule());
                barComponents.Add(i, new BarChartListInteropModule());
                StatisticsViewModel_Pie statisticsViewModel_Pie = new StatisticsViewModel_Pie();
                statisticsViewModel_Pie.user_arr_id = i;
                statisticsViewModel_Pie.user_fullname = fullname_dt[i];

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

                statisticsViewModel_Pie.pieOption = pieOptions;

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

                statisticsViewModel_Pie.barOption = barOptions;

                PieChart<int> pieRefs = new PieChart<int>();
                BarChart<int> barRefs = new BarChart<int>();

                statisticsViewModel_Pie.barRef = barRefs;
                statisticsViewModel_Pie.pieRef = pieRefs;

                LineChartOptions lineChartOptions = new Blazorise.Charts.LineChartOptions
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
                            Text = "Тренды среднего балла",
                        },

                    },
                    Parsing = new ChartParsing
                    {
                        XAxisKey = "num",
                        YAxisKey = "date",
                    },
                   
                };

                LineChartDataset<StatisticsLineData> _line_ch_dataset = new LineChartDataset<StatisticsLineData>();
                
                _line_ch_dataset.Data = new List<StatisticsLineData>();

                for (int z = 0; z < marks_idmarks.Count; z++)
                {
                    List<double> lst_average = new List<double>();
                    List<string> lst_dates = new List<string>();

                    if (marks_user_id[z] == id_dt[i])
                    {
                        if (marks_isTwoMarks[z] == "True")
                        {
                            int a = int.Parse(marks_first_mark[z]);
                            int b = int.Parse(marks_second_mark[z]);

                            double avg = (double)(((double)a + (double)b) / 2);
                            lst_average.Add(avg);
                            lst_dates.Add(marks_date[z]);
                            //2 оценки
                        }
                        else
                        {
                            //1 оценка, нет среднего
                            lst_average.Add(double.Parse(marks_first_mark[z]));
                            lst_dates.Add(marks_date[z]);
                        }
                    }
                    if (lst_average.Count > 0)
                    {
                        for (int azp = 0; azp < lst_average.Count; azp++)
                        {
                            StatisticsLineData sld = new StatisticsLineData();
                            sld.num = lst_average[azp];
                            sld.date_mark = DateTime.Parse(lst_dates[azp]).ToShortDateString() ;

                            _line_ch_dataset.Data.Add(sld);
                        };
                    }
                }

                statisticsViewModel_Pie._lineDatasetList = _line_ch_dataset;

                _line_ch_dataset.Stepped = true;
                statisticsViewModel_Pie._lineDatasetList2 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "before";
                statisticsViewModel_Pie._lineDatasetList3 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "after";
                statisticsViewModel_Pie._lineDatasetList4 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "middle";
                statisticsViewModel_Pie._lineDatasetList5 = _line_ch_dataset;

                statisticsViewModel_Pie.marks1 = getCountMarks(1, i);

                statisticsViewModel_Pie.marks2 = getCountMarks(2, i);

                statisticsViewModel_Pie.marks3 = getCountMarks(3, i);

                statisticsViewModel_Pie.marks4 = getCountMarks(4, i);

                statisticsViewModel_Pie.marks5 = getCountMarks(5, i);

                PieChartDataset<int> pieDatasetList = new PieChartDataset<int>();
                BarChartDataset<int> barDatasetList = new BarChartDataset<int>();

                double percentage1 = 0;
                double percentage2 = 0;
                double percentage3 = 0;
                double percentage4 = 0;
                double percentage5 = 0;


                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage1 = ((double)statisticsViewModel_Pie.marks1 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage1 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage2 = ((double)statisticsViewModel_Pie.marks2 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage2 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage3 = ((double)statisticsViewModel_Pie.marks3 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage3 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage4 = ((double)statisticsViewModel_Pie.marks4 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage4 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage5 = ((double)statisticsViewModel_Pie.marks5 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage5 = 0;
                }

                List<int> pieDatalistCommon = new List<int>();

                pieDatalistCommon.Add(statisticsViewModel_Pie.marks1);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks2);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks3);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks4);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks5);

                pieDatasetList = new PieChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                        ChartColor.FromRgba(255, 99, 132, 255), // Slice 1 aka "Red"

                        ChartColor.FromRgba(255, 205, 86, 255),
                        ChartColor.FromRgba(75, 192, 192, 255),
                        ChartColor.FromRgba(54, 162, 235, 255),
                        ChartColor.FromRgba(255, 255, 0, 255),

                    },
                    Label = "График оценок",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks1);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks2);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks3);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks4);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks5);


                barDatasetList = new BarChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                    ChartColor.FromRgba(255, 99, 132,255), // Slice 1 aka "Red"
                    
                    ChartColor.FromRgba(255, 205, 86,255),
                    ChartColor.FromRgba(75, 192, 192,255),
                    ChartColor.FromRgba(54, 162, 235,255),
                    ChartColor.FromRgba(255, 255, 0,255),

                    },
                    Label = "График оценок",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                barDatasetList.Data.Add(statisticsViewModel_Pie.marks1);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks2);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks3);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks4);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks5);

                statisticsViewModel_Pie.barDatasetList = barDatasetList;
                statisticsViewModel_Pie.pieDatasetList = pieDatasetList;
              
                List<string> labels = new List<string>();

                labels.Add("1 [ " + percentage1.ToString("N2") + "% ]");
                labels.Add("2 [ " + percentage2.ToString("N2") + "% ]");
                labels.Add("3 [ " + percentage3.ToString("N2") + "% ]");
                labels.Add("4 [ " + percentage4.ToString("N2") + "% ]");
                labels.Add("5 [ " + percentage5.ToString("N2") + "% ]");

                foreach (string marks in labels)
                {
                    //statisticsViewModel_Pie.pieRef.Data.Labels = new();
                    //statisticsViewModel_Pie.pieRef.Data.Labels.Add(marks);
                    //statisticsViewModel_Pie.barRef.Data.Labels = new();
                    //statisticsViewModel_Pie.barRef.Data.Labels.Add(marks);
                }

                string[] mark_labels = new[] { "1 [ " + percentage1.ToString("N2") + "% ]", "2 [ " + percentage2.ToString("N2") + "% ]", "3 [ " + percentage3.ToString("N2") + "% ]", "4 [ " + percentage4.ToString("N2") + "% ]", "5 [ " + percentage5.ToString("N2") + "% ]" };

                await statisticsViewModel_Pie.pieRef.AddLabelsDatasetsAndUpdate(mark_labels, statisticsViewModel_Pie.pieDatasetList);
                await statisticsViewModel_Pie.barRef.AddLabelsDatasetsAndUpdate(mark_labels, statisticsViewModel_Pie.barDatasetList);

                statisticsViewModel_Pie.pieDatasetLabels = mark_labels;
                statisticsViewModel_Pie.barDatasetLabels = mark_labels;

                await statisticsViewModel_Pie.line1.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList);
                await statisticsViewModel_Pie.line2.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList2);
                await statisticsViewModel_Pie.line3.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList3);
                await statisticsViewModel_Pie.line4.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList4);
                await statisticsViewModel_Pie.line5.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList5);

                //await statisticsViewModel_Pie.pieRef.Update();
                //await statisticsViewModel_Pie.barRef.Update();

                //await statisticsViewModel_Pie.line1.Update();
                //await statisticsViewModel_Pie.line2.Update();
                //await statisticsViewModel_Pie.line3.Update();
                //await statisticsViewModel_Pie.line4.Update();
                //await statisticsViewModel_Pie.line5.Update();

                statisticsViewModel_Pie.pieOption = pieOptions;
                statisticsViewModel_Pie.barOption = barOptions;

               
                svmp_main.Add(statisticsViewModel_Pie);
            }


            allowed_to_load_further = true;
          
            StateHasChanged();
            
        }


        public async Task LoadDataList(DateTime dt_start, DateTime dt_end)
        {
            svmp_main.Clear();
            pieComponents.Clear();
            barComponents.Clear();

            for (int i = 0; i < users_dt.Count; i++)
            {
                pieComponents.Add(i, new PieChartListInteropModule());
                barComponents.Add(i, new BarChartListInteropModule());
                StatisticsViewModel_Pie statisticsViewModel_Pie = new StatisticsViewModel_Pie();
                statisticsViewModel_Pie.user_arr_id = i;
                statisticsViewModel_Pie.user_fullname = fullname_dt[i];

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
                            Display = true,
                            Text = i.ToString(),
                        }
                    },
                };

                statisticsViewModel_Pie.pieOption = pieOptions;

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

                statisticsViewModel_Pie.barOption = barOptions;

                PieChart<int> pieRefs = new PieChart<int>();
                BarChart<int> barRefs = new BarChart<int>();

                statisticsViewModel_Pie.barRef = barRefs;
                statisticsViewModel_Pie.pieRef = pieRefs;

                LineChartOptions lineChartOptions = new Blazorise.Charts.LineChartOptions
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
                            Text = "Тренды среднего балла",
                        },

                    },
                    Parsing = new ChartParsing
                    {
                        XAxisKey = "num",
                        YAxisKey = "date",
                    },

                };

                LineChartDataset<StatisticsLineData> _line_ch_dataset = new LineChartDataset<StatisticsLineData>();

                _line_ch_dataset.Data = new List<StatisticsLineData>();

                for (int z = 0; z < marks_idmarks.Count; z++)
                {
                    DateTime mark_date = DateTime.Parse(marks_date[z]);
                    if (mark_date >= dt_start && mark_date <= dt_end)
                    { 
                    List<double> lst_average = new List<double>();
                    List<string> lst_dates = new List<string>();

                    if (marks_user_id[z] == id_dt[i])
                    {
                        if (marks_isTwoMarks[z] == "True")
                        {
                            int a = int.Parse(marks_first_mark[z]);
                            int b = int.Parse(marks_second_mark[z]);

                            double avg = (double)(((double)a + (double)b) / 2);
                            lst_average.Add(avg);
                            lst_dates.Add(marks_date[z]);
                            //2 оценки
                        }
                        else
                        {
                            //1 оценка, нет среднего
                            lst_average.Add(double.Parse(marks_first_mark[z]));
                            lst_dates.Add(marks_date[z]);
                        }
                    }
                    if (lst_average.Count > 0)
                    {
                        for (int azp = 0; azp < lst_average.Count; azp++)
                        {
                            StatisticsLineData sld = new StatisticsLineData();
                            sld.num = lst_average[azp];
                            sld.date_mark = DateTime.Parse(lst_dates[azp]).ToShortDateString();

                            _line_ch_dataset.Data.Add(sld);
                        };
                    }
                    }
                }

                _line_ch_dataset.Stepped = true;
                statisticsViewModel_Pie._lineDatasetList = _line_ch_dataset;

                _line_ch_dataset.Stepped = true;
                statisticsViewModel_Pie._lineDatasetList2 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "before";
                statisticsViewModel_Pie._lineDatasetList3 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "after";
                statisticsViewModel_Pie._lineDatasetList4 = _line_ch_dataset;

                _line_ch_dataset.Stepped = "middle";
                statisticsViewModel_Pie._lineDatasetList5 = _line_ch_dataset;

                statisticsViewModel_Pie.marks1 = getCountMarks(1, i,dt_start,dt_end);

                statisticsViewModel_Pie.marks2 = getCountMarks(2, i, dt_start, dt_end);

                statisticsViewModel_Pie.marks3 = getCountMarks(3, i, dt_start, dt_end);

                statisticsViewModel_Pie.marks4 = getCountMarks(4, i, dt_start, dt_end);

                statisticsViewModel_Pie.marks5 = getCountMarks(5, i, dt_start, dt_end);

                PieChartDataset<int> pieDatasetList = new PieChartDataset<int>();
                BarChartDataset<int> barDatasetList = new BarChartDataset<int>();

                double percentage1 = 0;
                double percentage2 = 0;
                double percentage3 = 0;
                double percentage4 = 0;
                double percentage5 = 0;


                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage1 = ((double)statisticsViewModel_Pie.marks1 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage1 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage2 = ((double)statisticsViewModel_Pie.marks2 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage2 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage3 = ((double)statisticsViewModel_Pie.marks3 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage3 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage4 = ((double)statisticsViewModel_Pie.marks4 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage4 = 0;
                }

                if ((statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) != 0)
                {
                    percentage5 = ((double)statisticsViewModel_Pie.marks5 / (statisticsViewModel_Pie.marks1 + statisticsViewModel_Pie.marks2 + statisticsViewModel_Pie.marks3 + statisticsViewModel_Pie.marks4 + statisticsViewModel_Pie.marks5) * 100);
                }
                else
                {
                    percentage5 = 0;
                }


                //foreach (string marks in new[] { "1 [ " + percentage1.ToString("N2") + "% ]", "2 [ " + percentage2.ToString("N2") + "% ]", "3 [ " + percentage3.ToString("N2") + "% ]", "4 [ " + percentage4.ToString("N2") + "% ]", "5 [ " + percentage5.ToString("N2") + "% ]" })
                //{
                //   svp_elem.set_stats_lables(marks);
                //  _config_template.Data.Labels.Add(marks);
                //}

                List<int> pieDatalistCommon = new List<int>();

                pieDatalistCommon.Add(statisticsViewModel_Pie.marks1);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks2);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks3);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks4);
                pieDatalistCommon.Add(statisticsViewModel_Pie.marks5);

                pieDatasetList = new PieChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                        ChartColor.FromRgba(255, 99, 132, 255), // Slice 1 aka "Red"

                        ChartColor.FromRgba(255, 205, 86, 255),
                        ChartColor.FromRgba(75, 192, 192, 255),
                        ChartColor.FromRgba(54, 162, 235, 255),
                        ChartColor.FromRgba(255, 255, 0, 255),

                    },
                    Label = "График оценок",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks1);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks2);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks3);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks4);
                pieDatasetList.Data.Add(statisticsViewModel_Pie.marks5);


                barDatasetList = new BarChartDataset<int>()
                {
                    Data = new List<int>(),
                    BackgroundColor = new List<ChartColor>()
                    {
                    ChartColor.FromRgba(255, 99, 132,255), // Slice 1 aka "Red"
                    
                    ChartColor.FromRgba(255, 205, 86,255),
                    ChartColor.FromRgba(75, 192, 192,255),
                    ChartColor.FromRgba(54, 162, 235,255),
                    ChartColor.FromRgba(255, 255, 0,255),

                    },
                    Label = "График оценок",
                    BorderColor = borderColors,
                    BorderWidth = 1
                };

                barDatasetList.Data.Add(statisticsViewModel_Pie.marks1);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks2);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks3);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks4);
                barDatasetList.Data.Add(statisticsViewModel_Pie.marks5);

                statisticsViewModel_Pie.barDatasetList = barDatasetList;
                statisticsViewModel_Pie.pieDatasetList = pieDatasetList;
               
                List<string> labels = new List<string>();

                labels.Add("1 [ " + percentage1.ToString("N2") + "% ]");
                labels.Add("2 [ " + percentage2.ToString("N2") + "% ]");
                labels.Add("3 [ " + percentage3.ToString("N2") + "% ]");
                labels.Add("4 [ " + percentage4.ToString("N2") + "% ]");
                labels.Add("5 [ " + percentage5.ToString("N2") + "% ]");

                foreach (string marks in labels)
                {
                    //statisticsViewModel_Pie.pieRef.Data.Labels = new();
                    //statisticsViewModel_Pie.pieRef.Data.Labels.Add(marks);
                    //statisticsViewModel_Pie.barRef.Data.Labels = new();
                    //statisticsViewModel_Pie.barRef.Data.Labels.Add(marks);
                }

                string[] mark_labels = new[] { "1 [ " + percentage1.ToString("N2") + "% ]", "2 [ " + percentage2.ToString("N2") + "% ]", "3 [ " + percentage3.ToString("N2") + "% ]", "4 [ " + percentage4.ToString("N2") + "% ]", "5 [ " + percentage5.ToString("N2") + "% ]" };

                await statisticsViewModel_Pie.pieRef.AddLabelsDatasetsAndUpdate(mark_labels, statisticsViewModel_Pie.pieDatasetList);
                await statisticsViewModel_Pie.barRef.AddLabelsDatasetsAndUpdate(mark_labels, statisticsViewModel_Pie.barDatasetList);

                statisticsViewModel_Pie.pieDatasetLabels = mark_labels;
                statisticsViewModel_Pie.barDatasetLabels = mark_labels;

                await statisticsViewModel_Pie.line1.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList);
                await statisticsViewModel_Pie.line2.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList2);
                await statisticsViewModel_Pie.line3.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList3);
                await statisticsViewModel_Pie.line4.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList4);
                await statisticsViewModel_Pie.line5.AddDatasetsAndUpdate(statisticsViewModel_Pie._lineDatasetList5);

                //await statisticsViewModel_Pie.pieRef.Update();
                //await statisticsViewModel_Pie.barRef.Update();

                //await statisticsViewModel_Pie.line1.Update();
                //await statisticsViewModel_Pie.line2.Update();
                //await statisticsViewModel_Pie.line3.Update();
                //await statisticsViewModel_Pie.line4.Update();
                //await statisticsViewModel_Pie.line5.Update();

                statisticsViewModel_Pie.pieOption = pieOptions;
                statisticsViewModel_Pie.barOption = barOptions;


                svmp_main.Add(statisticsViewModel_Pie);
            }


            allowed_to_load_further = true;

            StateHasChanged();

        }

        async Task PersonalUserChange(ChangeEventArgs cea)
        {

        }

        string _personal_users { get; set; }

        string personal_users { get; set; }

        string personal_users_k { get; set; }

        async Task PersonalUserChanger(ChangeEventArgs cea)
        {
            isPersonalStatVisible = false;
            StateHasChanged();
            personal_users_k = cea.Value.ToString();
            await Task.Delay(200);
            isPersonalStatVisible = true;
           
        }

        async Task ClickOverHandler()
        {
               _pclim_1.LoadExternalData( svmp_main[0].pieDatasetList,  svmp_main[0].pieDatasetLabels);
                _pclim_2.LoadExternalData( svmp_main[1].pieDatasetList,  svmp_main[1].pieDatasetLabels);
                _pclim_3.LoadExternalData( svmp_main[2].pieDatasetList,  svmp_main[2].pieDatasetLabels);
                _pclim_4.LoadExternalData( svmp_main[3].pieDatasetList,  svmp_main[3].pieDatasetLabels);
                _pclim_5.LoadExternalData( svmp_main[4].pieDatasetList,  svmp_main[4].pieDatasetLabels);

                //_bclim_1.LoadExternalData( svmp_main[0].barDatasetList,  svmp_main[0].barDatasetLabels);
                //_bclim_2.LoadExternalData( svmp_main[1].barDatasetList,  svmp_main[1].barDatasetLabels);
                //_bclim_3.LoadExternalData( svmp_main[2].barDatasetList,  svmp_main[2].barDatasetLabels);
                //_bclim_4.LoadExternalData( svmp_main[3].barDatasetList,  svmp_main[3].barDatasetLabels);
                //_bclim_5.LoadExternalData( svmp_main[4].barDatasetList,  svmp_main[4].barDatasetLabels);

        }

        LineChartListInteropModule _lclim_1 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_2 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_3 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_4 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_5 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_6 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_7 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_8 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_9 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_10 = new LineChartListInteropModule();

        LineChartListInteropModule _lclim_11 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_12 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_13 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_14 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_15 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_16 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_17 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_18 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_19 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_20 = new LineChartListInteropModule();

        LineChartListInteropModule _lclim_21 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_22 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_23 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_24 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_25 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_26 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_27 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_28 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_29 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_30 = new LineChartListInteropModule();

        LineChartListInteropModule _lclim_31 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_32 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_33 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_34 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_35 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_36 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_37 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_38 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_39 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_40 = new LineChartListInteropModule();

        LineChartListInteropModule _lclim_41 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_42 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_43 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_44 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_45 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_46 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_47 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_48 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_49 = new LineChartListInteropModule();
        LineChartListInteropModule _lclim_50 = new LineChartListInteropModule();

        PieChartListInteropModule _pclim_1 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_2 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_3 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_4 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_5 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_6 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_7 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_8 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_9 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_10 = new PieChartListInteropModule();

        PieChartListInteropModule _pclim_11 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_12 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_13 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_14 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_15 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_16 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_17 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_18 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_19 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_20 = new PieChartListInteropModule();

        PieChartListInteropModule _pclim_21 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_22 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_23 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_24 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_25 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_26 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_27 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_28 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_29 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_30 = new PieChartListInteropModule();

        PieChartListInteropModule _pclim_31 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_32 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_33 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_34 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_35 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_36 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_37 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_38 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_39 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_40 = new PieChartListInteropModule();

        PieChartListInteropModule _pclim_41 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_42 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_43 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_44 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_45 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_46 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_47 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_48 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_49 = new PieChartListInteropModule();
        PieChartListInteropModule _pclim_50 = new PieChartListInteropModule();

        BarChartListInteropModule _bclim_1 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_2 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_3 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_4 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_5 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_6 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_7 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_8 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_9 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_10 = new BarChartListInteropModule();

        BarChartListInteropModule _bclim_11 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_12 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_13 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_14 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_15 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_16 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_17 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_18 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_19 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_20 = new BarChartListInteropModule();

        BarChartListInteropModule _bclim_21 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_22 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_23 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_24 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_25 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_26 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_27 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_28 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_29 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_30 = new BarChartListInteropModule();

        BarChartListInteropModule _bclim_31 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_32 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_33 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_34 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_35 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_36 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_37 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_38 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_39 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_40 = new BarChartListInteropModule();

        BarChartListInteropModule _bclim_41 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_42 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_43 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_44 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_45 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_46 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_47 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_48 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_49 = new BarChartListInteropModule();
        BarChartListInteropModule _bclim_50 = new BarChartListInteropModule();

        public int getCountMarks(int eval_mark, int i)
        {
            int counts = 0;


            for (int z = 0; z < marks_idmarks.Count; z++)
            {
                List<double> lst_average = new List<double>();
                List<string> lst_dates = new List<string>();

                if (marks_user_id[z] == id_dt[i])
                {
                    if (marks_isTwoMarks[z] == "True")
                    {
                        int a = int.Parse(marks_first_mark[z]);
                        int b = int.Parse(marks_second_mark[z]);

                        if (a == eval_mark)
                        {
                            counts++;
                        }

                        if (b == eval_mark)
                        {
                            counts++;
                        }

                    }
                    else
                    {
                        int a = int.Parse(marks_first_mark[z]);
                        //1 оценка, нет среднего
                        if (a == eval_mark)
                        {
                            counts++;
                        }
                    }
                }

            }
            return counts;
        }

        public int getCountMarks(int eval_mark, int i,DateTime dt_start, DateTime dt_end)
        {
            int counts = 0;


            for (int z = 0; z < marks_idmarks.Count; z++)
            {
                DateTime mark_date = DateTime.Parse(marks_date[z]);

                if (mark_date >= dt_start && mark_date <= dt_end)
                { 
                List<double> lst_average = new List<double>();
                List<string> lst_dates = new List<string>();

                if (marks_user_id[z] == id_dt[i])
                {
                    if (marks_isTwoMarks[z] == "True")
                    {
                        int a = int.Parse(marks_first_mark[z]);
                        int b = int.Parse(marks_second_mark[z]);

                        if (a == eval_mark)
                        {
                            counts++;
                        }

                        if (b == eval_mark)
                        {
                            counts++;
                        }

                    }
                    else
                    {
                        int a = int.Parse(marks_first_mark[z]);
                        //1 оценка, нет среднего
                        if (a == eval_mark)
                        {
                            counts++;
                        }
                    }
                }

            }
            
            }
            return counts;
        }
        bool end_loop { get; set; }
        protected async override Task OnInitializedAsync()
        {

        }

        public void DoCalcs()
        {
            for (int i = 0; i < svmp_main.Count; i++)
            {
                pieComponents[i].LoadExternalData(svmp_main[i].pieDatasetList, svmp_main[i].pieDatasetLabels);
                barComponents[i].LoadExternalData(svmp_main[i].barDatasetList, svmp_main[i].barDatasetLabels);
            }
        }

        List<PieChartOptions> pieChartOptionsCopied = new List<PieChartOptions>();
        List<BarChartOptions> barChartOptionsCopied = new List<BarChartOptions>();
       bool allowed_to_load_further {get;set;}
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           if (firstRender)
            {
                dt_start = DateTime.Now;
                dt_end = DateTime.Now;

                allowed_to_load_further = false;
                personal = false;
                avg_company = 0;
                academic_perfomace_company = 0;
                perfomance_company = 0;
                quality_of_work_company = 0;

                users_dt = await mysqlc.getListUsersLoginAsync();
                email_dt = await mysqlc.getListUsersEmailAsync();
                passwords_dt = await mysqlc.getListUsersPasswordAsync();
                id_dt = await mysqlc.getListUsersIDAsync();
                fullname_dt = await mysqlc.getListUsersFullnameAsync();
                position_dt = await mysqlc.getListUsersPositionsAsync();
                skills_dt = await mysqlc.getListUsersSkills();
                avatarpath_dt = mysqlc.getAvatarPaths(users_dt);
                projects_val_dt = await mysqlc.getListUsersProjectsAsync();

                marks_idmarks = await mysqlc.getListMarksIDAsync();
                marks_user_id = await mysqlc.getListMarksUserIDAsync();
                marks_date = await mysqlc.getListMarksDateAsync();
                marks_first_mark = await mysqlc.getListMarksFirstMarkAsync();
                marks_second_mark = await mysqlc.getListMarksSecondMarkAsync();
                marks_first_description = await mysqlc.getListMarksFirstDescriptionAsync();
                marks_second_description = await mysqlc.getListMarksSecondDescriptionAsync();
                marks_isTwoMarks = await mysqlc.getListMarksIsTwoMarksAsync();


                int p = 0;
                if (users_dt != null)
                {
                    p = users_dt.Count;
                }

                for (int i = 0; i < id_dt.Count; i++)
                {

                }

                InterpolationCatsString = "enabled";
                isOnlyWithMarks = false;
                isTop = false;

                //cum_dt.Add(cum);
                num++;
                //PrimaryCalcs();
                //SecondaryCalcs();


                isModalPieOpen = false;
                modalAllow = false;

                TopNumber = id_dt.Count;

                pageLoaded = true;

                isPie = true;

                dt_end = DateTime.Now;
                dt_start = DateTime.Now;
                await LoadDataList();


                for (int azi = 0; azi < svmp_main.Count; azi++)
                {
                    //pieChartOptionsCopied.Add(svmp_main[azi].pieOption);
                    //barChartOptionsCopied.Add(svmp_main[azi].barOption);

                    svmp_main[azi].barRef.AddLabelsDatasetsAndUpdate(svmp_main[azi].barDatasetLabels, svmp_main[azi].barDatasetList);
                    svmp_main[azi].pieRef.AddLabelsDatasetsAndUpdate(svmp_main[azi].pieDatasetLabels, svmp_main[azi].pieDatasetList);
  
                }

                getCompanyEvaluations();

                ready = true;

                //await barChart.Clear();
                //await barChart.AddLabelsDatasetsAndUpdate(Labels, GetBarChartDataset());

                end_loop = false;

                personal_users = "0";
                personal_users_k = "0";

                StatWindowString = StatWindow.common.ToString();
                StatWindowString2 = StatWindow.personal.ToString();

                isPersonalStatVisible = true;

                StateHasChanged();
            }
        }
        
        private string[] Labels = { "Red", "Blue", "Yellow", "Green", "Purple", "Orange" };
        ChartOptions chartOptions = new()
        {
            AspectRatio = 1.5
        };
        private Chart<double> barChart;
        List<double> RandomizeData(int min, int max)
        {
            return Enumerable.Range(0, 6).Select(x => random.Next(min, max) * random.NextDouble()).ToList();
        }
        private List<string> backgroundColors = new() { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

        private Random random = new(DateTime.Now.Millisecond);
        private BarChartDataset<double> GetBarChartDataset()
        {
            return new()
            {
                Label = "# of randoms",
                Data = RandomizeData(1,50),
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                BorderWidth = 1
            };
        }

        //private PieConfig modal_pie_config = new PieConfig();

        //private List<PieConfig> _config = new List<PieConfig>();

        public bool ready = false;

        public void StatWindowStringChanger(ChangeEventArgs cea)
        {
            if (cea.Value.ToString() == StatWindow.common.ToString())
            {
                personal = false;
                StatWindowString2 = StatWindow.common.ToString();
            }
            else
            {
                personal = true;
                StatWindowString2 = StatWindow.personal.ToString();
            }
            StateHasChanged();
        }

        bool isPersonalStatVisible { get; set; } 

        protected override void OnInitialized()
        {
            svp_index = 0;
        }

        public void SecondaryChartsConfig(int i, List<double> AverageByMonthCollection, List<string> MonthCollection)
        {
            //ChartJs.Blazor.LineChart.LineConfig _config_template = new ChartJs.Blazor.LineChart.LineConfig();
            //ChartJs.Blazor.LineChart.LineConfig _config_template1 = new ChartJs.Blazor.LineChart.LineConfig();
            //ChartJs.Blazor.LineChart.LineConfig _config_template2 = new ChartJs.Blazor.LineChart.LineConfig();
            //ChartJs.Blazor.LineChart.LineConfig _config_template3 = new ChartJs.Blazor.LineChart.LineConfig();
            //ChartJs.Blazor.LineChart.LineConfig _config_template4 = new ChartJs.Blazor.LineChart.LineConfig();
            //ChartJs.Blazor.LineChart.LineConfig _config_template5 = new ChartJs.Blazor.LineChart.LineConfig();

            //_config_template = new LineConfig
            //{
            //    Options = new LineOptions
            //    {

            //        Responsive = true,
            //        Legend = new Legend
            //        {
            //            Display = false,
            //        },
            //        Title = new OptionsTitle
            //        {
            //            Display = false,
            //            Text = "Тренды среднего балла",
            //        }

            //    }
            //};

            //foreach (string dates in MonthCollection)
            //{
            //    _config_template.Data.Labels.Add(dates);

            //}
            //LineDataset<double> dataset = new LineDataset<double>();


            //foreach (double a in AverageByMonthCollection)
            //{
            //    dataset.Add(a);
            //}

            //_config_template5 = _config_template;
            //_config_template4 = _config_template;
            //_config_template3 = _config_template;
            //_config_template2 = _config_template;
            //_config_template1 = _config_template;

            //dataset.BackgroundColor = ColorUtil.ColorHexString(54, 162, 235);
            //dataset.SteppedLine = SteppedLine.False;
            //_config_template1.Data.Datasets.Add(dataset);
            //_line1.Add(_config_template1);

            //dataset.BackgroundColor = ColorUtil.ColorHexString(0, 0, 255);
            //dataset.SteppedLine = SteppedLine.True;
            //_config_template2.Data.Datasets.Add(dataset);
            //_line2.Add(_config_template2);

            //dataset.BackgroundColor = ColorUtil.ColorHexString(0, 102, 204);
            //dataset.SteppedLine = SteppedLine.Before;
            //_config_template3.Data.Datasets.Add(dataset);
            //_line3.Add(_config_template3);

            //dataset.BackgroundColor = ColorUtil.ColorHexString(51, 153, 102);
            //dataset.SteppedLine = SteppedLine.After;
            //_config_template4.Data.Datasets.Add(dataset);
            //_line4.Add(_config_template4);

            //dataset.BackgroundColor = ColorUtil.ColorHexString(204, 255, 204);
            //dataset.SteppedLine = SteppedLine.Middle;
            //_config_template5.Data.Datasets.Add(dataset);
            //_line5.Add(_config_template5);

        }

        public void PrimaryChartsConfig(int i, int marks_1, int marks_2, int marks_3, int marks_4, int marks_5)
        {
        //    pcc_cnt++;
        //    svp_elem = new StatisticsView_Pie();
    
        //    PieConfig _config_template = new PieConfig();


        //    _config_template = new PieConfig
        //    {
        //        Options = new PieOptions
        //        {
        //            OnClick = new DelegateHandler<ChartMouseEvent>(OnClickHandler),
        //            Responsive = true,
        //            Legend = new Legend
        //            {
        //                Display = false,
        //            },
        //            Title = new OptionsTitle
        //            {
        //                Display = false,
        //                Text = i.ToString(),
        //            }
        //        }
        //    };


        //    double percentage1 = 0;
        //    double percentage2 = 0;
        //    double percentage3 = 0;
        //    double percentage4 = 0;
        //    double percentage5 = 0;


        //    if ((marks_1 + marks_2 + marks_3 + marks_4 + marks_5) != 0)
        //    {
        //        percentage1 = ((double)marks_1 / (marks_1 + marks_2 + marks_3 + marks_4 + marks_5) * 100);
        //    }
        //    else
        //    {
        //        percentage1 = 0;
        //    }

        //    if ((marks_1 + marks_2 + marks_3 + marks_4 + marks_5) != 0)
        //    {
        //        percentage2 = ((double)marks_2 / (marks_1 + marks_2 + marks_3 + marks_4 + marks_5) * 100);
        //    }
        //    else
        //    {
        //        percentage2 = 0;
        //    }

        //    if ((marks_1 + marks_2 + marks_3 + marks_4 + marks_5) != 0)
        //    {
        //        percentage3 = ((double)marks_3 / (marks_1 + marks_2 + marks_3 + marks_4 + marks_5) * 100);
        //    }
        //    else
        //    {
        //        percentage3 = 0;
        //    }

        //    if ((marks_1 + marks_2 + marks_3 + marks_4 + marks_5) != 0)
        //    {
        //        percentage4 = ((double)marks_4 / (marks_1 + marks_2 + marks_3 + marks_4 + marks_5) * 100);
        //    }
        //    else
        //    {
        //        percentage4 = 0;
        //    }

        //    if ((marks_1 + marks_2 + marks_3 + marks_4 + marks_5) != 0)
        //    {
        //        percentage5 = 100 - percentage4 - percentage3 - percentage2 - percentage1;
        //    }
        //    else
        //    {
        //        percentage5 = 0;
        //    }


        //    foreach (string marks in new[] { "1 [ " + percentage1.ToString("N2") + "% ]", "2 [ " + percentage2.ToString("N2") + "% ]", "3 [ " + percentage3.ToString("N2") + "% ]", "4 [ " + percentage4.ToString("N2") + "% ]", "5 [ " + percentage5.ToString("N2") + "% ]" })
        //    {
        //        svp_elem.set_stats_lables(marks);
        //        _config_template.Data.Labels.Add(marks);
        //    }


        //    PieDataset<int> dataset = new PieDataset<int>(new[] { marks_1, marks_2, marks_3, marks_4, marks_5 })
        //    {
        //        BackgroundColor = new[]
        //    {
        //    ColorUtil.ColorHexString(255, 99, 132), // Slice 1 aka "Red"
        //    ColorUtil.ColorHexString(255, 205, 86), // Slice 2 aka "Yellow"
        //    ColorUtil.ColorHexString(75, 192, 192), // Slice 3 aka "Green"
        //    ColorUtil.ColorHexString(54, 162, 235), // Slice 4 aka "Blue"
        //    ColorUtil.ColorString(255,255,0),
        //}
        //    };
        //    svp_elem.set_dataset_lst(dataset);
        //    _config_template.Data.Datasets.Add(dataset);
        //    _config.Add(_config_template);
        //    svp_elem.set_modal_pie_config(_config_template);
        //    svp.Add(svp_elem);
        }

        public static string modal_static { get; set; }

        public void ModalShow(string clear_ID)
        {
            if (modalAllow)
            {

                svp_index = int.Parse(clear_ID);

                modal_user = fullname_dt[int.Parse(clear_ID)];
                OpenModalPieBarDialog();
            }
        }
      
}
}
