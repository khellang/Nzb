using JetBrains.Annotations;

namespace Nzb
{
    public interface INzbSegment : IFluentInterface
    {
        long Bytes { get; }

        int Number { get; }

        [NotNull]
        string MessageId { get; }
    }
}