using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EviCRM.Server.ViewModels
{

    public class TaskTracking
    {

    }

   public class TaskTrackingModelCardMarksdownActions
    {
        public string task_id { get; set; }
        public string task_author { get; set; }

        public string task_cardmark { get; set; }

    }

    public class TaskTrackingModelQA
    {
        public string task_id { get; set; }
        public string task_author { get; set; }
        public double rate { get; set; }

        public string comment { get; set; }
    }

    public class TaskTrackingModelMembers
    {
        public string task_id { get; set; }

        public string task_author { get; set; }

        public string user { get; set; }
    }

    public class TaskTrackingModelDivs
    {
        public string task_id { get; set; }

        public string task_author { get; set; }

        public string div { get; set; }
    }

    public class TaskTrackingModelChangeDates
    {
        public string task_id { get; set; }

        public string task_author { get; set; }

        public DateTime date{ get; set; }
    }

    public class TaskTrackingModelDelegate
    {
        public string task_id { get; set; }

        public string task_author { get; set; }

        public string from_id { get; set; }

        public string whom_id { get; set; }

        public string action_id { get; set; }

        public string comment { get; set; }
    }

    public class TaskTrackingModelInfo
    {
        public string ttm_info_body { get; set; }
        public List<string> ttm_info_forwhom { get; set; }

        public string task_id { get; set; }
        public string task_author { get; set; }
    }

    public class TaskTrackingModelCardMarksdown
    {
        public string[] ttm_cardmark_selected;
        public string ttm_cardmark_down_author_name;

        public string task_id { get; set; }
        public string task_author { get; set; }
    }



    public class TaskTrackingModelCardCommit
    {
        public string task_id { get; set; }
        public string task_author { get; set; }
        public string ttm_commit_author { get; set; }
        public string ttm_commit_title { get; set; }
        public string ttm_commit_body { get; set; }
        public string ttm_commit_id { get; set; }
        public List<string> ttm_cast_coauthors { get; set; }
    }


    public class TaskTrackingModelNotify
    {
        public string task_id { get; set; }
        public string task_author { get; set; }

        public List<string> ttm_cmd_remind_whom { get; set; }
    }

    public class TaskTrackingModelFail
    {
        public string task_id { get; set; }
        public string task_author { get; set; }

        public string ttm_cmd_fail_body { get; set; }
    }


    public class TaskTrackingModelSendAttachments
    {
        public string task_id { get; set; }
        public string task_author { get; set; }
        public string ttm_sendattachments_body { get; set; }
        public List<IFormFile> ttm_sendattachments { get; set; }

    }

    public class TaskTrackingModelTaskAction
    {
        public string task_id { get; set; }
        public string task_author { get; set; }
        //FAIL
        public string ttm_cmd_fail_body { get; set; }

        //REMIND
        public List<string> ttm_cmd_remind_whom { get; set; }

        //REVISION
        public string ttm_cmd_revision_body { get; set; }
    }



    public class TaskTrackingModelWarning
    {
        public string task_id { get; set; }
        public string task_author { get; set; }

        public string ttm_warning_title { get; set; }
        public string ttm_warning_body { get; set; }
        public List<string> ttm_whom_warning { get; set; }

        public List<string> ttm_cast_coauthors { get; set; }

    }

    public class TaskTrackingModelRevision
    {
        public string task_id { get; set; }
        public string task_author { get; set; }

        public string ttm_cmd_revision_body { get; set; }
    }

    public class TaskTrackingModelAttachments
    {
        public string task_author { get; set; }
        public string task_id { get; set; }

        public List<string> ttm_sendattachments { get; set; }
    }
    public class TaskTrackingModelInformation
    {
        public string task_author { get; set; }
        public string task_id { get; set; }

        public string ttm_info_body { get; set; }

        public List<string> ttm_info_forwhom { get; set; }
    }

    public class TaskTrackingModelRefuse
    {
        public string task_author { get; set; }
        public string task_id { get; set; }


    }

    public class TaskTrackingModelDelay
    {
        public string task_author { get; set; }
        public string task_id { get; set; }
    }


    public class TaskTrackingModelMarkdown
    {
        public string task_author { get; set; }
        public string task_id { get; set; }

        public List<string> ttm_cardmark_selected { get; set; }

        public string ttm_cardmark_down_author_name { get; set; }
    }

    public class TaskTrackingModelQuestion
    {
        public string task_author { get; set; }
        public string task_id { get; set; }

        public string ttm_question_title { get; set; }

        public string ttm_question_body { get; set; }


        public string ttm_questioncard_id { get; set; }
        public List<string> ttm_whom_question { get; set; }

    }

    public class TaskTrackingModelCommon
    {
        public string task_author { get; set; }
        public string task_id { get; set; }

    }

}