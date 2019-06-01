using System.Collections.Generic;
using System.Linq;
using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using ExHelper.API.UseCases;
using FluentAssertions;
using Xunit;

namespace ExHelper.Tests.UseCases
{
    public class ExcelProcessorTests
    {
        [Theory]
        [InlineData("./Resources/sample_sheet.xlsx")]
        [InlineData("./Resources/sample_sheet.xls")]
        public void Process_ShouldProcessFiles(string fileName)
        {

            //Given
            var processor = new ExcelProcessor(Enumerable.Empty<Validator>());
            //When
            var config = new API.Models.ExcelConfig
            {
                SheetName = "one_row",
                Fields = new List<FieldConfig>
                {
                    new FieldConfig { Index = 0  },
                    new FieldConfig { Index = 1  },
                    new FieldConfig { Index = 2, Type = "numeric" },
                    new FieldConfig { Index = 3  },
                }
            };

            var result = processor.Process(fileName, config);
            //Then

            var firstItem = result.Json.First();
            var name = ((dynamic)firstItem).Name;
            double age = ((dynamic)firstItem).Age;

            Assert.Equal("Kevin", name);
            Assert.Equal(10, age);
            result.Errors.Should().BeEmpty();
        }
    }
}