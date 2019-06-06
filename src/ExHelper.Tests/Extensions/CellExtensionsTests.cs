using ExHelper.API.Extensions;
using NPOI.SS.UserModel;
using Xunit;
using Moq;
using FluentAssertions;
using ExHelper.API.Models;
using System.Globalization;
using System;
using System.Collections.Generic;

namespace ExHelper.Tests.Extensions
{
    public class CellExtensionsTests
    {
        private readonly Mock<ICell> _cell;
        public CellExtensionsTests()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-us");
            _cell = new Mock<ICell>();
        }

        [Theory]
        [InlineData("30.3", 30.3d)]
        [InlineData("179,769,313,48.12", 17976931348.12d)]
        [InlineData("1", 1)]
        [InlineData("-5", -5d)]
        [InlineData("aaa", null)]
        public void GetValue_Numeric_WhenTypeIsNotNumeric_ReturnsValidNumeric(string strValue, double? expected)
        {
            //Given
            _cell.Setup(x => x.ToString()).Returns(strValue);
            _cell.SetupGet(x => x.CellType).Returns(CellType.String);

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig { Type = "numeric" });

            //Then
            value.Should().Be(expected);
        }

        [Theory]
        [InlineData("30.3", 30.3d)]
        [InlineData("17976931348", 17976931348d)]
        [InlineData("1", 1)]
        [InlineData("-5", -5d)]
        public void GetValue_NumericTest(string strValue, double? expected)
        {
            //Givend
            _cell.Setup(x => x.ToString()).Returns(strValue);
            _cell.SetupGet(x => x.CellType).Returns(CellType.Numeric);
            _cell.SetupGet(x => x.NumericCellValue).Returns(expected.Value);

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig { Type = "numeric" });

            //Then
            value.Should().Be(expected);
        }


        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void GetValue_BooleanTests(string strValue, bool? expected)
        {
            //Given
            _cell.Setup(x => x.ToString()).Returns(strValue);

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig { Type = "boolean" });

            //Then
            value.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(DateScenarios))]
        public void GetValue_DateTests(string strDate, string[] formats, DateResult expected)
        {
            //Given
            _cell.Setup(x => x.ToString()).Returns(strDate);

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig
            {
                Type = "date",
                Validations = new ValidationOptions
                {
                    Formats = formats
                }
            });

            //Then
            value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetValue_WhenDateFormatIsInvalid_ReturnsInvalidDate()
        {
            //Given
            _cell.Setup(x => x.ToString()).Returns("xxx");

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig
            {
                Type = "date",
                Validations = new ValidationOptions { }
            });

            //Then
            value.Should().BeOfType(typeof(DateResult));
        }

        public static IEnumerable<object[]> DateScenarios => new List<object[]>
        {
            new object[] {
                "09/04/1992 15:11:12",
                new[] { "dd/MM/yyyy HH:mm:ss" },
                DateResult.Valid("09/04/1992 15:11:12", new DateTime(1992, 04, 09, 15, 11, 12))
            },
            new object[] {
                "09/04/1992",
                new[] { "dd/MM/yyyy" },
                DateResult.Valid("09/04/1992", new DateTime(1992, 04, 09))
            },
            new object[] { "28/02/2019", new[] { "dd/MM/yyyy" }, DateResult.Valid("28/02/2019", new DateTime(2019, 02, 28)) },
            new object[] { "28/02/2019 01", new[] { "dd/MM/yyyy ss" }, DateResult.Valid("28/02/2019 01", new DateTime(2019, 02, 28, 0, 0, 1)) },
            new object[] { "null",  null, new DateResult("null", default, false)  },
            new object[] { null,  null, new DateResult(null, default, false) },
        };

        [Theory(DisplayName = "When field is datetime, and format is not specified, should use dd/MM/yyyy")]
        [MemberData(nameof(DateWithoutFormatScenarios))]
        public void GetValue_DateTimeWithoutFormat(string strDate, DateResult expected)
        {
            //Given
            _cell.Setup(x => x.ToString()).Returns(strDate);

            //When
            var value = CellExtensions.GetValue(_cell.Object, new FieldConfig
            {
                Type = "date"
            });

            //Then
            value.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable<object[]> DateWithoutFormatScenarios => new List<object[]>
        {
            new object[] { "09/04/1992 15:11:12", DateResult.Valid("09/04/1992 15:11:12", new DateTime(1992, 04, 09, 15, 11, 12)) },
            new object[] { "09/04/1992",  DateResult.Valid("09/04/1992", new DateTime(1992, 04, 09)) },
            new object[] { "28/02/2019",  DateResult.Valid("28/02/2019", new DateTime(2019, 02, 28)) },
            new object[] { "null",  new DateResult("null", default, false) },
            new object[] { null,   new DateResult(null, default, false) },
        };

    }
}