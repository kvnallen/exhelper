using System;
using System.Collections.Generic;
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
                case "date":
                    return cell.DateCellValue;
                default:
                    return cell?.ToString();
            }
        }

        public static void GetValue(object cellm)
        {
            throw new NotImplementedException();
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
            if(cell.CellType == CellType.Numeric) return cell.NumericCellValue;
            string cellStr = cell.ToString();
            if (double.TryParse(cellStr, out var result))
                return result;

            return null;
        }
    }
}
