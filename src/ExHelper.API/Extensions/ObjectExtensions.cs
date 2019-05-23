using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExHelper.API.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic ToObject(this IDictionary<string, object> source)
        {
            dynamic expandoObject = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)expandoObject;

            foreach (var kvp in source)
            {
                eoColl.Add(kvp);
            }

            return expandoObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }

}
