using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class NzbDocument : INzbDocument
    {
        public static readonly System.Text.Encoding DefaultEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

        private static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        private static readonly XNamespace Namespace = "http://www.newzbin.com/DTD/2003/nzb";

        private readonly Lazy<long> _bytes;

        private NzbDocument() : this(new Dictionary<string, string>(), new List<NzbFile>())
        {
        }

        private NzbDocument([NotNull] IReadOnlyDictionary<string, string> metadata,
            [NotNull] IReadOnlyList<NzbFile> files)
        {
            Metadata = Check.NotNull(metadata, "metadata");
            Files = Check.NotNull(files, "files");

            _bytes = new Lazy<long>(() => Files.Sum(x => x.Bytes));
        }

        [NotNull]
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }

        [NotNull]
        public IReadOnlyList<INzbFile> Files { get; private set; }

        public long Bytes
        {
            get { return _bytes.Value; }
        }

        /// <summary>
        /// Loads the document from the specified stream, using <see cref="DefaultEncoding"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        [NotNull]
        public static Task<INzbDocument> Load([NotNull] Stream stream)
        {
            return Load(stream, DefaultEncoding);
        }

        /// <summary>
        /// Loads the document from the specified stream, using the specified encoding.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding to use.</param>
        [NotNull]
        public static async Task<INzbDocument> Load([NotNull] Stream stream, [NotNull] System.Text.Encoding encoding)
        {
            Check.NotNull(stream, "stream");
            Check.NotNull(encoding, "encoding");

            using (var reader = new StreamReader(stream, encoding))
            {
                return Parse(await reader.ReadToEndAsync());
            }
        }

        /// <summary>
        /// Parses the specified text.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        [NotNull]
        public static INzbDocument Parse([NotNull] string text)
        {
            Check.NotEmpty(text, "text");

            var document = XDocument.Parse(text);

            var nzbElement = document.Element(Namespace + "nzb");
            if (nzbElement == null)
            {
                return new NzbDocument();
            }

            var metadata = ParseMetadata(nzbElement);

            var files = ParseFiles(nzbElement);

            return new NzbDocument(metadata, files);
        }

        private static IReadOnlyDictionary<string, string> ParseMetadata(XContainer nzbElement)
        {
            var metadata = new Dictionary<string, string>();

            var headElement = nzbElement.Element(Namespace + "head");
            if (headElement == null)
            {
                return metadata;
            }

            foreach (var metaElement in headElement.Elements(Namespace + "meta"))
            {
                var typeAttribute = metaElement.Attribute("type");
                if (typeAttribute != null)
                {
                    metadata.Add(typeAttribute.Value, metaElement.Value);
                }
            }

            return metadata;
        }

        private static IReadOnlyList<NzbFile> ParseFiles(XContainer nzbElement)
        {
            var files = new List<NzbFile>();

            foreach (var fileElement in nzbElement.Elements(Namespace + "file"))
            {
                var poster = fileElement.AttributeValueOrEmpty("poster");

                var unixTimestamp = fileElement.AttributeValueOrEmpty("date").TryParseOrDefault<long>(long.TryParse);

                var date = ConvertUnixTimestamp(unixTimestamp);

                var subject = fileElement.AttributeValueOrEmpty("subject");

                var groups = ParseGroups(fileElement);

                var segments = ParseSegments(fileElement);

                files.Add(new NzbFile(poster, date, subject, groups, segments));
            }

            return files;
        }

        private static DateTimeOffset ConvertUnixTimestamp(long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp).ToLocalTime();
        }

        private static IReadOnlyList<string> ParseGroups(XContainer fileElement)
        {
            var groups = new List<string>();

            var groupsElement = fileElement.Element(Namespace + "groups");
            if (groupsElement == null)
            {
                return groups;
            }

            foreach (var groupElement in groupsElement.Elements(Namespace + "group"))
            {
                groups.Add(groupElement.Value);
            }

            return groups;
        }

        private static IReadOnlyList<NzbSegment> ParseSegments(XContainer fileElement)
        {
            var segments = new List<NzbSegment>();

            var segmentsElement = fileElement.Element(Namespace + "segments");
            if (segmentsElement == null)
            {
                return segments;
            }

            foreach (var element in segmentsElement.Elements(Namespace + "segment"))
            {
                var bytes = element.AttributeValueOrEmpty("bytes").TryParseOrDefault<long>(long.TryParse);

                var number = element.AttributeValueOrEmpty("number").TryParseOrDefault<int>(int.TryParse);

                var messageId = element.Value;

                segments.Add(new NzbSegment(bytes, number, messageId));
            }

            return segments;
        }

        private string DebuggerDisplay
        {
            get { return string.Format("Files: {0}", Files.Count); }
        }
    }
}