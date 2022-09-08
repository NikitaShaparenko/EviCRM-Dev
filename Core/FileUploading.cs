using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using IO = System.IO;

using Microsoft.AspNetCore.Hosting;

namespace EviCRM.Server.Core
{
    [Route("api/")]
    [ApiController]
    public class FileUploading:ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileUploading(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("Alexandra/{command}:{value}")]
        public async Task<bool> AlexandraWebServerHandler(string command, string value)
        {
            return true;
        }

        [HttpPost("AppendFileMarks/{fragment}")]
        public async Task<bool> UploadFileChunkMarks(int fragment, IFormFile file)
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            try
            {

                var fileName = Path.Combine(_env.WebRootPath, "uploads", "markschart", file.FileName);

                //if (fragment == 0 && IO.File.Exists(fileName))
                //{
                //    IO.File.Delete(fileName);
                //}
                using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }
            return false;
        }

        [HttpPost("AppendFileMarks2/{fragment}")]
        public async Task<bool> UploadFileChunkMarks2(int fragment, IFormFile file)
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            try
            {

                var fileName = Path.Combine(_env.WebRootPath, "uploads", "markschart", file.FileName);

                //if (fragment == 0 && IO.File.Exists(fileName))
                //{
                //    IO.File.Delete(fileName);
                //}
                using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }
            return false;
        }

        [HttpPost("CalendarUploadFile/{fragment}")]
        public async Task<bool> UploadFileChunk(int fragment, IFormFile file)
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            try
            {
                
                var fileName = Path.Combine(_env.WebRootPath, "uploads", "calendar",file.FileName);

                //if (fragment == 0 && IO.File.Exists(fileName))
                //{
                //    IO.File.Delete(fileName);
                //}
                using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }
            return false;
        }

        [HttpPost("AppendFileTaskTracking/{fragment}")]
        public async Task<bool> UploadFileChunkTaskTracking(int fragment, IFormFile file)
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            try
            {

                var fileName = Path.Combine(_env.WebRootPath, "uploads", "tasktracking", file.FileName);

                //if (fragment == 0 && IO.File.Exists(fileName))
                //{
                //    IO.File.Delete(fileName);
                //}
                using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }
            return false;
        }
    }
}