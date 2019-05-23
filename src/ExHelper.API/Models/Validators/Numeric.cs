using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExHelper.API.Models.Validators
{
    public class Numeric : Validator
    {
        public override string Type => throw new NotImplementedException();

        public override bool CanUse(FieldConfig config) => config.Type == "numeric";

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            if (value is null && !config.Validations.NotNull) return ValidationResult.Success();

            if (double.TryParse(value?.ToString(), out var result))
            {
                return ValidationResult.Success(result);
            }

            return ValidationResult.Fail(config, "Invalid number", row);
        }
    }
}

