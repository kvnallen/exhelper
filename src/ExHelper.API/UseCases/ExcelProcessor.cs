using ExHelper.API.Extensions;
using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
            var hssfwb = new XSSFWorkbook(excelFile);
            var sheet = hssfwb.GetSheet(config.SheetName);
            var errors = new List<Error>();
            var objects = new List<object> { };

            for (int rowIndex = config.StartRow; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                if (sheet.GetRow(rowIndex) is IRow row)
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {
                        if (IsEmptyRow(row))
                            continue;

                        var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                        var fieldConfig = config.FieldAt(cellIndex);

                        if (fieldConfig is null)
                        {
                            errors.Add(new Error("missing_configuration", $"Missing configuration for field {cellIndex}", cellIndex, rowIndex));
                            continue;
                        }

                        var fieldName = GetFieldName(sheet, cellIndex, fieldConfig);
                        var value = GetCellValue(fieldConfig, cell);

                        var valueErrors = _validators
                            .Where(x => x.CanUse(fieldConfig))
                            .Select(x => x.Validate(value, rowIndex, fieldConfig))
                            .Where(x => !x.IsValid)
                            .SelectMany(x => x.Errors)
                            .ToList();

                        if (!valueErrors.Any())
                        {
                            dictionary.Add(fieldName, value);
                        }

                        errors.AddRange(valueErrors);
                    }

                    if (dictionary.Keys.Any())
                    {
                        objects.Add(dictionary.ToObject());
                    }
                }
            }

            var sheetObject = errors.Any() ? Enumerable.Empty<object>() : objects;

            return new ExcelResult(sheet.PhysicalNumberOfRows, start, config.SheetName, sheetObject, errors);
        }

        private static bool IsEmptyRow(IRow row) => row.Cells.All(x => x.CellType == CellType.Blank);

        private static string GetFieldName(ISheet sheet, int cellIndex, FieldConfig fieldConfig)
        {
            return fieldConfig?.Name ?? sheet.GetRow(0).GetCell(cellIndex).StringCellValue;
        }

        private object GetCellValue(FieldConfig fieldConfig, ICell cell)
        {
            if (cell is null) return null;

            switch (fieldConfig.Type)
            {
                case "numeric":
                    {
                        if (double.TryParse(cell.ToString(), out var result))
                            return result;

                        return null;
                    }
                case "boolean" when cell.CellType == CellType.Boolean:
                    return cell.BooleanCellValue;
                case "date":
                    return cell.DateCellValue;
                case "list" when cell.CellType == CellType.String:
                    return cell.ToString().Split(",");
                default:
                    return cell?.ToString();
            }
        }
    }
}