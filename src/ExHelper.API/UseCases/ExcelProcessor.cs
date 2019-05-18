using ExHelper.API.Extensions;
using ExHelper.API.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExHelper.API.UseCases
{
    public class ExcelProcessor
    {
        public ExcelResult Process(Stream excelFile, ExcelConfig config)
        {
            var start = DateTime.Now;
            var hssfwb = new HSSFWorkbook(excelFile);
            var sheet = hssfwb.GetSheet(config.SheetName);
            var errors = new List<Error>();
            var objects = new List<object>{ new { } };

            for (int rowIndex = config.StartRow; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                if (sheet.GetRow(rowIndex) is IRow row)
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {

                        var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                        var fieldConfig = config.FieldAt(cellIndex);
                        var fieldName = fieldConfig.Name ?? sheet.GetRow(0).GetCell(cellIndex).StringCellValue;

                        dictionary.Add(fieldName, cell.StringCellValue);
                        // Apply validation for each row/column  
                        // Mount object with values

                    }

                    objects.Add(dictionary.ToObject<object>());
                }
            }

            return new ExcelResult(sheet.PhysicalNumberOfRows, start, config.SheetName, new { objects }, errors);
        }
    }
}