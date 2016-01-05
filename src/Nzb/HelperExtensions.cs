using System.Xml.Linq;
using JetBrains.Annotations;

namespace Nzb
{
    internal static class HelperExtensions
    {
        public delegate bool TryParse<T>(string stringValue, out T value);

        [CanBeNull]
        public static T TryParseOrDefault<T>(this string stringValue , TryParse<T> action)
        {
            T value;
            return action.Invoke(stringValue, out value) ? value : default(T);
        }

        [NotNull]
        public static string AttributeValueOrEmpty(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName)?.Value ?? string.Empty;
        }
    }
}