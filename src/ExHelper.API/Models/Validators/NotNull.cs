using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public class NotNull : Validator
    {
        public override string Type => "not_null";

        public override bool CanUse(FieldConfig config) => config.Validations.NotNull;

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            if (value == null)
                return ValidationResult.Fail(config, "The field should not be null", row);

            return ValidationResult.Success(null);
        }
    }
}