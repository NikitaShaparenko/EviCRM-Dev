namespace EviCRM.Server.Core
{
    using EviCRM.Server.Models.Auth;
    using EviCRM.Server.ViewModels;
    using MySqlConnector;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using static EviCRM.Server.Pages.Projects.ProjectsCreate;
    using EviCRM.Server.ViewModels;
    using EviCRM.Server.Pages.Maps.Models;

    public class MySQL_Controller
        {
        public MySqlConnection mysql_connection = new MySqlConnection();
        public BackendController bc = new BackendController();

        #region Alexandra Maps

        public async Task<List<string>> getListAlexandraLocationDateTime(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT datetime from alexandra_locations WHERE user = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraLocationIDPKEY(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idalexandra_location from alexandra_locations WHERE user = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraLocationLat(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT lat from alexandra_locations WHERE user = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraLocationLng(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT lng from alexandra_locations WHERE user = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraLocationName(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT name from alexandra_locations WHERE user = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public string editAlexandraMapPoint(MapsPoint_ViewModel mvpm, string point_id)
        {
            return "UPDATE alexandra_locations SET lat = '" + mvpm.Lat + "' , lng = '" + mvpm.Lng + "' , user = '" + mvpm.user_login + "' , name = '" + mvpm.Name + "' , datetime = '" + mvpm.addedDateTime.ToString() + "' WHERE idalexandra_location = " + point_id + ";";
        }

        public string deleteAlexandraMapPoint(string point_id)
        {
            return "DELETE FROM alexandra_locations WHERE idalexandra_location = " + point_id + ";";
        }

        #endregion

        #region Alexandra Contacts

        public string deleteAlexandraContactByID(string id_contact)
        {
            return "DELETE FROM alexandra_contacts WHERE idalexandra_contacts = " + id_contact + ";";
        }

        public async Task<List<string>> getListAlexandraContactsIDPKEY(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idalexandra_contacts from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraContactsFirstname(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT firstname from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraContactsLastname(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT lastname from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraContactsPhonenumber(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT phonenumber from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraContactsUserId(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT userId from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAlexandraContactsVcard(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT vcard from alexandra_contacts WHERE login = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        #endregion

        #region Maps MySQL

        public async Task<List<string>> getListMapsPointsIDMaps(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmaps_points from maps_points WHERE point_userlogin = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMapsPointsE(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT point_e from maps_points WHERE point_userlogin = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMapsPointsN(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT point_n from maps_points WHERE point_userlogin = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMapsPointsDescription(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT point_description from maps_points WHERE point_userlogin = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMapsPointsWhenAdd(string user_login)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT point_whenadd from maps_points WHERE point_userlogin = '" + user_login + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public string addMapPoint(MapsPoint_ViewModel mpvm)
        {
            return "INSERT INTO maps_points (point_e, point_n, point_userlogin, point_description, point_whenadd) VALUES ('" + mpvm.Lat + "', '" +  mpvm.Lng + "', '" + mpvm.user_login + "', '" + mpvm.Name +"', '" +  mpvm.addedDateTime.ToString() + "')";
        }

        public string editMapPoint(MapsPoint_ViewModel mvpm, string point_id)
        {
            return "UPDATE maps_points SET point_e = '" + mvpm.Lat + "' , point_n = '" + mvpm.Lng + "' , point_userlogin = '" + mvpm.user_login  + "' , point_description = '" + mvpm.Name + "' , point_whenadd = '" + mvpm.addedDateTime.ToString() + "' WHERE idmaps_points = " + point_id + ";";
        }

        public string deleteMapPoint(string point_id)
        {
            return "DELETE FROM maps_points WHERE idmaps_points = " + point_id + ";";
        }

        #endregion

        #region Markdown Cards
        public async Task<List<string>> getListMarkdownIdmarkdownAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_cards from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardNameAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_name from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDescriptionAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_description from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateStartAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_start from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateEndAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_end from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownImgPathAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_img_path from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardBackColorAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_backcolor from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardTaskIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_taskid from markdown_cards";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownIdmarkdownAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_cards from markdown_cards WHERE idmarkdown_cards = '" +  ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardNameAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_name from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDescriptionAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_description from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateStartAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_start from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateEndAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_end from markdown_cards WHERE idmarkdown_cards = '" +  ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownImgPathAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_img_path from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardBackColorAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_backcolor from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardTaskIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_taskid from markdown_cards WHERE idmarkdown_cards = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListMarkdownIdmarkdownAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_cards from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardNameAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_name from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDeskIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_deskid from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }
        public async Task<List<string>> getListMarkdownCardDescriptionAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_description from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateStartAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_start from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardDateEndAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_date_end from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownImgPathAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_img_path from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardBackColorAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_backcolor from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownCardTaskIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_card_taskid from markdown_cards WHERE markdown_card_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public string createMarkdownCard(Markdown_Card card)
        {
            return "INSERT INTO markdown_cards (markdown_card_name, markdown_card_description, markdown_card_date_start, markdown_card_date_end, markdown_card_img_path, markdown_card_backcolor, markdown_card_taskid) VALUES ('" + card.markdown_card_name + "', '" + card.markdown_card_description + "', '" + card.markdown_card_date_start + "', '" + card.markdown_card_date_end + "', '" + card.markdown_card_img_path + "', '" + card.markdown_card_backcolor + "', '" + card.markdown_card_taskid + "');";
        }

        public string editMarkdownCard(Markdown_Card card)
        {
            return "UPDATE markdown_cards SET markdown_card_name = '" + card.markdown_card_name + "' , markdown_card_description = '" + card.markdown_card_description + "' , markdown_card_date_start = '" + card.markdown_card_date_start + "' , markdown_card_date_end = '" + card.markdown_card_date_end + "' , markdown_card_img_path = '" + card.markdown_card_img_path + "' , markdown_card_backcolor = '" + card.markdown_card_backcolor + "' , markdown_card_taskid = '" + card.markdown_card_taskid  + "' WHERE id = " + card.idmarkdown_cards + ";";
        }

        public string deleteMarkdownCard(Markdown_Card card)
        {
            return "DELETE FROM markdown_cards WHERE idmarkdown_cards = " + card.idmarkdown_cards + ";";
        }

        #endregion

        #region Markdown Desks

        public async Task<List<string>> getListMarkdownDeskIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_desks from markdown_desks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskNameAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_name from markdown_desks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskPersonalIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_personal_id from markdown_desks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskTaskIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_task_id from markdown_desks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_desks from markdown_desks WHERE idmarkdown_desks = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskNameAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_name from markdown_desks WHERE idmarkdown_desks = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskPersonalIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_personal_id from markdown_desks WHERE idmarkdown_desks = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskTaskIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_task_id from markdown_desks WHERE idmarkdown_desks = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_desks from markdown_desks WHERE markdown_desk_task_id = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskNameAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_name from markdown_desks WHERE markdown_desk_task_id = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownDeskPersonalIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_desk_personal_id from markdown_desks WHERE markdown_desk_task_id = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public string createMarkdownDesk(Markdown_Desk desk)
        {
            return "INSERT INTO markdown_desks (markdown_desk_name, markdown_desk_personal_id, markdown_desk_task_id) VALUES ('" + desk.markdown_desk_name + "', '" + desk.markdown_desk_personal_id + "', '" + desk.markdown_desk_task_id + "');";
        }

        public string editMarkdownDesk(Markdown_Desk desk)
        {
            return "UPDATE markdown_desks SET markdown_desk_name = '" + desk.markdown_desk_name + "' , markdown_desk_personal_id = '" + desk.markdown_desk_personal_id + "' , markdown_desk_task_id = '" + desk.markdown_desk_task_id + "' WHERE id = " + desk.idmarkdown_desks + ";";
        }

        public string deleteMarkdownDesk(Markdown_Desk desk)
        {
            return "DELETE FROM markdown_desks WHERE idmarkdown_desks = " + desk.idmarkdown_desks + ";";
        }

        #endregion

        #region Markdown Todos

        public async Task<List<string>> getListMarkdownTodoIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_todo_list from markdown_todo_list";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoTaskIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_taskid from markdown_todo_list";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCheckedAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_checked from markdown_todo_list";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoNameAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_name from markdown_todo_list";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCardIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_cardid from markdown_todo_list";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoTaskIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_taskid FROM markdown_todo_list WHERE idmarkdown_todo_list = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCheckedAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_checked from markdown_todo_list WHERE idmarkdown_todo_list = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoNameAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_name from markdown_todo_list WHERE idmarkdown_todo_list = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCardIDAsyncByID(string ID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_cardid from markdown_todo_list WHERE idmarkdown_todo_list = '" + ID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarkdown_todo_list FROM markdown_todo_list WHERE markdown_todo_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCheckedAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_checked from markdown_todo_list WHERE markdown_todo_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoNameAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_name from markdown_todo_list WHERE markdown_todo_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarkdownTodoCardIDAsyncByTaskID(string taskID)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT markdown_todo_cardid from markdown_todo_list WHERE markdown_todo_taskid = '" + taskID + "'";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public string createMarkdownTodo(Markdown_Todo todo)
        {
            return "INSERT INTO markdown_todo_list (markdown_todo_taskid, markdown_todo_checked, markdown_todo_name, markdown_todo_cardid) VALUES ('" + todo.markdown_todo_taskid  + "', '" + todo.markdown_todo_checked.ToString() + "', '" + todo.markdown_todo_name + "', '" + todo.markdown_todo_cardid + "');";
        }

        public string editMarkdownTodo(Markdown_Todo todo)
        {
            return "UPDATE markdown_todo_list SET markdown_todo_taskid = '" + todo.markdown_todo_taskid + "' , markdown_todo_checked = '" + todo.markdown_todo_checked.ToString() + "' , markdown_todo_name = '" + todo.markdown_todo_name + "' , markdown_todo_cardid = '" + todo.markdown_todo_cardid + "' WHERE id = " + todo.idmarkdown_todo_list + ";";
        }

        public string deleteMarkdownTodo(Markdown_Todo todo)
        {
            return "DELETE FROM markdown_todo_list WHERE " + todo.idmarkdown_todo_list + ";";
        }

        #endregion


        public async Task<List<string>> getListTaskMembersDivsAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT task_members_divs from tasks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        
        public async Task<List<string>> getListTaskMembersDivByIDAsync(string id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT task_members_divs from tasks WHERE id = " + id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public string DeleteCardMarkDownCard(string ID)
        {
           return "DELETE FROM task_tracking_card WHERE ID = " + ID;
        }
        public string AddCardMark(string task_id, string card_body, string card_marksdown, string card_status, string card_action_author, string card_action_datetime)
        {
            return "INSERT INTO task_tracking_card (task_id, card_body, card_marksdown, card_status, card_action_author) VALUES ('" + task_id + "', '" + card_body + "', '" + card_marksdown + "', '" + card_status + "', '" + card_action_author + "');";
        }
        public string setMarkdown(string ID)
        {
            return "UPDATE task_tracking_card SET card_marksdown = 'checked', card_action_datetime = '" + DateTime.Now.ToShortDateString() + "' WHERE ID = '" + ID + "'";
        }

        public string deleteSchedule(string id_mysql)
        {
            return "DELETE FROM calendar_scheludes WHERE idcalendar_scheludes = " + id_mysql;
        }

        public string ProSupressor_DateStart(string proj_id, string new_date)
        {
            return "UPDATE projects SET proj_start = '" + new_date + "' WHERE id = " + proj_id;
        }

        public string ProSupressor_DateEnd(string proj_id, string new_date)
        {
            return "UPDATE projects SET proj_end = '" + new_date + "' WHERE id = " + proj_id;
        }

        public string TOSCRIPT_ADD(string cmd, string var1, string var2, string task_id, string author)
            {
                return "INSERT INTO task_tracking_main (task_cmd, task_variable1, task_variable2, task_id, task_author) VALUES ('" + cmd + "', '" + var1 + "', '" + var2 + "', '" + task_id + "', '" + author + "');";
            }
            public string TOSCRIPT_ADD(string cmd, string var1, string var2, string var3, string task_id, string author)
            {
                return "INSERT INTO task_tracking_main (task_cmd, task_variable1, task_variable2,task_variable3, task_id, task_author) VALUES ('" + cmd + "', '" + var1 + "', '" + var2 + "', '" + var3 + "', '" + task_id + "', '" + author + "');";
            }

        public string updateDivisionByID(ContactsDivisionsModel cdm)
        {
            return "UPDATE divisions SET division_name = '" + cdm.div_name + "' , division_cast = '" + cdm.div_cast + "' , division_responsible = '" + cdm.div_responsible + "' WHERE id = " + cdm.div_ids + ";";
        }


        #region Calendar

        public string updateTaskTrackDateStartByID(string newDate, string task_id)
        {
            return "UPDATE tasks SET task_startdate = '" + newDate + "' WHERE id = " + task_id + ";";
        }

        public string updateTaskTrackDateEndByID(string newDate, string task_id)
        {
            return "UPDATE tasks SET task_enddate = '" + newDate + "' WHERE id = " + task_id + ";";
        }
        public string updateSchelude(string id, string calendarId, string startDate, string endDate, string title, string body, string user_, string isinoffice, string notify, string attachments, string primarykey)
        {
            return "UPDATE calendar_scheludes SET id = '"+ id + "' , calendarId = '" + calendarId + "', startDate = '" +startDate + "', endDate = '" + endDate + "', title = '" + title + "', body = '" + body + "', isinoffice = '" + isinoffice + "', isnotify = '" + notify + "', attachments = '" + attachments + "', users = '" + user_ + "' WHERE idcalendar_scheludes = " + primarykey + ";";
        }

        public string setSchelude(string id, string calendarId, string startDate, string endDate, string title, string body, string users, string isinoffice, string notify, string attach_lst)
        {
            return "INSERT INTO calendar_scheludes (id, calendarId, startDate, endDate, title, body, users, isinoffice, isnotify, attachments) VALUES ('" +  id + "', '" + calendarId + "', '" + startDate + "', '" + endDate + "', '" + title + "', '" + body + "', '" + users + "', '" + isinoffice + "', '" + notify + "', '" + attach_lst + "');";
        }

         public string updateScheludesIdByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET id ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesCalendarIdByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET calendarId ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesStartDateByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET startDate ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesEndDateByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET endDate ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesTitleByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET title ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesBodyByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET body ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }

        public string updateScheludesCalendarCategoryByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET calendarCategory ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesisVisibleByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET isVisible ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesisAllDayByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET isAllDay ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesStateByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET state ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }
        public string updateScheludesUserByID(string ID, string new_data)
        {
            return "UPDATE calendar_scheludes SET users ='" + new_data + "' WHERE idcalendar_scheludes = '" + ID + "';";
        }


        public async Task<List<string>> getListCalendarIDCalendarScheludesAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idcalendar_scheludes FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT id FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarAttachmentsAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT attachments FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIsInOfficeAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT isinoffice FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIsNotifyAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT isnotify FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarCalendarIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT calendarId FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIDStartDateAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT startDate FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIDEndDateAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT endDate FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarTitleAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT title FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarIDBodyAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT body FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarCategoryAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT calendarCategory FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarisVisibleAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT isVisible FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarisAllDayAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT isAllDay FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarStateAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT state FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListCalendarUserAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT users FROM calendar_scheludes";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        #endregion
        #region Marks

        public async Task<List<string>> getListMarksUserIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT user_id FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksFirstAttachmentsAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT firstAttachments FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksSecondAttachmentsAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT secondAttachments FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksDateAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT date FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListMarksFirstMarkAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT first_mark FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksFirstDescriptionAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT first_description FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksSecondMarkAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT second_mark FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idmarks FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListMarksSecondDescriptionAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT second_description FROM marks";

            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

      
        public async Task<List<string>> getListMarksIsTwoMarksAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT isTwoMarks FROM marks";


            string value = "";
            while (mysql_connection.State != System.Data.ConnectionState.Closed)
            {

            }
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public string deleteMark(string idmark)
        {
            return "DELETE FROM marks WHERE idmarks = " + idmark;
        }

        public string updateMark(EviCRM.Server.Models.MarksChart.Marks model)
        {
            return "UPDATE marks SET user_id = '" + model.userID + "' , date = '" + model.date + "', first_mark = '" + model.firstMark + "', second_mark = '" + model.secondMark + "', first_description = '" + model.firstComment + "', second_description = '" + model.secondComment+ "', firstAttachments = '" + bc.getMultipleStringEncodingString(model.firstAttachments) + "', secondAttachments = '" + bc.getMultipleStringEncodingString(model.secondAttachments) + "' WHERE idmarks = " + model.idmarks + ";";
        }

        public string updateMark(string user_id, string date, string first_mark, string first_description, string primarykey,string attachments)
        {
            return "UPDATE marks SET user_id = '" + user_id + "' , date = '" + date + "', first_mark = '" + first_mark  + "', first_description = '" + first_description + "', attachments = '" + attachments + "' WHERE idmarks = " + primarykey + ";";
        }

        public string setMark(string user_id, string date,string first_mark, string first_description)
        {
            return "INSERT INTO marks (user_id, date, first_mark, first_description) VALUES ('" + user_id + "', '" + date + "', '" + first_mark + "', '" + first_description+ "');";
        }

        public string setMark(EviCRM.Server.Models.MarksChart.Marks model)
        {
            return "INSERT INTO marks (user_id, date, first_mark, second_mark, first_description, second_description, isTwoMarks, firstAttachments, secondAttachments) VALUES ('" + model.userID + "', '" + model.date.ToString() + "', '" + model.firstMark.ToString() + "', '" + model.secondMark.ToString() + "', '" + model.firstComment + "', '" + model.secondComment + "', '" + model.isTwoMarks.ToString() + "', '" + bc.getMultipleStringEncodingString(model.firstAttachments) + "', '" + bc.getMultipleStringEncodingString(model.secondAttachments) + "');";
        }
        public string getMarksDateByID(string ID, string new_data)
        {
            return "SELECT date FROM marks WHERE idmarks = '" + ID + "';";
        }
        public string getMarksFirstMarkByID(string ID, string new_data)
        {
            return "SELECT first_mark FROM marks WHERE idmarks = '" + ID + "';";
        }
        public string getMarksSecondMarkByID(string ID, string new_data)
        {
            return "SELECT second_mark FROM marks WHERE idmarks = '" + ID + "';";
        }
        public string getMarksFirstDescriptionByID(string ID, string new_data)
        {
            return "SELECT first_description FROM marks WHERE idmarks = '" + ID + "';";
        }
        public string getMarksSecondDescriptionByID(string ID, string new_data)
        {
            return "SELECT second_description FROM marks WHERE idmarks = '" + ID + "';";
        }
        public string getMarksAttachmentsByID(string ID, string new_data)
        {
            return "SELECT marks attachments FROM marks WHERE idmarks = '" + ID + "';";
        }

        public string updateMarksDateByID(string ID, string new_data)
        {
            return "UPDATE marks SET date ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }
        public string updateMarksFirstMarkByID(string ID, string new_data)
        {
            return "UPDATE marks SET first_mark ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }
        public string updateMarksSecondMarkByID(string ID, string new_data)
        {
            return "UPDATE marks SET second_mark ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }
        public string updateMarksFirstDescriptionByID(string ID, string new_data)
        {
            return "UPDATE marks SET first_description ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }
        public string updateMarksSecondDescriptionByID(string ID, string new_data)
        {
            return "UPDATE marks SET second_description ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }
        public string updateMarksAttachmentsByID(string ID, string new_data)
        {
            return "UPDATE marks SET attachments ='" + new_data + "' WHERE idmarks = '" + ID + "';";
        }

        #endregion

        #region tasks


        public async Task<List<string>> getListTaskTrackingMainCmdAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_cmd from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingMainVar1Async(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_variable1 from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingMainVar2Async(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_variable2 from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListTaskTrackingCardDelegateAsync(string task_id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT card_delegate from task_tracking_card WHERE task_id =" + task_id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }
            public async Task<List<string>> getListTaskTrackingMainVar3Async(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_variable3 from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingAuthorAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_author from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingMainDateTimeAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_datetime from task_tracking_main WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingCardBodyAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT card_body from task_tracking_card WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListTaskTrackingCardIDAsync(string task_id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT ID from task_tracking_card WHERE task_id =" + task_id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }
        public async Task<List<string>> getListTaskTrackingCardMarksDownAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT card_marksdown from task_tracking_card WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingCardStatusAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT card_status from task_tracking_card WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingCardActionDateTimeAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT card_action_datetime from task_tracking_card WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskTrackingCardActionAuthorAsync(string task_id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT card_action_author from task_tracking_card WHERE task_id =" + task_id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskProjAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_proj from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }



            public async Task<List<string>> getListTaskNameAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_name from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskNameByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_name from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListUsersThinClientByIDAsync(string id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT thinclient_user from users WHERE id = " + id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListUsersThinClientAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT thinclient_user from users";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListSkillsGeneralCollectionAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT skillscol from skills";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListTaskMembersAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_members from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public string setListTaskMembersByIDAsync(string str, string ID)
        {
            return "UPDATE tasks SET task_members ='" + str + "' WHERE id = '" + ID + "';";
        }

        public string setListTaskMembersDivsByIDAsync(string str, string ID)
        {
            return "UPDATE tasks SET task_members_divs ='" + str + "' WHERE id = '" + ID + "';";
        }

        public async Task<List<string>> getListTaskMembersByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_members from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskStatusAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_status from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListTaskPersonalStatusAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT tasks_personal_status from tasks";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListTaskPersonalStatusByIDAsync(string task_id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT tasks_personal_status from tasks WHERE id = " + task_id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }
        public async Task<List<string>> getListTaskDescriptionAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_description from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListTaskStatusByIDAsync(string id)
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT task_status from tasks WHERE id = " + id;

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListTasksAuthorAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_author from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTasksAuthorByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_author from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskIDAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT id from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskStartDateAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_startdate from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskEndDateAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_enddate from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskStartDateByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_startdate from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskProjByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_proj from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }
            public async Task<List<string>> getListTaskEndDateByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_enddate from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListTaskBudgetAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_budget from tasks";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListTaskBudgetByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT task_budget from tasks WHERE id = " + id;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public string getUserIDByLogin(string login)
        {
            return "SELECT id FROM users WHERE login = '" + login + "';";
        }
      
             public string getUserAvatarPathByLogin(string login)
        {
            return "SELECT avatar FROM users WHERE login = '" + login + "';";
        }
        public string getDivisionAvatarPathByID(string div_id)
        {
            return "SELECT division_avatar FROM divisions WHERE id = '" + div_id + "';";
        }

        public string getUserAvatarPathByID(string user_id)
        {
            return "SELECT avatar FROM users WHERE id = '" + user_id + "';";
        }

        public string getUsernameByLogin(string login)
            {
                return "SELECT fullname FROM users WHERE login = '" + login + "';";
            }

            public string getDivisionNameByID(string ID)
            {
                return "SELECT division_name FROM divisions WHERE id = '" + ID + "';";
            }

            public string killDivisionByID(string ID)
            {
                return "DELETE FROM divisions WHERE id = '" + ID + "';";
            }
            public string getTasksNameByID(string ID)
            {
                return "SELECT task_name FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTasksStartByID(string ID)
            {
                return "SELECT task_startdate FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTasksEndByID(string ID)
            {
                return "SELECT task_enddate FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTasksDescriptionByID(string ID)
            {
                return "SELECT task_description FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTaskStartDateByID(string ID)
            {
                return "SELECT task_startdate FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskEndDateByID(string ID)
            {
                return "SELECT task_enddate FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTaskEndDateRealByID(string ID)
            {
                return "SELECT task_enddate_real FROM tasks WHERE id = '" + ID + "';";
            }

            public string getTaskAttachmentsByID(string ID)
            {
                return "SELECT task_attachments FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskBudgetByID(string ID)
            {
                return "SELECT task_budget FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskMembersByID(string ID)
            {
                return "SELECT task_members FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskStatusByID(string ID)
            {
                return "SELECT task_status FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskResponsibleMemberByID(string ID)
            {
                return "SELECT task_responsible_member FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskAuthorByID(string ID)
            {
                return "SELECT task_author FROM tasks WHERE id = '" + ID + "';";
            }
            public string getTaskNotifyByID(string ID)
            {
                return "SELECT task_notify FROM tasks WHERE id = '" + ID + "';";
            }

        public string getProjectUsersRolesByIdentityName(string session_cookie_data)
		{
            return "SELECT role FROM users WHERE login = '" + session_cookie_data + "';";
		}
        public string getTaskProjByID(string ID)
            {
                return "SELECT task_proj FROM tasks WHERE id = '" + ID + "';";
            }

            #endregion
            #region projects

            public string getEmailInboxIDByMessageID(string msgID)
            {
                return "SELECT idaux_ FROM aux_email_inbox WHERE message_ID = '" + msgID + "';";
            }
            public string getKeyStoringVal1ByID(string indexID)
            {
                return "SELECT keys_val1 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }
            public string getKeyStoringVal2ByID(string indexID)
            {
                return "SELECT keys_val2 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }
            public string getKeyStoringVal3ByID(string indexID)
            {
                return "SELECT keys_val3 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }
            public string getKeyStoringVal4ByID(string indexID)
            {
                return "SELECT keys_val4 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }

            public string getKeyStoringVal5ByID(string indexID)
            {
                return "SELECT keys_val5 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }

            public string getKeyStoringVal6ByID(string indexID)
            {
                return "SELECT keys_val6 FROM key_store WHERE idkey_store = '" + indexID + "';";
            }

            public string getKeyStoringProgByID(string indexID)
            {
                return "SELECT keys_prog FROM key_store WHERE idkey_store = '" + indexID + "';";
            }

            public string getEmailInboxMsgIDbyID(string idaux)
            {
                return "SELECT message_ID FROM aux_email_inbox WHERE idaux_ = '" + idaux + "';";
            }

            public string getEmailInboxUnreadByID(string msgID)
            {
                return "SELECT unread FROM aux_email_inbox WHERE message_ID = '" + msgID + "';";
            }

            public string setAddEmailInbox(string messageID, string recepient)
            {
                return "INSERT INTO aux_email_inbox (message_ID,recepient) VALUES ('" + messageID + "'; '" + recepient + ");";
            }



            public async Task<List<string>> getEmailIDAUXByID(string ID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT idaux_ from aux_email_inbox WHERE message_ID = '" + ID + "';";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getEmailUnreadByID(string ID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT unread from aux_email_inbox WHERE idaux_ =" + ID;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getEmailUnreadByMessageID(string msgID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT unread from aux_email_inbox WHERE message_ID =" + msgID;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }



            public string getProjectsIDByName(string name)
            {
                return "SELECT id FROM projects WHERE proj_name = '" + name + "';";
            }

            public string getProjectsNameByID(string ID)
            {
                return "SELECT proj_name FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectsDescriptionByID(string ID)
            {
                return "SELECT proj_description FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectDetailsByID(string ID)
            {
                return "SELECT proj_details FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectDetailsPointsByID(string ID)
            {
                return "SELECT proj_details FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectStartDateByID(string ID)
            {
                return "SELECT proj_start FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectEndDateByID(string ID)
            {
                return "SELECT proj_end FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectUsersByID(string ID)
            {
                return "SELECT proj_users FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectUsersRolesByID(string ID)
            {
                return "SELECT proj_users_roles FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectAttachmentsByID(string ID)
            {
                return "SELECT proj_attachments FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectAttachmentsListByID(string ID)
            {
                return "SELECT proj_attachments_list FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectWeeksChangesByID(string ID)
            {
                return "SELECT proj_weeks_changes FROM projects WHERE id = '" + ID + "';";
            }
            public string getProjectWeeksChangesValsByID(string ID)
            {
                return "SELECT proj_weeks_changes_vals FROM projects WHERE id = '" + ID + "';";
            }

            public async Task<List<string>> getUserCollectionByID(string ID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT * from users WHERE ID=" + ID;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListDivisionsIDAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT id from divisions";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjAvatarPathAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_avatar_path from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListDivisionsNameAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT division_name from divisions";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

        public async Task<List<string>> getListProjDivisions()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT proj_divs from projects";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListDivisionsAvatarAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT division_avatar from divisions";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        //-----------------

        public async Task<List<string>> getListAuxWorkingDaysIDAUXAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT idaux_workingdays from aux_workingdays";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListAuxWorkingDaysUserIDAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT user_id from aux_workingdays";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListAuxWorkingDaysDateAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT date from aux_workingdays";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        public async Task<List<string>> getListAuxWorkingDaysDayStartAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT day_start from aux_workingdays";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }

        public async Task<List<string>> getListAuxWorkingDaysDayEndAsync()
        {
            List<string> dt = new List<string>();

            string cmd = "SELECT day_end from aux_workingdays";

            string value = "";
            await mysql_connection.OpenAsync();

            using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader.GetValue(0).ToString();
                dt.Add(value);
            }
            mysql_connection.Close();
            return dt;
        }


        //----------------


        public async Task<List<string>> getListDivisionsCastByIDAsync(string id)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT division_cast from divisions WHERE id = '" + id + "'";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListDivisionsCastAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT division_cast from divisions";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListProjStatusAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_status from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjStartDateAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_start from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjMembersAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_users from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjEndAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_end from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjIDAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT id from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjDescriptionAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_details from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjNameAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_name from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListDivisionsResponsibleAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT division_responsible from divisions";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListProjUsersAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_users from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }




            public async Task<List<string>> getTasksCollectionByID(string ID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT * from tasks WHERE ID=" + ID;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            #endregion



            public async Task<string> MySql_ContextAsyncL( string cmd)
            {
                string value = "";
           
            await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncTCM(string cmd)
            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncR(string cmd)
            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncFUD(ContactsProfileFillModel cpfm, string cmd)
            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncAdmHealth(string cmd)
            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncAdmHealthNM(string cmd)
            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_ContextAsyncGeneral(string cmd)
            {
                string value = "";
                
            while (mysql_connection.State == System.Data.ConnectionState.Connecting || mysql_connection.State == System.Data.ConnectionState.Open)
            {
               
            }
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public async Task<string> MySql_UserDataGeneral(string cmd)

            {
                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    Debug.WriteLine(value);
                    // do something with 'value'
                }

                mysql_connection.Close();
                return value;
            }

            public string getUserDataCompletedProjectsByID(string ID)
            {
                return "SELECT completed_proj FROM users WHERE id = '" + ID + "';";
            }
            public string getUserDataPendingProjectsByID(string ID)
            {
                return "SELECT pending_proj FROM users WHERE id = '" + ID + "';";
            }
            public string getUserDataTotalRevenueByID(string ID)
            {
                return "SELECT total_revenue FROM users WHERE id = '" + ID + "';";
            }
            public string getUserDataRevenueByID(string ID)
            {
                return "SELECT revenue FROM users WHERE id = '" + ID + "';";
            }
            public string getUserDataBioByID(string ID)
            {
                return "SELECT description FROM users WHERE id = '" + ID + "';";
            }
            public string getUserDataFullNameByID(string ID)
            {
                return "SELECT fullname FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDataEmailByID(string ID)
            {
                return "SELECT email FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDataMobileByID(string ID)
            {
                return "SELECT mobilephone FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDataLocationByID(string ID)
            {
                return "SELECT location FROM users WHERE id = '" + ID + "';";
            }

            #region Set field



            public string updateDivisionCastByID(string ID, string new_data)
            {
                return "UPDATE divisions SET division_cast ='" + new_data + "' WHERE id = '" + ID + "';";
            }
            public string updateDivisionNameByID(string ID, string new_data)
            {
                return "UPDATE divisions SET division_name ='" + new_data + "' WHERE id = '" + ID + "';";
            }
            public string updateDivisionResponsibleByID(string ID, string new_data)
            {
                return "UPDATE divisions SET division_responsible ='" + new_data + "' WHERE id = '" + ID + "';";
            }
            public string updateUserPasswordByID(string ID, string new_data)
            {
                return "UPDATE users SET password ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserTokenByID(string ID, string new_data)
            {
                return "UPDATE users SET token ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserTimeTokenByID(string ID, string new_data)
            {
                return "UPDATE users SET timetoken ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserFillInfo(string ID, string fullname, string bio, string position, string division, string mobilephone, string location, string telegram_chatid, string avatar, string skills)
            {
                return "UPDATE users SET fullname ='" + fullname + "', position ='" + position + "', division ='" + division + "', mobilephone ='" + mobilephone + "', location ='" + location + "', telegram_chatid ='" + telegram_chatid + "', description ='" + bio + "', skills ='" + skills + "', avatar = '" + avatar + "', registered ='1' WHERE id = '" + ID + "';";
            }

            public string updateUserFullNameByID(string ID, string new_data)
            {
                return "UPDATE users SET fullname ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserCompanyByID(string ID, string new_data)
            {
                return "UPDATE users SET company ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserPositionByID(string ID, string new_data)
            {
                return "UPDATE users SET position ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserDepartmentByID(string ID, string new_data)
            {
                return "UPDATE users SET token ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserDivisionByID(string ID, string new_data)
            {
                return "UPDATE users SET division ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserSkillsByID(string ID, string new_data)
            {
                return "UPDATE users SET skills ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserMobilephoneByID(string ID, string new_data)
            {
                return "UPDATE users SET mobilephone ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserLocationByID(string ID, string new_data)
            {
                return "UPDATE users SET location ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserExperienceS1ByID(string ID, string new_data)
            {
                return "UPDATE users SET experience_s1 ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserExperienceS2ByID(string ID, string new_data)
            {
                return "UPDATE users SET experience_s2 ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserCompletedPorjByID(string ID, string new_data)
            {
                return "UPDATE users SET completed_proj ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserPendingProjByID(string ID, string new_data)
            {
                return "UPDATE users SET pending_proj ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserTotalRevenueByID(string ID, string new_data)
            {
                return "UPDATE users SET total_revenue ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateRevenueByID(string ID, string new_data)
            {
                return "UPDATE users SET revenue ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserBirthdayByID(string ID, string new_data)
            {
                return "UPDATE users SET birthday ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserLastLoginByID(string ID, string new_data)
            {
                return "UPDATE users SET last_login ='" + new_data + "' WHERE id = '" + ID + "';";
            }

            public string updateUserDescriptionByID(string ID, string new_data)
            {
                return "UPDATE users SET description ='" + new_data + "' WHERE id = '" + ID + "';";
            }
            public string updateUserActivationByID(string ID, string newdata)
            {
                return "UPDATE users SET flag_activated ='" + newdata + "' WHERE id = '" + ID + "';";
            }

        public string updateUserRoleByID(string ID, string newdata)
        {
            return "UPDATE users SET role ='" + newdata + "' WHERE id = '" + ID + "';";
        }

        public string getUserNameByID(string ID)
            {
                return "SELECT login FROM users WHERE id = '" + ID + "';";
            }




            public async Task<List<string>> getListDivisionByID(string ID)
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT * from divisions WHERE ID=" + ID;

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            #endregion

            #region Create field

            public string setCreateUser(string username, string password, string email)
            {
                return "INSERT INTO users (login, password, email) VALUES ('" + username + "', '" + password + "', '" + email + "');";
            }

            public string setAddTelegramMessage(string chatID, string text)
            {
                return "INSERT INTO telegram_push (chatID, message_body) VALUES ('" + chatID + "', '" + text + "');";

            }

            public string setCreateTask(string name, string description, string startdate, string enddate, string attachments, string budget, string members, string divs, string author, string proj,string task_personal_status)
            {
                return "INSERT INTO tasks (task_name,task_description,task_startdate,task_enddate,task_attachments,task_budget,task_members,task_members_divs,task_author,task_status,task_proj,tasks_personal_status) VALUES ('" + name + "', '" + description + "', '" + startdate + "', '" + enddate + "', '" + attachments + "', '" + budget + "', '" + members + "', '" + divs + "', '" + author + "', '" + "waiting" + "', '" + proj + "', '" + task_personal_status + "'); ";
            }

            public string setCreateProject(string proj_name, string proj_description, string proj_details, DateTime proj_start, DateTime proj_end, string proj_users, string proj_divs, string proj_tasks, ProjectStatus proj_status, string proj_avatar_path)
            {
                return "INSERT INTO projects (proj_name, proj_description, proj_details, proj_start, proj_end, proj_users, proj_divs, proj_tasks, proj_status, proj_avatar_path) VALUES ('" + proj_name + "', '" + proj_description + "', '" + proj_details + "', '" + proj_start.ToShortDateString() + "', '" + proj_end.ToShortDateString() + "', '" + proj_users + "', '" + proj_divs + "', '" + proj_tasks + "', '" + proj_status.ToString() + "', '" + proj_avatar_path + "'); ";
            }


        public string update_personal_status(string task_id, string new_status_lst)
        {
            return "UPDATE tasks SET tasks_personal_status = '" + new_status_lst + "' WHERE id = '" + task_id + "'";
        }

        public string updateTaskStatus(string task_id, string task_val)
        {
            return "UPDATE tasks SET task_status = '" + task_val + "' WHERE id = '" + task_id + "'";
        }
            public string AddDivision(string div_name, string div_cast, string div_responsible)
            {
                return "INSERT INTO divisions (division_name, division_cast, division_responsible) VALUES ('" + div_name + "', '" + div_cast + "', '" + div_responsible + "'); ";
            }

            #endregion




            #region Delete field
            public string delUserByID(string ID)
            {
                return "DELETE FROM users WHERE id = '" + ID + "';";
            }


            #endregion


            #region Get Field

            public async Task<List<string>> getListUsersFlagActivatedAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT flag_activated from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListProjShortDescriptionAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT proj_description from projects";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersDivisionAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT division from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersBiosAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT description from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }



            public async Task<List<string>> getListUsersRoleAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT role from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }


            public async Task<List<string>> getListUsersPasswordAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT password from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersEmailAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT email from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersFullnameAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT fullname from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersAvatarAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT avatar from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersLoginAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT login from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersNameAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT fullname from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersIDAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT id from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersPositionsAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT position from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public async Task<List<string>> getListUsersProjectsAsync()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT list_proj from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }



            public async Task<List<string>> getListUsersSkills()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT skills from users";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public List<string> getAvatarPaths(List<string> users)
            {
                List<string> lstr = new List<string>();
                foreach (string user in users)
                {
                    lstr.Add("/" + user + ".png");
                }
                return lstr;
            }

            public string getUserFillProfileInfoByID(string id)
            {
                return "SELECT registered FROM users WHERE id = '" + id + "';";
            }



            public string getUserActivatedByID(string id)
            {
                return "SELECT flag_activated FROM users WHERE id = '" + id + "';";
            }

            public string getUserIDByName(string username)
            {
                return "SELECT id FROM users WHERE login = '" + username + "';";
            }

            public string getUserNameByEmail(string email)
            {
                return "SELECT id FROM users WHERE email = '" + email + "';";
            }

            public string getUserPasswordByID(string ID)
            {
                return "SELECT password FROM users WHERE id = '" + ID + "';";
            }

            public string getUserTokenByID(string ID)
            {
                return "SELECT token FROM users WHERE id = '" + ID + "';";
            }

            public string getUserTimeTokenByID(string ID)
            {
                return "SELECT timetoken FROM users WHERE id = '" + ID + "';";
            }

            public string getUserFullNameByID(string ID)
            {
                return "SELECT fullname FROM users WHERE id = '" + ID + "';";
            }

            public string getUserCompanyByID(string ID)
            {
                return "SELECT company FROM users WHERE id = '" + ID + "';";
            }

            public string getUserPositionByID(string ID)
            {
                return "SELECT position FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDepartmentByID(string ID)
            {
                return "SELECT token FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDivisionByID(string ID)
            {
                return "SELECT division FROM users WHERE id = '" + ID + "';";
            }

            public string getUserTelegramByUsername(string ID)
            {
                return "SELECT telegram_chatid FROM users WHERE login = '" + ID + "';";
            }

            public string getUserSkillsByID(string ID)
            {
                return "SELECT skills FROM users WHERE id = '" + ID + "';";
            }

            public string getUserMobilephoneByID(string ID)
            {
                return "SELECT mobilephone FROM users WHERE id = '" + ID + "';";
            }

            public string getUserLocationByID(string ID)
            {
                return "SELECT location FROM users WHERE id = '" + ID + "';";
            }

            public string getUserExperienceS1ByID(string ID)
            {
                return "SELECT experience_s1 FROM users WHERE id = '" + ID + "';";
            }

            public string getUserExperienceS2ByID(string ID)
            {
                return "SELECT experience_s2 FROM users WHERE id = '" + ID + "';";
            }

            public string getUserCompletedPorjByID(string ID)
            {
                return "SELECT completed_proj FROM users WHERE id = '" + ID + "';";
            }

            public string getUserPendingProjByID(string ID)
            {
                return "SELECT pending_proj FROM users WHERE id = '" + ID + "';";
            }

            public string getUserTotalRevenueByID(string ID)
            {
                return "SELECT total_revenue FROM users WHERE id = '" + ID + "';";
            }

            public string getRevenueByID(string ID)
            {
                return "SELECT revenue FROM users WHERE id = '" + ID + "';";
            }

            public string getUserBirthdayByID(string ID)
            {
                return "SELECT birthday FROM users WHERE id = '" + ID + "';";
            }

            public string getUserLastLoginByID(string ID)
            {
                return "SELECT last_login FROM users WHERE id = '" + ID + "';";
            }

            public string getUserDescriptionByID(string ID)
            {
                return "SELECT description FROM users WHERE id = '" + ID + "';";
            }


            public string getAHlinkByID(string ID)
            {
                return "SELECT link FROM a_health WHERE idhealth = '" + ID + "';";
            }

            public async Task<List<string>> getAMHealthLinks()
            {
                List<string> dt = new List<string>();

                string cmd = "SELECT link from a_health";

                string value = "";
                await mysql_connection.OpenAsync();

                using var command = new MySqlCommand(cmd, mysql_connection); using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    value = reader.GetValue(0).ToString();
                    dt.Add(value);
                }
                mysql_connection.Close();
                return dt;
            }

            public string setAddLink(string link)
            {
                return "INSERT INTO a_health (link) VALUES ('" + link + "');";
            }

            public string delLink(string link)
            {
                string doclear = "";
                for (int i = 0; i < link.Length; i++)
                {
                    doclear += link[i];
                }
                return "DELETE FROM a_health WHERE link='" + doclear + "';";
            }

            public string setRoleByID(string ID, string role)
            {
                return "UPDATE users SET role ='" + role + "' WHERE id = '" + ID + "';";
            }

            public string getRoleByID(string ID)
            {
                return "SELECT role FROM users WHERE id = '" + ID + "';";
            }
            #endregion


            #region TOScript

            public string toscript_general(string cmd, string var1, string var2)
            {
                return "INSERT INTO task_tracking_main (task_cmd, task_variable1, task_variable2) VALUES ('" + cmd + "', '" + var1 + "', '" + var2 + "');";
            }


            #endregion
        }
    }
