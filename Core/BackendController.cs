using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Http;
using EviCRM.Server.Core;

namespace EviCRM.Server.Core
{

    public class CRC32
    {
        private readonly uint[] _table;
        private const uint Poly = 0xedb88320;

        private uint ComputeChecksum(IEnumerable<byte> bytes)
        {
            var crc = 0xffffffff;
            foreach (var t in bytes)
            {
                var index = (byte)((crc & 0xff) ^ t);
                crc = (crc >> 8) ^ _table[index];
            }

            return ~crc;
        }

        public IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }

        public CRC32()
        {
            _table = new uint[256];
            for (uint i = 0; i < _table.Length; ++i)
            {
                var temp = i;
                for (var j = 8; j > 0; --j)
                    if ((temp & 1) == 1)
                        temp = (temp >> 1) ^ Poly;
                    else
                        temp >>= 1;
                _table[i] = temp;
            }
        }
    }
    public class BackendController
    {
        const int backendPort = 9251;
        const string backendAddress = "127.0.0.1";

        public static MySQL_Controller mysqlc = new MySQL_Controller();
        public static BackendController bc = new BackendController();

        public string SHA512_Encode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
            public string getMultArgs_Combine(string args1, string args2)
        {
            return (args1 + "$===$" + args2);
        }


        public string getMultArgs_Combine(List<string> args1, List<string> args2)
        {
            List<string> answer = new List<string>();

            for (int az = 0; az< args1.Count; az++)
            {
                answer.Add(getMultArgs_Combine(args1[az], args2[az]));
            }
            return bc.getMultipleStringEncodingString(answer);
        }

        public List<string> getMultArgs_DeCombine_Args1(string input_line)
        {
            List<string> il_dt = bc.getMultipleStringDecodingString(input_line);
            List<string> il_answer_dt = new List<string>();

            foreach(string il in il_dt)
            {
                // arg1=arg2

                if (il.Contains("$===$"))
                {
                    int equal_pos = il.IndexOf("$===$");
                    if (equal_pos == -1)
                    {
                        il_answer_dt.Add(il.Substring(0,equal_pos - 1));
                    }
                }
            }
            return il_answer_dt;
        }

        public List<string> getMultArgs_DeCombine_Args2(string input_line)
        {
            List<string> il_dt = bc.getMultipleStringDecodingString(input_line);
            List<string> il_answer_dt = new List<string>();

            foreach (string il in il_dt)
            {
                // arg1=arg2

                if (il.Contains("$===$"))
                {
                    int equal_pos = il.IndexOf("$===$");
                    if (equal_pos != -1)
                    {
                        il_answer_dt.Add(il.Substring(equal_pos + 5, (il.Length - equal_pos - 5)));
                    }
                }
            }
            return il_answer_dt;
        }

        public List<string> getMultArgs_DeCombine_Args1(List<string> il_dt)
        {
            List<string> il_answer_dt = new List<string>();

            foreach (string il in il_dt)
            {
                // arg1=arg2

                if (il.Contains("$===$"))
                {
                    int equal_pos = il.IndexOf("$===$");
                    if (equal_pos != -1)
                    {
                        il_answer_dt.Add(il.Substring(0, equal_pos));
                    }
                }
            }
            return il_answer_dt;
        }

        public List<string> getMultArgs_DeCombine_Args2(List<string> il_dt)
        {
            List<string> il_answer_dt = new List<string>();

            foreach (string il in il_dt)
            {
                // arg1=arg2

                if (il.Contains("$===$"))
                {
                    int equal_pos = il.IndexOf("$===$");
                    if (equal_pos != -1)
                    {
                        il_answer_dt.Add(il.Substring(equal_pos+5, (il.Length-equal_pos - 5)));
                    }
                }
            }
            return il_answer_dt;
        }


        public async Task SendAlexandraEnvironment(IWebHostEnvironment iwebhost)
        {
            List<string> cmd_bn = new List<string>();
            List<string> cmd_bv = new List<string>();

            cmd_bn.Add("cmd");
            cmd_bv.Add("set_env_path");

            cmd_bn.Add("env_path");
            cmd_bv.Add(Base64Encode(iwebhost.WebRootPath));

            List<string> chatID_collections = new List<string>();

            string telegram_status_cmd = bc.backend_GetLinkBuilder(cmd_bn, cmd_bv);
            string telegram_status = await bc.backend_PostAsync("http://localhost:9254/get/", telegram_status_cmd);
        }

        public async Task<string> SendTelegramMessage(string input_text, string task_id)
        {

            //List<string> chatID_collection = new List<string>();

            List<string> task_members = await mysqlc.getListTaskMembersByIDAsync(task_id);

            List<string> task_author = await mysqlc.getListTasksAuthorByIDAsync(task_id);

            List<string> task_members_decoded = bc.getMultipleStringDecodingString(task_members[0]);


            List<string> workers = new List<string>();
            List<string> divs = new List<string>();

            List<string> users_dt = new List<string>();
            users_dt = await mysqlc.getListUsersLoginAsync();

            foreach (string str_dl in task_members_decoded)
            {
                if (users_dt.Contains(str_dl))
                {
                    workers.Add(str_dl);
                }
                else
                {

                    divs.Add(str_dl);

                }
            }

            List<string> address_book = new List<string>();
            address_book.Add(task_author[0]);
            foreach (string worker in workers)
            {
                address_book.Add(worker);
            }
            foreach (string div in divs)
            {
                List<string> div_cast = await mysqlc.getListDivisionsCastByIDAsync(div);
                string div_cast_f = div_cast[0];
                List<string> div_cast_formatted = bc.getMultipleStringDecodingString(div_cast_f);

                foreach (string cast_str in div_cast_formatted)
                {
                    address_book.Add(cast_str);
                }

            }

            List<string> duplicates = new List<string>();


            foreach (string str_copy in address_book)
            {
                duplicates.Add(str_copy);
            }

            
            string result = "";
          

            foreach (string str_d in duplicates)
            {
                List<string> cmd_bn = new List<string>();
                List<string> cmd_bv = new List<string>();

                cmd_bn.Add("cmd");
                cmd_bv.Add("telegram_direct");

                cmd_bn.Add("login");
                cmd_bv.Add(str_d);

                cmd_bn.Add("message");
                cmd_bv.Add(Base64Encode(input_text));
                List<string> chatID_collections = new List<string>();

                string telegram_status_cmd = bc.backend_GetLinkBuilder(cmd_bn, cmd_bv);
                string telegram_status = await bc.backend_PostAsync("http://localhost:9254/get/", telegram_status_cmd);

            }

            return result;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

            public void SendAvatarToCropHandler()
        {
            List<string> cmd_bn = new List<string>();
            List<string> cmd_bv = new List<string>();

            cmd_bn.Add("cmd");
            cmd_bv.Add("avatar_crop");

            List<string> chatID_collections = new List<string>();

            string telegram_status_cmd = bc.backend_GetLinkBuilder(cmd_bn, cmd_bv);
            string telegram_status = bc.Post(telegram_status_cmd);
        }

        public List<string> parseUsers(string members_string)
        {
            List<string> users = new List<string>();

            string[] words = members_string.Split('#');

            foreach (var word in words)
            {
                users.Add(word);
            }
            return users;
        }

        public async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public string backend_GetLinkBuilder(List<string> input_data_names, List<string> input_data_values)
        {
            string base_url = "http://localhost:9254/get?";
            for (int i = 0; i < input_data_names.Count - 1; i++)
            {
                base_url += input_data_names[i] + "=" + input_data_values[i] + "&";
            }
            base_url += input_data_names[input_data_values.Count - 1] + "=" + input_data_values[input_data_values.Count - 1];
            return base_url;
        }

        public string backend_GetLinkBuilder(string input_data_name, string input_data_value)
        {
            string base_url = "http://127.0.0.1:9254/get";
            base_url += input_data_name + "=" + input_data_value;
            return base_url;
        }

        public async Task<string> backend_GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);
            return await Task.Run(() => content);
        }
        public async Task<string> backend_PostAsync(string uri, string data)
        {
            var httpClient = new HttpClient();
            
            var response = await httpClient.PostAsync(uri, new StringContent(data));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => content);
        }

        public string Post(string data_in)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:9254/get/");
            // var token = UUIDGenerator();

            var postData = Uri.EscapeDataString(data_in);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString.ToString();
        }

        private static object UUIDGenerator()
        {
            throw new NotImplementedException();
        }



        public bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
        public int getHTTPCode(string link)
        {
            var builder = new UriBuilder(link);
            var uri = builder.Uri;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.Timeout = 1000;
                var response = (HttpWebResponse)webRequest.GetResponse();
                // This will have statii from 200 to 30x
                return (int)response.StatusCode;
            }
            catch (WebException we) when (we.Response is HttpWebResponse response)
            {
                // Statii 400 to 50x will be here
                if (int.TryParse(response.StatusCode.ToString(), out int a))
                {
                    return int.Parse(response.StatusCode.ToString());
                }
                else
                {
                    return 200;
                }
            }
            catch (Exception ex)
            {
                return int.Parse("405");
            }
        }

        public string getMultipleStringEncodingString(List<string> input_str)
        {
            if (input_str != null)
            { 

            string encoded = "";
            foreach (string str in input_str)
            {
                encoded += str + "$$$";
            }
                return encoded;
            }
            return "";
           
        }
        public List<string> getMultipleStringDecodingString(string input_str)
        {
            List<string> encoded = new List<string>();
            if (input_str != null)
            {
                string[] subs = input_str.Split("$$$", StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in subs)
                {
                    encoded.Add(str);
                }
                return encoded;
            }
            else
            {
                encoded.Add("");
                return encoded;
            }
        }

        public string getHTTPCodeDescription(int http_code)
        {
            switch (http_code)
            {
                case 200:
                    return "[Успех] Всё в порядке!";
                case 301:
                    return "[Успех] Файл перемещён, используется переадрессация";
                case 404:
                    return "[ОШИБКА] Файл не найден!";
                case 405:
                    return "[ОШИБКА] Метод не поддерживается!";
                case 500:
                    return "[ОШИБКА] Критическая ошибка сервера";
                case 403:
                    return "[ОШИБКА] Ошибка доступа, доступ к данной папке каким-либо образом ограничен";
                case 502:
                    return "[ОШИБКА] Сервер не может обработать запрос и не знает что ответить";
                default:
                    return "[Инфо] Не знаю такого кода";
            }

        }
        
    }
}
