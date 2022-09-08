namespace EviCRM.Server.Pages.Charts
{
    public class WatcherEvent
    {
        public string Sector { get; set; }

        public double Count { get; set; }

        public DateTime Date { get; } = DateTime.Now;
    }

    public class WatcherEvent2
    {
        public DateTime Sector { get; set; }

        public double Count { get; set; }

        public DateTime Date { get; } = DateTime.Now;
    }

    public class PieChartEvent
    {
        public string Sector { get; set; }

        public int Count { get; set; }

        public DateTime Date { get; } = DateTime.Now;
    }
}
