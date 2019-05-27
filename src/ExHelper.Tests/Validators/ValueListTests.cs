using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace ExHelper.Tests.Validators
{
    public class ValueListTests
    {
        [Fact]
        public void Validade_WhenValueInList_ReturnTrue()
        {
            //Given
            var validator = new InList();
            var planetsNames = new string[] { "Mercury", "Venus", "Earth", "Mars", "Ceres", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto" };

            //When
            var result = validator.Validate("Venus", 0, new API.Models.FieldConfig()
            {
                Validations = new ValidationOptions { ListValues = planetsNames }
            });
            //Then
            result.IsValid.Should().BeTrue();
        }
    }
}
