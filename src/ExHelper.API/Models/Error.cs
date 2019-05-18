using System;
using System.Collections.Generic;
using System.Linq;

namespace ExHelper.API.Models
{
    public class Error
    {
        public Error(string key, string value, int column, int row)
        {
            Key = key;
            Value = value;
            Column = column;
            Row = row;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }

        internal static IEnumerable<Error> Empty()
        {
            return Enumerable.Empty<Error>();
        }

        public static List<Error> AsList(string key, string value, int column, int row){
            return new List<Error> {  new Error(key, value, column, row) };
        }
    }
}