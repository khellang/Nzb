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
        private readonly Lazy<long> _bytes;

        public NzbFile([NotNull] string poster,
            DateTimeOffset date,
            [NotNull] string subject,
            [NotNull] IReadOnlyList<string> groups,
            [NotNull] IReadOnlyList<NzbSegment> segments)
        {
            Poster = Check.NotNull(poster, "poster");
            Date = date;
            Subject = Check.NotNull(subject, "subject");
            Groups = Check.NotNull(groups, "groups");
            Segments = Check.NotNull(segments, "segments");

            _bytes = new Lazy<long>(() => Segments.Sum(x => x.Bytes));
        }

        /// <summary>
        /// Gets the poster of the file.
        /// </summary>
        /// <value>The poster of the file.</value>
        public string Poster { get; private set; }

        /// <summary>
        /// Gets the date the server saw this file.
        /// </summary>
        /// <value>The date the server saw this file.</value>
        public DateTimeOffset Date { get; private set; }

        /// <summary>
        /// Gets the subject of the Usenet article.
        /// </summary>
        /// <value>The subject of the Usenet article.</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the groups this file has been posted in.
        /// </summary>
        /// <value>The groups this files has been posted in.</value>
        public IReadOnlyList<string> Groups { get; private set; }

        /// <summary>
        /// Gets the segments that makes up this file.
        /// </summary>
        /// <value>The segments that makes up this file.</value>
        public IReadOnlyList<INzbSegment> Segments { get; private set; }

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
            get { return Subject; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Subject;
        }
    }
}