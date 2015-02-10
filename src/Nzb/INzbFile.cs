using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a file linked in a NZB document.
    /// </summary>
    public interface INzbFile : IFluentInterface
    {
        /// <summary>
        /// Gets the poster of the file.
        /// </summary>
        /// <value>The poster of the file.</value>
        [NotNull]
        string Poster { get; }

        /// <summary>
        /// Gets the date the server saw this file.
        /// </summary>
        /// <value>The date the server saw this file.</value>
        DateTimeOffset Date { get; }

        /// <summary>
        /// Gets the subject of the Usenet article.
        /// </summary>
        /// <value>The subject of the Usenet article.</value>
        [NotNull]
        string Subject { get; }

        /// <summary>
        /// Gets the groups this file has been posted in.
        /// </summary>
        /// <value>The groups this files has been posted in.</value>
        [NotNull, ItemNotNull]
        IReadOnlyList<string> Groups { get; }

        /// <summary>
        /// Gets the segments that makes up this file.
        /// </summary>
        /// <value>The segments that makes up this file.</value>
        [NotNull, ItemNotNull]
        IReadOnlyList<INzbSegment> Segments { get; }

        /// <summary>
        /// Gets the total number of bytes for all the file's segments.
        /// </summary>
        /// <value>The total number of bytes for all the file's segments.</value>
        long Bytes { get; }
    }
}