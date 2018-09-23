using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Cachetwo.SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var distributedCache = new MemoryDistributedCache(Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));

            Console.WriteLine("Running distributed sample 1");
            DistributedCacheSamples.Sample(distributedCache);

            Console.WriteLine("Running distributed sample 2");
            DistributedCacheSamples.Sample2(distributedCache);

            using (var memoryCache = new MemoryCache(Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions())))
            {
                Console.WriteLine("Running memory sample 1");
                MemoryCacheSamples.Sample(memoryCache);
            }
        }
    }
}
