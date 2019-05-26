using ExHelper.API.Models;
using ExHelper.API.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                return processor.Process(stream, config);
            }
        }
    }
}