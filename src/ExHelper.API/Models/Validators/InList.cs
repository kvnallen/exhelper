using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static ExHelper.API.Extensions.StringExtensions;

namespace ExHelper.API.Models.Validators
{
    public class InList : Validator
    {
        public override bool CanUse(FieldConfig config) => config.Validations.ListValues.Count() > 0;

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Fail(config, "The column value is null or empty", row);
            if (config.Validations.ListValues == null) return ValidationResult.Fail(config, "The list of values is null", row);
            if (config.Validations.ListValues.Count() == 0) return ValidationResult.Fail(config, "The list of values is empty", row);

            var simplifiedList = SimplifyTextList(config.Validations.ListValues);
            var simplifiedValue = value.ToString().RemoveDiacritics().RemoveSpecialCharacters().ToLower();

            if (simplifiedList.Contains(simplifiedValue))
                return ValidationResult.Success(null);

            return ValidationResult.Fail(config, $"The field must contain one of the following values: {string.Join(',', config.Validations.ListValues)}", row);
        }

        private string[] SimplifyTextList(string[] list)
        {
            return list.ToList().Select(t => t.RemoveDiacritics().RemoveSpecialCharacters().ToLower()).ToArray();
        }

    }
}
