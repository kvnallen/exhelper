namespace ExHelper.API.Models.Validators
{
    public class NotEmpty : Validator
    {
        public override bool CanUse(FieldConfig config) => config.Validations.NotEmpty;

        public override ValidationResult Validate(object value, int row, FieldConfig config)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.Fail(config, "should not be empty", row);
            }

            return ValidationResult.Success();
        }
    }
}