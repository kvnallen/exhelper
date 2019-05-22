using ExHelper.API.Extensions;
using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
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
        private readonly IEnumerable<Validator> _validators;

        public ExcelProcessor(IEnumerable<Validator> validators)
        {
            this._validators = validators;
        }

        public ExcelResult Process(Stream excelFile, ExcelConfig config)
        {
            var start = DateTime.Now;
            var hssfwb = new HSSFWorkbook(excelFile);
            var sheet = hssfwb.GetSheet(config.SheetName);
            var errors = new List<Error>();
            var objects = new List<object> { new { } };

            for (int rowIndex = config.StartRow; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                if (sheet.GetRow(rowIndex) is IRow row)
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {
                        var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                        var fieldConfig = config.FieldAt(cellIndex);
                        var fieldName = GetFieldName(sheet, cellIndex, fieldConfig);
                        var value = GetCellValue(fieldConfig, cell);

                        var valueErrors = _validators
                            .Where(x => x.ForType(fieldConfig))
                            .Select(x => x.Validate(value))
                            .Where(x => !x.isValid)
                            .SelectMany(x => x.errors)
                            .ToList();

                        if (!valueErrors.Any())
                        {
                            dictionary.Add(fieldName, value);
                        }
                        // Apply validation for each row/column

                    }

                    objects.Add(dictionary.ToObject<object>());
                }
            }

            return new ExcelResult(sheet.PhysicalNumberOfRows, start, config.SheetName, new { objects }, errors);
        }

        private static string GetFieldName(ISheet sheet, int cellIndex, FieldConfig fieldConfig)
        {
            return fieldConfig.Name ?? sheet.GetRow(0).GetCell(cellIndex).StringCellValue;
        }

        private object GetCellValue(FieldConfig fieldConfig, ICell cell)
        {
            switch (fieldConfig.Type)
            {
                case "numeric":
                    return cell.NumericCellValue;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}