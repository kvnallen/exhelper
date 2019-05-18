using ExHelper.API.Models;
using ExHelper.API.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("")]
        public async Task<dynamic> Get(IFormFile file, ExcelConfig config)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return processor.Process(stream, config);
            }
        }
    }
}