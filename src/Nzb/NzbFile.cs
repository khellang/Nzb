using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a file linked in a <see cref="NzbDocument"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public sealed class NzbFile
    {
        /// <summary>
        /// Creates an instance of an <see cref="NzbFile"/>.
        /// </summary>
        /// <param name="poster">The file's poster.</param>
        /// <param name="date">The date the server saw this file.</param>
        /// <param name="subject">The subject of the Usenet article.</param>
        /// <param name="groups">The groups this file has been posted in.</param>
        /// <param name="segments">A list of segments that make up this file.</param>
        /// <param name="bytes">Total number of bytes for all the file's segments.</param>
        public NzbFile([NotNull] string poster,
            DateTimeOffset date,
            [NotNull] string subject,
            [NotNull] IReadOnlyList<string> groups,
            [NotNull] IReadOnlyList<NzbSegment> segments,
            long bytes)
        {
            Poster = Check.NotNull(poster, nameof(poster));
            Date = date;
            Subject = Check.NotNull(subject, nameof(subject));
            Groups = Check.NotNull(groups, nameof(groups));
            Segments = Check.NotNull(segments, nameof(segments));
            Bytes = bytes;
        }

        /// <summary>
        /// Gets the poster of the file.
        /// </summary>
        /// <value>The poster of the file.</value>
        [NotNull]
        public string Poster { get; }

        /// <summary>
        /// Gets the date the server saw this file.
        /// </summary>
        /// <value>The date the server saw this file.</value>
        public DateTimeOffset Date { get; }

        /// <summary>
        /// Gets the subject of the Usenet article.
        /// </summary>
        /// <value>The subject of the Usenet article.</value>
        [NotNull]
        public string Subject { get; }

        /// <summary>
        /// Gets the groups this file has been posted in.
        /// </summary>
        /// <value>The groups this files has been posted in.</value>
        [NotNull, ItemNotNull]
        public IReadOnlyList<string> Groups { get; }

        /// <summary>
        /// Gets the segments that makes up this file.
        /// </summary>
        /// <value>The segments that makes up this file.</value>
        [NotNull, ItemNotNull]
        public IReadOnlyList<NzbSegment> Segments { get; }

        /// <summary>
        /// Gets the total number of bytes for all the file's segments.
        /// </summary>
        /// <value>The total number of bytes for all the file's segments.</value>
        public long Bytes { get; }

        private string DebuggerDisplay => ToString();

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Subject}, Poster: {Poster}, Size: {Bytes} bytes";
        }
    }
}
