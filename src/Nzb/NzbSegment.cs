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

        public long Bytes { get; private set; }

        public int Number { get; private set; }

        public string MessageId { get; private set; }

        private string DebuggerDisplay
        {
            get { return string.Format("#{0} ({1} bytes)", Number, Bytes); }
        }

        public override string ToString()
        {
            return MessageId;
        }
    }
}