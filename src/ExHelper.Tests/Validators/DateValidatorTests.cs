using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using FluentAssertions;
using System;
using Xunit;

namespace ExHelper.Tests.Validators
{
    public class DateValidatorTests
    {
        DateValidator validator;
        public DateValidatorTests()
        {
            validator = new DateValidator();
        }

        [Theory]
        [InlineData("date", true)]
        [InlineData("DaTe", true)]
        [InlineData("DATE ", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void CanUse_Tests(string type, bool expected)
        {
            //When
            var result = validator.CanUse(new FieldConfig { Type = type });
            //Then

            result.Should().Be(expected);
        }

        [Fact]
        public void Validate_WhenIsNullAndFieldIsRequired_ReturnError()
        {
            //When
            var error = validator.Validate(new DateResult(null, null, false), 0, new FieldConfig { Validations = new ValidationOptions { NotNull = true } });

            //Then
            error.IsValid.Should().BeFalse();
            error.Errors[0].Value.Should().Contain("not be null");
        }

        [Fact]
        public void Validate_WhenFormatIsValid_ReturnsFalse()
        {
            //When
            var result = validator.Validate(new DateResult("invalid_format", DateTime.Now, true), 1, new FieldConfig());

            //Then
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WhenNotFormattedSuccessfully_ReturnError()
        {
            //When
            var result = validator.Validate(new DateResult("invalid_format", null, false), 1, new FieldConfig());

            //Then
            result.IsValid.Should().BeFalse();
            result.Errors[0].Value.Should().Contain("invalid_format");
        }

        [Fact]
        public void Validate_WhenObjectIsNotDateResult_ReturnError()
        {
            //When
            var result = validator.Validate("", 1, new FieldConfig());

            //Then
            result.IsValid.Should().BeFalse();
            result.Errors[0].Value.Should().Contain("invalid date");
        }
    }
}