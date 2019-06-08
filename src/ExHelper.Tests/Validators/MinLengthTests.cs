using ExHelper.API.Models.Validators;
using ExHelper.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace ExHelper.Tests.Validators
{
    public class MinLengthTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(null, false)]
        public void CanUse_Tests(int? length, bool expected)
        {
            //Given
            var validator = new Length();
            //When
            var config = new FieldConfigBuilder().MinLength(length).Build();
            var result = validator.CanUse(config);
            //Then
            result.Should().Be(expected);
        }

        [Theory(DisplayName = "When value's length is greater or equal than minimum length, returns success")]
        [InlineData("HEY", 3)]
        [InlineData("HEY BRO!", 3)]
        public void Validate_WhenValueLengthIsGreaterOrEqual_ReturnsSuccess()
        {
            //Given
            var validator = new Length();
            //When
            

            //Then
        }



        // [Theory(DisplayName = "When value's length is lower than minLength (config) returns error")]
        // [InlineData(1, "")]
        // public void Validate_WhenLengthIsLowerThanMinimum_ReturnsErro(int length, bool expected)
        // {
        //     //Given
        //     var validator = new MinLength();
        //     //When
        //     var config = new FieldConfigBuilder().MinLength(length).Build();
        //     var result = validator.Validate(config, 0, config);
        //     //Then
        //     result.Should().Be(expected);
        // }
    }
}