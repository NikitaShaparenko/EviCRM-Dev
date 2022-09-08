using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using EviCRM.Server.Pages.Maps.Models;
using EviCRM.Server.Pages.Maps.Models.Events;

namespace EviCRM.Server.Pages.Maps.Models.Events
{
    public class DragEndEvent : Event
    {
        public float Distance { get; set; }
    }

    public class DragEvent : Event
    {
        public LatLng LatLng { get; set; }

        public LatLng OldLatLng { get; set; }
    }

    public class ErrorEvent : Event
    {
        public string Message { get; set; }

        public int Code { get; set; }
    }

    public class Event
    {
        public string Type { get; set; }
 }
    public class MouseEvent : Event
    {
        public LatLng LatLng { get; set; }

        public PointF LayerPoint { get; set; }

        public PointF ContainerPoint { get; set; }

    }

    public class PopupEvent : Event
    {
        public Popup Popup { get; set; }
    }

    public class ResizeEvent : Event
    {
        public PointF OldSize { get; set; }
        public PointF NewSize { get; set; }
    }

    public class TooltipEvent : Event
    {
        public Tooltip Tooltip { get; set; }
    }
}
