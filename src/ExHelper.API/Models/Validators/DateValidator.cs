using System;

namespace ExHelper.API.Models.Validators
{
    public class DateValidator : Validator
    {
        public DateValidator()
        {
        }

        public override bool CanUse(FieldConfig config)
            => string.Compare(config.Type?.Trim(), "date", true) >= 0;

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            var (valid, result) = IsValidDateResult(value);

            if (result.Result is null && (config.Validations.NotNull || config.Validations.NotEmpty))
            {
                return ValidationResult.Fail(config, "Date should not be null", row);
            }

            if (!valid)
            {
                return ValidationResult.Fail(config, $"Invalid date {result.Value}", row);
            }

            return ValidationResult.Success(value);
        }

        private static (bool valid, DateResult result)  IsValidDateResult(object value)
        {
            if(value is DateResult result)
            {
                return (result.IsValid, result);
            }

            return (false, default);
        }
    }
}