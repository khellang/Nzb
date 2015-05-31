using System.Diagnostics;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NzbSegment : INzbSegment
    {
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
        public string MessageId { get; }

        private string DebuggerDisplay => ToString();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"#{Number} - {MessageId} ({Bytes} bytes)";
        }
    }
}