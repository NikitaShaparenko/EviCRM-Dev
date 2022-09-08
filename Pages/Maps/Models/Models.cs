using EviCRM.Server.Pages.Maps.Models.Events;
using Microsoft.JSInterop;
using System.Drawing;

namespace EviCRM.Server.Pages.Maps.Models
{
    public class MapPopupData
    {
        public MapPopupData()
        {
        }

        public LatLng LatLng { get; set; }
        public string Content { get; set; }
    }
    public class Circle : Path
    {

        /// <summary>
        /// Center of the circle.
        /// </summary>
        public LatLng Position { get; set; }

        /// <summary>
        /// Radius of the circle, in meters.
        /// </summary>
        public float Radius { get; set; }

    }

    public abstract class DivOverlay : Layer
    {

        /// <summary>
        /// The offset of the popup position. Useful to control the anchor of the popup when opening it on some overlays.
        /// </summary>
        public virtual Point Offset { get; set; } = new Point(0, 7);

        /// <summary>
        /// A custom CSS class name to assign to the popup.
        /// </summary>
        public virtual string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// Map pane where the popup will be added.
        /// </summary>
        public override string Pane { get; set; } = "popupPane";

    }

    public class GeoJsonDataLayer : InteractiveLayer
    {
        public string GeoJsonData { get; set; }
    }

    public abstract class GridLayer : Layer
    {

        /// <summary>
        /// Width and height of tiles in the grid.
        /// </summary>
        public Size TileSize { get; set; } = new Size(256, 256);

        public double Opacity { get; set; } = 1.0;

        /// <summary>
        /// By default, a smooth zoom animation will update grid layers every integer zoom level. Setting this option to false will update the grid layer only when the smooth animation ends.
        /// </summary>
        public bool UpdateWhenZooming { get; set; } = true;

        /// <summary>
        /// Tiles will not update more than once every updateInterval milliseconds when panning.
        /// </summary>
        public int UpdateInterval { get; set; } = 200;

        public int ZIndex { get; set; } = 1;

        /// <summary>
        /// If set, tiles will only be loaded inside the set.
        /// </summary>
        public Tuple<LatLng, LatLng> Bounds { get; set; }

    }

    public class Icon
    {

        /// <summary>
        /// (required) The URL to the icon image (absolute or relative to your script path).
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The URL to a retina sized version of the icon image (absolute or relative to your script path). Used for Retina screen devices.
        /// </summary>
        public string RetinalUrl { get; set; }

        /// <summary>
        /// Size of the icon image in pixels.
        /// </summary>
        public Size? Size { get; set; }

        /// <summary>
        /// The coordinates of the "tip" of the icon (relative to its top left corner). The icon will be aligned so that this point is at the marker's geographical location. Centered by default if size is specified, also can be set in CSS with negative margins.
        /// </summary>
        public Point? Anchor { get; set; }

        /// <summary>
        /// The coordinates of the point from which popups will "open", relative to the icon anchor.
        /// </summary>
        public Point PopupAnchor { get; set; } = Point.Empty;

        /// <summary>
        /// The coordinates of the point from which tooltips will "open", relative to the icon anchor.
        /// </summary>
        public Point TooltipAnchor { get; set; } = Point.Empty;

        /// <summary>
        /// The URL to the icon shadow image. If not specified, no shadow image will be created.
        /// </summary>
        public string ShadowUrl { get; set; }

        public string ShadowRetinalUrl { get; set; }

        /// <summary>
        /// Size of the shadow image in pixels.
        /// </summary>
        public Size? ShadowSize { get; set; }

        /// <summary>
        /// The coordinates of the "tip" of the shadow (relative to its top left corner) (the same as iconAnchor if not specified).
        /// </summary>
        public Size? ShadowAnchor { get; set; }

        /// <summary>
        /// A custom class name to assign to both icon and shadow images. Empty by default.
        /// </summary>
        public string ClassName { get; set; } = string.Empty;

    }

    public class ImageLayer : InteractiveLayer
    {

        /// <summary>
        /// The opacity of the image overlay.
        /// </summary>
        public float Opacity { get; set; } = 1.0f;

        /// <summary>
        /// Text for the alt attribute of the image (useful for accessibility).
        /// </summary>
        public string Alt { get; set; } = string.Empty;

        /// <summary>
        /// Whether the crossOrigin attribute will be added to the image. If a String is provided, the image will have its crossOrigin attribute set to the String provided. This is needed if you want to access image pixel data. Refer to CORS Settings for valid String values.
        /// </summary>
        public string CrossOrigin { get; set; }

        /// <summary>
        /// URL to the overlay image to show in place of the overlay that failed to load.
        /// </summary>
        public string ErrorOverlayUrl { get; set; } = string.Empty;

        /// <summary>
        /// The explicit zIndex of the overlay layer.
        /// </summary>
        public int zIndex { get; set; } = 1;

        /// <summary>
        /// A custom class name to assign to the image. Empty by default.
        /// </summary>
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        ///  Image url.
        /// </summary>
        public string Url { get; }

        public PointF Corner1 { get; }

        public PointF Corner2 { get; }

        public ImageLayer(string url, PointF corner1, PointF corner2)
        {
            Url = url;
            Corner1 = corner1;
            Corner2 = corner2;
        }

    }

    public abstract class InteractiveLayer : Layer
    {

        /// <summary>
        /// If false, the layer will not emit mouse events and will act as a part of the underlying map. (events currently not implemented in BlazorLeaflet)
        /// </summary>
        public bool IsInteractive { get; set; } = true;

        /// <summary>
        /// When true, a mouse event on this layer will trigger the same event on the map (unless L.DomEvent.stopPropagation is used).
        /// </summary>
        public virtual bool IsBubblingMouseEvents { get; set; } = true;

        #region events

        public delegate void MouseEventHandler(InteractiveLayer sender, MouseEvent e);

        public event MouseEventHandler OnClick;

        [JSInvokable]
        public void NotifyClick(MouseEvent eventArgs)
        {
            OnClick?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnDblClick;

        [JSInvokable]
        public void NotifyDblClick(MouseEvent eventArgs)
        {
            OnDblClick?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnMouseDown;

        [JSInvokable]
        public void NotifyMouseDown(MouseEvent eventArgs)
        {
            OnMouseDown?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnMouseUp;

        [JSInvokable]
        public void NotifyMouseUp(MouseEvent eventArgs)
        {
            OnMouseUp?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnMouseOver;

        [JSInvokable]
        public void NotifyMouseOver(MouseEvent eventArgs)
        {
            OnMouseOver?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnMouseOut;

        [JSInvokable]
        public void NotifyMouseOut(MouseEvent eventArgs)
        {
            OnMouseOut?.Invoke(this, eventArgs);
        }

        public event MouseEventHandler OnContextMenu;

        [JSInvokable]
        public void NotifyContextMenu(MouseEvent eventArgs)
        {
            OnContextMenu?.Invoke(this, eventArgs);
        }

        #endregion

    }

    public class LatLng
    {
        public float Lat { get; set; }

        public float Lng { get; set; }

        public float Alt { get; set; }

        public PointF ToPointF() => new PointF(Lat, Lng);

        public LatLng() { }

        public LatLng(PointF position) : this(position.X, position.Y) { }

        public LatLng(float lat, float lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public LatLng(float lat, float lng, float alt) : this(lat, lng)
        {
            Alt = alt;
        }
    }

    public abstract class Layer
    {
        /// <summary>
        /// Unique identifier used by the interoperability service on the client side to identify layers.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// By default the layer will be added to the map's overlay pane. Overriding this option will cause the layer to be placed on another pane by default.
        /// </summary>
        public virtual string Pane { get; set; } = "overlayPane";

        /// <summary>
        /// String to be shown in the attribution control, e.g. "© OpenStreetMap contributors". It describes the layer data and is often a legal obligation towards copyright holders and tile providers.
        /// </summary>
        public string Attribution { get; set; }

        /// <summary>
        /// The tooltip assigned to this marker.
        /// </summary>
        public Tooltip Tooltip { get; set; }

        /// <summary>
        /// The popup shown when the marker is clicked.
        /// </summary>
        public Popup Popup { get; set; }

        protected Layer()
        {
            Id = StringHelper.GetRandomString(20);
        }

        #region events

        public delegate void EventHandler(Layer sender, Event e);

        public event EventHandler OnAdd;

        [JSInvokable]
        public void NotifyAdd(Event eventArgs)
        {
            OnAdd?.Invoke(this, eventArgs);
        }

        public event EventHandler OnRemove;

        [JSInvokable]
        public void NotifyRemove(Event eventArgs)
        {
            OnRemove?.Invoke(this, eventArgs);
        }

        public delegate void PopupEventHandler(Layer sender, PopupEvent e);

        public event PopupEventHandler OnPopupOpen;

        [JSInvokable]
        public void NotifyPopupOpen(PopupEvent eventArgs)
        {
            OnPopupOpen?.Invoke(this, eventArgs);
        }

        public event PopupEventHandler OnPopupClose;

        [JSInvokable]
        public void NotifyPopupClose(PopupEvent eventArgs)
        {
            OnPopupClose?.Invoke(this, eventArgs);
        }

        public delegate void TooltipEventHandler(Layer sender, TooltipEvent e);

        public event TooltipEventHandler OnTooltipOpen;

        [JSInvokable]
        public void NotifyTooltipOpen(TooltipEvent eventArgs)
        {
            OnTooltipOpen?.Invoke(this, eventArgs);
        }

        public event TooltipEventHandler OnTooltipClose;

        [JSInvokable]
        public void NotifyTooltipClose(TooltipEvent eventArgs)
        {
            OnTooltipClose?.Invoke(this, eventArgs);
        }

        #endregion
    }

    public class Marker : InteractiveLayer
    {
        /// <summary>
        /// The position of the marker on the map.
        /// </summary>
        public LatLng Position { get; set; }

        /// <summary>
        /// Icon instance to use for rendering the marker. See <see href="https://leafletjs.com/reference-1.5.0.html#icon">Icon documentation</see> for details on how to customize the marker icon. If not specified, a common instance of <see href="https://leafletjs.com/reference-1.5.0.html#icon-default">L.Icon.Default</see> is used.
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Whether the marker can be tabbed to with a keyboard and clicked by pressing enter.
        /// </summary>
        public bool IsKeyboardAccessible { get; set; } = true;

        /// <summary>
        /// Text for the browser tooltip that appear on marker hover (no tooltip by default).
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Text for the alt attribute of the icon image (useful for accessibility).
        /// </summary>
        public string Alt { get; set; } = string.Empty;

        /// <summary>
        /// By default, marker images zIndex is set automatically based on its latitude. Use this option if you want to put the marker on top of all others (or below), specifying a high value like 1000 (or high negative value, respectively).
        /// </summary>
        public int ZIndexOffset { get; set; }

        /// <summary>
        /// The opacity of the marker.
        /// </summary>
        public double Opacity { get; set; } = 1.0;

        /// <summary>
        /// If true, the marker will get on top of others when you hover the mouse over it.
        /// </summary>
        public bool RiseOnHover { get; set; }

        /// <summary>
        /// The z-index offset used for the riseOnHover feature.
        /// </summary>
        public int RiseOffset { get; set; } = 250;

        public override string Pane { get; set; } = "markerPane";

        public override bool IsBubblingMouseEvents { get; set; } = false;

        /// <summary>
        /// Whether the marker is draggable with mouse/touch or not.
        /// </summary>
        public bool Draggable { get; set; }

        /// <summary>
        /// Whether to pan the map when dragging this marker near its edge or not.
        /// </summary>
        public bool UseAutoPan { get; set; }

        /// <summary>
        /// Distance (in pixels to the left/right and to the top/bottom) of the map edge to start panning the map.
        /// </summary>
        public Point AutoPanPadding { get; set; } = new Point(50, 50);

        /// <summary>
        /// Number of pixels the map should pan by.
        /// </summary>
        public int AutoPanSpeed { get; set; } = 10;

        public Marker(float x, float y) : this(new LatLng(x, y)) { }

        public Marker(PointF position) : this(position.X, position.Y) { }

        public Marker(LatLng latLng)
        {
            Position = latLng;
        }

        #region events

        public delegate void DragEventHandler(Marker sender, DragEvent e);

        public event DragEventHandler OnMove;

        [JSInvokable]
        public void NotifyMove(DragEvent eventArgs)
        {
            OnMove?.Invoke(this, eventArgs);
        }

        public delegate void EventHandlerMarker(Marker sender, Event e);

        public event EventHandlerMarker OnDragStart;

        [JSInvokable]
        public void NotifyDragStart(Event eventArgs)
        {
            OnDragStart?.Invoke(this, eventArgs);
        }

        public event EventHandlerMarker OnMoveStart;

        [JSInvokable]
        public void NotifyMoveStart(Event eventArgs)
        {
            OnMoveStart?.Invoke(this, eventArgs);
        }

        public event DragEventHandler OnDrag;

        [JSInvokable]
        public void NotifyDrag(DragEvent eventArgs)
        {
            OnDrag?.Invoke(this, eventArgs);
        }

        public delegate void DragEndEventHandler(Marker sender, DragEndEvent e);

        public event DragEndEventHandler OnDragEnd;

        [JSInvokable]
        public void NotifyDragEnd(DragEndEvent eventArgs)
        {
            OnDragEnd?.Invoke(this, eventArgs);
        }

        public event EventHandlerMarker OnMoveEnd;

        [JSInvokable]
        public void NotifyMoveEnd(Event eventArgs)
        {
            OnMoveEnd?.Invoke(this, eventArgs);
        }

        #endregion

    }

    /// <summary>
    /// Class for .mbtiles tilesets. Requires the Leaflet.TileLayer.MBTiles plugin
    /// </summary>
    public class MbTilesLayer : Layer
    {
        /// <summary>
        /// Instantiates a tile layer object given a URL template.
        /// </summary>
        public string UrlTemplate { get; set; }

        /// <summary>
        /// The minimum zoom level down to which this layer will be displayed (inclusive).
        /// </summary>
        public float MinimumZoom { get; set; }

        /// <summary>
        /// The maximum zoom level up to which this layer will be displayed (inclusive).
        /// </summary>
        public float MaximumZoom { get; set; } = 18;
    }

    public abstract class Path : InteractiveLayer
    {

        /// <summary>
        /// Whether to draw stroke along the path. Set it to false to disable borders on polygons or circles.
        /// </summary>
        public bool DrawStroke { get; set; } = true;

        /// <summary>
        /// Stroke color.
        /// </summary>
        public Color StrokeColor { get; set; } = Color.FromArgb(0x33, 0x88, 0xFF);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public int StrokeWidth { get; set; } = 3;

        /// <summary>
        /// Stroke opacity.
        /// </summary>
        public double StrokeOpacity { get; set; } = 1.0;

        /// <summary>
        /// A string that defines <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/stroke-linecap">shape to be used at the end</see> of the stroke.
        /// </summary>
        public string LineCap { get; set; } = "round";

        /// <summary>
        /// A string that defines <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/stroke-linejoin">shape to be used at the corners</see> of the stroke.
        /// </summary>
        public string LineJoin { get; set; } = "round";

        /// <summary>
        /// A string that defines the stroke <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/stroke-dasharray">dash pattern</see>. Doesn't work on Canvas-powered layers in some old browsers.
        /// </summary>
        public string StrokeDashArray { get; set; }

        /// <summary>
        /// A string that <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/stroke-dashoffset">defines the distance into the dash pattern to start the dash</see>. Doesn't work on Canvas-powered layers in some old browsers.
        /// </summary>
        public string StrokeDashOffset { get; set; }

        /// <summary>
        /// Whether to fill the path with color. Set it to false to disable filling on polygons or circles.
        /// </summary>
        public bool Fill { get; set; }

        /// <summary>
        /// Fill color. Defaults to the value of the color option
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Fill opacity.
        /// </summary>
        public double FillOpacity { get; set; } = 0.2;

        /// <summary>
        /// A string that defines <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/fill-rule">how the inside of a shape</see> is determined.
        /// </summary>
        public string FillRule { get; set; } = "evenodd";

    }
    public class Tooltip : DivOverlay
    {

        public override string Pane => "tooltipPane";

        public override Point Offset { get; set; } = Point.Empty;

        /// <summary>
        /// Direction where to open the tooltip. Possible values are: right, left, top, bottom, center, auto. auto will dynamically switch between right and left according to the tooltip position on the map.
        /// </summary>
        public string Direction { get; set; } = "auto";

        /// <summary>
        /// Whether to open the tooltip permanently or only on mouseover.
        /// </summary>
        public bool IsPermanent { get; set; }

        /// <summary>
        /// If true, the tooltip will follow the mouse instead of being fixed at the feature center.
        /// </summary>
        public bool IsSticky { get; set; }

        /// <summary>
        /// Tooltip container opacity.
        /// </summary>
        public double Opacity { get; set; } = 0.9;

        /// <summary>
        /// The content of the tooltip.
        /// </summary>
        public string Content { get; set; }

    }
    public class Polygon : Polyline
    { }

    public class Polyline<TShape> : Path
    {

        public TShape Shape { get; set; }

        /// <summary>
        /// How much to simplify the polyline on each zoom level. More means better performance and smoother look, and less means more accurate representation.
        /// </summary>
        public double SmoothFactory { get; set; } = 1.0;

        /// <summary>
        /// Disable polyline clipping.
        /// </summary>
        public bool NoClipEnabled { get; set; }

    }

    public class Polyline : Polyline<PointF[][]>
    { }

    public class Popup : DivOverlay
    {

        public override string Pane => "popupPane";

        /// <summary>
        /// Max width of the popup, in pixels.
        /// </summary>
        public int MaximumWidth { get; set; } = 300;

        /// <summary>
        /// Min width of the popup, in pixels.
        /// </summary>
        public int MinimumWidth { get; set; } = 50;

        /// <summary>
        /// If set, creates a scrollable container of the given height inside a popup if its content exceeds it.
        /// </summary>
        public int? MaximumHeight { get; set; }

        /// <summary>
        /// Set it to false if you don't want the map to do panning animation to fit the opened popup.
        /// </summary>
        public bool AutoPanEnabled { get; set; } = true;

        /// <summary>
        /// The margin between the popup and the top left corner of the map view after autopanning was performed.
        /// </summary>
        public Point? AutoPanPaddingTopLeft { get; set; }

        /// <summary>
        /// The margin between the popup and the bottom right corner of the map view after autopanning was performed.
        /// </summary>
        public Point? AutoPanPaddingBottomLeft { get; set; }

        /// <summary>
        /// Equivalent of setting both top left and bottom right autopan padding to the same value.
        /// </summary>
        public Point AutoPanPadding { get; set; } = new Point(5, 5);

        /// <summary>
        /// Set it to true if you want to prevent users from panning the popup off of the screen while it is open.
        /// </summary>
        public bool KeepInView { get; set; }

        /// <summary>
        /// Controls the presence of a close button in the popup.
        /// </summary>
        public bool ShowCloseButton { get; set; } = true;

        /// <summary>
        /// Set it to false if you want to override the default behavior of the popup closing when another popup is opened.
        /// </summary>
        public bool AutoClose { get; set; } = true;

        /// <summary>
        /// Set it to false if you want to override the default behavior of the ESC key for closing of the popup.
        /// </summary>
        public bool CloseOnEscapeKey { get; set; } = true;

        /// <summary>
        /// The content of the popup.
        /// </summary>
        public string Content { get; set; }

    }

    public class Rectangle : Polyline<System.Drawing.RectangleF>
    {
    }

    /// <summary>
    /// Shapefile layer - Requires Leaflet.Shapefile plugin
    /// </summary>
    public class ShapefileLayer : Layer
    {
        /// <summary>
        /// Instantiates a tile layer object given a URL template.
        /// </summary>
        public string UrlTemplate { get; set; }
    }
    public class TileLayer : GridLayer
    {

        /// <summary>
        /// Instantiates a tile layer object given a URL template.
        /// </summary>
        public string UrlTemplate { get; set; }

        /// <summary>
        /// The minimum zoom level down to which this layer will be displayed (inclusive).
        /// </summary>
        public float MinimumZoom { get; set; }

        /// <summary>
        /// The maximum zoom level up to which this layer will be displayed (inclusive).
        /// </summary>
        public float MaximumZoom { get; set; } = 18;

        /// <summary>
        /// Subdomains of the tile service.
        /// </summary>
        public string[] Subdomains { get; set; } = new string[] { "abc" };

        /// <summary>
        /// URL to the tile image to show in place of the tile that failed to load.
        /// </summary>
        public string ErrorTileUrl { get; set; }

        /// <summary>
        /// If set to true, the zoom number used in tile URLs will be reversed (maxZoom - zoom instead of zoom)
        /// </summary>
        public bool IsZoomReversed { get; set; }

        /// <summary>
        /// The zoom number used in tile URLs will be offset with this value.
        /// </summary>
        public double ZoomOffset { get; set; }

        /// <summary>
        /// If true and user is on a retina display, it will request four tiles of half the specified size and a bigger zoom level in place of one to utilize the high resolution.
        /// </summary>
        public bool DetectRetina { get; set; }

    }

}
