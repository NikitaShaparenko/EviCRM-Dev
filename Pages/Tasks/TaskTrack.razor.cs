using EviCRM.Server.Core;

using EviCRM.Server.Core;
using System.Collections.Generic;
using System.Net;

using EviCRM.Server.Models;
using EviCRM.Server.ViewModels;
using System.Net.Sockets;
using System.Globalization;
using EviCRM.Server.Pages.Tasks;
using Majorsoft.Blazor.Extensions.BrowserStorage;

using System.Web.Mvc;

using Blzr.BootstrapSelect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Majorsoft.Blazor.Components.Notifications;

namespace EviCRM.Server.Pages.Tasks
{
    public partial class TaskTrack
    {
        public string ttm_commit_title { get; set; }
        public string dlist { get; set; }

        public List<string> workers = new List<string>();

        bool ready = false;
        private int _localStorageCount;
        private IList<KeyValuePair<string, string>> _localStorageItems;

        private BackendController bc = new BackendController();
        private MySQL_Controller mysqlc = new MySQL_Controller();
        private Sentinel sentinel = new Sentinel();

        List<string> tasks_name_dt = new List<string>();
        List<string> tasks_budgets_dt = new List<string>();
        List<string> tasks_members_dt = new List<string>();
        List<string> tasks_description_dt = new List<string>();
        List<string> tasks_members_divs_dt = new List<string>();
        List<string> tasks_status_dt = new List<string>();
        List<string> tasks_author_dt = new List<string>();
        List<string> tasks_start_dt = new List<string>();
        List<string> tasks_end_dt = new List<string>();
        List<string> tasks_proj_dt = new List<string>();

        List<string> tasktracking_author = new List<string>();
        List<string> tasktracking_author_view_patched = new List<string>();
        List<string> tasktracking_cmd = new List<string>();
        List<string> tasktracking_datetime = new List<string>();
        List<string> tasktracking_var1 = new List<string>();
        List<string> tasktracking_var2 = new List<string>();
        List<string> tasks_id_dt = new List<string>();
        List<string> tasks_personal_status_dt = new List<string>();

        List<string> tasktracking_var3 = new List<string>();
        List<string> users_dt = new List<string>();

        //Массивы отвечающие за часть, которая а-ля Трелло
        List<string> card_body = new List<string>();
        List<string> card_id = new List<string>();
        List<string> card_marksdown = new List<string>();
        List<string> card_author = new List<string>();
        List<string> card_status = new List<string>();
        List<string> card_action_datetime = new List<string>();
        List<string> card_delegate_dt = new List<string>();

        List<string> fullname_dt = new List<string>();
        List<string> divisions_ids = new List<string>();
        List<string> divisions_name = new List<string>();

        List<string> users_avatars_dt = new List<string>();
        List<string> divs_avatars_dt = new List<string>();

        List<string> personal_status { get; set; }
        
        List<string> personal_status_arg1 {get;set;}
        List<string> personal_status_arg2 {get;set;}

        string task_personal_status { get; set; }

        int task_id { get; set; }
        string task_id_cookie { get; set; }
        bool isAdmin { get; set; }

        List<string> header_projs = new List<string>();

        string current_task_status { get; set; }

        string current_task_personal_status { get; set; }

        public void TaskStatusLoader()
        {

        }

        [Parameter] public List<string> users_avatars { get; set; }
        [Parameter] public List<string> users_list { get; set; }

        public async Task OnCloseFailClosed()
        {
            modalCloseFailedOpened = false;
            await UpdateTaskTrackingCarousel();

            StateHasChanged();
        }

        public int getUserArrIDByLogin(string login)
        {
            for (int p = 0; p < users_list.Count; p++)
            {
                if (users_list[p] == login)
                {
                    return p;
                }
            }
            return 0;
        }
        public async Task OnRemindBoardClosed()
        {
            modalRemindOpened = false;
            await UpdateTaskTrackingCarousel();

            StateHasChanged();
        }

        string header_task_author { get; set; }
        bool modalChangeOpened { get; set; }
        bool modalBeforeHeadOpened { get; set; }
        bool modalRemindOpened { get; set; }
        bool modalCloseFailedOpened { get; set; }
        bool modalRevisionOpened { get; set; }
        bool modalDateChangeOpened { get; set; }

        bool task_tracking_carousel_loaded { get; set; }
        private void ModalChangeOpen()
        {
            modalChangeOpened = true;
            StateHasChanged();
        }

       async Task OnCloseRevisionClosed(bool accepted)
        {
            modalRevisionOpened = false;
            await UpdateTaskTrackingCarousel();
            StateHasChanged();
        }

       async Task OnUpdateRevision(string str)
        {
            current_task_status = str;
            await UpdateTaskTrackingCarousel();
            StateHasChanged();
        }

        async Task ModalBeforeHeadOpen()
        {
            TOScript to_script = new TOScript();

            TaskTrackingModelCommon ttmc = new TaskTrackingModelCommon();
            ttmc.task_author = task_id_cookie;
            ttmc.task_id = task_id.ToString();

            await to_script.COMPLETEBEFOREHAND(ttmc);
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateTaskStatus(task_id.ToString(), "completed"));

            current_task_status = "completed";

            await UpdateTaskTrackingCarousel();
            StateHasChanged();
        }

        private async Task PersonalStatusChange(string new_personal_status)
        {
            global_personal_status = new_personal_status;
            int z = getPersonalStatusArrayIDByUsername(user_);
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.update_personal_status(task_id.ToString(), personal_status_updater_packer(z, global_personal_status)));
            await UpdateTaskTrackingCarousel();
            await UpdatePersonalStatus();
        }

        private async Task UpdatePersonalStatus()
        {
            tasks_personal_status_dt = await mysqlc.getListTaskPersonalStatusByIDAsync(task_id.ToString());
            if (tasks_personal_status_dt.Count > 0)
            {
                task_personal_status = tasks_personal_status_dt[0];
            }

            personal_status = bc.getMultipleStringDecodingString(task_personal_status); //разбились на пары
            personal_status_arg1 = bc.getMultArgs_DeCombine_Args1(personal_status); //шапки пар
            personal_status_arg2 = bc.getMultArgs_DeCombine_Args2(personal_status); // значения пар

            int z = getPersonalStatusArrayIDByUsername(user_);
            if (personal_status_arg2.Count > 0)
            {
                global_personal_status = personal_status_arg2[z];
            }
            InvokeAsync(StateHasChanged);
        }
        private void ModalRemindOpen()
        {
            modalRemindOpened = true;
            StateHasChanged();
        }

        private void ModalCloseFailedOpen()
        {
            modalCloseFailedOpened = true;
            StateHasChanged();
        }

        private void ModalRevisionOpen()
        {
            modalRevisionOpened = true;
            StateHasChanged();
        }

        #region Toasts Notifications

        //Toasts
        private async Task ToastsNotifications_ShowAllToast()
        {
            foreach (var item in Enum.GetValues<NotificationTypes>())
            {
                _toastService.ShowToast($@"<strong>Toast оповещения:</strong> Это оповещение типа {item}", item);
            }
        }

        private Guid _lastToastId;

        private async Task ToastsNotifications_ShowCustomToast(string toastText, NotificationStyles ns, NotificationTypes ntype)
        {
            _lastToastId = _toastService.ShowToast(new ToastSettings()
            {
                Content = builder => builder.AddMarkupContent(0, toastText),
                NotificationStyle = ns,
                Type = ntype,
                AutoCloseInSec = _toastAutoCloseInSec,
                ShadowEffect = _toastShadowEffect,
                ShowCloseButton = _toastShowCloseButton,
                ShowCloseCountdownProgress = _toastShowCountdownProgress,
                ShowIcon = _toastShowIcon
            });
        }

        private async Task ToastsNotifications_ShowCustomToast_Primary(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Primary);
        }
        private async Task ToastsNotifications_ShowCustomToast_Secondary(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Secondary);
        }
        private async Task ToastsNotifications_ShowCustomToast_Info(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Info);
        }
        private async Task ToastsNotifications_ShowCustomToast_Success(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Success);
        }
        private async Task ToastsNotifications_ShowCustomToast_Warning(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Warning);
        }
        private async Task ToastsNotifications_ShowCustomToast_Danger(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Danger);
        }
        private async Task ToastsNotifications_ShowCustomToast_Light(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Light);
        }
        private async Task ToastsNotifications_ShowCustomToast_Dark(string toastText)
        {
            await ToastsNotifications_ShowCustomToast(toastText, NotificationStyles.Normal, NotificationTypes.Dark);
        }
        private async Task RemoveAllToasts()
        {
            _toastService.ClearAll();
        }
        private async Task RemoveLastToasts()
        {
            if (_lastToastId != Guid.Empty)
            {
                _toastService.RemoveToast(_lastToastId);
            }
        }
        #endregion

        public async Task ShowCustomNotification(ToastNotification toast)
        {
            switch(toast.get_ToastType())
            {
                case ToastNotification.ToastsTypes.Primary:
                    await ToastsNotifications_ShowCustomToast_Primary(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Secondary:
                    await ToastsNotifications_ShowCustomToast_Secondary(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Info:
                    await ToastsNotifications_ShowCustomToast_Info(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Success:
                    await ToastsNotifications_ShowCustomToast_Success(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Warning:
                    await ToastsNotifications_ShowCustomToast_Warning(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Danger:
                    await ToastsNotifications_ShowCustomToast_Danger(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Light:
                    await ToastsNotifications_ShowCustomToast_Light(toast.get_BodyText());
                    break;

                case ToastNotification.ToastsTypes.Dark:
                    await ToastsNotifications_ShowCustomToast_Dark(toast.get_BodyText());
                    break;
            }
        }

        public void SendTOScriptCmd(string simple_task)
        {

        }

        public void SendTOScriptCmd(string task_code, string args1)
        {

        }

        public void SendTOScriptCmd(string task_code, string args1, string args2)
        {

        }

        public string getDivNameByDivID(string div_id)
        {
            for (int i = 0; i<divisions_ids.Count;i++)
            {
                if (divisions_ids[i]==div_id)
                {
                    return divisions_name[i];
                }
            }
            return "( Отдел без имени)";
        }

        private void On_modalChangeOpen(bool accepted)
        {
            modalChangeOpened = false;
            StateHasChanged();

        }

        async Task RefreshUserList()
        {
            tasks_members_dt = await mysqlc.getListTaskMembersByIDAsync(task_id.ToString());
            tasks_members_divs_dt = await mysqlc.getListTaskMembersDivByIDAsync(task_id.ToString());
            await InvokeAsync(StateHasChanged);
        }

        public bool isSecondStep { get; set; }

        public async Task DateUpdates(string date_st, string date_fin)
        {
            await mysqlc.MySql_ContextAsyncL(mysqlc.updateTaskTrackDateStartByID(tasks_start_dt[0], task_id.ToString()));
            await mysqlc.MySql_ContextAsyncL(mysqlc.updateTaskTrackDateEndByID(tasks_end_dt[0], task_id.ToString()));
            StateHasChanged();
        }

        bool step2_allow { get; set; }
        TOScript to_script = new TOScript();

        TaskTracking.TaskTracking_DownActionBar ttd = new TaskTracking.TaskTracking_DownActionBar();

        async Task RogueConfirm()
        {
            if (DateTime.Parse(tasks_start_dt[0])>DateTime.Now)
            {
                global_personal_status = "approved";
            }
            else if (DateTime.Parse(tasks_end_dt[0])>DateTime.Now && DateTime.Parse(tasks_start_dt[0])<DateTime.Now)
            {
                global_personal_status = "pending";
            }
            else
            {
                global_personal_status = "delayed";
            }
            ttd.interop_setNewGlobalPersonalStatus(global_personal_status);

            EviCRM.Server.ViewModels.TaskTrackingModelCommon ttmc = new EviCRM.Server.ViewModels.TaskTrackingModelCommon();

            ttmc.task_author = user_;
            ttmc.task_id = task_id.ToString();

            await to_script.ROGER(ttmc);

            personal_status = bc.getMultipleStringDecodingString(task_personal_status); //разбились на пары
            personal_status_arg1 = bc.getMultArgs_DeCombine_Args1(personal_status); //шапки пар
            personal_status_arg2 = bc.getMultArgs_DeCombine_Args2(personal_status); // значения пар

            int z = getPersonalStatusArrayIDByUsername(user_);
            global_personal_status = global_personal_status;

            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.update_personal_status(task_id.ToString(),personal_status_updater_packer(z, global_personal_status)));
            await UpdateTaskTrackingCarousel();


            StateHasChanged();
        }

        string personal_status_updater_packer(int p,string value)
        {

            string str = "";

            for (int i = 0; i < personal_status.Count; i++)
            {
                if (p != i)
                {
                    str += personal_status[i] + "$$$";
                }
                else
                {
                    str += personal_status_arg1[i] + "$===$" + value + "$$$";
                }
            }

            personal_status_arg2[p] = value;
            global_personal_status = value;

            return str ;
        }

        async Task UpdateTaskTrackingCarousel()
        {
            task_tracking_carousel_loaded = false;

            tasktracking_cmd.Clear();
            tasktracking_datetime.Clear();
            tasktracking_var1.Clear();
            tasktracking_var2.Clear();
            tasktracking_var3.Clear();
            tasktracking_author.Clear();
            tasktracking_author_view_patched.Clear();
            StateHasChanged();
            Task.Delay(150);


            tasktracking_author = await mysqlc.getListTaskTrackingAuthorAsync(task_id.ToString());
            tasktracking_datetime = await mysqlc.getListTaskTrackingMainDateTimeAsync(task_id.ToString());
            tasktracking_var1 = await mysqlc.getListTaskTrackingMainVar1Async(task_id.ToString());
            tasktracking_var2 = await mysqlc.getListTaskTrackingMainVar2Async(task_id.ToString());
            tasktracking_var3 = await mysqlc.getListTaskTrackingMainVar3Async(task_id.ToString());
            tasktracking_cmd = await mysqlc.getListTaskTrackingMainCmdAsync(task_id.ToString());

            for (int i = 0; i < tasktracking_author.Count; i++)
            {
                tasktracking_author_view_patched.Add(getUsernameByLogin(tasktracking_author[i]));
            }

            task_tracking_carousel_loaded = true;
            StateHasChanged();
        }

        async Task OnOkFailClosed()
        {
            modalCloseFailedOpened = false;
            current_task_status = "failed";
            await UpdateTaskTrackingCarousel();
            StateHasChanged();
        }

        public void OnUpdatingDtStart(TaskTrackingChangeDatesModel ttcdm)
        {
            tasks_start_dt[0] = ttcdm.dt_start.ToShortDateString();
            tasks_end_dt[0] = ttcdm.dt_end.ToShortDateString();
            Task task = new Task(async () => await DateUpdates(ttcdm.dt_start.ToShortDateString(), ttcdm.dt_end.ToShortDateString()));
            task.RunSynchronously();
            StateHasChanged();
        }

        string user_ { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                new Timer(DateTimeCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }

        public async Task ModalDatesChangeOpen()
        {
            modalDateChangeOpened = true;
            await InvokeAsync(StateHasChanged);

        }

        private void DateTimeCallback(object state)
        {
            InvokeAsync(() => StateHasChanged());
        }

        string global_personal_status { get; set; }

        public string getUsernameByLogin(string login)
        {
            string username_full = "";

            for(int z = 0; z< users_dt.Count; z++)
            {
                if (users_dt[z] == login)
                {
                    return fullname_dt[z];
                }
            }

            return username_full;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                task_tracking_carousel_loaded = false;
                if (await localStorage.GetItemAsStringAsync("task_tracking_id") == null)
                {
                    task_id_cookie = "";
                }
                else
                {
                    task_id_cookie = await localStorage.GetItemAsStringAsync("task_tracking_id");
                }

                user_ = authStateProvider.GetAuthenticationStateAsync().Result.User.Identity.Name;

                string role = await mysqlc.MySql_ContextAsyncL(mysqlc.getProjectUsersRolesByIdentityName(user_));
                if (role == "admin" || role == "owner")
                {
                    isAdmin = true;
                }

                if (task_id_cookie != null && task_id_cookie != "")
                {
                    if (int.TryParse(task_id_cookie, out int a))
                    {
                        task_id = int.Parse(task_id_cookie);
                    }
                }

                tasks_description_dt = await mysqlc.getListTaskDescriptionAsync(task_id.ToString());

                tasks_name_dt = await mysqlc.getListTaskNameByIDAsync(task_id.ToString());
                tasks_budgets_dt = await mysqlc.getListTaskBudgetByIDAsync(task_id.ToString());
                
                tasks_members_dt = await mysqlc.getListTaskMembersByIDAsync(task_id.ToString());
                tasks_members_divs_dt = await mysqlc.getListTaskMembersDivByIDAsync(task_id.ToString());

                tasks_status_dt = await mysqlc.getListTaskStatusByIDAsync(task_id.ToString());
                tasks_author_dt = await mysqlc.getListTasksAuthorByIDAsync(task_id.ToString());
                tasks_start_dt = await mysqlc.getListTaskStartDateByIDAsync(task_id.ToString());
                tasks_end_dt = await mysqlc.getListTaskEndDateByIDAsync(task_id.ToString());
                tasks_proj_dt = await mysqlc.getListTaskProjByIDAsync(task_id.ToString());
                tasktracking_author = await mysqlc.getListTaskTrackingAuthorAsync(task_id.ToString());

                for (int i = 0; i < tasktracking_author.Count; i++)
                {
                    tasktracking_author_view_patched.Add(getUsernameByLogin(tasktracking_author[i]));
                }

                tasktracking_cmd = await mysqlc.getListTaskTrackingMainCmdAsync(task_id.ToString());
                tasktracking_datetime = await mysqlc.getListTaskTrackingMainDateTimeAsync(task_id.ToString());
                tasktracking_var1 = await mysqlc.getListTaskTrackingMainVar1Async(task_id.ToString());
                tasktracking_var2 = await mysqlc.getListTaskTrackingMainVar2Async(task_id.ToString());
                tasktracking_var3 = await mysqlc.getListTaskTrackingMainVar3Async(task_id.ToString());
                users_dt = await mysqlc.getListUsersLoginAsync();

                tasks_personal_status_dt = await mysqlc.getListTaskPersonalStatusByIDAsync(task_id.ToString());
                card_body = await mysqlc.getListTaskTrackingCardBodyAsync(task_id.ToString());
                card_marksdown = await mysqlc.getListTaskTrackingCardMarksDownAsync(task_id.ToString());
                card_delegate_dt = await mysqlc.getListTaskTrackingCardDelegateAsync(task_id.ToString());
                card_author = await mysqlc.getListTaskTrackingCardActionAuthorAsync(task_id.ToString());
                card_status = await mysqlc.getListTaskTrackingCardStatusAsync(task_id.ToString());
                card_action_datetime = await mysqlc.getListTaskTrackingCardActionDateTimeAsync(task_id.ToString());
                card_id = await mysqlc.getListTaskTrackingCardIDAsync(task_id.ToString());

                fullname_dt = await mysqlc.getListUsersFullnameAsync();
                divisions_ids = await mysqlc.getListDivisionsIDAsync();
                divisions_name = await mysqlc.getListDivisionsNameAsync();

                tasks_id_dt = await mysqlc.getListTaskIDAsync();

                users_avatars_dt = await mysqlc.getListUsersAvatarAsync();
                divs_avatars_dt = await mysqlc.getListDivisionsAvatarAsync();

                header_task_author = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(tasks_author_dt[0]));

                foreach (string strc in bc.getMultipleStringDecodingString(tasks_proj_dt[0]))
                {
                    header_projs.Add(await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getProjectsNameByID(strc)));
                }

                ready = true;

                current_task_status = tasks_status_dt[0];

                if (tasks_personal_status_dt.Count > 0)
                {
                    task_personal_status = tasks_personal_status_dt[0];
                }

                personal_status = bc.getMultipleStringDecodingString(task_personal_status); //разбились на пары
                personal_status_arg1 = bc.getMultArgs_DeCombine_Args1(personal_status); //шапки пар
                personal_status_arg2 = bc.getMultArgs_DeCombine_Args2(personal_status); // значения пар

                int z = getPersonalStatusArrayIDByUsername(user_);
                if (personal_status_arg2.Count > 0)
                { 
                global_personal_status = personal_status_arg2[z];
                }

                task_tracking_carousel_loaded = true;

                await UpdateTaskTrackingCarousel();
                StateHasChanged();


            }
        }

        public async Task TaskStatusUpdate(string new_status)
        {
            current_task_status = new_status;
            await UpdateTaskTrackingCarousel();
            StateHasChanged();
        }

        public int getPersonalStatusArrayIDByUsername(string username)
        {
            for (int i = 0; i < personal_status_arg1.Count; i++)
            {
                if (personal_status_arg1[i] == username)
                {
                    return i;
                }
            }
            return 0;
        }

        protected async override Task OnInitializedAsync()
        {


        }

        private async Task<string> GetLocalStorageItems(string key)
        {
            return await localStorage.GetItemAsStringAsync(key);
        }


        public void UpdatingDateChangeInterpreter(List<DateTime> dt_lst)
        {
            if (dt_lst.Count > 0)
            {
                tasks_end_dt[0] = dt_lst[1].ToString();
                tasks_start_dt[0] = dt_lst[0].ToString();
                //mysql place
            }
            StateHasChanged();
        }

        public void OnCloseDateChanger(bool accepted)
        {
            if (accepted)
            {
                modalDateChangeOpened = false;
                StateHasChanged();
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            string filename = Path.GetFileNameWithoutExtension(fileName);
            filename = filename.Substring(0, 60);

            if (fileName.Length > 60)
            {
                fileName = fileName.Substring(0, 60);
            }


            return (filename)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);

        }

        private void ErrorModalHandler(bool accepted)
        {
            UriHelper.NavigateTo("/tasks_list", true);
            StateHasChanged();

        }

 

    }
}
