using System.Drawing;


namespace EviCRM.Server.Pages.Maps.Data
{
    public class City
    {
        public string CoatOfArmsImageUrl { get; set; }

        public string Country { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PointF Coordinates { get; set; }
    }
}
