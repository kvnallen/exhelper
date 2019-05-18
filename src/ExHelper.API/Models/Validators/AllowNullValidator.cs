using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public class AllowNullValidator : Validator
    {
        private readonly object value;

        public AllowNullValidator(object value)
        {
            this.value = value;
        }

        public (bool isValid, IEnumerable<Error> errors) Validate()
        {
            if(this.value == null) 
                return (false, Error.AsList("", "The field should be empty", 0, 0));

            return (true, Error.Empty());
        }
    }
}