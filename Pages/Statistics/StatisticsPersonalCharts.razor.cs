using Blazorise.Charts;
using EviCRM.Server.Models.Statistics;
using EviCRM.Server.Pages.Charts.ChartListInterop;
using Microsoft.AspNetCore.Components;

namespace EviCRM.Server.Pages.Statistics
{
    public partial class StatisticsPersonalCharts
{

   public bool isReady { get; set; }

    ChartOptions chartOption = new()
    {
        Plugins = new ChartPlugins()
        {
            Legend = new ChartLegend()
            {
                Display = false,
            },
        },
        AspectRatio = 1.5,
    };



    PieChartDataset<int> pieDatasetList = new PieChartDataset<int>();
    BarChartDataset<int> barDatasetList = new BarChartDataset<int>();


    string[] pieDatasetLabels { get; set; }
    string[] barDatasetLabels { get; set; }

    PieChartListInteropModule _pclim = new PieChartListInteropModule();
    BarChartListInteropModule _bclim = new BarChartListInteropModule();
    LineChartListInteropModule _lclim = new LineChartListInteropModule();

    [Parameter]
    public List<int> ChartDatasetList { get; set; }

        public enum InterpolationCats
        {
            disabled,
            enabled,
            stepbefore,
            stepafter,
            stepmiddle
        };

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
            _lclim.UpdateInterpolationCase(InterpolationCatsString);
            await InvokeAsync(StateHasChanged);
        }

        [Parameter]
    public List<string> lst_id_dt { get; set; }

        [Parameter]
        public List<string> lst_fullname_dt { get; set; }

        [Parameter]
        public List<string> lst_marks_date { get; set; }

        [Parameter]
        public List<string> lst_marks_first_mark { get; set; }


        [Parameter]
        public List<string> lst_id_marks { get; set; }

        [Parameter]
        public List<string> lst_marks_user_id { get; set; }

        [Parameter]
        public List<string> lst_marks_second_mark { get; set; }

        [Parameter]
        public List<string> lst_user_dt { get; set; }

        [Parameter]
        public List<string> lst_marks_isTwoMarks { get; set; }
        
        [Parameter]
        public string current_user_id { get; set; }

        DateTime dt_start { get; set; }
    DateTime dt_start_sys { get; set; }

    DateTime dt_end { get; set; }
    DateTime dt_end_sys { get; set; }


        #region Chart Generator
        double avg_company { get; set; }

        double academic_perfomace_company { get; set; }

        double quality_of_work_company { get; set; }

        double perfomance_company { get; set; }

        double avg_personal { get; set; }

        double academic_perfomace_personal { get; set; }

        double quality_of_work_personal { get; set; }

        double perfomance_personal { get; set; }

        int personal_top_place { get; set; }


        private Dictionary<int, PieChartListInteropModule> pieComponents = new Dictionary<int, PieChartListInteropModule>();
        private Dictionary<int, BarChartListInteropModule> barComponents = new Dictionary<int, BarChartListInteropModule>();

        public List<StatisticsViewModel_Pie> svmp_main = new List<StatisticsViewModel_Pie>();



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

        async Task<double> getPersonalEvaluations(StatisticsViewModel_Pie svmp)
        {

            //academic_perfomace_company = 0.0;
            //avg_company = 0.0;
            //quality_of_work_company = 0.0;
            //perfomance_company = 0.0;

            double personal_avgs = 0;

            double diff_ch = svmp.marks1 + svmp.marks2 + svmp.marks3 + svmp.marks4 + svmp.marks5;
            if (diff_ch != 0)
            {
                personal_avgs = (double)(svmp.marks1 * 1 + svmp.marks2 * 2 + svmp.marks3 * 3 + svmp.marks4 * 4 + svmp.marks5 * 5) / (diff_ch);
            }
            avg_personal = personal_avgs;


            double t_v = 0;

            t_v = svmp.marks3 + svmp.marks4 + svmp.marks5;

            if (diff_ch != 0)
            {
                t_v = (double)(t_v) /diff_ch;
            }
            else
            {
                t_v = 0;
            }
            
            academic_perfomace_personal = t_v;

            t_v = 0;

            t_v = svmp.marks4 + svmp.marks5;

            if (diff_ch != 0)
            {
                t_v = (double)(t_v) / diff_ch;
            }
            else
            {
                t_v = 0;
            }

            quality_of_work_personal = t_v;

            t_v = 0;
            
            t_v += svmp.marks5 + svmp.marks4 * 0.64 + svmp.marks3 * 0.36 + svmp.marks2 * 0.16 + svmp.marks1 * 0.08;
            
            if (diff_ch != 0)
            {
                t_v = (double)t_v / diff_ch;
            }
            

            perfomance_personal = t_v;

            List<double> avgs = new List<double>();
            double avg = 0;
            foreach (var elem in svmp_main)
            {
                if (!(elem.marks1 == 0 && elem.marks2 == 0 && elem.marks3 == 0 && elem.marks4 == 0 && elem.marks5 == 0))
                {
                    avgs.Add(Double.Parse(elem.getAverage()));
                }
            }

            var args_sorted = from s in avgs orderby avgs descending select s;

            List<StatisticsViewModel_Pie> sorted_lst = new List<StatisticsViewModel_Pie>();

            foreach(var elem in svmp_main)
            {
                sorted_lst.Add(elem);
            }
            
            sorted_lst = SortByDecreaseAvgValue(sorted_lst);

            personal_top_place = getTopPositionByValue(sorted_lst, int.Parse(current_user_id));

            StateHasChanged();
            return 0;
        }

        public List<StatisticsViewModel_Pie> SortByIncreaseAvgValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderBy(x => x.getAverage(true)).ThenBy(x => x.user_fullname).ToList();
        }
        public List<StatisticsViewModel_Pie> SortByDecreaseAvgValue(List<StatisticsViewModel_Pie> originalList)
        {
            return originalList.OrderByDescending(x => x.getAverage(true)).ThenBy(x => x.user_fullname).ToList();
        }

        bool isForwardOrder = true;

        public void ModelSort()
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
                svmp_main = SortByIncreaseAvgValue(svmp_main_dubler);
            }
            else
            {
                svmp_main = SortByDecreaseAvgValue(svmp_main_dubler);
            }
            StateHasChanged();
        }

        int getTopPositionByValue(List<StatisticsViewModel_Pie> sorted_lst,int user_arr_id)
        {
            int zp = 0;
            for (int p = 0; p< sorted_lst.Count; p++)
            {
               if (sorted_lst[p].user_arr_id == user_arr_id)
                {
                    return (p+1);
                }
            }
            return zp;
        }

        int get_All_1MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < lst_id_marks.Count; i++)
            {
                if (lst_marks_first_mark[i] == "1" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (lst_marks_second_mark[i] == "1" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_2MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < lst_id_marks.Count; i++)
            {
                if (lst_marks_first_mark[i] == "2" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (lst_marks_second_mark[i] == "2" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_3MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < lst_id_marks.Count; i++)
            {
                if (lst_marks_first_mark[i] == "3" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (lst_marks_second_mark[i] == "3" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_4MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < lst_id_marks.Count; i++)
            {
                if (lst_marks_first_mark[i] == "4" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (lst_marks_second_mark[i] == "4" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;
        }

        int get_All_5MarksByUserID(string userID)
        {
            int marks = 0;

            for (int i = 0; i < lst_id_marks.Count; i++)
            {
                if (lst_marks_first_mark[i] == "5" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
                if (lst_marks_second_mark[i] == "5" && lst_marks_user_id[i] == userID)
                {
                    marks++;
                }
            }
            return marks;

            double getAverage(int i, bool isOverload)
            {
                double a = 0;

                int one = get_All_1MarksByUserID(lst_id_dt[i]);
                int two = get_All_2MarksByUserID(lst_id_dt[i]);
                int three = get_All_3MarksByUserID(lst_id_dt[i]);
                int four = get_All_4MarksByUserID(lst_id_dt[i]);
                int five = get_All_5MarksByUserID(lst_id_dt[i]);

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

           
        }
           

        public enum LoadingSource
    {
        IndexPage,
        StatisticsPage,
    }

    [Parameter]
    public LoadingSource loaded_from { get; set; }

    bool isPie { get; set; }

    string table_activity_page_workdaily { get; set; }

        string table_activity_page_workdaily_duration { get; set; }


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

    public async  Task DateStartChangeHandler(ChangeEventArgs cea)
    {
        dt_start_sys = DateTime.Parse(cea.Value.ToString());

            if (dt_start_sys.Date == DateTime.Now.Date && dt_end_sys.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start_sys, dt_end_sys);
            }

            // _pclim.ReDrawHandler();
            //_bclim.ReDrawHandler();
            await ReDrawHandler();
            
            StateHasChanged();
            
    }

    public async Task ReDrawHandler()
        {
            isPie = !isPie;
            StateHasChanged();
            await Task.Delay(100);
            isPie = !isPie;
            StateHasChanged();
        }

    public async Task DateEndChangeHandler(ChangeEventArgs cea)
    {
        dt_end_sys = DateTime.Parse(cea.Value.ToString());

            if (dt_start_sys.Date == DateTime.Now.Date && dt_end_sys.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start_sys, dt_end_sys);
            }
            StateHasChanged();
    }


        public string getUserArrayIDByUsername(string username)
        {
            for (int i = 0; i < lst_id_dt.Count; i++)
            {
                if (lst_user_dt[i] == username)
                {
                    return i.ToString();
                }
            }

            return "";
        }

        public int getUserArrayIDByUsername(string username, bool isIntegerRequired)
        {
            for (int i = 0; i < lst_id_dt.Count; i++)
            {
                if (lst_user_dt[i] == username)
                {
                    return i;
                }
            }

            return 0;
        }

        public double avg_diff(double avg_personal, double avg_company)
        {
            double dbl = 0.0;
            
            if (avg_personal > avg_company && avg_company != 0)
            {
                dbl = (avg_personal - avg_company) / avg_company * 100;
            }
            else
            {
                dbl = (avg_company - avg_personal) / avg_company * 100;
            }

            return dbl;
        }



        protected async override Task OnInitializedAsync()
    {
        isReady = false;
        
        dt_start = DateTime.Now;
        dt_end = DateTime.Now;

        dt_start_sys = DateTime.Now;
        dt_end_sys = DateTime.Now;


            //current_user_id = getUserArrayIDByUsername(current_user_id);


            if (dt_start_sys.Date == DateTime.Now.Date && dt_end_sys.Date == DateTime.Now.Date)
            {
                await LoadDataList();
            }
            else
            {
                await LoadDataList(dt_start_sys, dt_end_sys);
            }
            getCompanyEvaluations();

            int svmp_arr_num = getSVMP_ElementNumber();

            getPersonalEvaluations(svmp_main[svmp_arr_num]);


            isPie = true;
            isReady = true;

        InvokeAsync(StateHasChanged);
    }
    private List<string> backgroundColors = new() { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
    private List<string> borderColors = new() { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };


        public int getSVMP_ElementNumber()
        {
            int number = 0;

           for (int j = 0; j< svmp_main.Count;j++)
            {
                if (svmp_main[j].user_arr_id.ToString() == current_user_id)
                {
                    return j;
                }
            }
            return number;
        }
    private BarChartDataset<int> GetBarChartDataset()
    {
        return new()
        {
            Label = " таких оценок",
            //Data = ChartDataSetList,
            BackgroundColor = backgroundColors,
            BorderColor = borderColors,
            BorderWidth = 1
        };
    }

        public async Task LoadDataList(DateTime dt_start, DateTime dt_end)
        {
            svmp_main.Clear();
            pieComponents.Clear();
            barComponents.Clear();

            for (int i = 0; i < lst_user_dt.Count; i++)
            {
                pieComponents.Add(i, new PieChartListInteropModule());
                barComponents.Add(i, new BarChartListInteropModule());
                StatisticsViewModel_Pie statisticsViewModel_Pie = new StatisticsViewModel_Pie();
                statisticsViewModel_Pie.user_arr_id = i;
                statisticsViewModel_Pie.user_fullname = lst_fullname_dt[i];

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

                for (int z = 0; z < lst_id_marks.Count; z++)
                {
                    DateTime mark_date = DateTime.Parse(lst_marks_date[z]);
                    if (mark_date >= dt_start && mark_date <= dt_end)
                    {
                        List<double> lst_average = new List<double>();
                        List<string> lst_dates = new List<string>();

                        if (lst_marks_user_id[z] == lst_id_dt[i])
                        {
                            if (lst_marks_isTwoMarks[z] == "True")
                            {
                                int a = int.Parse(lst_marks_first_mark[z]);
                                int b = int.Parse(lst_marks_second_mark[z]);

                                double avg = (double)(((double)a + (double)b) / 2);
                                lst_average.Add(avg);
                                lst_dates.Add(lst_marks_date[z]);
                                //2 оценки
                            }
                            else
                            {
                                //1 оценка, нет среднего
                                lst_average.Add(double.Parse(lst_marks_first_mark[z]));
                                lst_dates.Add(lst_marks_date[z]);
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

                statisticsViewModel_Pie.marks1 = getCountMarks(1, i, dt_start, dt_end);

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
                //    };
                //    svp_elem.set_dataset_lst(dataset);
                //    _config_template.Data.Datasets.Add(dataset);
                //    _config.Add(_config_template);
                //    svp_elem.set_modal_pie_config(_config_template);
                //    svp.Add(svp_elem);



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



            StateHasChanged();

        }


        public async Task LoadDataList()
        {
            svmp_main.Clear();
            pieComponents.Clear();
            barComponents.Clear();
            StateHasChanged();

            for (int i = 0; i < lst_user_dt.Count; i++)
            {
                pieComponents.Add(i, new PieChartListInteropModule());
                barComponents.Add(i, new BarChartListInteropModule());
                StatisticsViewModel_Pie statisticsViewModel_Pie = new StatisticsViewModel_Pie();
                statisticsViewModel_Pie.user_arr_id = i;
                statisticsViewModel_Pie.user_fullname = lst_fullname_dt[i];

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

                for (int z = 0; z < lst_id_marks.Count; z++)
                {
                    List<double> lst_average = new List<double>();
                    List<string> lst_dates = new List<string>();

                    if (lst_marks_user_id[z] == lst_id_dt[i])
                    {
                        if (lst_marks_isTwoMarks[z] == "True")
                        {
                            int a = int.Parse(lst_marks_first_mark[z]);
                            int b = int.Parse(lst_marks_second_mark[z]);

                            double avg = (double)(((double)a + (double)b) / 2);
                            lst_average.Add(avg);
                            lst_dates.Add(lst_marks_date[z]);
                            //2 оценки
                        }
                        else
                        {
                            //1 оценка, нет среднего
                            lst_average.Add(double.Parse(lst_marks_first_mark[z]));
                            lst_dates.Add(lst_marks_date[z]);
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
                //    };
                //    svp_elem.set_dataset_lst(dataset);
                //    _config_template.Data.Datasets.Add(dataset);
                //    _config.Add(_config_template);
                //    svp_elem.set_modal_pie_config(_config_template);
                //    svp.Add(svp_elem);



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


            //allowed_to_load_further = true;

            StateHasChanged();

        }

        public int getCountMarks(int eval_mark, int i)
        {
            int counts = 0;


            for (int z = 0; z < lst_id_marks.Count; z++)
            {
                List<double> lst_average = new List<double>();
                List<string> lst_dates = new List<string>();

                if (lst_marks_user_id[z] == lst_id_dt[i])
                {
                    if (lst_marks_isTwoMarks[z] == "True")
                    {
                        int a = int.Parse(lst_marks_first_mark[z]);
                        int b = int.Parse(lst_marks_second_mark[z]);

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
                        int a = int.Parse(lst_marks_first_mark[z]);
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



        int getCountMarks(int eval_mark, int i, DateTime dt_start, DateTime dt_end)
    {
        int counts = 0;


        for (int z = 0; z < lst_id_marks.Count; z++)
        {
            DateTime mark_date = DateTime.Parse(lst_marks_date[z]);

            if (mark_date >= dt_start && mark_date <= dt_end)
            {
                List<double> lst_average = new List<double>();
                List<string> lst_dates = new List<string>();

                if (lst_marks_user_id[z] == lst_id_dt[i])
                {
                    if (lst_marks_isTwoMarks[z] == "True")
                    {
                        int a = int.Parse(lst_marks_first_mark[z]);
                        int b = int.Parse(lst_marks_second_mark[z]);

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
                        int a = int.Parse(lst_marks_first_mark[z]);
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

    #endregion

    ChartOptions chartOptions = new()
    {
        Plugins = new ChartPlugins()
        {
            Legend = new ChartLegend()
            {
                Display = false,
            },
        },
        AspectRatio = 1.5,
    };






    }
}