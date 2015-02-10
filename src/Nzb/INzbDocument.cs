using System.Collections.Generic;

namespace Nzb
{
    public interface INzbDocument : IFluentInterface
    {
        IReadOnlyDictionary<string, string> Metadata { get; }

        IReadOnlyList<INzbFile> Files { get; }

        long Bytes { get; }
    }
}