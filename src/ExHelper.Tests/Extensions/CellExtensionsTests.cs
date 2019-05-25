using ExHelper.API.Extensions;
using NPOI.SS.UserModel;
using Xunit;
using Moq;
using FluentAssertions;
using ExHelper.API.Models;
using System.Globalization;

namespace ExHelper.Tests.Extensions
{
    public class CellExtensionsTests
    {

        public CellExtensionsTests()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-us");
        }

        [Theory]
        [InlineData("30.3", 30.3d)]
        [InlineData("179,769,313,48.12", 17976931348.12d)]
        [InlineData("1", 1)]
        [InlineData("-5", -5d)]
        [InlineData("aaa", null)]
        public void GetValue_Numeric_WhenTypeIsNotNumeric_ReturnsValidNumeric(string strValue, double? expected)
        {
            //Givend
            var cellMock = new Mock<ICell>();
            cellMock.Setup(x => x.ToString()).Returns(strValue);
            cellMock.SetupGet(x => x.CellType).Returns(CellType.String);

            //When
            var value = CellExtensions.GetValue(cellMock.Object, new FieldConfig { Type = "numeric" });

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
            var cellMock = new Mock<ICell>();
            cellMock.Setup(x => x.ToString()).Returns(strValue);
            cellMock.SetupGet(x => x.CellType).Returns(CellType.Numeric);
            cellMock.SetupGet(x => x.NumericCellValue).Returns(expected.Value);

            //When
            var value = CellExtensions.GetValue(cellMock.Object, new FieldConfig { Type = "numeric" });

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
            //Givend
            var cellMock = new Mock<ICell>();
            cellMock.Setup(x => x.ToString()).Returns(strValue);

            //When
            var value = CellExtensions.GetValue(cellMock.Object, new FieldConfig { Type = "boolean" });

            //Then
            value.Should().Be(expected);
        }
    }
}