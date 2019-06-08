using ExHelper.API.Models;

namespace ExHelper.Tests.Builders
{
    public class FieldConfigBuilder
    {
        private FieldConfig _config;

        public FieldConfigBuilder()
        {
            _config = new FieldConfig();
        }

        public FieldConfigBuilder MinLength(int? minLength = null)
        {
            _config.Validations.MinLength = minLength;
            return this;
        }
        
        public FieldConfigBuilder NotEmpty()
        {
            _config.Validations.NotEmpty = true;
            return this;
        }

        public FieldConfig Build()
        {
            return _config;
        }
    }
}