using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ExHelper.API.Models
{
    public class ExcelResult
    {
        public ExcelResult(
            int totalRows, 
            DateTime startedAt, 
            string sheetName, 
            IEnumerable<ExpandoObject> jsonObject, IEnumerable<Error> errors)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentException("message", nameof(sheetName));
            }

            End = DateTime.Now;
            TotalRows = totalRows;
            StartedAt = startedAt;
            SheetName = sheetName;
            Json = jsonObject;
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public DateTime StartedAt { get; set; }
        public DateTime End { get; set; }
        public int TotalRows { get; set; }
        public string SheetName { get; set; }
        public IEnumerable<ExpandoObject> Json { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}