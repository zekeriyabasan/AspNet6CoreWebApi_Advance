using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/files")]
    [ApiController]
   
    public class FilesController: ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //folder
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Media");
            if(!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //path
            var path = Path.Combine(folder, file.FileName);

            //stream
            using (var stream = new FileStream (path,FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //respons
            return Ok(new {
                file = file.FileName,
                path = path,
                size = file.Length
            });


        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFileAsync(string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //filepath
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Media",fileName);

            //ContentType : (MIME)
            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            //read
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            //respons
            return File(bytes,contentType,Path.GetFileName(filePath));


        }
    }
}
