using System;

namespace ExHelper.API.Models
{
    public class DateResult
    {
        public DateResult(string value, DateTime? result, bool isValid)
        {
            Value = value;
            Result = result;
            IsValid = isValid;
        }

        public string Value { get; }
        public DateTime? Result { get; }
        public bool IsValid { get; }

        public static DateResult Valid(string value, DateTime? result)
        {
            return new DateResult(value, result, true);
        }
    }
}
