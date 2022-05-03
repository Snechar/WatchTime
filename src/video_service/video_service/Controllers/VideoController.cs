using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace video_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        [HttpGet("{file}")]
        public async Task GetStream(string file)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                string videoPathFile = Path.Combine(workingDirectory, $"Files\\{file}.mp4");
                string[] validExtensions = { ".mp4", ".mov" };
                if (!System.IO.File.Exists(videoPathFile))
                {
                    foreach (var item in validExtensions)
                    {
                        videoPathFile = Path.Combine(workingDirectory, $"Files\\{file}{item}");
                        if (System.IO.File.Exists(videoPathFile))
                        {
                            break;
                        }
                    }
                    if (!System.IO.File.Exists(videoPathFile))
                    {
                        throw new Exception("File not found");
                    }
                }
                byte[] buffer = new byte[1024 * 1024 * 4]; // 'Chunks' of 4MB
                long startPosition = 0;
                if (!string.IsNullOrEmpty(Request.Headers["Range"]))
                {
                    string[] range = Request.Headers["Range"].ToString().Split(new char[] { '=', '-' });
                    startPosition = long.Parse(range[1]);
                }

                using FileStream inputStream = new(videoPathFile, FileMode.Open, FileAccess.Read, FileShare.Read)
                {
                    Position = startPosition
                };
                int chunkSize = await inputStream.ReadAsync(buffer.AsMemory(0, buffer.Length));
                long fileSize = inputStream.Length;

                if (chunkSize > 0)
                {
                    Response.StatusCode = 206;
                    Response.Headers["Accept-Ranges"] = "bytes";
                    Response.Headers["Content-Range"] = string.Format($" bytes {startPosition}-{fileSize - 1}/{fileSize}");
                    Response.ContentType = "application/octet-stream";

                    using Stream outputStream = Response.Body;
                    await outputStream.WriteAsync(buffer.AsMemory(0, chunkSize));
                };
            }
            catch (Exception)
            {

                Response.StatusCode = 500;
                Response.ContentType = "text/plain";
                Response.Headers["string"] = "Could not find song";
            }
           
        }
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("Files").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
