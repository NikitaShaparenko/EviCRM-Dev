using BlazorCalendar;
using BlazorCalendar.Models;
using EviCRM.Server.Core;

namespace EviCRM.Server.Pages.Calendar
{
    public partial class CalendarSimple
    {

		MySQL_Controller mysqlc = new MySQL_Controller();
		BackendController bc = new BackendController();
		SystemCore systemcore = new SystemCore();
		Sentinel sentinel = new Sentinel();

		ModalDialog _scheduler;

		private DateTime today = DateTime.Today;
		private int months = 12;
		private List<BlazorCalendar.Models.Tasks> TasksList_Calendar;
		private string fakeConsole = "";
		private BlazorCalendar.PriorityLabel PriorityDisplay = PriorityLabel.Code;
		private bool draggable = true;

		private BlazorCalendar.DisplayedView SelectedView = DisplayedView.Monthly;
		private string HeaderStyle = "cursor:pointer";
		public bool isNewHandler = true;
		public int modal_ID { get; set; }
		public bool isAdmin { get; set; }

		List<string> cs_idcalendar_scheludes_dt = new List<string>();
		List<string> cs_id_dt = new List<string>();
		List<string> cs_calendarId_dt = new List<string>();
		List<string> cs_startDate_dt = new List<string>();
		List<string> cs_endDate_dt = new List<string>();
		List<string> cs_title_dt = new List<string>();
		List<string> cs_body_dt = new List<string>();
		List<string> cs_user_dt = new List<string>();

		List<string> cs_isinoffice = new List<string>();
		List<string> cs_notify = new List<string>();
		List<string> cs_attachments_dt = new List<string>();

		List<EviCRM.Server.Models.CalendarGraphic> cg_dt = new List<EviCRM.Server.Models.CalendarGraphic>();

		EviCRM.Server.Models.CalendarCallbackModel editViewModelBase = new EviCRM.Server.Models.CalendarCallbackModel();

		public string current_user { get; set; }

		public string modal_event_title { get; set; }
		public string modal_event_date_click { get; set; }
		public string modal_event_id { get; set; }

		private void OnDeleteDialogClose(bool accepted)
		{
			DeleteDialogOpen = false;
			StateHasChanged();
		}

		private void OpenDeleteDialog()
		{
			DeleteDialogOpen = true;
			StateHasChanged();
		}

		public bool DeleteDialogOpen { get; set; }

		public string operationModal { get; set; }

		public async Task CallbackDeleteInterpreter(int id)
		{
			int array_id = getArrayCounterByTaskID(id);
			int task_array_id = getTaskArrayCounterByTaskID(id);

			cs_id_dt.RemoveAt(array_id);
			cs_calendarId_dt.RemoveAt(array_id);
			cs_startDate_dt.RemoveAt(array_id);
			cs_endDate_dt.RemoveAt(array_id);
			cs_title_dt.RemoveAt(array_id);
			cs_body_dt.RemoveAt(array_id);
			cs_isinoffice.RemoveAt(array_id);
			cs_notify.RemoveAt(array_id);
			cs_user_dt.RemoveAt(array_id);

			cg_dt.RemoveAt(array_id);
			cs_attachments_dt.RemoveAt(array_id);

			int tl_c = task_array_id;

			if (tl_c != int.MinValue)
			{
				TasksList_Calendar.RemoveAt(tl_c);
			}
			await mysqlc.MySql_ContextAsyncL(mysqlc.deleteSchedule(cs_idcalendar_scheludes_dt[array_id].ToString()));
			cs_idcalendar_scheludes_dt.RemoveAt(array_id);

		}

		public async Task CallbackInterpreter(EviCRM.Server.Models.CalendarCallbackModel ccm)
		{
			BlazorCalendar.Models.Tasks ccm_tempate = ccm.tasks;

			switch (ccm.calendar_id)
			{
				case "corporate":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#ff0000";
					ccm_tempate.Code = "Работа";
					break;

				case "notes":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#9966cc";
					ccm_tempate.Code = "Заметки";
					break;

				case "personal":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#ffe5b4";
					ccm_tempate.Code = "Личное";
					break;

				default:
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#556832";
					ccm_tempate.Code = "Всё остальное";
					break;

			}

			EviCRM.Server.Models.CalendarGraphic ccm_cg = new EviCRM.Server.Models.CalendarGraphic();

			ccm_cg.Code = ccm_tempate.Code;
			ccm_cg.Color = ccm_tempate.Color;
			ccm_cg.ForeColor = ccm_tempate.ForeColor;

			cg_dt.Add(ccm_cg);

			TasksList_Calendar.Add(ccm_tempate);

			cs_idcalendar_scheludes_dt.Add(Guid.NewGuid().GetHashCode().ToString());
			cs_id_dt.Add(ccm.tasks.ID.ToString());
			cs_calendarId_dt.Add(ccm.calendar_id);
			cs_startDate_dt.Add(ccm.tasks.DateStart.ToString());
			cs_endDate_dt.Add(ccm.tasks.DateEnd.ToString());
			cs_title_dt.Add(ccm.tasks.Caption);
			cs_body_dt.Add(ccm.tasks.Comment);
			cs_isinoffice.Add(ccm.isInOffice.ToString());
			cs_notify.Add(ccm.isNotify.ToString());
			cs_user_dt.Add(current_user);

			cs_attachments_dt.Add(bc.getMultipleStringEncodingString(ccm.lst_attachs));

			await mysqlc.MySql_ContextAsyncL(mysqlc.setSchelude(ccm.tasks.ID.ToString(), ccm.calendar_id, ccm.tasks.DateStart.ToShortDateString(), ccm.tasks.DateEnd.ToShortDateString(), ccm.tasks.Caption, ccm.tasks.Comment, user_, ccm.isInOffice.ToString(), ccm.isInOffice.ToString(), bc.getMultipleStringEncodingString(ccm.lst_attachs)));
		}

		public async Task CallbackUpdateInterpeter(EviCRM.Server.Models.CalendarCallbackModel ccm)
		{
			BlazorCalendar.Models.Tasks ccm_tempate = ccm.tasks;

			switch (ccm.calendar_id)
			{
				case "corporate":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#ff0000";
					ccm_tempate.Code = "Работа";
					break;

				case "notes":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#9966cc";
					ccm_tempate.Code = "Заметки";
					break;

				case "personal":
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#ffe5b4";
					ccm_tempate.Code = "Личное";
					break;

				default:
					ccm_tempate.ForeColor = "#ffffff";
					ccm_tempate.Color = "#556832";
					ccm_tempate.Code = "Всё остальное";
					break;

			}

			int id = getTaskArrayCounterByTaskID(ccm_tempate.ID);

			TasksList_Calendar.RemoveAt(id);
			TasksList_Calendar.Insert(id, ccm_tempate);

			cs_id_dt[id] = ccm_tempate.ID.ToString();
			cs_calendarId_dt[id] = ccm.calendar_id;
			cs_startDate_dt[id] = ccm_tempate.DateStart.ToString();
			cs_endDate_dt[id] = ccm_tempate.DateEnd.ToString();
			cs_title_dt[id] = ccm_tempate.Caption;
			cs_body_dt[id] = ccm_tempate.Comment;
			cs_isinoffice[id] = ccm.isInOffice.ToString();
			cs_notify[id] = ccm.isNotify.ToString();
			cs_user_dt[id] = current_user;
			cs_attachments_dt[id] = bc.getMultipleStringEncodingString(ccm.lst_attachs);

			string lst_attachments = bc.getMultipleStringEncodingString(ccm.lst_attachs);

			await mysqlc.MySql_ContextAsyncL(mysqlc.updateSchelude(ccm.tasks.ID.ToString(), ccm.calendar_id, ccm.tasks.DateStart.ToShortDateString(), ccm.tasks.DateEnd.ToShortDateString(), ccm.tasks.Caption, ccm.tasks.Comment, user_, ccm.isInOffice.ToString(), ccm.isInOffice.ToString(), bc.getMultipleStringEncodingString(ccm.lst_attachs), cs_idcalendar_scheludes_dt[id].ToString()));

		}

		string user_ { get; set; }

		#region getArrays...
		int getArrayCounterByTaskID(int taskID)
		{
			for (int i = 0; i < cs_id_dt.Count; i++)
			{
				if (cs_id_dt[i] == taskID.ToString())
				{
					return i;
				}
			}
			return int.MinValue;
		}

		int getTaskArrayCounterByTaskID(int taskID)
		{
			for (int i = 0; i < TasksList_Calendar.Count; i++)
			{
				if (TasksList_Calendar[i].ID == taskID)
				{
					return i;
				}
			}
			return int.MinValue;
		}
		#endregion

		public async Task LoadScheludeData()
		{
			user_ = authStateProvider.GetAuthenticationStateAsync().Result.User.Identity.Name;

			string role = await mysqlc.MySql_ContextAsyncL(mysqlc.getProjectUsersRolesByIdentityName(user_));

			if (role == "admin" || role == "owner")
			{
				isAdmin = true;
			}
			else
			{
				isAdmin = false;
			}

			cs_idcalendar_scheludes_dt = await mysqlc.getListCalendarIDCalendarScheludesAsync();
			cs_id_dt = await mysqlc.getListCalendarIDAsync();
			cs_calendarId_dt = await mysqlc.getListCalendarCalendarIDAsync();
			cs_startDate_dt = await mysqlc.getListCalendarIDStartDateAsync();
			cs_endDate_dt = await mysqlc.getListCalendarIDEndDateAsync();
			cs_title_dt = await mysqlc.getListCalendarTitleAsync();
			cs_body_dt = await mysqlc.getListCalendarIDBodyAsync();
			cs_isinoffice = await mysqlc.getListCalendarIsInOfficeAsync();
			cs_notify = await mysqlc.getListCalendarIsNotifyAsync();

			cs_attachments_dt = await mysqlc.getListCalendarAttachmentsAsync();
			cs_user_dt = await mysqlc.getListCalendarUserAsync();


			foreach (string str in cs_calendarId_dt)
			{
				EviCRM.Server.Models.CalendarGraphic cg_elem = new EviCRM.Server.Models.CalendarGraphic();
				switch (str)
				{
					case "corporate":
						cg_elem.ForeColor = "#ffffff";
						cg_elem.Color = "#ff0000";
						cg_elem.Code = "Работа";
						break;

					case "notes":
						cg_elem.ForeColor = "#ffffff";
						cg_elem.Color = "#9966cc";
						cg_elem.Code = "Заметки";
						break;

					case "personal":
						cg_elem.ForeColor = "#ffffff";
						cg_elem.Color = "#ffe5b4";
						cg_elem.Code = "Личное";
						break;

					default:
						cg_elem.ForeColor = "#ffffff";
						cg_elem.Color = "#556832";
						cg_elem.Code = "Всё остальное";
						break;
				}
				cg_dt.Add(cg_elem);
			}

			TasksList_Calendar.Clear();


			for (int i = 0; i < cs_id_dt.Count; i++)
			{
				if (cs_user_dt[i] == user_)
				{
					TasksList_Calendar.Add(ScheduleGenerator(i));
				}
			}

			//StateHasChanged();
		}

		public BlazorCalendar.Models.Tasks ScheduleGenerator(int i)
		{
			BlazorCalendar.Models.Tasks schedule = new BlazorCalendar.Models.Tasks
			{
				ID = int.Parse(cs_id_dt[i]),
				DateStart = DateTime.Parse(cs_startDate_dt[i]),
				DateEnd = DateTime.Parse(cs_endDate_dt[i]),
				Code = cg_dt[i].Code,
				Color = cg_dt[i].Color,
				Caption = cs_title_dt[i],
				Comment = cs_body_dt[i],
				ForeColor = cg_dt[i].ForeColor,
				NotBeDraggable = false,
			};
			return schedule;
		}

        protected override async Task OnInitializedAsync()
        {
			TasksList_Calendar = new();
			current_user = user_;

			await LoadScheludeData();
			await InvokeAsync(StateHasChanged);
		}

		private void DayClick(ClickEmptyDayParameter clickEmptyDayParameter)
		{
			fakeConsole = "Empty day :" + clickEmptyDayParameter.Day.ToShortDateString();
			isNewHandler = true;
			modal_event_date_click = clickEmptyDayParameter.Day.ToShortDateString();
			operationModal = "CREATE";
			OpenDeleteDialog();
		}


		private void TaskClick(ClickTaskParameter clickTaskParameter)
		{
			fakeConsole = "ID task(s) :" + string.Join(", ", clickTaskParameter.IDList);

			operationModal = "EDIT";

			modal_ID = clickTaskParameter.IDList[0];
			int i = getArrayCounterByTaskID(modal_ID);

			if (isAdmin && cs_calendarId_dt[i] == "corporate")
			{
				operationModal = "EDIT";
			}
			if (!isAdmin && cs_calendarId_dt[i] == "corporate")
			{
				operationModal = "VIEW";
			}

			if (i != int.MinValue)
			{
				editViewModelBase.tasks.ID = int.Parse(cs_id_dt[i]);
				editViewModelBase.tasks.DateStart = DateTime.Parse(cs_startDate_dt[i]);
				editViewModelBase.tasks.DateEnd = DateTime.Parse(cs_endDate_dt[i]);
				editViewModelBase.tasks.Code = cg_dt[i].Code;
				editViewModelBase.tasks.Color = cg_dt[i].Color;
				editViewModelBase.tasks.Caption = cs_title_dt[i];
				editViewModelBase.tasks.Comment = cs_body_dt[i];
				editViewModelBase.tasks.ForeColor = cg_dt[i].ForeColor;
				editViewModelBase.isInOffice = bool.Parse(cs_isinoffice[i]);
				editViewModelBase.isNotify = bool.Parse(cs_notify[i]);
				editViewModelBase.calendar_id = cs_calendarId_dt[i];
				editViewModelBase.lst_attachs = bc.getMultipleStringDecodingString(cs_attachments_dt[i]);
			}

			OpenDeleteDialog();
		}

	}
}
