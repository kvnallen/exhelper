namespace ExHelper.API.Models
{
    public class ValidationOptions
    {
        public bool AllowNull { get; set; }
        public bool AllowEmpty { get; set; }
        public string Format { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public string Regex { get; set; }
    }
}