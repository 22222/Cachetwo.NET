using Cachetwo.Distributed;
using Microsoft.Extensions.Caching.Distributed;

namespace Cachetwo.SampleConsole
{
    public static class DistributedCacheSamples
    {
        public static void Sample(IDistributedCache distributedCache)
        {
            int[] value = new[] { 1, 2 };
            distributedCache.Set("sample", value, System.TimeSpan.FromMinutes(15));

            int[] storedValue = distributedCache.Get<int[]>("sample");
            System.Diagnostics.Debug.Assert(System.Linq.Enumerable.SequenceEqual(value, storedValue));
        }

        public static void Sample2(IDistributedCache distributedCache)
        {
            int[] value = new[] { 1, 2 };
            int[] storedValue = distributedCache.GetOrCreateIfNotDefault("sample", () => value, System.TimeSpan.FromMinutes(15));
            System.Diagnostics.Debug.Assert(System.Linq.Enumerable.SequenceEqual(value, storedValue));
        }
    }
}
