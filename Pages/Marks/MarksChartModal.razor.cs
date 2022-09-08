namespace EviCRM.Server.Pages.Marks
{
    public partial class MarksChartModal
    {
        static String BytesToString(long byteCount)
        {
            string[] suf = { "Байт", "кб", "мб", "гб", "тб", "пб", "эксб" }; //
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
