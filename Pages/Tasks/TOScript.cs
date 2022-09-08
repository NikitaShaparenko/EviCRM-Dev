using EviCRM.Server.Core;
using EviCRM.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySqlConnector;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting.Internal;

namespace EviCRM.Server.Pages.Tasks
{
    public class TOScript
    {
        
        MySQL_Controller mysqlc = new MySQL_Controller();
        BackendController bc = new BackendController();

        public async Task<bool> sendCommit(TaskTrackingModelCardCommit ttmcc)
        {
            string TOSCRIPT_CMD = "COMMIT";

            string task_author = ttmcc.task_author;
            string task_id = ttmcc.task_id;

            List<string> str_lst = new List<string>();


            if (ttmcc.ttm_cast_coauthors != null)
            {
                if (ttmcc.ttm_cast_coauthors.Count > 0)
                    foreach (string elem in ttmcc.ttm_cast_coauthors)
                    {
                        str_lst.Add(elem);
                    }

            }
            string TOSCRIPT_VAR1 = ttmcc.ttm_commit_title;
            string TOSCRIPT_VAR2 = ttmcc.ttm_commit_body;
            string TOSCRIPT_VAR3 = bc.getMultipleStringEncodingString(str_lst);


            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, TOSCRIPT_VAR3, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Произошли новые изменения в задаче: \n";
            push_msg += ttmcc.ttm_commit_title + "\n";
            push_msg += ttmcc.ttm_commit_body + "\n";
            push_msg += "\n Совместно с: \n";
            if (ttmcc.ttm_cast_coauthors != null)
            {
                foreach (string elem in ttmcc.ttm_cast_coauthors)
                {
                    push_msg += await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(elem)) + ", ";
                }
            }
            await bc.SendTelegramMessage(push_msg, task_id);

            return true; ;
        }

        public async Task<bool> sendMessage(TaskTrackingModelInformation ttmi)
        {
            string TOSCRIPT_CMD = "MESSAGE";

            string task_author = ttmi.task_author;
            string task_id = ttmi.task_id;

            List<string> str_lst = new List<string>();


            if (ttmi.ttm_info_forwhom != null)
            {
                if (ttmi.ttm_info_forwhom.Count > 0)
                    foreach (string elem in ttmi.ttm_info_forwhom)
                    {
                        str_lst.Add(elem);
                    }

            }
            string TOSCRIPT_VAR1 = ttmi.ttm_info_body;
            string TOSCRIPT_VAR2 = bc.getMultipleStringEncodingString(str_lst);

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Написал новое сообщенеи: \n";
            push_msg += ttmi.ttm_info_body  + "\n";
            push_msg += "\n Совместно с: \n";
            if (ttmi.ttm_info_forwhom != null)
            {
                if (ttmi.ttm_info_forwhom.Count > 0)
                    foreach (string elem in ttmi.ttm_info_forwhom)
                    {
                        push_msg += await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(elem)) + ", ";
                }
            }
            await bc.SendTelegramMessage(push_msg, task_id);

            return true; ;
        }

        public async Task<bool> NOTIFY(TaskTrackingModelNotify ttmn)
        {
            string TOSCRIPT_CMD = "NOTIFY";

            string task_author = ttmn.task_author;
            string task_id = ttmn.task_id;

            List<string> str_lst = new List<string>();

            if (ttmn.ttm_cmd_remind_whom.Count > 0)
                foreach (string elem in ttmn.ttm_cmd_remind_whom)
                {
                    str_lst.Add(elem);
                }

            string TOSCRIPT_VAR1 = bc.getMultipleStringEncodingString(str_lst);
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Отправлено напоминание участникам: \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }
      
        public async Task<bool> FAIL(TaskTrackingModelFail ttmf)
        {
            string TOSCRIPT_CMD = "FAIL";

            string task_author = ttmf.task_author;
            string task_id = ttmf.task_id;

            string TOSCRIPT_VAR1 = ttmf.ttm_cmd_fail_body;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача закрыта владельцем\\администратором\\создателем как провальная! \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> COMPLETE(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "COMPLETE";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));
            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача завершена в срок! \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> COMPLETEBEFOREHAND(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "COMPLETEBEFOREHAND";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача завершена заблаговременно: \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> COMPLETEDELAY(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "COMPLETEDELAY";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача завершена с задержкой! \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> REVISION(TaskTrackingModelRevision ttmr)
        {
            string TOSCRIPT_CMD = "REVISION";

            string task_author = ttmr.task_author;
            string task_id = ttmr.task_id;

            string TOSCRIPT_VAR1 = ttmr.ttm_cmd_revision_body;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача отправлена на доработку!: \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> sendInfo(TaskTrackingModelInformation ttmi)
        {
            string TOSCRIPT_CMD = "MESSAGE";

            string task_author = ttmi.task_author;
            string task_id = ttmi.task_id;

            string TOSCRIPT_VAR1 = ttmi.ttm_info_body;
            string TOSCRIPT_VAR2 = "";

            List<string> str_lst = new List<string>();

            if (ttmi.ttm_info_forwhom.Count > 0)
                foreach (string elem in ttmi.ttm_info_forwhom)
                {
                    str_lst.Add(elem);
                }

            TOSCRIPT_VAR2 = bc.getMultipleStringEncodingString(str_lst);

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "В чате новое сообщение: \n";
            push_msg += ttmi.ttm_info_body + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
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
        public async Task<bool> sendWarning(TaskTrackingModelWarning ttmw)
        {
            string TOSCRIPT_CMD = "WARNING";

            string task_author = ttmw.task_author;
            string task_id = ttmw.task_id;

            List<string> str_lst = new List<string>();

            if (ttmw.ttm_whom_warning.Count > 0)
                foreach (string elem in ttmw.ttm_whom_warning)
                {
                    str_lst.Add(elem);
                }

            string TOSCRIPT_VAR1 = ttmw.ttm_warning_title;
            string TOSCRIPT_VAR2 = ttmw.ttm_warning_body;
            string TOSCRIPT_VAR3 = bc.getMultipleStringEncodingString(str_lst);

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, TOSCRIPT_VAR3, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "ПРЕДУПРЕЖДЕНИЕ! : \n";
            push_msg += ttmw.ttm_warning_title + "\n";
            push_msg += ttmw.ttm_warning_body + "\n";
            push_msg += "\n Совместно с: \n";
            if (ttmw.ttm_cast_coauthors != null)
            {
                foreach (string elem in ttmw.ttm_cast_coauthors)
                {
                    push_msg += await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(elem)) + ", ";
                }
            }
            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> CANCELED(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "CANCELED";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Задача отменена владельцем / админом! \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> ROGER(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "ROGER";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь подтвердил, что приступил к выполнению задачи! \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> PAUSE(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "PAUSE";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Администратор / создатель поставил задачу на паузу \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> FAIL(TaskTrackingModelCommon ttmc)
        {
            string TOSCRIPT_CMD = "FAIL";

            string task_author = ttmc.task_author;
            string task_id = ttmc.task_id;

            string TOSCRIPT_VAR1 = "";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Создатель/ админ / владелец решили закрыть задачу как провал и уже не мучаться \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> ADDGOALS(TaskTrackingModelCardMarksdownActions ttmcma)
        {
            string TOSCRIPT_CMD = "ADDGOALS";

            string task_author = ttmcma.task_author;
            string task_id = ttmcma.task_id;

            string TOSCRIPT_VAR1 = ttmcma.task_cardmark;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Добавил цель к задаче '" + ttmcma.task_cardmark + "' \n";

            await bc.SendTelegramMessage(push_msg, task_id);
            

            return true;
        }

        public async Task<bool> DELETEGOALS(TaskTrackingModelCardMarksdownActions ttmcma)
        {
            string TOSCRIPT_CMD = "DELETEGOALS";

            string task_author = ttmcma.task_author;
            string task_id = ttmcma.task_id;

            string TOSCRIPT_VAR1 = ttmcma.task_cardmark;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Удалил цель из задачи \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> MARKGOALS(TaskTrackingModelCardMarksdownActions ttmcma)
        {
            string TOSCRIPT_CMD = "MARKGOALS";

            string task_author = ttmcma.task_author;
            string task_id = ttmcma.task_id;

            string TOSCRIPT_VAR1 = ttmcma.task_cardmark;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Отметил цель: " + ttmcma.task_cardmark + " как выполненную \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> QA(TaskTrackingModelQA ttm_qa)
        {
            string TOSCRIPT_CMD = "QA";

            string task_author = ttm_qa.task_author;
            string task_id = ttm_qa.task_id;

            string TOSCRIPT_VAR1 = ttm_qa.rate.ToString("N2");
            string TOSCRIPT_VAR2 = ttm_qa.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Оставил итоговый отзыв по задаче: " + ttm_qa.comment + " и выставил оценку: " + ttm_qa.rate + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> CONCLUSION(TaskTrackingModelQA ttm_qa)
        {
            string TOSCRIPT_CMD = "CONCLUSION";

            string task_author = ttm_qa.task_author;
            string task_id = ttm_qa.task_id;

            string TOSCRIPT_VAR1 = ttm_qa.rate.ToString("N2");
            string TOSCRIPT_VAR2 = ttm_qa.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Оставил итоговый отзыв по задаче: " + ttm_qa.comment + " и выставил оценку: " + ttm_qa.rate + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> ADDMEMBER(TaskTrackingModelMembers ttm_m)
        {
            string TOSCRIPT_CMD = "ADDMEMBER";

            string task_author = ttm_m.task_author;
            string task_id = ttm_m.task_id;

            string TOSCRIPT_VAR1 = ttm_m.user;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Добавил(а) пользователя к выполнению задачи \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> ADDDIV(TaskTrackingModelDivs ttm_d)
        {
            string TOSCRIPT_CMD = "ADDDIV";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.div;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Добавил(а) отдел к выполнению задачи \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> KILLMEMBER(TaskTrackingModelMembers ttm_m)
        {
            string TOSCRIPT_CMD = "KILLMEMBER";

            string task_author = ttm_m.task_author;
            string task_id = ttm_m.task_id;

            string TOSCRIPT_VAR1 = ttm_m.user;
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Исключила пользователя из выполнения задачи \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> CHANGEDEADLINE(TaskTrackingModelChangeDates ttm_cd)
        {
            string TOSCRIPT_CMD = "CHANGEDEADLINE";

            string task_author = ttm_cd.task_author;
            string task_id = ttm_cd.task_id;

            string TOSCRIPT_VAR1 = ttm_cd.date.ToShortDateString();
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Изменил срок окончания задачи на " + TOSCRIPT_VAR1 + " \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> CHANGESTARTDATE(TaskTrackingModelChangeDates ttm_cd)
        {
            string TOSCRIPT_CMD = "CHANGESTARTDATE";

            string task_author = ttm_cd.task_author;
            string task_id = ttm_cd.task_id;

            string TOSCRIPT_VAR1 = ttm_cd.date.ToShortDateString();
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Изменил дату начала выполнения на " + TOSCRIPT_VAR1 + " \n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELEGATE(TaskTrackingModelDelegate ttm_d)
        {
            string TOSCRIPT_CMD = "DELEGATE";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.from_id;
            string TOSCRIPT_VAR2 = ttm_d.whom_id;
            string TOSCRIPT_VAR3 = ttm_d.action_id;
            string TOSCRIPT_VAR4 = ttm_d.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, TOSCRIPT_VAR3, task_id, task_author));

            TOSCRIPT_VAR1 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.from_id));
            TOSCRIPT_VAR2 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.whom_id));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += TOSCRIPT_VAR1 + " поручил " + TOSCRIPT_VAR2 + " выполнить задачу" + ttm_d.comment + ", это действие имеет ID = " + ttm_d.action_id + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> PERSONALCOMPLETE(TaskTrackingModelDelegate ttm_d)
        {
            string TOSCRIPT_CMD = "PERSONALCOMPLETE";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.from_id;
            string TOSCRIPT_VAR2 = ttm_d.whom_id;
            string TOSCRIPT_VAR3 = ttm_d.action_id;
            string TOSCRIPT_VAR4 = ttm_d.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            TOSCRIPT_VAR1 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.from_id));
            TOSCRIPT_VAR2 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.whom_id));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += TOSCRIPT_VAR2 + " отчитался о выполнении своей части задачи"  + "\n";
            push_msg += "Ранее: " + TOSCRIPT_VAR1 + " поручил " + TOSCRIPT_VAR2 + " выполнить задачу" + ttm_d.comment + ", это действие имеет ID = " + ttm_d.action_id + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> PERSONALDELAY(TaskTrackingModelDelegate ttm_d)
        {
            string TOSCRIPT_CMD = "PERSONALDELAY";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.from_id;
            string TOSCRIPT_VAR2 = ttm_d.whom_id;
            string TOSCRIPT_VAR3 = ttm_d.action_id;
            string TOSCRIPT_VAR4 = ttm_d.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            TOSCRIPT_VAR1 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.from_id));
            TOSCRIPT_VAR2 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.whom_id));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += TOSCRIPT_VAR2 + " запросил перенос сроков исполнения" + "\n";
            push_msg += "Ранее: " + TOSCRIPT_VAR1 + " поручил " + TOSCRIPT_VAR2 + " выполнить задачу" + ttm_d.comment + ", это действие имеет ID = " + ttm_d.action_id + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> PERSONALREFUSE(TaskTrackingModelDelegate ttm_d)
        {
            string TOSCRIPT_CMD = "PERSONALREFUSE";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.from_id;
            string TOSCRIPT_VAR2 = ttm_d.whom_id;
            string TOSCRIPT_VAR3 = ttm_d.action_id;
            string TOSCRIPT_VAR4 = ttm_d.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            TOSCRIPT_VAR1 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.from_id));
            TOSCRIPT_VAR2 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.whom_id));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += TOSCRIPT_VAR2 + " отказался выполнять эту часть задачи" + "\n";
            push_msg += "Ранее: " + TOSCRIPT_VAR1 + " поручил " + TOSCRIPT_VAR2 + " выполнить задачу" + ttm_d.comment + ", это действие имеет ID = " + ttm_d.action_id + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> PERSONALUNDELEGATE(TaskTrackingModelDelegate ttm_d)
        {
            string TOSCRIPT_CMD = "PERSONALUNDELEGATE";

            string task_author = ttm_d.task_author;
            string task_id = ttm_d.task_id;

            string TOSCRIPT_VAR1 = ttm_d.from_id;
            string TOSCRIPT_VAR2 = ttm_d.whom_id;
            string TOSCRIPT_VAR3 = ttm_d.action_id;
            string TOSCRIPT_VAR4 = ttm_d.comment;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            TOSCRIPT_VAR1 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.from_id));
            TOSCRIPT_VAR2 = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(ttm_d.whom_id));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += TOSCRIPT_VAR2 + " вернул исполнение задачи команде" + "\n";
            push_msg += "Ранее: " + TOSCRIPT_VAR1 + " поручил " + TOSCRIPT_VAR2 + " выполнить задачу" + ttm_d.comment + ", это действие имеет ID = " + ttm_d.action_id + "\n";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> sendAttachments(TaskTrackingModelAttachments ttmcm)
        {
            string TOSCRIPT_CMD = "ADDATTACHMENTS";

            string task_author = ttmcm.task_author;
            string task_id = ttmcm.task_id;

            List<string> attach_list = new List<string>();
            string attach_u = "";

            if (ttmcm.ttm_sendattachments.Count > 0)
                foreach (string file in ttmcm.ttm_sendattachments)
                {

                //    var uniqueFileName = GetUniqueFileName(file.FileName);
                //    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "task_uploads");
                //    var filePath = Path.Combine(uploads, uniqueFileName);
                 attach_list.Add(file);
                //    file.CopyTo(new FileStream(filePath, FileMode.Create));
                }

            attach_u = bc.getMultipleStringEncodingString(attach_list);

            //string TOSCRIPT_VAR1 = ttmcm.ttm_sendattachments_body;
            string TOSCRIPT_VAR1 = "В процессе выполнения загружены новые файлы";

            string TOSCRIPT_VAR2 = attach_u;

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "В задаче добавились файлы!: \n \n";

            foreach (string attach in attach_list)
            {
                push_msg += "http://evicrm.store/task_uploads/" + attach + "\n";
            }

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> sendQuestion(TaskTrackingModelQuestion ttmq)
        {
            string TOSCRIPT_CMD = "QUESTION";

            string task_author = ttmq.task_author;
            string task_id = ttmq.task_id;

            List<string> str_lst = new List<string>();

            if (ttmq.ttm_whom_question.Count > 0)
                foreach (string elem in ttmq.ttm_whom_question)
                {
                    str_lst.Add(elem);
                }

            string TOSCRIPT_VAR1 = ttmq.ttm_question_title;
            string TOSCRIPT_VAR2 = ttmq.ttm_question_body;
            string TOSCRIPT_VAR3 = bc.getMultipleStringEncodingString(str_lst);

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, TOSCRIPT_VAR3, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "В чате задан вопрос : \n";
            push_msg += ttmq.ttm_question_title + "\n";
            push_msg += ttmq.ttm_question_body + "\n";
            await bc.SendTelegramMessage(push_msg, task_id);
            return true;
        }

        public async Task<bool> REFUSE_NOTME(TaskTrackingModelRefuse ttmr)
        {
            string TOSCRIPT_CMD = "REFUSE";

            string task_author = ttmr.task_author;
            string task_id = ttmr.task_id;

            string TOSCRIPT_VAR1 = "NOTME";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь отказался выполнять задачу: \n";
            push_msg += "По причине: не связана со мной";

            await bc.SendTelegramMessage(push_msg, task_id);
            return true;
        }

        public async Task<bool> REFUSE_IMPOSSIBLE(TaskTrackingModelRefuse ttmr)
        {

            string TOSCRIPT_CMD = "REFUSE";

            string task_author = ttmr.task_author;
            string task_id = ttmr.task_id;

            string TOSCRIPT_VAR1 = "IMPOSSIBLE";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь отказался выполнять задачу: \n";
            push_msg += "По причине: считаю задачу невозможной";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> REFUSE_NOWAY(TaskTrackingModelRefuse ttmr)
        {

            string TOSCRIPT_CMD = "REFUSE";

            string task_author = ttmr.task_author;
            string task_id = ttmr.task_id;

            string TOSCRIPT_VAR1 = "NOWAY";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь отказался выполнять задачу: \n";
            push_msg += "По причине: не согласен с условиями";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELAY_OTHERTASK(TaskTrackingModelDelay ttmd)
        {

            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "OTHERTASK";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь запросил перенос сроков: \n";
            push_msg += "По причине: Наслоение других задач";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELAY_LACKTIME(TaskTrackingModelDelay ttmd)
        {
            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "LACKTIME";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь запросил перенос сроков: \n";
            push_msg += "По причине: недостаток времени";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELAY_FORCEM(TaskTrackingModelDelay ttmd)
        {

            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "FORCEM";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь запросил перенос сроков: \n";
            push_msg += "По причине: форс-мажор";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELAY_NOMONEY(TaskTrackingModelDelay ttmd)
        {
            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "NOMONEY";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь запросил перенос сроков \n";
            push_msg += "По причине: не хватает финансирования";

            await bc.SendTelegramMessage(push_msg, task_id);

            return true;
        }

        public async Task<bool> DELAY_GUILTYMEMBER(TaskTrackingModelDelay ttmd)
        {
            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "GUILTYMEMBER";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            return true;
        }

        public async Task<bool> DELAY_OTHER(TaskTrackingModelDelay ttmd)
        {
            string TOSCRIPT_CMD = "DELAY";

            string task_author = ttmd.task_author;
            string task_id = ttmd.task_id;

            string TOSCRIPT_VAR1 = "OTHER";
            string TOSCRIPT_VAR2 = "";

            string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));
            //string msg = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.TOSCRIPT_ADD(TOSCRIPT_CMD, TOSCRIPT_VAR1, TOSCRIPT_VAR2, task_id, task_author));

            string push_msg = "[Задача - " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(task_id)) + "] \n";
            push_msg += "(Пользователь: " + await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getUsernameByLogin(task_author)) + ") \n \n";
            push_msg += "Пользователь запросил перенос сроков \n";
            push_msg += "По причине: виноват другой исполнитель";

            await bc.SendTelegramMessage(push_msg, task_id);
            return true;
        }

        public async Task<bool> setMarkdown(TaskTrackingModelMarkdown model)
        {
            List<string> marks_selected = model.ttm_cardmark_selected;
            string author = model.ttm_cardmark_down_author_name;
            string task_id = model.task_id;

            int int_task_id = int.MinValue;
            List<string> pending_commands_cmd = new List<string>();
            List<string> pending_commands_var1 = new List<string>();
            List<string> pending_commands_var2 = new List<string>();

            if (int.TryParse(task_id, out int d))
            {
                int_task_id = int.Parse(task_id);
            }

            List<string> lst_marks_selected = new List<string>();

            foreach (string str in marks_selected)
            {
                lst_marks_selected.Add(str);
            }

            string value = await mysqlc.MySql_ContextAsyncGeneral(mysqlc.getTasksNameByID(model.task_id));

            if (value == null | value == "")
            {
                int user_id = 0;

                List<string> old_data = await mysqlc.getListTaskTrackingCardMarksDownAsync(model.task_id);

                for (int z = 0; z < lst_marks_selected.Count(); z++)
                {
                    if (old_data[z] != lst_marks_selected[z])
                    {
                        pending_commands_cmd.Add("MARKSGOALS");
                        pending_commands_var1.Add(model.ttm_cardmark_down_author_name);
                        pending_commands_var2.Add(z.ToString());
                    }
                }

                using var connection = new MySqlConnection("server = 82.146.44.138;user = mysql_c;password = 512Dcfbd28#;database=evicrm");

                for (int az = 0; az < pending_commands_cmd.Count; az++)
                {
                    await connection.OpenAsync();
                    using var command = new MySqlCommand(mysqlc.toscript_general(pending_commands_cmd[az], pending_commands_var1[az], pending_commands_var2[az]), connection);
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var value2 = reader.GetValue(0);
                    }
                }


            }

            else
            {
                Debug.WriteLine("Задача с таким именем уже существует!");
            }

            return true;
        }
    }

}
