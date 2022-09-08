using EviCRM.Server.Core;
using Majorsoft.Blazor.Components.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EviCRM.Server.Pages.Calendar
{
    public partial class CalendarTasks
    {
        #region Toasts Notifications

        private string _toastText = $@"<strong>Toast:</strong> This is a(n) {NotificationTypes.Primary} notification";
        private bool _toastShowIcon = true;
        private bool _toastShowCloseButton = true;
        private bool _toastShowCountdownProgress = true;
        private uint _toastAutoCloseInSec = 5;
        private uint _toastShadowEffect = 5;
        private NotificationStyles _toastStyle;
        private NotificationTypes _toastTypeLevel;

        private ElementReference log1;
        private string _toastLog = "";

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
        private Guid _lastToastId;

        #endregion

        #region DTP
        void DatePickerChanged_start__(ChangeEventArgs e)
        {
            var selectedStartDateString = e.Value.ToString();
            modal_event_startdate = DateTime.Parse(selectedStartDateString);

        }

        void DatePickerChanged_end__(ChangeEventArgs e)
        {
            var selectedStartDateString = e.Value.ToString();
            modal_event_enddate = DateTime.Parse(selectedStartDateString);

        }

        async Task HandledateChange_DTP_Start(ChangeEventArgs args)
        {
            modal_event_startdate = DateTime.Parse(args.Value.ToString());
        }
        async Task HandledateChange_DTP_End(ChangeEventArgs args)
        {
            modal_event_enddate = DateTime.Parse(args.Value.ToString());
        }
        #endregion

        SystemCore systemcore = new SystemCore();

        public Task DeleteAttachment(int j)
        {
            attach_lst.RemoveAt(j);
            attach_lst_links.RemoveAt(j);
            return Task.CompletedTask;
        }

        string getUnicalFileName(string filename_we, string extension)
        {
            string guid_str = Guid.NewGuid().ToString();
            return (filename_we + "_" + Path.GetRandomFileName() + extension);
        }

        protected async override Task OnInitializedAsync()
        {

            switch (operationType)
            {
                case "CREATE":
                    modal_title = "Создание события";
                    modal_id = Guid.NewGuid().GetHashCode();

                    modal_event_title = "";
                    modal_event_description = "";
                    modal_calendar_type = "personal";
                    modal_remind = false;
                    modal_type_of_work = "office";

                    break;
                case "EDIT":
                    modal_title = "Редактирование события";
                    modal_id = ID;
                    break;
                case "VIEW":
                    modal_title = "Просмотр события";
                    modal_id = ID;
                    break;
            }
            isFileLoaded = false;

            if (event_Date == null)
            {

                modal_event_startdate = DateTime.Now;
                modal_event_enddate = DateTime.Now;
            }
            else
            {
                modal_event_startdate = DateTime.Parse(event_Date);
                modal_event_enddate = DateTime.Parse(event_Date);
            }


            if (operationType == "EDIT" || operationType == "VIEW")
            {
                modal_event_title = editViewModel.tasks.Caption;
                modal_event_description = editViewModel.tasks.Comment;
                modal_event_startdate = editViewModel.tasks.DateStart;
                modal_event_enddate = editViewModel.tasks.DateEnd;
                modal_remind = editViewModel.isNotify;
                modal_calendar_type = editViewModel.calendar_id;
                attach_lst = editViewModel.lst_attachs;
                foreach (string att_lnk in attach_lst)
                {
                    attach_lst_links.Add("https://evicrm.store/uploads/calendar/" + att_lnk);
                }

                if (!editViewModel.isInOffice)
                {
                    modal_type_of_work = "freelance";
                }
                else
                {
                    modal_type_of_work = "office";
                }

            }

            file_fileName = "";
            file_UploadedPercentage = 0;
            file_Size = 0;
            file_UploadedBytes = 0;

            remindChecked = false;
            modal_calendar_remind_type = "";

            StateHasChanged();

        }

        private async Task ModalSaveChanges()
        {
            EviCRM.Server.Models.CalendarCallbackModel e_ccm = new EviCRM.Server.Models.CalendarCallbackModel();

            e_ccm.tasks.ID = modal_id;
            e_ccm.calendar_id = modal_calendar_type;
            e_ccm.tasks.Caption = modal_event_title;
            e_ccm.tasks.Comment = modal_event_description;
            e_ccm.tasks.DateStart = modal_event_startdate;
            e_ccm.tasks.DateEnd = modal_event_enddate;

            e_ccm.lst_attachs = attach_lst;
            if (e_ccm.tasks.DateEnd < e_ccm.tasks.DateStart)
            {
                await ToastsNotifications_ShowCustomToast_Danger("Дата конца не может быть раньше даты начала!");
            }
            else
            {
                if (modal_type_of_work == "office")
                {
                    e_ccm.isInOffice = true;
                }
                else
                {
                    e_ccm.isInOffice = false;
                }

                e_ccm.isNotify = modal_remind;

                await OnUpdate.InvokeAsync(e_ccm);
                await OnClose.InvokeAsync(true);
            }
        }

        private async Task ModalSave()
        {
            EviCRM.Server.Models.CalendarCallbackModel e_ccm = new EviCRM.Server.Models.CalendarCallbackModel();

            e_ccm.tasks.ID = modal_id;
            e_ccm.calendar_id = modal_calendar_type;
            e_ccm.tasks.Caption = modal_event_title;
            e_ccm.tasks.Comment = modal_event_description;
            e_ccm.tasks.DateStart = modal_event_startdate;
            e_ccm.tasks.DateEnd = modal_event_enddate;

            if (e_ccm.tasks.DateEnd < e_ccm.tasks.DateStart)
            {
                await ToastsNotifications_ShowCustomToast_Danger("Дата конца не может быть раньше даты начала!");
            }
            else
            {

                e_ccm.lst_attachs = attach_lst;

                if (modal_type_of_work == "office")
                {
                    e_ccm.isInOffice = true;
                }
                else
                {
                    e_ccm.isInOffice = false;
                }

                e_ccm.isNotify = modal_remind;

                await event_callback.InvokeAsync(e_ccm);
                await OnClose.InvokeAsync(true);
            }
        }

        async Task OnInputFileChange(InputFileChangeEventArgs e, string unique_filename, int FileNumber)
        {
            var startIndex = uploadedFiles.Count;
            
            await using var timer = new Timer(_ => InvokeAsync(() => StateHasChanged()));
            timer.Change(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));

            const long CHUNKSIZE = 1024 * 400; // subjective

            IBrowserFile file = null;

            if (e.FileCount > 1)
            {
                int file_c = 0;
                var file_dump_col = e.GetMultipleFiles(e.FileCount);
                foreach (IBrowserFile file_in in file_dump_col)
                {
                    if (file_c == FileNumber)
                    {
                        file = file_dump_col[file_c];
                    }
                    file_c++;
                }
            }
            else
            {
                file = e.File;
            }

            long uploadedBytes = 0;
            long totalBytes = file.Size;
            long percent = 0;
            int fragment = 0;
            long chunkSize;

            var clientlocal = ClientFactory.CreateClient("LocalApi");

            using (var inStream = file.OpenReadStream(long.MaxValue))
            {
                _uploading = true;
                while (_uploading)
                {
                    try
                    {
                        chunkSize = CHUNKSIZE;
                        if (uploadedBytes + CHUNKSIZE > totalBytes)
                        {
                            chunkSize = totalBytes - uploadedBytes;
                        }
                        var chunk = new byte[chunkSize];
                        await inStream.ReadAsync(chunk, 0, chunk.Length);
                        using var formFile = new MultipartFormDataContent();
                        var fileContent = new StreamContent(new MemoryStream(chunk));
                        formFile.Add(fileContent, "file", unique_filename);
                        var response = await clientlocal.PostAsync($"api/CalendarUploadFile/{fragment++}", formFile);
                        uploadedBytes += chunkSize;
                        percent = uploadedBytes * 100 / totalBytes;

                        file_UploadedPercentage = percent;
                        file_Size = totalBytes;
                        file_UploadedBytes = uploadedBytes;
                        file_fileName = file.Name;

                        systemcore.Log("CalendarTasks", "Начинается загрузка большого файла '" + unique_filename + "'", SystemCore.LogLevels.Info);
                        systemcore.Log("CalendarTasks", $"Загружено {percent}%  {uploadedBytes} из {totalBytes} | Фрагмент: {fragment}", SystemCore.LogLevels.Debug);

                        if (percent >= 100)
                        {
                            //Загрузка завершена

                            _uploading = false;
                            file_UploadedPercentage = 0;
                            file_Size = 0;
                            file_UploadedBytes = 0;
                            file_fileName = "";
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    catch (Exception ex)
                    {
                        systemcore.Log("CalendarTasks", "Произошла ошибка при загрузке большого файла '" + unique_filename + "'", SystemCore.LogLevels.Error);
                    }
                }
            }
        }
    }
}
