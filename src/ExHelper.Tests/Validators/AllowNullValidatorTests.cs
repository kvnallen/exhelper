using ExHelper.API.Models.Validators;
using FluentAssertions;
using Xunit;

namespace ExHelper.Tests.Validators
{
    public class AllowNullValidatorTests
    {
        [Fact]
        public void Validate_Test()
        {
            //Given
            var validator = new NotNull();
            //When
            var result = validator.Validate(new { }, 0, new API.Models.FieldConfig());
            //Then
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenObjectIsNull_ReturnsFalse(){
            //Given
            var validator = new NotNull();
            //When
            var result = validator.Validate(null, 0, new API.Models.FieldConfig());
            //Then
            result.IsValid.Should().BeFalse();
        }
    }
}