using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nzb
{
    public interface INzbFile : IFluentInterface
    {
        [NotNull]
        string Poster { get; }

        DateTimeOffset Date { get; }

        [NotNull]
        string Subject { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<string> Groups { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<INzbSegment> Segments { get; }

        long Bytes { get; }
    }
}