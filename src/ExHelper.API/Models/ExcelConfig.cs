using System.Collections.Generic;

namespace ExHelper.API.Models
{
    public class ExcelConfig
    {
        public int StartRow { get; set; }
        public string SheetName { get; set; }
        public IEnumerable<FieldConfig> Fields { get; set; }
    }
}