using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public class AllowNullValidator : Validator
    {
        public override string Type => "not_null";

        public override  (bool isValid, IEnumerable<Error> errors) Validate(object value)
        {
            if (value == null)
                return (false, Error.AsList("", "The field should be empty", 0, 0));

            return (true, Error.Empty());
        }
    }
}