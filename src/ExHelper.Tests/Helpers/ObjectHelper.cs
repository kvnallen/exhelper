using System.Dynamic;

namespace ExHelper.Tests.Helpers
{
    public static class ObjectHelper
    {
        public static T GetValue<T>(this ExpandoObject obj, string propName)
        {
            var prop = obj.GetType().GetProperty(propName);
            T value = (T)prop.GetValue(obj, null);
            return value;
        }
    }
}
