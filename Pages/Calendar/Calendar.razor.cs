using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using EviCRM.Server.Services;
using Microsoft.JSInterop;
using EviCRM.Server.Models;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using EviCRM.Server.Models.TUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;


namespace EviCRM.Server.Pages
{
    /// <summary>
    /// Enum Used to Advance the Calendar Forward, Reverse, or to Today
    /// </summary>
    public enum CalendarMove
    {
        Next,
        Previous,
        Today
    }

    public partial class TUICalendar : ComponentBase, IDisposable
    {

        private DotNetObjectReference<TUICalendar> _ObjectReference;

        /// <summary>
        /// Used to Queue events in SetParametersAsync. Code cannot be left until after all parameters have been set
        /// </summary>
        private Queue<Task> _OnParameterChangeEvents = new Queue<Task>();

        /// <summary>
        /// Direct access to some calendar functions via the Interop
        /// </summary>
        public TUICalendarInteropService CalendarInterop { get; private set; } = null;

        /// <summary>
        /// Calendar display options and defaults, can be null
        /// </summary>
        [Parameter]
        public TUICalendarOptions CalendarOptions { get; set; } = null;

        public EventCallback<TUICalendarOptions> CalendarOptionsChanged { get; set; }

        /// <summary>
        /// Calendar Properties for each calendar, i.e. colors, name, etc
        /// </summary>
        [Parameter]
        public IEnumerable<TUICalendarProps> CalendarProperties { get; set; } = null;

        /// <summary>
        /// Change the Calendar View Mode to Day, Week, or Month View
        /// The initial setting of this parameter has no affect.
        /// The calendar Options initial view will override this
        /// </summary>
        [Parameter]
        public TUICalendarViewName CalendarViewName { get; set; }

        /// <summary>
        /// One-Way bind to this value to change/jump to any date
        /// which will be made visible for any given calendar view name
        /// Initial setting of this parameter will have no affect during 
        /// loading of this component
        /// </summary>
        [Parameter]
        public DateTimeOffset? GoToDate { get; set; }

        [Inject]
        public IJSRuntime jsRuntime { get; set; }

        /// <summary>
        /// Invoked when a calendar Event or Task is changed
        /// </summary>
        [Parameter]
        public EventCallback<TUISchedule> OnChangeCalendarEventOrTask { get; set; }

        /// <summary>
        /// Invoked when a calendar Event or Task is Clicked
        /// </summary>
        [Parameter]
        public EventCallback<string> OnClickCalendarEventOrTask { get; set; }

        /// <summary>
        /// Raised when a calendar Event or Task is Created
        /// </summary>
        [Parameter]
        public EventCallback<TUISchedule> OnCreateCalendarEventOrTask { get; set; }

        /// <summary>
        /// Raised when a calendar Event or Task is Deleted
        /// </summary>
        [Parameter]
        public EventCallback<string> OnDeleteCalendarEventOrTask { get; set; }

        /// <summary>
        /// Not Working
        /// </summary>
        /*
        [Parameter]
        public EventCallback<string> OnDoubleClickCalendarEventOrTask { get; set; }
        */

        /// <summary>
        /// IEnumerable of all events/tasks etc of type TUISchedule.
        /// This is the initial set of schedules/events to be loaded
        /// </summary>
        [Parameter]
        public ICollection<TUISchedule> Schedules { get; set; }

        /// <summary>
        /// The End Date of the Range of days displated on the calendar
        /// </summary>
        [Parameter]
        public DateTimeOffset? VisibleEndDateRange { get; set; }

        /// <summary>
        /// The End Date of the Range of days displated on the calendar
        /// </summary>
        [Parameter]
        public EventCallback<DateTimeOffset?> VisibleEndDateRangeChanged { get; set; }

        /// <summary>
        /// The Start Date of the Range of days displayed on the calendar
        /// </summary>
        [Parameter]
        public DateTimeOffset? VisibleStartDateRange { get; set; }

        /// <summary>
        /// The Start Date of the Range of days displayed on the calendar
        /// </summary>
        [Parameter]
        public EventCallback<DateTimeOffset?> VisibleStartDateRangeChanged { get; set; }

        /// <summary>
        /// When the user created a new schedule from the UI
        /// </summary>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        [JSInvokable("CreateSchedule")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateSchedule(JsonElement newSchedule)
        {
            var schedule = JsonSerializer.Deserialize<TUISchedule>(newSchedule.ToString());
            Schedules.Add(schedule);
            await OnCreateCalendarEventOrTask.InvokeAsync(schedule);

           // await mysqlc.MySql_ContextAsyncL(mysqlc.setSchelude(NullSupressor(schedule.id), NullSupressor(schedule.calendarId), NullSupressor(schedule.start.ToString()), NullSupressor(schedule.end.ToString()), NullSupressor(schedule.title), NullSupressor(schedule.body), NullSupressor(schedule.category), NullSupressor(schedule.isVisible.ToString()), NullSupressor(schedule.isAllDay.ToString()), NullSupressor(schedule.state.ToString()), NullSupressor(session_cookie_data)));

            Debug.WriteLine("New Schedule Created");
        }

        public string NullSupressor(string input)
        {
            if (input == null)
            {
                return "";
            }
            else
            {
                return input;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_ObjectReference != null)
            {
                //Now dispose our object reference so our component can be garbage collected
                _ObjectReference.Dispose();
            }
        }

        /// <summary>
        /// Clears all schedules from the calendar.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async ValueTask ClearCalendar()
        {
            await CalendarInterop.Clear();
        }

        EviCRM.Server.Core.MySQL_Controller mysqlc = new EviCRM.Server.Core.MySQL_Controller(); 

        /// <summary>
        /// Call this method and Advance the calendar, in any view, forward,backward, or to today.
        /// </summary>
        /// <param name="moveTo">Previous, Next, or Today</param>
        /// <returns></returns>
        public async ValueTask MoveCalendar(CalendarMove moveTo)
        {
            await CalendarInterop.MoveCalendar(moveTo);
            await SetDateRange();
        }

        /// <summary>
        /// When a schedule is deleted from calendar UI, this is invoked        
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        [JSInvokable("DeleteSchedule")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnDeleteSchedule(string scheduleId)
        {
            await OnDeleteCalendarEventOrTask.InvokeAsync(scheduleId);
            Debug.WriteLine($"Schedule {scheduleId} Deleted!");
        }

        /// <summary>
        /// When a schedule is clicked from the calendar UI, this is invoked
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        [JSInvokable("OnClickSchedule")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnScheduleClick(string scheduleId)
        {
            await OnClickCalendarEventOrTask.InvokeAsync(scheduleId);
            Debug.WriteLine($"Schedule {scheduleId} Clicked!");
        }

        /// <summary>
        /// Any time a new parameter is added, it must be MANUALLY set here
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var viewName = parameters.GetValueOrDefault<TUICalendarViewName>("CalendarViewName");
            if (viewName != CalendarViewName)
            {
                CalendarViewName = viewName;
                _OnParameterChangeEvents.Enqueue(CalendarInterop?.ChangeView(viewName).AsTask());
                _OnParameterChangeEvents.Enqueue(SetDateRange());
            }

            var newDateDisplay = parameters.GetValueOrDefault<DateTimeOffset?>("GoToDate");
            if (newDateDisplay != GoToDate)
            {
                GoToDate = newDateDisplay;
                _OnParameterChangeEvents.Enqueue(CalendarInterop?.SetDate(newDateDisplay).AsTask());
                _OnParameterChangeEvents.Enqueue(SetDateRange());
            }

            var calendarOptions = parameters.GetValueOrDefault<TUICalendarOptions>("CalendarOptions");
            if (calendarOptions is not null)
            {
                if (!calendarOptions.Equals(CalendarOptions))
                {
                    CalendarOptions = calendarOptions;
                    _OnParameterChangeEvents.Enqueue(CalendarInterop?.SetCalendarOptionsAsync(calendarOptions).AsTask());
                    _OnParameterChangeEvents.Enqueue(SetDateRange());
                }
            }
            CalendarProperties = parameters.GetValueOrDefault<IEnumerable<TUICalendarProps>>("CalendarProperties");
            Schedules = parameters.GetValueOrDefault<ICollection<TUISchedule>>("Schedules");

            //Visible Date Range
            VisibleEndDateRange = parameters.GetValueOrDefault<DateTimeOffset?>("VisibleEndDateRange");
            VisibleStartDateRange = parameters.GetValueOrDefault<DateTimeOffset?>("VisibleStartDateRange");
            VisibleStartDateRangeChanged = parameters.GetValueOrDefault<EventCallback<DateTimeOffset?>>("VisibleStartDateRangeChanged");
            VisibleEndDateRangeChanged = parameters.GetValueOrDefault<EventCallback<DateTimeOffset?>>("VisibleEndDateRangeChanged");

            //Events
            OnChangeCalendarEventOrTask = parameters.GetValueOrDefault<EventCallback<TUISchedule>>("OnChangeCalendarEventOrTask");
            OnCreateCalendarEventOrTask = parameters.GetValueOrDefault<EventCallback<TUISchedule>>("OnCreateCalendarEventOrTask");
            OnClickCalendarEventOrTask = parameters.GetValueOrDefault<EventCallback<string>>("OnClickCalendarEventOrTask");
            OnDeleteCalendarEventOrTask = parameters.GetValueOrDefault<EventCallback<string>>("OnDeleteCalendarEventOrTask");

            await base.SetParametersAsync(ParameterView.Empty);

        }

        /// <summary>
        /// When an schedule is updated from the UI, this is invoked.
        /// </summary>
        /// <param name="scheduleBeingModified"></param>
        /// <param name="updatedScheduleFields"></param>
        /// <returns></returns>
        [JSInvokable("UpdateSchedule")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateSchedule(dynamic scheduleBeingModified, dynamic updatedScheduleFields)
        {
            var currentSchedule = JsonSerializer.Deserialize<TUISchedule>(scheduleBeingModified.ToString());
            var updatedSchedule = CalendarInterop.UpdateSchedule(currentSchedule, updatedScheduleFields); //Todo: Combine changes with actual schedule
            await OnChangeCalendarEventOrTask.InvokeAsync(updatedSchedule); //Todo: Test This callback!
            Debug.WriteLine($"Schedule {currentSchedule.id} Modified");
        }

        /*@Todo: Waiting for Double click in TUI API
        [JSInvokable("OnDoubleClickSchedule")]
        public async Task OnScheduleDoubleClick(string scheduleId)
        {
            await OnDoubleClickCalendarEventOrTask.InvokeAsync(scheduleId);
            Debug.WriteLine($"Schedule {scheduleId} Double-Clicked!");
        }
        */

        string session_cookie_data { get; set; }
        string session_role { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //session_cookie_data = GetAuthenticationStateAsync().Result.User.Identity.Name;
            session_role = await mysqlc.MySql_ContextAsyncL(mysqlc.getProjectUsersRolesByIdentityName(session_cookie_data));

            if (firstRender)
            {
               // await CalendarInterop.InitCalendarAsync(_ObjectReference, CalendarOptions);
                await CalendarInterop.SetCalendars(CalendarProperties);
                await CalendarInterop.CreateSchedulesAsync(Schedules);

                await SetDateRange();
            }
        }

        protected override async void OnInitialized()
        {
            _ObjectReference = DotNetObjectReference.Create(this);
            if (CalendarInterop is null)
            {
                CalendarInterop = new TUICalendarInteropService(jsRuntime);
            }
            await Task.Run(async () =>
            {
                CreateTasks();
            });



            }

        public async Task CreateTasks()
        {
            await LoadScheludeData();


            List<TUISchedule> lsc = ListScheludeCompiler();

            await CalendarInterop.CreateSchedulesAsync(lsc);
        }

        //MySQL Module

        //EviCRM.Server.Core.MySQL_Controller mysqlc = new EviCRM.Server.Core.MySQL_Controller();

        List<string> cs_idcalendar_scheludes_dt = new List<string>();
        List<string> cs_id_dt = new List<string>();
        List<string> cs_calendarId_dt = new List<string>();
        List<string> cs_startDate_dt = new List<string>();
        List<string> cs_endDate_dt = new List<string>();
        List<string> cs_title_dt = new List<string>();
        List<string> cs_body_dt = new List<string>();
        List<string> cs_calendarCategory_dt = new List<string>();
        List<string> cs_isVisible_dt = new List<string>();
        List<string> cs_isAllDay_dt = new List<string>();
        List<string> cs_state_dt = new List<string>();
        List<string> cs_user_dt = new List<string>();

        public async Task LoadScheludeData()
        {
            cs_idcalendar_scheludes_dt = await mysqlc.getListCalendarIDCalendarScheludesAsync();
            cs_id_dt = await mysqlc.getListCalendarIDAsync();
            cs_calendarId_dt = await mysqlc.getListCalendarCalendarIDAsync();
            cs_startDate_dt = await mysqlc.getListCalendarIDStartDateAsync();
            cs_endDate_dt = await mysqlc.getListCalendarIDEndDateAsync();
            cs_title_dt = await mysqlc.getListCalendarTitleAsync();
            cs_body_dt = await mysqlc.getListCalendarIDBodyAsync();
            cs_calendarCategory_dt = await mysqlc.getListCalendarCategoryAsync();
            cs_isVisible_dt = await mysqlc.getListCalendarisVisibleAsync();
            cs_isAllDay_dt = await mysqlc.getListCalendarisAllDayAsync();
            cs_state_dt = await mysqlc.getListCalendarStateAsync();
            cs_user_dt = await mysqlc.getListCalendarUserAsync();
        }

        public List<TUISchedule> ListScheludeCompiler()
        {
            List<TUISchedule> scheludes = new List<TUISchedule>();
            for (int i = 0; i < cs_id_dt.Count; i++)
            {
                if (cs_id_dt[i] != null && cs_id_dt[i] != "")
                {
                    scheludes.Add(ScheludeCompiler(i));
                }
            }
            return scheludes;
        }

        public TUISchedule ScheludeCompiler(int cnt)
        {
            var startDate = cs_startDate_dt[cnt];
            var endDate = cs_endDate_dt[cnt];
            var sched = new TUISchedule()
            {
                id = cs_id_dt[cnt],
                calendarId = cs_calendarId_dt[cnt],
                start = DateTimeOffset.Parse(cs_startDate_dt[cnt]),
                end = DateTimeOffset.Parse(cs_endDate_dt[cnt]),
                title = cs_title_dt[cnt],
                body = cs_body_dt[cnt],
                category = cs_calendarCategory_dt[cnt],
                isVisible = bool.Parse(cs_isVisible_dt[cnt]),
                isAllDay = bool.Parse(cs_isAllDay_dt[cnt]),
                state = cs_state_dt[cnt]
            };

            return sched;
        }




        protected override async Task OnParametersSetAsync()
        {
            if (CalendarInterop is not null)
            {
                while (_OnParameterChangeEvents.Count > 0)
                {
                    try
                    {
                        await _OnParameterChangeEvents.Dequeue();
                    }
                    catch (NullReferenceException ex)
                    {
                        //do nothing
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Since there is no subsequent rendering required by blazor after the first render, this set to false
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender()
        {
            return false;
        }

        /// <summary>
        /// Each time there is a view change or advance of the calendar, ask the calendar what date range is visible
        /// </summary>
        /// <returns></returns>
        private async Task SetDateRange()
        {
            if (CalendarInterop is not null)
            {
                await VisibleStartDateRangeChanged.InvokeAsync(await CalendarInterop.GetDateRangeStart());
                await VisibleEndDateRangeChanged.InvokeAsync(await CalendarInterop.GetDateRangeEnd());
            }
        }
    }
}
