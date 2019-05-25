using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public abstract class Validator
    {
        public abstract bool CanUse(FieldConfig config);

        public abstract ValidationResult Validate(object value, int row, FieldConfig config);
    }
}