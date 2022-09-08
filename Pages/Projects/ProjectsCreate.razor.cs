using Blazor.Cropper;
using EviCRM.Server.Core;
using Microsoft.AspNetCore.Components.Forms;
using MySqlConnector;
using System.Diagnostics;

namespace EviCRM.Server.Pages.Projects
{
    public partial class ProjectsCreate
    {
        private BackendController bc = new BackendController();
        private MySQL_Controller mysqlc = new MySQL_Controller();

        EviCRM.Server.ViewModels.ProjectCreateModel pcm = new EviCRM.Server.ViewModels.ProjectCreateModel();

        List<string> users_dt = new List<string>();
        List<string> division_dt_id = new List<string>();
        List<string> division_dt_name = new List<string>();
        List<string> users_dt_name = new List<string>();
        List<string> tasks_dt_id = new List<string>();
        List<string> tasks_dt_names = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            pcm.proj_start = DateTime.Now;
            pcm.proj_end = DateTime.Now;

            users_dt = await mysqlc.getListUsersLoginAsync();
            division_dt_id = await mysqlc.getListDivisionsIDAsync();
            division_dt_name = await mysqlc.getListDivisionsNameAsync();
            users_dt_name = await mysqlc.getListUsersNameAsync();
            tasks_dt_id = await mysqlc.getListTaskIDAsync();
            tasks_dt_names = await mysqlc.getListTaskNameAsync();

            select_data_personal = new List<SelectData>();
            select_data_divs = new List<SelectData>();
            select_data_free_tasks = new List<SelectData>();

            //Загрузка персонала
            for (int i = 0; i < users_dt.Count; i++)
            {
                SelectData sd = new SelectData();

                sd.Name = users_dt_name[i];
                sd.Id = users_dt[i];

                select_data_personal.Add(sd);

            }

            //Загрузка отделов
            for (int i = 0; i < division_dt_id.Count; i++)
            {
                SelectData sd = new SelectData();

                sd.Name = division_dt_name[i];
                sd.Id = division_dt_id[i];

                select_data_divs.Add(sd);

            }

            //Загрузка свободных задач

            for (int i = 0; i < tasks_dt_id.Count; i++)
            {
                SelectData sd = new SelectData();

                sd.Name = tasks_dt_names[i];
                sd.Id = tasks_dt_id[i];
                select_data_free_tasks.Add(sd);

            }

        }




        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }


        string avatar_path = "";

        private long maxFileSize = 1024 * 15 * 1024;
        private int maxAllowedFiles = 100;

        string getUnicalFileName(string filename_we, string extension)
        {
            string guid_str = Guid.NewGuid().ToString();
            return (filename_we + "_" + Path.GetRandomFileName() + extension);
        }

        private async Task LoadAvatar(InputFileChangeEventArgs e)
        {
            string filename = "";
            int file_num = -1;

            file_num++;
            try
            {

                filename = e.File.Name;
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "wwwroot/uploads/users/avatars", filename);

                //if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath,_env.EnvironmentName, "uploads/calendar", filename)) || filename.Length > 100)

                if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath, "wwwroot/uploads/users/avatars", filename)) || filename.Length > 100)
                {
                    string filename_we = Path.GetFileNameWithoutExtension(e.File.Name);
                    string extension = Path.GetExtension(e.File.Name);
                    filename = getUnicalFileName(filename_we, extension);
                }

                path = Path.Combine(_env.ContentRootPath, "wwwroot/uploads/users/avatars", filename);

                if (e.File.Size > maxFileSize)
                {
                    //attach_lst_status.Add("");
                    await ToastsNotifications_ShowCustomToast_Danger("Файл слишком большой для аватарки, его нельзя загрузить!");

                }
                else
                {
                    await using FileStream fs = new(path, FileMode.Create);
                    await e.File.OpenReadStream(maxFileSize).CopyToAsync(fs);
                    avatar_path = filename;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File: {Filename} Error: {Error}",
                    e.File.Name, ex.Message);
            }
            finally
            {

                StateHasChanged();
            }


        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public async Task CreateProject()
        {

            if (pcm.proj_name == "" || pcm.proj_name == null)
            {
                await ToastsNotifications_ShowCustomToast_Danger("У проекта не может быть пустое имя!");
                return;
            }
            if (pcm.proj_short_desc == "" || pcm.proj_short_desc == null)
            {
                await ToastsNotifications_ShowCustomToast_Danger("У проекта не может быть пустое краткое имя!");
                return;
            }

            string body_text = await proj_description.GetHtmlAsync();

            if (body_text == "" || body_text == null)
            {
                await ToastsNotifications_ShowCustomToast_Danger("Необходимо ввести описание проекта!");
                return;
            }
            
            if (pcm.proj_end < DateTime.Now)
            {
                await ToastsNotifications_ShowCustomToast_Warning("Пытаешься создать проект задним числом. Это запрещено!");
                return;
            }

            if (pcm.proj_start > pcm.proj_end)
            {
                DateTime dt = new DateTime();
                dt = pcm.proj_start;
                pcm.proj_start = pcm.proj_end;
                pcm.proj_end = dt;
                await ToastsNotifications_ShowCustomToast_Danger("Проект не может начаться позже, чем закончится! Я поменял введённые даты местами, необходимо проверить правильность или продолжи так");
            }
            else
            {

                string value = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getProjectsIDByName(pcm.proj_name.ToString()));

                List<string> proj_members_list = new List<string>();
                List<string> proj_divs_list = new List<string>();
                List<string> proj_tasks_list = new List<string>();

                

                string filename = "";
                string filename_without_extension = "";
                string extension = "";

                string attach_u = "";
                string members_u = "";

                string filePath = "";

                if (value == null | value == "")
                {
                    int user_id = 0;


                    foreach (string member in SelectedValues_personal)
                    {
                        proj_members_list.Add(member);
                    }
                    foreach (string div in SelectedValues_divs)
                    {
                        proj_divs_list.Add(div);
                    }
                    foreach (string task in SelectedValues_free_tasks)
                    {
                        proj_tasks_list.Add(task);
                    }

                    string str_compiled_members = "";
                    string str_compiled_divs = "";
                    string str_compiled_tasks = "";

                    if (proj_members_list.Count > 0)
                    {
                        str_compiled_members = bc.getMultipleStringEncodingString(proj_members_list);
                    }
                    if (proj_divs_list.Count > 0)
                    {
                        str_compiled_divs = bc.getMultipleStringEncodingString(proj_divs_list);
                    }
                    if (proj_members_list.Count > 0)
                    {
                        str_compiled_tasks = bc.getMultipleStringEncodingString(proj_tasks_list);
                    }

                    string uniqueFileName_avatar = "";

                    using var connection = new MySqlConnection("server = 82.146.44.138;user = mysql_c;password = 512Dcfbd28#;database=evicrm");
                    await connection.OpenAsync();

                    body_text = Base64Encode(body_text);

                    using var command = new MySqlCommand(mysqlc.setCreateProject(pcm.proj_name, pcm.proj_short_desc, body_text, pcm.proj_start, pcm.proj_end, str_compiled_members, str_compiled_divs, str_compiled_tasks, ProjectStatus.waiting, avatar_path), connection);
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var value2 = reader.GetValue(0);
                        Debug.WriteLine(value2);

                        // do something with 'value'
                    }
                    UriHelper.NavigateTo("/proj_list");
                    await ToastsNotifications_ShowCustomToast_Info("Проект успешно создан!");
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await ToastsNotifications_ShowCustomToast_Danger("Проект с таким имененм уже существует!");
                    Debug.WriteLine("Проект с таким именем уже существует!");
                }
            }
        }
    }
}