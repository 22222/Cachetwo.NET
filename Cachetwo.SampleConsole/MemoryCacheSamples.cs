using Cachetwo.Memory;
using Microsoft.Extensions.Caching.Memory;

namespace Cachetwo.SampleConsole
{
    public static class MemoryCacheSamples
    {
        public static void Sample(IMemoryCache memoryCache)
        {
            string value = memoryCache.GetOrCreate("sample", () => "hi", System.TimeSpan.FromMinutes(15));
            System.Diagnostics.Debug.Assert(value == "hi");
        }
    }
}
