namespace ExHelper.API.Models
{
    public class FieldConfig
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }
        public ValidationOptions Validations { get; set; }
    }
}