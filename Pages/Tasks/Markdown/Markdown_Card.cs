using Meziantou.Framework;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace EviCRM.Server.Pages.Tasks.Markdown
{
    public partial class Markdown_Card
    {

        string file_fileName { get; set; }
        long file_UploadedPercentage { get; set; }
        long file_Size { get; set; }
        long file_UploadedBytes { get; set; }

        List<FileUploadProgress> uploadedFiles = new();

        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 15 * 1024;
        private int maxAllowedFiles = 100;
        private bool isLoading;

        List<string> _attachments = new List<string>();

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

        public void ColorChanger(string new_color)
        {

        }

        public void Close_Trigger()
        {
            OnClose.InvokeAsync(true);
        }

        public void ContextMenu()
        {
            isMenuShown = !isMenuShown;
            StateHasChanged();
        }

        public void ContextMenu_EditCardPic()
        {

        }

        public void ContextMenu_DeleteCardPic()
        {

        }

        public void ContextMenu_MoveCard()
        {

        }

        public void ContextMenu_CopyCard()
        {

        }

        public void ContextMenu_Delete()
        {

        }

        public void Card_AddMarks()
        {

        }

        public void Card_Labels()
        {

        }

        public void Card_AddAttachments()
        {

        }

        public void Card_Members()
        {

        }

        public void Markdown(int i)
        {

        }

        public void KillCardMarkdown(int i)
        {

        }

        string FormatBytes(long value)
    => ByteSize.FromBytes(value).ToString("fi2", CultureInfo.CurrentCulture);

        record FileUploadProgress(string FileName, long Size)
        {
            public long UploadedBytes { get; set; }
            public double UploadedPercentage => (double)UploadedBytes / (double)Size * 100d;
        }

        bool _uploading;
        string echo;

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

                        var response = await clientlocal.PostAsync($"api/AppendFile/{fragment++}", formFile);
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

        public void DeleteAttachment(int a)
        {

        }

        string getUnicalFileName(string filename_we, string extension)
        {
            string guid_str = Guid.NewGuid().ToString();
            return (filename_we + "_" + Path.GetRandomFileName() + extension);
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
                    var path = Path.Combine(_env.ContentRootPath, "wwwroot/uploads/calendar", filename);

                    //if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath,_env.EnvironmentName, "uploads/calendar", filename)) || filename.Length > 100)

                    if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath, "wwwroot/uploads/calendar", filename)) || filename.Length > 100)
                    {
                        string filename_we = Path.GetFileNameWithoutExtension(file.Name);
                        string extension = Path.GetExtension(file.Name);
                        filename = getUnicalFileName(filename_we, extension);
                    }

                    if (file.Size > maxFileSize)
                    {
                        //attach_lst_status.Add("");
                        await OnInputFileChange(e, filename, file_num);
                    }
                    else
                    {
                        await using FileStream fs = new(path, FileMode.Create);
                        await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                        _attachments.Add("https://evicrm.store/uploads/calendar/" + filename);
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
                    attach_lst_links.Add("https://evicrm.store/uploads/calendar/" + filename);
                    //attach_lst_status.Add("ok");
                    StateHasChanged();
                }
            }


        }

    }
}
