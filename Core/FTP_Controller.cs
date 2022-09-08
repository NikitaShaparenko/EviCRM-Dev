
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace EviCRM.Server.Core
{
    public class FTP_Controller
    {
        public string getUserDecrypt(string idkey)
        {
            string str = "";
            return str;
        }



        public bool CreateFolder(string host, string UserId, string Password, string path)
        {
            //string path = "/Index";
            bool IsCreated = true;
            try
            {
                WebRequest request = WebRequest.Create(host + path);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(UserId, Password);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine("FTP Client response: " + resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                IsCreated = false;
            }
            return IsCreated;
        }

        public bool DoesFtpDirectoryExist(string host, string UserId, string Password, string dirPath)
        {
            bool isexist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(UserId, Password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isexist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }
            return isexist;
        }

        public void UploadFile(IFormFile uploadfile_from, string full_uri_path, string host, string UserId, string Password)
        {
            //string From = @"F:\Kaushik\Test.xlsx";
            //string To = "ftp://192.168.1.103:24/directory/Test.xlsx";

            FtpWebRequest request =
        (FtpWebRequest)WebRequest.Create(full_uri_path);
            request.Credentials = new NetworkCredential(UserId, Password);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                uploadfile_from.CopyTo(ftpStream);
            }

        }

        private List<string> GetAllFtpFiles(string host, string UserId, string Password, string ParentFolderpath)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);
                ftpRequest.Credentials = new NetworkCredential(UserId, Password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var lineArr = line.Split('/');
                    line = lineArr[lineArr.Count() - 1];
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteFTPFolder(string Folderpath, string host, string UserId, string Password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Folderpath);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = new System.Net.NetworkCredential(UserId, Password); ;
            request.GetResponse().Close();
        }
    }
}
