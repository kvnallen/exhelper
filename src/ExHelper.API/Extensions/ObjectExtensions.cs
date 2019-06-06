using System.Collections.Generic;
using System.Dynamic;

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
    }

}
