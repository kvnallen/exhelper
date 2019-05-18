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
            var validator = new AllowNullValidator(new { });
            //When
            var result = validator.Validate();
            //Then
            result.isValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenObjectIsNull_ReturnsFalse(){
            //Given
            var validator = new AllowNullValidator(null);
            //When
            var result = validator.Validate();
            //Then
            result.isValid.Should().BeFalse();
        }
    }
}