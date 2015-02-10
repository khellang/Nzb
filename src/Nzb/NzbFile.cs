using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NzbFile : INzbFile
    {
        private readonly Lazy<long> _bytes;

        public NzbFile([NotNull] string poster,
            DateTimeOffset date,
            [NotNull] string subject,
            [NotNull] IReadOnlyList<string> groups,
            [NotNull] IReadOnlyList<NzbSegment> segments)
        {
            Poster = Check.NotNull(poster, "poster");
            Date = date;
            Subject = Check.NotNull(subject, "subject");
            Groups = Check.NotNull(groups, "groups");
            Segments = Check.NotNull(segments, "segments");

            _bytes = new Lazy<long>(() => Segments.Sum(x => x.Bytes));
        }

        [NotNull]
        public string Poster { get; private set; }

        public DateTimeOffset Date { get; private set; }

        [NotNull]
        public string Subject { get; private set; }

        [NotNull]
        public IReadOnlyList<string> Groups { get; private set; }

        [NotNull]
        public IReadOnlyList<INzbSegment> Segments { get; private set; }

        public long Bytes
        {
            get { return _bytes.Value; }
        }

        private string DebuggerDisplay
        {
            get { return Subject; }
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}