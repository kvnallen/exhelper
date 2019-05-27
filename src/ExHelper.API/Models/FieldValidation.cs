namespace ExHelper.API.Models
{
    public class ValidationOptions
    {
        public bool NotNull { get; set; }
        public bool NotEmpty { get; set; }
        public string Format { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public string Regex { get; set; }
        public string[] ListValues { get; set; }
    }
}