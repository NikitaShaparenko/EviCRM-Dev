using Blazorise.RichTextEdit;
using EviCRM.Server.ViewModels;
using Majorsoft.Blazor.Components.Tabs;
using Meziantou.Framework;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace EviCRM.Server.Pages.Tasks.TaskTracking
{
    public partial class TaskTracking_DownActionBar
    {
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

        List<string> _attachments = new List<string>();

        string FormatBytes(long value)
           => ByteSize.FromBytes(value).ToString("fi2", CultureInfo.CurrentCulture);

        record FileUploadProgress(string FileName, long Size)
        {
            public long UploadedBytes { get; set; }
            public double UploadedPercentage => (double)UploadedBytes / (double)Size * 100d;
        }

        bool _uploading;
        string echo;

        string SelectedValues2 { get; set; }

        string SelectedValues { get; set; }

        List<string> SelectedValues_rtfWarningWhom_personal { get; set; }
        List<string> SelectedValues_rtfWarningWhom_divs { get; set; }

        List<string> SelectedValues_ttm_message_whom_personal { get; set; }
        List<string> SelectedValues_ttm_message_whom_div { get; set; }

        List<string> SelectedValues_ttm_question_whom_personal { get; set; }
        List<string> SelectedValues_ttm_question_whom_div { get; set; }

        List<string> SelectedValues_ttm_commit_personal { get; set; }
        List<string> SelectedValues_ttm_commit_divs { get; set; }

        [Parameter]
        public List<string> personal_status_arg1 { get; set; }

        [Parameter]
        public List<string> personal_status_arg2 { get; set; }

        [Parameter]
        public List<string> personal_status { get; set; }

        [Parameter]
        public EventCallback<bool> OnCarouselRefresh { get; set; }

        protected override async Task OnInitializedAsync()
        {
            select_data = new List<SelectData>();
            select_data_divs = new List<SelectData>();
            select_data_personal = new List<SelectData>();


            for (int i = 0; i < divisions_ids.Count; i++)
            {
                SelectData sd = new SelectData();
                sd.Id = divisions_ids[i];
                sd.Name = divisions_name[i];
                select_data_divs.Add(sd);
            }
            for (int i = 0; i < users_dt.Count; i++)
            {
                SelectData sd = new SelectData();
                sd.Id = users_dt[i];
                sd.Name = fullname_dt[i];
                select_data_personal.Add(sd);
            }

            foreach (string att_lnk in _attachments)
            {
                attach_lst_links.Add("https://evicrm.store/uploads/tasktracking/" + att_lnk);
            }
            file_fileName = "";
            file_UploadedPercentage = 0;
            file_Size = 0;
            file_UploadedBytes = 0;

            StateHasChanged();
        }

        protected RichTextEdit richTextEditRef;
        protected bool readOnly;
        protected string contentAsHtml;
        protected string contentAsDeltaJson;
        protected string contentAsText;
        protected string savedContent;

        private IList<SelectData> select_data;

        private IList<SelectData> select_data_personal;
        private IList<SelectData> select_data_divs;


        public async Task OnSave()
        {
            savedContent = await richTextEditRef.GetHtmlAsync();
            await richTextEditRef.ClearAsync();
        }

        public void OnCloseRefuseDoing(bool accepted)
        {
            modal_RefuseDoingOpened = false;
            
            if (accepted)
            {
                OnPersonalStatusChange.InvokeAsync("failed");
            }

            OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        public void OnCloseFinishTask(bool accepted)
        {
            modal_finishTaskOpened = false;

            if (accepted)
            {
                OnPersonalStatusChange.InvokeAsync("completed");
            }

            OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        public void OnCloseDelay(bool accepted)
        {
            modal_DelayOpened = false;

            if (accepted)
            {
                OnPersonalStatusChange.InvokeAsync("delayed");
            }

            OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        bool isAllPointsCompleted = false;

        async Task post_message()
        {
            ViewModels.TaskTrackingModelInformation ttmi = new ViewModels.TaskTrackingModelInformation();
            ttmi.task_author = task_author;
            ttmi.ttm_info_body = contentAsHtml_rtfMessage;
            ttmi.ttm_info_forwhom = SelectedOptions_message.ToList<string>();
            ttmi.task_id = task_id;

            await to_script.sendMessage(ttmi);

            await rtfMessage.ClearAsync();
            await OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        async Task post_commit()
        {
            ViewModels.TaskTrackingModelCardCommit ttmcc = new ViewModels.TaskTrackingModelCardCommit();
            ttmcc.task_author = task_author;
            ttmcc.task_id = task_id;
            ttmcc.ttm_commit_id = task_id;
            ttmcc.ttm_commit_author = task_author;
            //GetHTML();
            ttmcc.ttm_commit_body = contentAsHtml_rtfMessage;
            ttmcc.ttm_commit_title = ttm_commit_title;
            ttmcc.ttm_cast_coauthors = SelectedOptions_commit.ToList<string>();

            await to_script.sendCommit(ttmcc);

            await rtfCommit.ClearAsync();
            ttm_commit_title = "";
            await OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        async Task post_warning()
        {
            ViewModels.TaskTrackingModelWarning ttmw = new ViewModels.TaskTrackingModelWarning();
            ttmw.task_author = task_author;
            ttmw.task_id = task_id;
            ttmw.ttm_cast_coauthors = SelectedOptions_warning.ToList<string>();
            ttmw.ttm_warning_body = contentAsHtml_rtfWarning;
            ttmw.ttm_warning_title = ttm_warning_title;
            ttmw.ttm_whom_warning = SelectedOptions_warning.ToList<string>();

            await to_script.sendWarning(ttmw);

            await rtfWarning.ClearAsync();
            ttm_warning_title = "";
            await OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }

        async Task post_attachments()
        {
            ViewModels.TaskTrackingModelAttachments ttma = new ViewModels.TaskTrackingModelAttachments();
            ttma.task_author = task_author;
            ttma.task_id = task_id;
            ttma.ttm_sendattachments = attach_lst_links;

            await rtfSenderFile.ClearAsync();
            attach_lst.Clear();

            await to_script.sendAttachments(ttma);

            await OnCarouselRefresh.InvokeAsync(true);
            StateHasChanged();
        }


        async Task post_question()
        {
            ViewModels.TaskTrackingModelQuestion ttmq = new ViewModels.TaskTrackingModelQuestion();

            ttmq.task_author = task_author;
            ttmq.task_id = task_id;
            ttmq.ttm_questioncard_id = task_id;
            ttmq.ttm_question_body = contentAsHtml_rtfQuestion;
            ttmq.ttm_question_title = ttm_question_title;
            ttmq.ttm_whom_question = SelectedOptions_question.ToList<string>();

            await to_script.sendQuestion(ttmq);
            await OnCarouselRefresh.InvokeAsync(true);
            await rtfQuestion.ClearAsync();
            ttm_question_title = "";

            StateHasChanged();
        }
   
    string file_fileName { get; set; }
    long file_UploadedPercentage { get; set; }
    long file_Size { get; set; }
    long file_UploadedBytes { get; set; }

    private List<IBrowserFile> loadedFiles = new();
    private long maxFileSize = 1024 * 15 * 1024;
    private int maxAllowedFiles = 100;
    private bool isLoading;

    bool isFileLoaded { get; set; }

    List<string> attach_lst = new List<string>();
    List<string> attach_lst_links = new List<string>();



    async Task OnInputFileChange(InputFileChangeEventArgs e, string unique_filename, int FileNumber)
    {
       
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
                    {// remainder
                        chunkSize = totalBytes - uploadedBytes;
                    }
                    var chunk = new byte[chunkSize];
                    await inStream.ReadAsync(chunk, 0, chunk.Length);
                    // upload this fragment
                    using var formFile = new MultipartFormDataContent();
                    var fileContent = new StreamContent(new MemoryStream(chunk));
                    formFile.Add(fileContent, "file", unique_filename);
                    // post 

                    var response = await clientlocal.PostAsync($"api/AppendFileTaskTracking/{fragment++}", formFile);
                    // Update our progress data and UI
                    uploadedBytes += chunkSize;
                    percent = uploadedBytes * 100 / totalBytes;

                    file_UploadedPercentage = percent;
                    file_Size = totalBytes;
                    file_UploadedBytes = uploadedBytes;
                    file_fileName = file.Name;

                    echo = $"Uploaded {percent}%  {uploadedBytes} of {totalBytes} | Fragment: {fragment}";
                    if (percent >= 100)
                    {// upload complete
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
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    private async Task LoadFilesAttachments(InputFileChangeEventArgs e)
    {
        string filename = "";
        isLoading = true;
        loadedFiles.Clear();

        int file_num = -1;

        foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
        {
            file_num++;
            try
            {
                loadedFiles.Add(file);
                filename = file.Name;
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "wwwroot","uploads","tasktracking", filename);

                //if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath,_env.EnvironmentName, "uploads/calendar", filename)) || filename.Length > 100)

                if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "tasktracking", filename)) || filename.Length > 100)
                {
                    string filename_we = Path.GetFileNameWithoutExtension(file.Name);
                    string extension = Path.GetExtension(file.Name);
                    filename = getUnicalFileName(filename_we, extension);
                }

                if (file.Size > maxFileSize)
                {
                    await OnInputFileChange(e, filename, file_num);
                }
                else
                {
                    await using FileStream fs = new(path, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File: {Filename} Error: {Error}",
                    file.Name, ex.Message);
            }
            finally
            {
                attach_lst.Add(filename);
                attach_lst_links.Add("https://evicrm.store/uploads/tasktracking/" + filename);
                StateHasChanged();
            }
        }


    }

    private RichTextEdit rtfWarning;
    private RichTextEdit rtfQuestion;
    private RichTextEdit rtfSenderFile;
    private RichTextEdit rtfMessage;
    private RichTextEdit rtfCommit;

    protected string contentAsHtml_rtfWarning;
    protected string contentAsDeltaJson_rtfWarning;
    protected string contentAsText_rtfWarning;
    protected string savedContent_rtfWarning;

    protected string contentAsHtml_rtfQuestion;
    protected string contentAsDeltaJson_rtfQuestion;
    protected string contentAsText_rtfQuestion;
    protected string savedContent_rtfQuestion;

    protected string contentAsHtml_rtfSenderFile;
    protected string contentAsDeltaJson_rtfSenderFile;
    protected string contentAsText_rtfSenderFile;
    protected string savedContent_rtfSenderFile;

    protected string contentAsHtml_rtfMessage;
    protected string contentAsDeltaJson_rtfMessage;
    protected string contentAsText_rtfMessage;
    protected string savedContent_rtfMessage;

    protected string contentAsHtml_rtfCommit;
    protected string contentAsDeltaJson_rtfCommit;
    protected string contentAsText_rtfCommit;
    protected string savedContent_rtfCommit;

    public async Task OnContentChanged_rtfWarning()
    {
        contentAsHtml_rtfWarning = await rtfWarning.GetHtmlAsync();
        contentAsDeltaJson_rtfWarning = await rtfWarning.GetDeltaAsync();
        contentAsText_rtfWarning = await rtfWarning.GetTextAsync();
    }

    public void interop_setNewGlobalPersonalStatus(string gps)
    {
        global_personal_status = gps;
        StateHasChanged();
    }

    public async Task OnSave_rtfWarning()
    {
        savedContent_rtfWarning = await rtfWarning.GetHtmlAsync();
        await rtfWarning.ClearAsync();
    }

    public async Task OnContentChanged_rtfQuestion()
    {
        contentAsHtml_rtfQuestion = await rtfQuestion.GetHtmlAsync();
        contentAsDeltaJson_rtfQuestion = await rtfQuestion.GetDeltaAsync();
        contentAsText_rtfQuestion = await rtfQuestion.GetTextAsync();
    }

    public async Task OnSave_rtfQuestion()
    {
        savedContent_rtfQuestion = await rtfQuestion.GetHtmlAsync();
        await rtfQuestion.ClearAsync();
    }

    public async Task OnContentChanged_rtfSenderFile()
    {
        contentAsHtml_rtfSenderFile = await rtfSenderFile.GetHtmlAsync();
        contentAsDeltaJson_rtfSenderFile = await rtfSenderFile.GetDeltaAsync();
        contentAsText_rtfSenderFile = await rtfSenderFile.GetTextAsync();
    }

    public async Task OnSave_rtfSenderFile()
    {
        savedContent_rtfSenderFile = await rtfSenderFile.GetHtmlAsync();
        await rtfSenderFile.ClearAsync();
    }

    public async Task OnContentChanged_rtfMessage()
    {
        contentAsHtml_rtfMessage = await rtfMessage.GetHtmlAsync();
        contentAsDeltaJson_rtfMessage = await rtfMessage.GetDeltaAsync();
        contentAsText_rtfMessage = await rtfMessage.GetTextAsync();
    }

    public async Task OnSave_rtfCommmit()
    {
        savedContent_rtfMessage = await rtfCommit.GetHtmlAsync();
        await rtfCommit.ClearAsync();
    }

    public async Task OnContentChanged_rtfCommit()
    {
        contentAsHtml_rtfCommit = await rtfCommit.GetHtmlAsync();
        contentAsDeltaJson_rtfCommit = await rtfCommit.GetDeltaAsync();
        contentAsText_rtfCommit = await rtfCommit.GetTextAsync();
    }

    public async Task OnSave_rtfComming()
    {
        savedContent_rtfCommit = await rtfCommit.GetHtmlAsync();
        await rtfCommit.ClearAsync();
    }

  

    private void modal_finishTaskOpen()
    {
        modal_finishTaskOpened = true;
        StateHasChanged();
    }
    private void modal_RefuseDoingOpen()
    {
        modal_RefuseDoingOpened = true;
        StateHasChanged();

    }
    private void modal_DelayOpen()
    {
        modal_DelayOpened = true;
        StateHasChanged();
    }

    private async Task OnTabChanged(TabItem tab)
    {
        _activeTab = tab;
        var index = _tabs.Tabs.ToList().IndexOf(tab);
    }

    private async Task SelectionChanged(ChangeEventArgs evt)
    {

    }

    public bool isTaskContainsSelf()
    {
        bool res = true;

        return res;
    }

    public async Task SendTOScriptCmd(string cmd)
    {
        TaskTrackingModelDelay ttmd = new TaskTrackingModelDelay();
        ttmd.task_author = task_author;
        ttmd.task_id = task_id;

        switch (cmd)
        {
            case "DELAY_OTHERTASK":
                await to_script.DELAY_OTHERTASK(ttmd);
                break;
            case "DELAY_LACKTIME":
                await to_script.DELAY_LACKTIME(ttmd);
                break;
            case "DELAY_FORCEM":
                await to_script.DELAY_FORCEM(ttmd);
                break;
            case "DELAY_NOMONEY":
                await to_script.DELAY_NOMONEY(ttmd);
                break;
            case "DELAY_GUILTYMEMBER":
                await to_script.DELAY_GUILTYMEMBER(ttmd);
                break;
            case "DELAY_OTHER":
                await to_script.DELAY_OTHER(ttmd);
                break;
            default:
                await to_script.DELAY_FORCEM(ttmd);
                break;

        }
            await OnPersonalStatusChange.InvokeAsync("delayed");
            await OnCarouselRefresh.InvokeAsync(true);
            await OnClose.InvokeAsync(false);
    }

    public class SelectData
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}

    }