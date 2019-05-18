using System;
using System.Collections.Generic;

namespace ExHelper.API.Models
{
    public class ExcelResult
    {
        public ExcelResult(DateTime startedAt, string sheetName, object json, IEnumerable<Error> errors)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentException("message", nameof(sheetName));
            }

            End = DateTime.Now;
            StartedAt = startedAt;
            SheetName = sheetName;
            Json = json;
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public DateTime StartedAt { get; set; }
        public DateTime End { get; set; }
        public string SheetName { get; set; }
        public object Json { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}