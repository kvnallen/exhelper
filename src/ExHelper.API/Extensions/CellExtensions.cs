using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExHelper.API.Models;
using NPOI.SS.UserModel;

namespace ExHelper.API.Extensions
{
    public static class CellExtensions
    {
        public static object GetValue(this ICell cell, FieldConfig config)
        {
            if (cell is null) return null;

            switch (config.Type)
            {
                case "numeric": return TryGetNumeric(cell);
                case "boolean": return TryGetBool(cell);
                case "date": return TryGetDateTime(cell, config);
                case "list" when cell.CellType == CellType.String:
                    return cell.ToString().Split(",");
                default:
                    return cell?.ToString();
            }
        }

        private static DateTime? TryGetDateTime(ICell cell, FieldConfig config)
        {
            var cellValue = cell.ToString();

            var formats = config.Validations?.Formats is null || !config.Validations.Formats.Any()
                ? new[] { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy" }
                : config.Validations.Formats;

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(cellValue, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date))
                {
                    return date;
                }
            }

            return null;
        }

        private static bool? TryGetBool(ICell cell)
        {
            var value = cell.ToString();
            var binaryBool = new Dictionary<string, bool>
            {
                ["1"] = true,
                ["0"] = false
            };

            if (binaryBool.ContainsKey(value))
                return binaryBool[value];

            if (bool.TryParse(value, out var result))
                return result;

            return null;
        }

        private static double? TryGetNumeric(ICell cell)
        {
            if (cell.CellType == CellType.Numeric) return cell.NumericCellValue;
            string cellStr = cell.ToString();
            if (double.TryParse(cellStr, out var result))
                return result;

            return null;
        }
    }
}
