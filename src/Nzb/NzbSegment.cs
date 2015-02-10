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
            MessageId = Check.NotNull(messageId, "messageId");
        }

        /// <summary>
        /// Gets the number of bytes.
        /// </summary>
        /// <value>The number of bytes.</value>
        public long Bytes { get; private set; }

        /// <summary>
        /// Gets the segment number.
        /// </summary>
        /// <value>The segment number.</value>
        public int Number { get; private set; }

        /// <summary>
        /// Gets the Usenet message identifier.
        /// </summary>
        /// <value>The Usenet message identifier.</value>
        public string MessageId { get; private set; }

        private string DebuggerDisplay
        {
            get { return string.Format("#{0} ({1} bytes)", Number, Bytes); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return MessageId;
        }
    }
}