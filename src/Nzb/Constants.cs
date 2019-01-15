using System.Xml.Linq;

namespace Nzb
{
    internal static class Constants
    {
        private static readonly XNamespace Namespace = "http://www.newzbin.com/DTD/2003/nzb";

        public static readonly XName NzbElement = Namespace + "nzb";

        public static readonly XName HeadElement = Namespace + "head";

        public static readonly XName MetaElement = Namespace + "meta";

        public static readonly XName FileElement = Namespace + "file";

        public static readonly XName GroupsElement = Namespace + "groups";

        public static readonly XName GroupElement = Namespace + "group";

        public static readonly XName SegmentsElement = Namespace + "segments";

        public static readonly XName SegmentElement = Namespace + "segment";

        public const string TypeAttribute = "type";

        public const string PosterAttribute = "poster";

        public const string DateAttribute = "date";

        public const string SubjectAttribute = "subject";

        public const string BytesAttribute = "bytes";

        public const string NumberAttribute = "number";
    }
}
