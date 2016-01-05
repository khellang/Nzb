using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a segment of a file in a NZB document.
    /// </summary>
    [PublicAPI]
    public interface INzbSegment : IFluentInterface
    {
        /// <summary>
        /// Gets the number of bytes.
        /// </summary>
        /// <value>The number of bytes.</value>
        long Bytes { get; }

        /// <summary>
        /// Gets the segment number.
        /// </summary>
        /// <value>The segment number.</value>
        int Number { get; }

        /// <summary>
        /// Gets the Usenet message identifier.
        /// </summary>
        /// <value>The Usenet message identifier.</value>
        [NotNull]
        string MessageId { get; }
    }
}