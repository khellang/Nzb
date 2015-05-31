using System.Xml.Linq;

namespace Nzb
{
    internal static class HelperExtensions
    {
        public delegate bool TryParse<T>(string stringValue, out T value);

        public static T TryParseOrDefault<T>(this string stringValue , TryParse<T> action)
        {
            T value;
            return action.Invoke(stringValue, out value) ? value : default(T);
        }

        public static string AttributeValueOrEmpty(this XElement element, string attributeName) =>
            element.Attribute(attributeName)?.Value ?? string.Empty;
    }
}