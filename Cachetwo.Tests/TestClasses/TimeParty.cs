using System;

namespace Cachetwo
{
    public class TimeParty
    {
        public int UnixDateTimeSeconds { get; set; }

        public int? UnitDateTimeSecondsOrNull { get; set; }

        public DateTime DateTimeUtc { get; set; }

        public DateTime? DateTimeUtcOrNull { get; set; }

        public DateTime DateTimeLocal { get; set; }

        public DateTime? DateTimeLocalOrNull { get; set; }

        public DateTime DateTimeUnspecified { get; set; }

        public DateTime? DateTimeUnspecifiedOrNull { get; set; }

        public DateTimeOffset DateTimeOffsetUtc { get; set; }

        public DateTimeOffset? DateTimeOffsetUtcOrNull { get; set; }

        public DateTimeOffset DateTimeOffsetLocal { get; set; }

        public DateTimeOffset? DateTimeOffsetLocalOrNull { get; set; }
    }
}
