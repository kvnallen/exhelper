using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using ExHelper.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace ExHelper.Tests.Validators
{
    public class NotEmptyTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void CanUse_Tests(bool notEmpty, bool expected)
        {
            //Given
            var validator = new NotEmpty();

            //When
            var result = validator.CanUse(new FieldConfig { Validations = new ValidationOptions { NotEmpty = notEmpty } });

            //Then
            result.Should().Be(expected);
        }
        

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WhenIsEmptyOrWhiteSpaces_ReturnsError(string value)
        {
           ///Given
            var validator = new NotEmpty();

            //When
            var config = new FieldConfigBuilder().Build();
            var result = validator.Validate(value, 0, config);

            //Then
            result.IsValid.Should().BeFalse();
            result.Errors[0].Value.Should().Contain("should not be empty");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1d)]
        [InlineData(true)]
        [InlineData(null)]
        public void Validate_WhenTypeIsNotStringOrHasValue_ReturnsSuccess(object obj)
        {
           //Given
            var validator = new NotEmpty();

            //When
            var config = new FieldConfigBuilder().Build();
            var result = validator.Validate(obj, 0, config);

            //Then
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("hi")]
        [InlineData("-")]
        public void Validate_WhenIsNotEmpty_ReturnsSuccess(string value)
        {
           //Given
            var validator = new NotEmpty();

            //When
            var config = new FieldConfigBuilder().Build();
            var result = validator.Validate(value, 0, config);

            //Then
            result.IsValid.Should().BeTrue();
        }
    }
}