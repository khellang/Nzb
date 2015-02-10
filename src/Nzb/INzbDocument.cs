using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a NZB document.
    /// </summary>
    /// <remarks>
    /// See <see href="http://wiki.sabnzbd.org/nzb-specs" /> for the specification.
    /// </remarks>
    public interface INzbDocument : IFluentInterface
    {
        /// <summary>
        /// Gets the metadata associated with the contents of the document.
        /// </summary>
        /// <value>The content metadata.</value>
        [NotNull, ItemNotNull]
        IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets the information about all the files linked in the document.
        /// </summary>
        /// <value>The files linked in the document.</value>
        [NotNull, ItemNotNull]
        IReadOnlyList<INzbFile> Files { get; }

        /// <summary>
        /// Gets the total number of bytes for all files linked in the document.
        /// </summary>
        /// <value>The total number of bytes for all files linked in the document.</value>
        long Bytes { get; }
    }
}