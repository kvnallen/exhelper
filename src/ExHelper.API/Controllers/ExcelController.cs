using ExHelper.API.Models;
using ExHelper.API.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExHelper.API.Controllers
{
    [Route("api/[controller]")]
    public class ExcelController : Controller
    {
        private readonly ExcelProcessor processor;

        public ExcelController(ExcelProcessor processor)
        {
            this.processor = processor;
        }

        [HttpGet]
        public async Task<ExcelResult> Get(
            IFormFile file,
            [FromForm(Name = "config")] string configJson)
        {
            var config = JsonConvert.DeserializeObject<ExcelConfig>(configJson);

            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var path = Path.Combine(tempPath, file.FileName);
            Directory.CreateDirectory(tempPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Close();
                var result = processor.Process(path, config);
                Directory.Delete(tempPath, true);
                return result;
            }
        }
    }
}