using System.Collections.Generic;
using System.Linq;

namespace ExHelper.API.Models
{
    public class ExcelConfig
    {
        public int StartRow { get; set; } = 1;
        public string SheetName { get; set; }
        public IEnumerable<FieldConfig> Fields { get; set; }

        public FieldConfig FieldAt(int index) => Fields.SingleOrDefault(x => x.Index == index);
    }
}