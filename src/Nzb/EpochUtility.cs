using System;

namespace Nzb
{
    internal static class EpochUtility
    {
#if NETSTANDARD1_0
        private static readonly DateTimeOffset UnixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
#endif

        public static DateTimeOffset ToUnixEpoch(this long timestamp)
        {
#if NETSTANDARD1_0
            return UnixEpoch.AddSeconds(timestamp).ToLocalTime();
#else
            return DateTimeOffset.FromUnixTimeSeconds(timestamp);
#endif
        }
    }
}
