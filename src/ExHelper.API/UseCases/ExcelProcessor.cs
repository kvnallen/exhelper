using ExHelper.API.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

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
            var jsonObject = new { };

            for (int rowIndex = config.StartRow; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                if (sheet.GetRow(rowIndex) is IRow row)
                {
                    // Apply validation for each row/column  
                    // Mount object with values
                }
            }

            return new ExcelResult(start, config.SheetName, jsonObject, errors);
        }
    }
}