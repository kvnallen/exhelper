namespace ExHelper.API.Models.Validators
{
    public class Length : Validator
    {
        public override bool CanUse(FieldConfig config)
        {
            return config.Validations.MinLength.HasValue
                            && config.Validations.MinLength.Value > 0;
        }

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            var length = $"{value}".Length;

            if (MinLengthIsValid(config, length))
            {
                return ValidationResult.Fail(config,
                    $"must have at least {config.Validations.MinLength.Value} characters", row);
            }

            if (length > config.Validations.MaxLength.Value)
            {
                return ValidationResult.Fail(config,
                    $"must have at most {config.Validations.MaxLength.Value} characters", row);
            }

            return ValidationResult.Success();
        }

        private static bool MinLengthIsValid(FieldConfig config, int length)
        {
            int? minLength = config.Validations.MinLength;
            if (!minLength.HasValue)
                return true;
                
            return length >= minLength;
        }
    }
}