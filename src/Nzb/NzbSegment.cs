using System.Diagnostics;
using JetBrains.Annotations;

namespace Nzb
{
    /// <summary>
    /// Represents a segment of an <see cref="NzbFile"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public sealed class NzbSegment
    {
        /// <summary>
        /// Creates an instance of an <see cref="NzbSegment"/>.
        /// </summary>
        /// <param name="bytes">The number of bytes.</param>
        /// <param name="number">The segment number.</param>
        /// <param name="messageId">The Usenet message identifier.</param>
        public NzbSegment(long bytes, int number, [NotNull] string messageId)
        {
            Bytes = bytes;
            Number = number;
            MessageId = Check.NotNull(messageId, nameof(messageId));
        }

        /// <summary>
        /// Gets the number of bytes.
        /// </summary>
        /// <value>The number of bytes.</value>
        public long Bytes { get; }

        /// <summary>
        /// Gets the segment number.
        /// </summary>
        /// <value>The segment number.</value>
        public int Number { get; }

        /// <summary>
        /// Gets the Usenet message identifier.
        /// </summary>
        /// <value>The Usenet message identifier.</value>
        [NotNull]
        public string MessageId { get; }

        private string DebuggerDisplay => ToString();

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"#{Number} - {MessageId}, Size: {Bytes} bytes";
        }
    }
}
