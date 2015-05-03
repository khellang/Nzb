using System;

namespace Nzb
{
    internal static class EpochUtility
    {
        private static readonly DateTimeOffset UnixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset ToUnixEpoch(this long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp).ToLocalTime();
        }
    }
}