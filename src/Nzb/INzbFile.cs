using System;
using System.Collections.Generic;

namespace Nzb
{
    public interface INzbFile : IFluentInterface
    {
        string Poster { get; }

        DateTimeOffset Date { get; }

        string Subject { get; }

        IReadOnlyList<string> Groups { get; }

        IReadOnlyList<INzbSegment> Segments { get; }

        long Bytes { get; }
    }
}