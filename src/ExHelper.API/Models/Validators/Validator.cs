using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public interface Validator
    {
        (bool isValid, IEnumerable<Error> errors) Validate();
    }
}