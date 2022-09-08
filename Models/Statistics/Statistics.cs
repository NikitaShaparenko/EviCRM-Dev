using Blazorise.Charts;

namespace EviCRM.Server.Models.Statistics
{

    public class StatisticsChartElement
    {

        public PieChartDataset<int> pieDatasetList { get; set; }
        public BarChartDataset<int> barDatasetList { get; set; }

        public PieChartDataset<int> personal_pieDatasetList { get; set; }
        public BarChartDataset<int> personal_barDatasetList { get; set; }

        public ChartOptions pieOption { get; set; }
        public ChartOptions barOption { get; set; }

        public string[] pieDatasetLabels { get; set; }

        public string[] barDatasetLabels { get; set; }

        public string[] personal_pieDatasetLabels { get; set; }

        public string[] personal_barDatasetLabels { get; set; }

        public int user_arr_id { get; set; }

        public string user_fullname { get; set; }


        #region Tasks Statuses
        public int tasks_status_waiting { get; set; }
        public int tasks_status_approved { get; set; }
        public int tasks_status_pending { get; set; }
        public int tasks_status_delayed { get; set; }
        public int tasks_status_completed { get; set; }
        public int tasks_status_canceled { get; set; }
        public int tasks_status_failed { get; set; }

        #endregion

        #region Tasks Personal Statuses
        public int tasks_personal_status_waiting { get; set; }
        public int tasks_personal_status_approved { get; set; }
        public int tasks_personal_status_pending { get; set; }
        public int tasks_personal_status_delayed { get; set; }
        public int tasks_personal_status_completed { get; set; }
        public int tasks_personal_status_canceled { get; set; }
        public int tasks_personal_status_failed { get; set; }

        public int tasks_personal_status_creater { get; set; }

        #endregion


        public StatisticsChartElement()
        {
            pieDatasetList = new PieChartDataset<int>();
            barDatasetList = new BarChartDataset<int>();
            pieOption = new ChartOptions();
            barOption = new ChartOptions();

              tasks_personal_status_waiting = 0;
         tasks_personal_status_approved = 0;
         tasks_personal_status_pending = 0;
         tasks_personal_status_delayed = 0;
         tasks_personal_status_completed = 0;
         tasks_personal_status_canceled = 0;
         tasks_personal_status_failed = 0;
            tasks_personal_status_creater = 0;

            tasks_status_waiting = 0;
            tasks_status_approved = 0;
            tasks_status_pending = 0;
            tasks_status_delayed = 0;
            tasks_status_completed = 0;
            tasks_status_canceled = 0;
            tasks_status_failed = 0;

        }

        public double get_totaltasks_Mark()
        {
            double mark = 0.0;

            mark += tasks_status_waiting * 1;
            mark += tasks_status_approved * 1.5;
            mark += tasks_status_pending * 2;
            mark += tasks_status_completed * 3;
            mark += (-1) * tasks_status_delayed * 1.5;
            mark += (-1) * tasks_status_failed * 3;
            mark += (-1) * tasks_status_failed * 1;

            return mark;
        }

        public double get_personaltasks_Mark()
        {
            double mark = 0.0;

            mark += tasks_personal_status_waiting * 1;
            mark += tasks_personal_status_approved * 1.5;
            mark += tasks_personal_status_pending * 2;
            mark += tasks_personal_status_completed * 3;
            mark += (-1) * tasks_personal_status_delayed * 1.5;
            mark += (-1) * tasks_personal_status_failed * 3;
            mark += (-1) * tasks_personal_status_failed * 1;

            return mark;
        }

        public string getDiffrencePercent(double avg_val)
        {
            double avgs = avg_val - (get_totaltasks_Mark() + get_personaltasks_Mark());

            double result = 0.0;


            if (avgs < 0)
            {
                result = (get_personaltasks_Mark() + get_totaltasks_Mark()) / avg_val * 100;
            }
            else
            {
                result = avg_val / (get_personaltasks_Mark() + get_totaltasks_Mark()) * 100;
            }

            if (double.IsNaN(result))
            {
                return " (не число) ";
            }
           
            return result.ToString("N3") + " %";

        }

    }
    public class Statistics
    {

    }

    public class StatisticsLineData
    {
        public double num { get; set; }
        public string date_mark { get; set; }
        public DateTime Date { get; } = DateTime.Now;
    }

    public class StatisticsViewModel_Pie
    {
       public string[] barDatasetLabels;
       public string[] pieDatasetLabels;
        public PieChart<int> pieRef { get; set; }
        public BarChart<int> barRef { get; set; }

        public ChartOptions pieOption {get;set;}
        
        public ChartOptions barOption { get; set; }

        #region Marks Datasets
        public int marks1 { get; set; }
        public int marks2 { get; set; }
        public int marks3 { get; set; }
        public int marks4 { get; set; }
        public int marks5 { get; set; }
        #endregion

        public int user_arr_id { get; set; }

        public string user_fullname { get; set; }

        public PieChartDataset<int> pieDatasetList { get; set; }

        public BarChartDataset<int> barDatasetList {get;set;}

        public LineChartDataset<StatisticsLineData> _lineDatasetList { get; set; }

        public LineChartDataset<StatisticsLineData> _lineDatasetList2 { get; set; }
        public LineChartDataset<StatisticsLineData> _lineDatasetList3 { get; set; }
        public LineChartDataset<StatisticsLineData> _lineDatasetList4 { get; set; }
        public LineChartDataset<StatisticsLineData> _lineDatasetList5 { get; set; }

        public double avg_deviation { get; set; }

        #region Lines Interpolation Options
        public LineChartOptions _line { get; set; }

        #endregion

        #region LineChartRefs
        public LineChart<StatisticsLineData> line1 { get; set; }
        public LineChart<StatisticsLineData> line2 { get; set; }
        public LineChart<StatisticsLineData> line3 { get; set; }
        public LineChart<StatisticsLineData> line4 { get; set; }
        public LineChart<StatisticsLineData> line5 { get; set; }
        #endregion


        

        public StatisticsViewModel_Pie()
        {
            pieRef = new PieChart<int>();
            barRef = new BarChart<int>();

            _line = new LineChartOptions();

            line1 = new LineChart<StatisticsLineData>();
            line2 = new LineChart<StatisticsLineData>();
            line3 = new LineChart<StatisticsLineData>();
            line4 = new LineChart<StatisticsLineData>();
            line5 = new LineChart<StatisticsLineData>();

            pieDatasetList = new PieChartDataset<int>();
            barDatasetList = new BarChartDataset<int>();

            marks1 = 0;
            marks2 = 0;
            marks3 = 0;
            marks4 = 0;
            marks5 = 0;

            pieOption = new ChartOptions();
            barOption = new ChartOptions();

            user_arr_id = 0;
            user_fullname = "";
    }
      
        public string getAverage()
        {
            double a = 0;

            int one = marks1;
            int two = marks2;
            int three = marks3;
            int four = marks4;
            int five = marks5;

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

            return a.ToString(string.Format("N4"));
        }

       

        public double getAverage(bool isDouble)
        {
            double a = 0;

            int one = marks1;
            int two = marks2;
            int three = marks3;
            int four = marks4;
            int five = marks5;

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

        public double getAvgDeviation(double total_avg)
        {
            return (Double.Parse(getAverage()) - total_avg);

        }

        public bool isContainsMark()
        {
            bool status = false;

            string getAverage_str = getAverage();
            double check_average = Double.Parse(getAverage_str);

            if (check_average == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

      public  double getDifferencePercentage( double total_avg)
        {
            double dbl = getAvgDeviation(total_avg);

            if (total_avg != 0)
            {
                return (double)((double)dbl / total_avg)*100;
            }
            else
            {
                return 0;
            }

            return 0;
        }

    }
}
