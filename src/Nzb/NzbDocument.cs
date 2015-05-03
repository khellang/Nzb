using System;
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
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class NzbDocument : INzbDocument
    {
        /// <summary>
        /// The default encoding for a NZB document.
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.GetEncoding("iso-8859-1");

        private static readonly IReadOnlyList<NzbSegment> EmptySegments = new NzbSegment[0];

        private static readonly IReadOnlyList<string> EmptyGroups = new string[0];

        private static readonly IReadOnlyDictionary<string, string> EmptyMetadata =
            new Dictionary<string, string>(capacity: 0);

        private readonly IReadOnlyDictionary<string, string> _metadata;

        private readonly IReadOnlyList<NzbFile> _files;

        private readonly Lazy<long> _bytes;

        private NzbDocument(IReadOnlyDictionary<string, string> metadata, IReadOnlyList<NzbFile> files)
        {
            _metadata = Check.NotNull(metadata, "metadata");
            _files = Check.NotNull(files, "files");
            _bytes = new Lazy<long>(() => Files.Sum(x => x.Bytes));
        }

        /// <summary>
        /// Gets the metadata associated with the contents of the document.
        /// </summary>
        /// <value>The content metadata.</value>
        public IReadOnlyDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets the information about all the files linked in the document.
        /// </summary>
        /// <value>The files linked in the document.</value>
        public IReadOnlyList<INzbFile> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Gets the total number of bytes for all files linked in the document.
        /// </summary>
        /// <value>The total number of bytes for all files linked in the document.</value>
        public long Bytes
        {
            get { return _bytes.Value; }
        }

        private string DebuggerDisplay
        {
            get { return ToString(); }
        }

        /// <summary>
        /// Loads the document from the specified stream, using <see cref="DefaultEncoding"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        [Pure, NotNull]
        public static Task<INzbDocument> Load([NotNull] Stream stream)
        {
            return Load(stream, DefaultEncoding);
        }

        /// <summary>
        /// Loads the document from the specified stream, using the specified encoding.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding to use.</param>
        [Pure, NotNull]
        public static async Task<INzbDocument> Load([NotNull] Stream stream, [NotNull] Encoding encoding)
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
        /// <exception cref="Nzb.InvalidNzbFormatException">The text represents an invalid NZB document.</exception>
        [Pure, NotNull]
        public static INzbDocument Parse([NotNull] string text)
        {
            Check.NotEmpty(text, "text");

            var document = XDocument.Parse(text);

            var nzbElement = document.Element(Constants.NzbElement);
            if (nzbElement == null)
            {
                throw new InvalidNzbFormatException("Could not find required 'nzb' element.");
            }

            var metadata = ParseMetadata(nzbElement);

            var files = ParseFiles(nzbElement);

            return new NzbDocument(metadata, files);
        }

        public override string ToString()
        {
            return string.Format("Files: {0}", _files.Count.ToString());
        }

        private static IReadOnlyDictionary<string, string> ParseMetadata(XContainer element)
        {
            var headElement = element.Element(Constants.HeadElement);
            if (headElement != null)
            {
                var metaElements = headElement.Elements(Constants.MetaElement).ToList();

                var metadata = new Dictionary<string, string>(capacity: metaElements.Count);

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

            return EmptyMetadata;
        }

        private static IReadOnlyList<NzbFile> ParseFiles(XContainer element)
        {
            return element.Elements(Constants.FileElement)
                .Select(fileElement => ParseFile(fileElement))
                .ToList();
        }

        private static NzbFile ParseFile(XElement element)
        {
            var poster = element.AttributeValueOrEmpty(Constants.PosterAttribute);

            var unixTimestamp = element
                .AttributeValueOrEmpty(Constants.DateAttribute)
                .TryParseOrDefault<long>(long.TryParse);

            var date = unixTimestamp.ToUnixEpoch();

            var subject = element.AttributeValueOrEmpty(Constants.SubjectAttribute);

            var groups = ParseGroups(element);

            var segments = ParseSegments(element);

            return new NzbFile(poster, date, subject, groups, segments);
        }

        private static IReadOnlyList<string> ParseGroups(XContainer element)
        {
            var groupsElement = element.Element(Constants.GroupsElement);
            if (groupsElement != null)
            {
                return groupsElement.Elements(Constants.GroupElement)
                    .Select(x => x.Value)
                    .ToList();
            }

            return EmptyGroups;
        }

        private static IReadOnlyList<NzbSegment> ParseSegments(XContainer element)
        {
            var segmentsElement = element.Element(Constants.SegmentsElement);
            if (segmentsElement != null)
            {
                return segmentsElement.Elements(Constants.SegmentElement)
                    .Select(segmentElement => ParseSegment(segmentElement))
                    .ToList();
            }

            return EmptySegments;
        }

        private static NzbSegment ParseSegment(XElement element)
        {
            var bytes = element
                .AttributeValueOrEmpty(Constants.BytesAttribute)
                .TryParseOrDefault<long>(long.TryParse);

            var number = element
                .AttributeValueOrEmpty(Constants.NumberAttribute)
                .TryParseOrDefault<int>(int.TryParse);

            var messageId = element.Value;

            return new NzbSegment(bytes, number, messageId);
        }
    }
}