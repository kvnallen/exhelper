using System;
using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
        public object Value { get; set; }

        internal static ValidationResult Success(object result) => new ValidationResult
        {
            IsValid = true,
            Value = result
        };
        internal static ValidationResult Success() => Success(null);
        internal static ValidationResult Fail(FieldConfig config, string v, int row) => new ValidationResult
        {
            Errors = Error.AsList(config.Name, "Invalid number", config.Index, row)
        };
    }
}
