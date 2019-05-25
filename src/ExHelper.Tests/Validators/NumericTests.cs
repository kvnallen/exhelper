using ExHelper.API.Models.Validators;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExHelper.Tests.Validators
{
    public class NumericTests
    {

        [Theory]
        [InlineData("33.33")]
        [InlineData("123")]
        [InlineData("33.456")]
        public void Validate_WhenCalled_ValidateResult(string value)
        {
            var validator = new Numeric();
            var result = validator.Validate(value, 0, new API.Models.FieldConfig { Validations = new API.Models.ValidationOptions() });
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WhenAllowedNullAndValueIsNull_ReturnsEmptyErrors()
        {
            var validator = new Numeric();
            var result = validator.Validate(null, 0, new API.Models.FieldConfig { Validations = new API.Models.ValidationOptions() });
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WhenNotAllowedNullAndValueIsNull_ReturnsError()
        {
            var validator = new Numeric();
            var result = validator.Validate(null, 0, new API.Models.FieldConfig
            {
                Validations = new API.Models.ValidationOptions
                {
                    NotNull = true
                }
            });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Value.Contains("Invalid number"));
        }
    }
}
