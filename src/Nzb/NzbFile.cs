using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NzbFile : INzbFile
    {
        private readonly string _poster;

        private readonly DateTimeOffset _date;

        private readonly string _subject;

        private readonly IReadOnlyList<string> _groups;

        private readonly IReadOnlyList<NzbSegment> _segments;

        private readonly Lazy<long> _bytes;

        public NzbFile([NotNull] string poster,
            DateTimeOffset date,
            [NotNull] string subject,
            [NotNull] IReadOnlyList<string> groups,
            [NotNull] IReadOnlyList<NzbSegment> segments)
        {
            _poster = Check.NotNull(poster, "poster");
            _date = date;
            _subject = Check.NotNull(subject, "subject");
            _groups = Check.NotNull(groups, "groups");
            _segments = Check.NotNull(segments, "segments");
            _bytes = new Lazy<long>(() => Segments.Sum(x => x.Bytes));
        }

        /// <summary>
        /// Gets the poster of the file.
        /// </summary>
        /// <value>The poster of the file.</value>
        public string Poster
        {
            get { return _poster; }
        }

        /// <summary>
        /// Gets the date the server saw this file.
        /// </summary>
        /// <value>The date the server saw this file.</value>
        public DateTimeOffset Date
        {
            get { return _date; }
        }

        /// <summary>
        /// Gets the subject of the Usenet article.
        /// </summary>
        /// <value>The subject of the Usenet article.</value>
        public string Subject
        {
            get { return _subject; }
        }

        /// <summary>
        /// Gets the groups this file has been posted in.
        /// </summary>
        /// <value>The groups this files has been posted in.</value>
        public IReadOnlyList<string> Groups
        {
            get { return _groups; }
        }

        /// <summary>
        /// Gets the segments that makes up this file.
        /// </summary>
        /// <value>The segments that makes up this file.</value>
        public IReadOnlyList<INzbSegment> Segments
        {
            get { return _segments; }
        }

        /// <summary>
        /// Gets the total number of bytes for all the file's segments.
        /// </summary>
        /// <value>The total number of bytes for all the file's segments.</value>
        public long Bytes
        {
            get { return _bytes.Value; }
        }

        private string DebuggerDisplay
        {
            get { return ToString(); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _subject;
        }
    }
}