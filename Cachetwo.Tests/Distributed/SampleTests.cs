using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cachetwo.Distributed
{
    public class SampleTests
    {
        private readonly IDistributedCache distributedCache;

        public SampleTests()
        {
            this.distributedCache = new MemoryDistributedCache(Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));
        }

        [Fact]
        public void Sample()
        {
            IDictionary<string, int> value = new Dictionary<string, int>
            {
                ["a"] = 1,
                ["b"] = 2,
            };
            distributedCache.Set("sample", value, TimeSpan.FromMinutes(15));

            IDictionary<string, int> storedValue = distributedCache.Get<IDictionary<string, int>>("sample");
            Assert.Equal(value, storedValue);
        }

        [Fact]
        public void Sample2()
        {
            int[] value = distributedCache.GetOrCreateIfNotDefault("sample", () => new int[] { 1, 2 }, TimeSpan.FromMinutes(15));
            Assert.Equal(new int[] { 1, 2 }, value);
        }
    }
}
