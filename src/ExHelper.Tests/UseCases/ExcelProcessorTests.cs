using System.IO;
using System.Linq;
using System.Reflection;
using ExHelper.API.Models.Validators;
using ExHelper.API.UseCases;
using Xunit;

namespace ExHelper.Tests.UseCases
{
    public class ExcelProcessorTests
    {
        [Theory]
        [EmbeddedResourceData("ExHelper.Tests/Resources/sample_worksheet.xlsx")]
        public void Process_ShouldProcessFile(Stream stream)
        {
            // var assembly = typeof(ExcelProcessorTests).GetTypeInfo().Assembly;
            // var stream = assembly.GetManifestResourceStream("./Resources/sample_worksheet.xlsx");
           
            //Given
            var processor = new ExcelProcessor(Enumerable.Empty<Validator>());
            //When
            processor.Process(stream, new API.Models.ExcelConfig());
            //Then
        }
    }
}