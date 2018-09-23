using System;

namespace Cachetwo
{
    public class StringComparisonMetadata
    {
        public int StringComparisonId { get; set; }

        public string StringComparisonName => Enum.GetName(typeof(StringComparison), (StringComparison)StringComparisonId);
    }
}
