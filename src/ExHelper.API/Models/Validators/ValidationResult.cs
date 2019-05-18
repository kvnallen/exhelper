using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<Error> Errors { get; set; }
        public object Value { get; set; }
    }
}
