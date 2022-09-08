using Blazorise.RichTextEdit;
using EviCRM.Server.Core;
using Meziantou.Framework;
using Microsoft.AspNetCore.Components.Forms;
using MySqlConnector;
using System.Diagnostics;
using System.Globalization;

namespace EviCRM.Server.Pages.Tasks
{
    public partial class TasksCreate
    {
        public class SelectData
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }

        protected async override Task OnInitializedAsync()
        {
            users_dt = await mysqlc.getListUsersLoginAsync();
            division_dt = await mysqlc.getListUsersDivisionAsync();
            fullname_dt = await mysqlc.getListUsersFullnameAsync();

            divisions_ids = await mysqlc.getListDivisionsIDAsync();
            divisions_name = await mysqlc.getListDivisionsNameAsync();

            proj_id_dt = await mysqlc.getListProjIDAsync();
            proj_name_dt = await mysqlc.getListProjNameAsync();
            proj_start_dt = await mysqlc.getListProjStartDateAsync();
            proj_end_dt = await mysqlc.getListProjEndAsync();

            select_data = new List<SelectData>();
            select_data_users = new List<SelectData>();
            select_data_projs = new List<SelectData>();

            for (int i = 0; i < divisions_ids.Count; i++)
            {
                SelectData sd = new SelectData();
                sd.Id = divisions_ids[i];
                sd.Name = divisions_name[i];
                select_data.Add(sd);
            }

            for (int i = 0; i < users_dt.Count; i++)
            {
                SelectData sd = new SelectData();
                sd.Id = users_dt[i];
                sd.Name = fullname_dt[i];
                select_data_users.Add(sd);
            }

            for (int i = 0; i < proj_id_dt.Count; i++)
            {
                SelectData sd = new SelectData();
                sd.Id = proj_id_dt[i];
                sd.Name = proj_name_dt[i];
                select_data_projs.Add(sd);
            }

            dt_start = DateTime.Now;
            dt_end = DateTime.Now;
            ready_tr = true;
            file_fileName = "";
            file_UploadedPercentage = 0;
            file_Size = 0;
            file_UploadedBytes = 0;

            foreach (string att_lnk in attach_lst)
            {
                attach_lst_links.Add("https://evicrm.store/uploads/tasktracking/" + att_lnk);
            }

            SessionUser = authStateProvider.GetAuthenticationStateAsync().Result.User.Identity.Name;

            StateHasChanged();
        }


    }
}
