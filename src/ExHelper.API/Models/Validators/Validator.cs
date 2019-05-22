using System.Collections.Generic;

namespace ExHelper.API.Models.Validators
{
    public abstract class Validator
    {
        FieldConfig config;
        public abstract string Type { get; }

        public bool ForType(FieldConfig config)
        {
            if (config.Type == Type)
            {
                this.config = config;
                return true;
            }

            return false;
        }
        public abstract (bool isValid, IEnumerable<Error> errors) Validate(object value);
    }
}