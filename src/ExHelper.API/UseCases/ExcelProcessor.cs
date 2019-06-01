using ExHelper.API.Extensions;
using ExHelper.API.Models;
using ExHelper.API.Models.Validators;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public ExcelResult Process(string filePath, ExcelConfig config)
        {
            var start = DateTime.Now;

            var sheet = GetSheet(filePath, config);
            var errors = new List<Error>();
            var objects = new List<ExpandoObject> { };

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
                        var value = cell.GetValue(fieldConfig);

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

            var sheetObject = errors.Any() ? Enumerable.Empty<ExpandoObject>() : objects;

            return new ExcelResult(sheet.PhysicalNumberOfRows, start, config.SheetName, sheetObject, errors);
        }

        private static ISheet GetSheet(string filePath, ExcelConfig config)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (filePath.EndsWith("xls"))
                {
                    var poifs = new NPOI.POIFS.FileSystem.NPOIFSFileSystem(new FileInfo(filePath), true);
                    var hssfwb = WorkbookFactory.Create(poifs);
                    return hssfwb.GetSheet(config.SheetName);
                }

                return new XSSFWorkbook(file).GetSheet(config.SheetName);
            }
        }

        private static bool IsEmptyRow(IRow row) => row.Cells.All(x => x.CellType == CellType.Blank);

        private static string GetFieldName(ISheet sheet, int cellIndex, FieldConfig fieldConfig)
        {
            return fieldConfig?.Name ?? sheet.GetRow(0).GetCell(cellIndex).StringCellValue;
        }
    }
}