using EviCRM.Server.Core;
using System.Collections.Generic;
using System.Net;
using EviCRM.Server.Models.MarksChart;
using EviCRM.Server.Pages.Tasks.Components;
using EviCRM.Server.ViewModels;
using System.IO;
using EviCRM.Server.Pages.Modals;
using EviCRM.Server.Models;

namespace EviCRM.Server.Pages.Marks
{
    public partial class MarksChart
    {
        public async Task CreateMarkInterpreter(EviCRM.Server.Models.MarksChart.Marks mcm)
        {
            await mysqlc.MySql_ContextAsyncL(mysqlc.setMark(mcm));

            marks_date.Add(mcm.date.ToString());
            marks_first_description.Add(mcm.firstComment);
            marks_first_mark.Add(mcm.firstMark.ToString());
            marks_idmarks.Add(mcm.idmarks.ToString());
            marks_isTwoMarks.Add(mcm.isTwoMarks.ToString());
            marks_second_description.Add(mcm.secondComment);
            marks_second_mark.Add(mcm.secondMark.ToString());
            marks_user_id.Add(mcm.userID);

            if (mcm.firstAttachments != null)
            {
                marks_first_attachments.Add(bc.getMultipleStringEncodingString(mcm.firstAttachments));
            }

            if (mcm.secondAttachments != null)
            {
                marks_second_attachments.Add(bc.getMultipleStringEncodingString(mcm.secondAttachments));
            }

            StateHasChanged();
        }

        public async Task UpdateInterpreter(EviCRM.Server.Models.MarksChart.Marks model)
        {
            int i = 0;
            for (int j = 0; j < marks_idmarks.Count; j++)
            {
                if (model.idmarks.ToString() == marks_idmarks[j])
                {
                    i = j;
                }
            }
            await mysqlc.MySql_ContextAsyncL(mysqlc.updateMark(model));

            marks_date[i] = model.date.ToString();
            marks_first_description[i] = model.firstComment;
            marks_first_mark[i] = model.firstMark.ToString();
            marks_idmarks[i] = model.idmarks.ToString();
            marks_isTwoMarks[i] = model.isTwoMarks.ToString();
            marks_second_description[i] = model.secondComment;
            marks_second_mark[i] = model.secondMark.ToString();
            marks_user_id[i] = model.userID;
            if (marks_first_attachments[i] != null)
            {
                marks_first_attachments[i] = bc.getMultipleStringEncodingString(model.firstAttachments);
            }
            if (marks_second_attachments[i] != null)
            {
                marks_second_attachments[i] = bc.getMultipleStringEncodingString(model.secondAttachments);
            }

            StateHasChanged();
        }


        public async Task DeleteInterpreter(string idmark)
        {
            int i = 0;
            for (int j = 0; j < marks_idmarks.Count; j++)
            {
                if (marks_idmarks[j].ToString() == idmark)
                {
                    i = j;
                }
            }
            await mysqlc.MySql_ContextAsyncL(mysqlc.deleteMark(idmark));

            marks_date.RemoveAt(i);
            marks_first_description.RemoveAt(i);
            marks_first_mark.RemoveAt(i);
            marks_idmarks.RemoveAt(i);
            marks_isTwoMarks.RemoveAt(i);
            marks_second_description.RemoveAt(i);
            marks_second_mark.RemoveAt(i);
            marks_user_id.RemoveAt(i);
            marks_first_attachments.RemoveAt(i);
            marks_second_attachments.RemoveAt(i);

            StateHasChanged();
        }

        public string getUserMarkByIDAndDay(int i, int days)
        {
            string view = "";

            DateTime dtp = new DateTime();
            DateTime dtp2 = new DateTime();

            dtp2 = new DateTime(_global_year.Year, _global_month.Month, days);


            for (int j = 0; j < marks_idmarks.Count; j++)
            {
                dtp = DateTime.Parse(marks_date[j]);
                if (dtp.ToShortDateString() == dtp2.ToShortDateString())
                {
                    if (id_dt[i] == marks_user_id[j])
                    {
                        if (marks_isTwoMarks[j] != "False")
                        {
                            view = marks_first_mark[j] + " / " + marks_second_mark[j];
                        }
                        else
                        {
                            view = marks_first_mark[j];
                        }
                    }
                }

            }
            return view;
        }

        protected async override Task OnInitializedAsync()
        {
            _global_year = DateTime.Now;
            _global_month = DateTime.Now;
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

            StateHasChanged();

            users_dt = await mysqlc.getListUsersLoginAsync();
            email_dt = await mysqlc.getListUsersEmailAsync();
            passwords_dt = await mysqlc.getListUsersPasswordAsync();
            id_dt = await mysqlc.getListUsersIDAsync();
            fullname_dt = await mysqlc.getListUsersFullnameAsync();
            position_dt = await mysqlc.getListUsersPositionsAsync();
            skills_dt = await mysqlc.getListUsersSkills();
            avatarpath_dt = mysqlc.getAvatarPaths(users_dt);
            projects_val_dt = await mysqlc.getListUsersProjectsAsync();

            marks_idmarks = await mysqlc.getListMarksIDAsync();
            marks_user_id = await mysqlc.getListMarksUserIDAsync();
            marks_date = await mysqlc.getListMarksDateAsync();
            marks_first_mark = await mysqlc.getListMarksFirstMarkAsync();
            marks_second_mark = await mysqlc.getListMarksSecondMarkAsync();
            marks_first_description = await mysqlc.getListMarksFirstDescriptionAsync();
            marks_second_description = await mysqlc.getListMarksSecondDescriptionAsync();
            marks_isTwoMarks = await mysqlc.getListMarksIsTwoMarksAsync();

            marks_first_attachments = await mysqlc.getListMarksFirstAttachmentsAsync();
            marks_second_attachments = await mysqlc.getListMarksSecondAttachmentsAsync();

            int p = 0;
            if (users_dt != null)
            {
                p = users_dt.Count;
            }

            modal_operationType = "";
            mcm = new EviCRM.Server.Models.MarksChart.Marks();
            modal_Date = DateTime.Now;
            modal_ID = 0;
            modal_Name = "";

            day_num = DateTime.DaysInMonth(_global_year.Year, _global_month.Month);
            DayCounter();

            StateHasChanged();

            ready = true;
        }

        public void ShowInfo(int i, int day, bool isPatched)
        {
            modal_ID = 0;
            modal_Date = new DateTime(_global_year.Year, _global_month.Month, day);
            modal_Name = fullname_dt[i].ToString();

            if (isAdmin)
            {
                if (isPatched)
                {
                    DateTime dt_n = new DateTime(_global_year.Year, _global_month.Month, day);

                    modal_operationType = "CREATE";
                    mcm.date = dt_n;
                    mcm.firstComment = "";
                    mcm.firstMark = 5;
                    mcm.secondComment = "";
                    mcm.secondMark = 5;
                    mcm.userID = id_dt[i];
                    mcm.isTwoMarks = true;
                    mcm.firstAttachments = new List<string>();
                    mcm.secondAttachments = new List<string>();
                    OpenDeleteDialog();
                }
                else
                {
                    modal_operationType = "EDIT";

                    DateTime dt_n = new DateTime(_global_year.Year, _global_month.Month, day);

                    mcm.date = dt_n;
                    mcm.firstComment = marks_first_description[convertUserID_to_TaskID(i, dt_n)];
                    mcm.firstMark = int.Parse(marks_first_mark[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.secondComment = marks_second_description[convertUserID_to_TaskID(i, dt_n)];
                    mcm.secondMark = int.Parse(marks_second_mark[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.userID = id_dt[i];
                    mcm.isTwoMarks = bool.Parse(marks_isTwoMarks[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.idmarks = int.Parse(marks_idmarks[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.firstAttachments = bc.getMultipleStringDecodingString(marks_first_attachments[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.secondAttachments = bc.getMultipleStringDecodingString(marks_second_attachments[convertUserID_to_TaskID(i, dt_n)]);

                    OpenDeleteDialog();

                }
            }
            else
            {
                if (isPatched)
                {


                }
                else
                {
                    modal_operationType = "VIEW";
                    DateTime dt_n = new DateTime(_global_year.Year, _global_month.Month, day);

                    mcm.date = dt_n;
                    mcm.firstComment = marks_first_description[convertUserID_to_TaskID(i, dt_n)];
                    mcm.firstMark = int.Parse(marks_first_mark[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.secondComment = marks_second_description[convertUserID_to_TaskID(i, dt_n)];
                    mcm.secondMark = int.Parse(marks_second_mark[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.userID = id_dt[i];
                    mcm.isTwoMarks = bool.Parse(marks_isTwoMarks[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.idmarks = int.Parse(marks_idmarks[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.firstAttachments = bc.getMultipleStringDecodingString(marks_first_attachments[convertUserID_to_TaskID(i, dt_n)]);
                    mcm.secondAttachments = bc.getMultipleStringDecodingString(marks_second_attachments[convertUserID_to_TaskID(i, dt_n)]);

                    OpenDeleteDialog();
                }
            }
        }
    }

}
