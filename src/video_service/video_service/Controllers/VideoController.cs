using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using video_service.Context;
using video_service.Model;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace video_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

       private readonly VideoContext _context;
        private readonly IConfiguration _configuration;

        public VideoController(VideoContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }
         
        [HttpGet("{file}")]
        public async Task GetStream(string file)
        {
            try
            {
                string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine(workingDirectory);
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
                Response.Headers["string"] = "Could not find video";
            }


           
        }
        [HttpGet("getvideo/{page}")]
        public async Task<ActionResult<List<VideoModel>>> GetVideos(int page)
        {
            try
            {
               List<VideoModel> list = _context.videoModels.OrderByDescending(x=>x.dateTime).Skip(30 * (page-1)).Take(30 * page).Include(p=>p.Author).ToList();
                return list;
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("postvideo")]
        public async Task PostVideo([FromForm]VideoUploadModel data)
        {
            
            var identity = User.Identity.Name;
            Console.WriteLine(identity);
            var id =  User.FindFirst("id").Value;
            Console.WriteLine(id);
            try
            {
                var file = data.file;
                string[] validExtensions = { ".mp4", ".mov" };
                long validLength = 2000000000;
                if(file.Length > validLength)
                {
                    Response.StatusCode = 500;
                    Response.ContentType = "text/plain";
                    Response.Headers["string"] = "File Too Big";
                    throw new Exception("File Too Big");
                }
                if (file.Length > 0)
                {
                    Console.WriteLine("Got here");
                    string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string videoPathFile = Path.Combine(workingDirectory, $"Files\\");
                    string ID = Guid.NewGuid().ToString("N");
                    string IDVideo = Guid.NewGuid().ToString("N");
                    if (!validExtensions.Contains(Path.GetExtension(Convert.ToBase64String(Encoding.UTF8.GetBytes(file.FileName)))))
                    {
                        Console.WriteLine("Got here");
                        Response.StatusCode = 500;
                        Response.ContentType = "text/plain";
                        Response.Headers["string"] = "Invalid file format";
                        
                    }
                    Console.WriteLine("Got here3");
                    Console.WriteLine(videoPathFile + IDVideo + Path.GetExtension(Convert.ToBase64String(Encoding.UTF8.GetBytes(file.FileName))));
                    bool exists = System.IO.Directory.Exists(videoPathFile);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(videoPathFile);
                    }
                    using (FileStream fileStream = System.IO.File.Create(videoPathFile + IDVideo + Path.GetExtension(file.FileName)))
                    {
                        Console.WriteLine("Got here4");
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        Console.WriteLine($"Added file {IDVideo}, with author {identity}");
                        Console.WriteLine("Got here5");

                        if (_context.channelModels.FirstOrDefault(x => x.Id == id) != null)
                        {
                            var dbData = new VideoModel()
                            {
                                Id = IDVideo,
                                Title = data.Title,
                                Author = _context.channelModels.FirstOrDefault(x => x.Id == id).Channel,
                                dateTime = DateTime.UtcNow
                            };
                            await _context.videoModels.AddAsync(dbData);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            UserModel channelUser;
                            if (_context.userModels.FirstOrDefault(x => x.Id == id) != null)
                            {
                                channelUser = _context.userModels.FirstOrDefault(x => x.Id == id);
                            }
                            else
                            {
                                channelUser = new UserModel() { Id = id, Name = identity };
                            }
                            var dbDataChannel = new ChannelModel()
                            {
                                Id = ID,
                                Channel = channelUser
                            };
                            await _context.channelModels.AddAsync(dbDataChannel);
                            await _context.SaveChangesAsync();
                            var dbData = new VideoModel()
                            {
                                Id = IDVideo,
                                Title = data.Title,
                                Author = dbDataChannel.Channel,
                                dateTime = DateTime.UtcNow,
                            };
                            await _context.videoModels.AddAsync(dbData);
                            await _context.SaveChangesAsync();

                        }
                        Console.WriteLine("Got here6");
                        Response.StatusCode = 200;
                        Response.ContentType = "text/plain";
                        Response.Headers["string"] = "Uploaded";
                    }



                }
                else
                {
                    throw new Exception("Could not upload file");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.ContentType = "text/plain";
                Response.Headers["string"] = $"{ex}";
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
