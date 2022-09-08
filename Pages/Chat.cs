using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EviCRM.Server.Pages
{
    public class ChatMessageForwardingOptions
    {
        public string in_forward_id { get; set; }

        public bool is_forward { get; set; }
    }

    public enum ChatMessageTypes
    {
        text,
        audio,
        document
    }

    public class ChatMessage
    {
        public string id { get; set; }

        public DateTime datetime { get; set; }
        public string body { get; set; }

        public ChatMessageForwardingOptions forward_options { get; set; }

        public string chatid { get; set; }

        public string user { get; set; }

        public ChatMessageTypes type { get; set; }

    }

    public class ChatDialog
    {
        public string chatid { get; set; }

        public string chat_name { get; set; }

        public string avatar_url { get; set; }

        public bool isGroup { get; set; }

        public DateTime last_updated { get; set; }

        public string last_message { get; set; }

        public string penfriend { get; set; }
    
    }

    public partial class Chat
    {
        public const string russian_capital_alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        List<string> users_list_sorted = new List<string>();
        List<string> users_fullname_dt = new List<string>();
        List<string> users_dt = new List<string>();

        List<ChatMessage> msg_list = new List<ChatMessage>();
        List<ChatDialog> dialog_list = new List<ChatDialog>();

        List<string> users_avatars_lst = new List<string>();

        string user_ = "";

        public bool UsersListLetterContains(char letter)
        {
            bool status = false;

            foreach (string user in users_fullname_dt)
            {
                if (user[0] == letter)
                {
                    status = true;
                }
            }
            return status;
        }

        public async Task Preload_LoadDialogs()
        {
            List<string> dialogs_str_sender = new List<string>();
            dialogs_str_sender = redis.Redis_GetKeysByMask("dialogs_" + Base64Encode(user_) + "_*");

            foreach(string dialog in dialogs_str_sender)
            {
                ChatDialog chatDialog = new ChatDialog();
                chatDialog = getDialogDecoded(dialog);
                dialog_list.Add(chatDialog);
            }

            List<string> dialogs_str_recepient = new List<string>();
            dialogs_str_recepient = redis.Redis_GetKeysByMask("dialogs_*_" + Base64Encode(user_));

            foreach (string dialog in dialogs_str_recepient)
            {
                ChatDialog chatDialog = new ChatDialog();
                chatDialog = getDialogDecoded(dialog);
                dialog_list.Add(chatDialog);
            }

            await InvokeAsync(StateHasChanged);
        }


        public async Task Preload_LoadChats()
        {
            List<string> chats_str = new List<string>();
            chats_str = redis.Redis_GetKeysByMask("chats_" + Base64Encode(user_) + "_*");

            foreach (string message in chats_str)
            {
                ChatMessage chatMessage = new ChatMessage();
                chatMessage = getMessageDecoded(message);
                msg_list.Add(chatMessage);
            }

            await InvokeAsync(StateHasChanged);
        }

        public List<string> SortByIncreaseNameValue(List<string> originalList)
        {
            return originalList.OrderBy(x => x).ToList();
        }

        public List<string> SortByDecreaseNameValue(List<string> originalList)
        {
            return originalList.OrderByDescending(x => x).ToList();
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

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                new Timer(DateTimeCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }

        public bool isDialogExists(string login)
        {
            List<string> dialogs_str_sender = new List<string>();
            dialogs_str_sender = redis.Redis_GetKeysByMask("dialogs_" + Base64Encode(user_) + "_*");

            foreach (string dialog in dialogs_str_sender)
            {
                return true;
            }

            List<string> dialogs_str_recepient = new List<string>();
            dialogs_str_recepient = redis.Redis_GetKeysByMask("dialogs_*_" + Base64Encode(user_));

            foreach (string dialog in dialogs_str_recepient)
            {
                return true;
            }

            return false;
        }

        private void DateTimeCallback(object state)
        {
            Thread.Sleep(10);
            //current_date_time = getCurrentDateTime();
            InvokeAsync(StateHasChanged);
        }

        public async Task ChatUserlistOpen(string element)
        {
            if (isDialogExists(element))
            {
                //Открыть существующий диалог
            }
            else
            {
                //Создать новый диалог тет-а-тет

                ChatDialog chatDialog = new ChatDialog();

                chatDialog.chatid = Guid.NewGuid().ToString();
                chatDialog.chat_name = getFullnameByLogin(element);
                //chatDialog.avatar_url = users_avatars_lst[getUserArrCntByLogin(element)];
                chatDialog.isGroup = false;
                chatDialog.last_updated = DateTime.Now;
                chatDialog.last_message = "";
                chatDialog.penfriend = element;

            }
        }

        public async Task LoadRecentChats()
        {

        }

        public string getFullnameByLogin(string login)
        {
            string fullname = "";

            int i = getUserArrCntByLogin(login);
           if (i != int.MinValue)
            {
                return users_fullname_dt[i];
            }
            return fullname;

        }


        public ChatDialog getDialogDecoded(string dialog_coded)
        {
            string decoded_dialog = Base64Decode(dialog_coded);
            List<string> dialog_details = new List<string>();

            dialog_details = bc.getMultipleStringDecodingString(decoded_dialog);

           for (int i = 0; i<dialog_details.Count;i++)
            {
                dialog_details[i] = Base64Decode(dialog_details[i]);
            }

           ChatDialog chatDialog = new ChatDialog();

            if (dialog_details.Count > 4)
            { 
                chatDialog.chatid = dialog_details[0];
                chatDialog.chat_name = dialog_details[1];
                chatDialog.avatar_url = dialog_details[2];
                chatDialog.isGroup = bool.Parse(dialog_details[3]);
                chatDialog.last_updated = DateTime.Parse(dialog_details[4]);
            }

            return chatDialog;

        }

        public string getDialogCoded(ChatDialog dialog)
        {
        string coded_dialog = "";
            List<string> coded_dialog_parts = new List<string>();

            coded_dialog_parts.Add(Base64Encode(dialog.chatid));
            coded_dialog_parts.Add(Base64Encode(dialog.chat_name));
            coded_dialog_parts.Add(Base64Encode(dialog.avatar_url));
            coded_dialog_parts.Add(Base64Encode(dialog.isGroup.ToString()));
            coded_dialog_parts.Add(Base64Encode(dialog.last_updated.ToString()));
            
            coded_dialog = bc.getMultipleStringEncodingString(coded_dialog_parts);
            coded_dialog = Base64Encode(coded_dialog);

            return coded_dialog;

        }

        public string getMessageCoded(ChatMessage msg)
        {
            string coded_msg = "";
            List<string> coded_msg_parts = new List<string>();

            coded_msg_parts.Add(Base64Encode(msg.id.ToString()));
            coded_msg_parts.Add(Base64Encode(msg.datetime.ToString()));
            coded_msg_parts.Add(Base64Encode(msg.body));
            coded_msg_parts.Add(Base64Encode(msg.forward_options.is_forward.ToString()));
            coded_msg_parts.Add(Base64Encode(msg.forward_options.in_forward_id));
            coded_msg_parts.Add(Base64Encode(msg.chatid));
            coded_msg_parts.Add(Base64Encode(msg.user));
            coded_msg_parts.Add(Base64Encode(msg.type.ToString()));

            coded_msg = bc.getMultipleStringEncodingString(coded_msg_parts);
            coded_msg = Base64Encode(coded_msg);

            return coded_msg;

        }

        public ChatMessage getMessageDecoded(string msg_coded)
        {
            string decoded_msg = Base64Decode(msg_coded);
            List<string> msg_details = new List<string>();

            msg_details = bc.getMultipleStringDecodingString(decoded_msg);

            for (int i = 0; i < msg_details.Count; i++)
            {
                msg_details[i] = Base64Decode(msg_details[i]);
            }

            ChatMessage chatMessage = new ChatMessage();

            if (msg_details.Count > 7)
            {
                chatMessage.id = msg_details[0];
                chatMessage.datetime = DateTime.Parse(msg_details[1]);
                chatMessage.body = msg_details[2];
                chatMessage.forward_options.is_forward = bool.Parse(msg_details[3]);
                chatMessage.forward_options.in_forward_id = msg_details[4];
                chatMessage.chatid = msg_details[5];
                chatMessage.user = msg_details[6];

                switch (msg_details[7])
                {
                    case "text":
                        chatMessage.type = ChatMessageTypes.text;
                        break;

                    case "audio":
                        chatMessage.type = ChatMessageTypes.audio;
                        break;

                    case "document":
                        chatMessage.type = ChatMessageTypes.document;
                        break;
                }
            }

            return chatMessage;

        }

}
}
