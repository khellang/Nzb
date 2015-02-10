namespace Nzb
{
    public interface INzbSegment : IFluentInterface
    {
        long Bytes { get; }

        int Number { get; }

        string MessageId { get; }
    }
}