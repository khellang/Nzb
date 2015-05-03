using System.Diagnostics;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NzbSegment : INzbSegment
    {
        private readonly long _bytes;

        private readonly int _number;

        private readonly string _messageId;

        public NzbSegment(long bytes, int number, [NotNull] string messageId)
        {
            _bytes = bytes;
            _number = number;
            _messageId = Check.NotNull(messageId, "messageId");
        }

        /// <summary>
        /// Gets the number of bytes.
        /// </summary>
        /// <value>The number of bytes.</value>
        public long Bytes
        {
            get { return _bytes; }
        }

        /// <summary>
        /// Gets the segment number.
        /// </summary>
        /// <value>The segment number.</value>
        public int Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Gets the Usenet message identifier.
        /// </summary>
        /// <value>The Usenet message identifier.</value>
        public string MessageId
        {
            get { return _messageId; }
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
            return string.Format("#{0} - {1} ({2} bytes)", Number.ToString(), MessageId, Bytes.ToString());
        }
    }
}