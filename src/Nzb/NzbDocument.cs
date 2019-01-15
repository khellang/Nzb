using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a NZB document.
    /// </summary>
    /// <remarks>
    /// See <see href="http://wiki.sabnzbd.org/nzb-specs" /> for the specification.
    /// </remarks>
    [PublicAPI]
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public sealed class NzbDocument
    {
        /// <summary>
        /// The default encoding for a NZB document.
        /// </summary>
        [NotNull]
        public static readonly Encoding DefaultEncoding = Encoding.GetEncoding("iso-8859-1");

        private static readonly IReadOnlyList<NzbSegment> EmptySegments = new NzbSegment[0];

        private static readonly IReadOnlyList<string> EmptyGroups = new string[0];

        private static readonly IReadOnlyDictionary<string, string> EmptyMetadata = new Dictionary<string, string>(capacity: 0);

        private NzbDocument(IReadOnlyDictionary<string, string> metadata, IReadOnlyList<NzbFile> files, long bytes)
        {
            Metadata = Check.NotNull(metadata, nameof(metadata));
            Files = Check.NotNull(files, nameof(files));
            Bytes = bytes;
        }

        /// <summary>
        /// Gets the metadata associated with the contents of the document.
        /// </summary>
        /// <value>The content metadata.</value>
        [NotNull]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets the information about all the files linked in the document.
        /// </summary>
        /// <value>The files linked in the document.</value>
        [NotNull, ItemNotNull]
        public IReadOnlyList<NzbFile> Files { get; }

        /// <summary>
        /// Gets the total number of bytes for all files linked in the document.
        /// </summary>
        /// <value>The total number of bytes for all files linked in the document.</value>
        public long Bytes { get; }

        private string DebuggerDisplay => ToString();

        /// <summary>
        /// Loads the document from the specified stream, using <see cref="DefaultEncoding"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        [Pure, NotNull]
        public static Task<NzbDocument> Load([NotNull] Stream stream) => Load(stream, DefaultEncoding);

        /// <summary>
        /// Loads the document from the specified stream, using the specified encoding.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <exception cref="InvalidNzbFormatException">The <paramref name="stream"/> represents an invalid NZB document.</exception>
        [Pure, NotNull]
        public static async Task<NzbDocument> Load([NotNull] Stream stream, [NotNull] Encoding encoding)
        {
            Check.NotNull(stream, nameof(stream));
            Check.NotNull(encoding, nameof(encoding));

            using (var reader = new StreamReader(stream, encoding))
            {
                return Parse(await reader.ReadToEndAsync());
            }
        }

        /// <summary>
        /// Parses the specified text.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <exception cref="InvalidNzbFormatException">The <paramref name="text"/> represents an invalid NZB document.</exception>
        [Pure, NotNull]
        public static NzbDocument Parse([NotNull] string text)
        {
            Check.NotEmpty(text, nameof(text));

            var document = XDocument.Parse(text);

            var nzbElement = document.Element(Constants.NzbElement);

            if (nzbElement == null)
            {
                throw new InvalidNzbFormatException("Could not find required 'nzb' element.");
            }

            var metadata = ParseMetadata(nzbElement);

            var files = ParseFiles(nzbElement, out var bytes);

            return new NzbDocument(metadata, files, bytes);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => $"Files: {Files.Count}, Size: {Bytes} bytes";

        private static IReadOnlyDictionary<string, string> ParseMetadata(XContainer element)
        {
            var headElement = element.Element(Constants.HeadElement);

            if (headElement == null)
            {
                return EmptyMetadata;
            }

            var metaElements = headElement.Elements(Constants.MetaElement).ToArray();

            var metadata = new Dictionary<string, string>(capacity: metaElements.Length);

            foreach (var metaElement in metaElements)
            {
                var typeAttribute = metaElement.Attribute(Constants.TypeAttribute);

                if (typeAttribute != null)
                {
                    metadata.Add(typeAttribute.Value, metaElement.Value);
                }
            }

            return metadata;
        }

        private static IReadOnlyList<NzbFile> ParseFiles(XContainer element, out long bytes)
        {
            var fileElements = element.Elements(Constants.FileElement).ToArray();

            var files = new List<NzbFile>(capacity: fileElements.Length);

            bytes = 0;

            foreach (var fileElement in fileElements)
            {
                var file = ParseFile(fileElement);

                bytes += file.Bytes;

                files.Add(file);
            }

            return files;
        }

        private static NzbFile ParseFile(XElement element)
        {
            var poster = element.AttributeValueOrEmpty(Constants.PosterAttribute);

            var unixTimestamp = element
                .AttributeValueOrEmpty(Constants.DateAttribute)
                .TryParseOrDefault((string s, out long l) => long.TryParse(s, out l));

            var date = unixTimestamp.ToUnixEpoch();

            var subject = element.AttributeValueOrEmpty(Constants.SubjectAttribute);

            var groups = ParseGroups(element);

            var segments = ParseSegments(element, out long bytes);

            return new NzbFile(poster, date, subject, groups, segments, bytes);
        }

        private static IReadOnlyList<string> ParseGroups(XContainer element)
        {
            var groupsElement = element.Element(Constants.GroupsElement);

            if (groupsElement == null)
            {
                return EmptyGroups;
            }

            var groupElements = groupsElement.Elements(Constants.GroupElement).ToArray();

            var groups = new List<string>(capacity: groupElements.Length);

            foreach (var groupElement in groupElements)
            {
                groups.Add(groupElement.Value);
            }

            return groups;
        }

        private static IReadOnlyList<NzbSegment> ParseSegments(XContainer element, out long bytes)
        {
            bytes = 0;

            var segmentsElement = element.Element(Constants.SegmentsElement);

            if (segmentsElement == null)
            {
                return EmptySegments;
            }

            var segmentElements = segmentsElement.Elements(Constants.SegmentElement).ToArray();

            var segments = new List<NzbSegment>(capacity: segmentElements.Length);

            foreach (var segmentElement in segmentElements)
            {
                var segment = ParseSegment(segmentElement);

                bytes += segment.Bytes;

                segments.Add(segment);
            }

            return segments;
        }

        private static NzbSegment ParseSegment(XElement element)
        {
            var bytes = element
                .AttributeValueOrEmpty(Constants.BytesAttribute)
                .TryParseOrDefault((string s, out long l) => long.TryParse(s, out l));

            var number = element
                .AttributeValueOrEmpty(Constants.NumberAttribute)
                .TryParseOrDefault((string s, out int i) => int.TryParse(s, out i));

            var messageId = element.Value;

            return new NzbSegment(bytes, number, messageId);
        }
    }
}
