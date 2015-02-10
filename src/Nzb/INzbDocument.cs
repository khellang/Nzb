using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nzb
{
    public interface INzbDocument : IFluentInterface
    {
        [NotNull, ItemNotNull]
        IReadOnlyDictionary<string, string> Metadata { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<INzbFile> Files { get; }

        long Bytes { get; }
    }
}