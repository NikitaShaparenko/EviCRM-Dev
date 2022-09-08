using System;
using EviCRM.Server.Pages.Maps.Models;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.JSInterop;
using EviCRM.Server.Pages.Maps.Models.Events;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace EviCRM.Server.Pages.Maps
{

    public class StringHelper
    {

        private static readonly Random _random = new Random();

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

    }

    // Exception thrown when the user tried to manipulate the map before it has been initialized.
    public class UninitializedMapException : Exception
    {

        public UninitializedMapException()
        {

        }

        public UninitializedMapException(string message) : base(message)
        {

        }

    }
    public class Map
    {
        // Initial geographic center of the map

        public LatLng modal_maps_interop_latlng = new LatLng();
        public bool modal_maps_interop_isModalCalled { get; set; }

        public bool modal_maps_interop_isCreate { get; set; }


        public LatLng Center { get; set; } = new LatLng();

        // Initial map zoom level
        public float Zoom { get; set; }

        public float? MinZoom { get; set; }

        public float? MaxZoom { get; set; }
        public Tuple<LatLng, LatLng> MaxBounds { get; set; }
        public bool ZoomControl { get; set; } = true;

        public event Action OnInitialized;

        public string Id { get; }

        private ObservableCollection<Layer> _layers = new ObservableCollection<Layer>();

        private readonly IJSRuntime _jsRuntime;

        private bool _isInitialized;

        public Map(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            Id = StringHelper.GetRandomString(10);

            _layers.CollectionChanged += OnLayersChanged;
        }

       public void RaiseOnInitialized()
        {
            _isInitialized = true;
            OnInitialized?.Invoke();
        }

        
        public void AddLayer(Layer layer)
        {
            if (layer is null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            if (!_isInitialized)
            {
                throw new UninitializedMapException();
            }

            _layers.Add(layer);
        }

        public async Task OpenMapPopup(MapPopupData mapPopupData) => await LeafletInterops.OpenPopupOnMap(_jsRuntime, Id, mapPopupData);

      
        public async Task CloseMapPopup() => await LeafletInterops.ClosePopupOnMap(_jsRuntime, Id);


        public void RemoveLayer(Layer layer)
        {
            if (layer is null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            if (!_isInitialized)
            {
                throw new UninitializedMapException();
            }

            _layers.Remove(layer);
        }

        public IReadOnlyCollection<Layer> GetLayers()
        {
            return _layers.ToList().AsReadOnly();
        }

        private void OnLayersChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in args.NewItems)
                {
                    var layer = item as Layer;
                    LeafletInterops.AddLayer(_jsRuntime, Id, layer);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in args.OldItems)
                {
                    if (item is Layer layer)
                    {
                        LeafletInterops.RemoveLayer(_jsRuntime, Id, layer.Id);
                    }
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Replace
                     || args.Action == NotifyCollectionChangedAction.Move)
            {
                foreach (var oldItem in args.OldItems)
                    if (oldItem is Layer layer)
                        LeafletInterops.RemoveLayer(_jsRuntime, Id, layer.Id);

                foreach (var newItem in args.NewItems)
                    LeafletInterops.AddLayer(_jsRuntime, Id, newItem as Layer);
            }
        }

        public void FitBounds(PointF corner1, PointF corner2, PointF? padding = null, float? maxZoom = null)
        {
            LeafletInterops.FitBounds(_jsRuntime, Id, corner1, corner2, padding, maxZoom);
        }

        public void PanTo(PointF position, bool animate = false, float duration = 0.25f, float easeLinearity = 0.25f, bool noMoveStart = false)
        {
            LeafletInterops.PanTo(_jsRuntime, Id, position, animate, duration, easeLinearity, noMoveStart);
        }

        public async Task<LatLng> GetCenter() => await LeafletInterops.GetCenter(_jsRuntime, Id);
        public async Task<float> GetZoom() =>
            await LeafletInterops.GetZoom(_jsRuntime, Id);


        public async Task ZoomIn(MouseEventArgs e) => await LeafletInterops.ZoomIn(_jsRuntime, Id, e);

      
        public async Task ZoomOut(MouseEventArgs e) => await LeafletInterops.ZoomOut(_jsRuntime, Id, e);

        #region events

        public delegate void MapEventHandler(object sender, Event e);
        public delegate void MapResizeEventHandler(object sender, ResizeEvent e);

        public event MapEventHandler OnZoomLevelsChange;
        [JSInvokable]
        public void NotifyZoomLevelsChange(Event e) => OnZoomLevelsChange?.Invoke(this, e);

        public event MapResizeEventHandler OnResize;
        [JSInvokable]
        public void NotifyResize(ResizeEvent e) => OnResize?.Invoke(this, e);

        public event MapEventHandler OnUnload;
        [JSInvokable]
        public void NotifyUnload(Event e) => OnUnload?.Invoke(this, e);

        public event MapEventHandler OnViewReset;
        [JSInvokable]
        public void NotifyViewReset(Event e) => OnViewReset?.Invoke(this, e);

        public event MapEventHandler OnLoad;
        [JSInvokable]
        public void NotifyLoad(Event e) => OnLoad?.Invoke(this, e);

        public event MapEventHandler OnZoomStart;
        [JSInvokable]
        public void NotifyZoomStart(Event e) => OnZoomStart?.Invoke(this, e);

        public event MapEventHandler OnMoveStart;
        [JSInvokable]
        public void NotifyMoveStart(Event e) => OnMoveStart?.Invoke(this, e);

        public event MapEventHandler OnZoom;
        [JSInvokable]
        public void NotifyZoom(Event e) => OnZoom?.Invoke(this, e);

        public event MapEventHandler OnMove;
        [JSInvokable]
        public void NotifyMove(Event e) => OnMove?.Invoke(this, e);

        public event MapEventHandler OnZoomEnd;
        [JSInvokable]
        public void NotifyZoomEnd(Event e) => OnZoomEnd?.Invoke(this, e);

        public event MapEventHandler OnMoveEnd;
        [JSInvokable]
        public void NotifyMoveEnd(Event e) => OnMoveEnd?.Invoke(this, e);

        public event MouseEventHandler OnMouseMove;
        [JSInvokable]
        public void NotifyMouseMove(MouseEvent eventArgs) => OnMouseMove?.Invoke(this, eventArgs);

        public event MapEventHandler OnKeyPress;
        [JSInvokable]
        public void NotifyKeyPress(Event eventArgs) => OnKeyPress?.Invoke(this, eventArgs);

        public event MapEventHandler OnKeyDown;
        [JSInvokable]
        public void NotifyKeyDown(Event eventArgs) => OnKeyDown?.Invoke(this, eventArgs);

        public event MapEventHandler OnKeyUp;
        [JSInvokable]
        public void NotifyKeyUp(Event eventArgs) => OnKeyUp?.Invoke(this, eventArgs);

        public event MouseEventHandler OnPreClick;
        [JSInvokable]
        public void NotifyPreClick(MouseEvent eventArgs) => OnPreClick?.Invoke(this, eventArgs);

        #endregion events

        #region InteractiveLayerEvents

        public delegate void MouseEventHandler(Map sender, MouseEvent e);

        public event MouseEventHandler OnClick;
        [JSInvokable]
        public void NotifyClick(MouseEvent eventArgs)
        {
            OnClick?.Invoke(this, eventArgs);
        }
        public event MouseEventHandler OnDblClick;
        [JSInvokable]
        public void NotifyDblClick(MouseEvent eventArgs) => OnDblClick?.Invoke(this, eventArgs);

        public event MouseEventHandler OnMouseDown;
        [JSInvokable]
        public void NotifyMouseDown(MouseEvent eventArgs) => OnMouseDown?.Invoke(this, eventArgs);

        public event MouseEventHandler OnMouseUp;
        [JSInvokable]
        public void NotifyMouseUp(MouseEvent eventArgs) => OnMouseUp?.Invoke(this, eventArgs);

        public event MouseEventHandler OnMouseOver;
        [JSInvokable]
        public void NotifyMouseOver(MouseEvent eventArgs) => OnMouseOver?.Invoke(this, eventArgs);

        public event MouseEventHandler OnMouseOut;
        
        [JSInvokable]
        public void NotifyMouseOut(MouseEvent eventArgs)
        {
            CloseMapPopup();
            OnMouseOut?.Invoke(this, eventArgs);
        }
        public event MouseEventHandler OnContextMenu;

        [Parameter]
        public EventCallback<MouseEvent> OnContext { get; set; }

        [JSInvokable]
        public void NotifyContextMenu(MouseEvent eventArgs)
        {
            modal_maps_interop_isModalCalled = true;
            modal_maps_interop_latlng = eventArgs.LatLng;
            modal_maps_interop_isCreate = true;

            OnContextMenu?.Invoke(this, eventArgs);
            OnContext.InvokeAsync(eventArgs);
        }
        #endregion InteractiveLayerEvents
    }
}