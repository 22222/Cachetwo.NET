using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cachetwo.Memory
{
    public sealed class SampleTests : IDisposable
    {
        private readonly IMemoryCache memoryCache;

        public SampleTests()
        {
            this.memoryCache = new MemoryCache(Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions()));
        }

        public void Dispose()
        {
            memoryCache.Dispose();
        }

        [Fact]
        public void Sample3()
        {
            string value = memoryCache.GetOrCreate("sample", () => "hi", TimeSpan.FromMinutes(15));
            Assert.Equal("hi", value);
        }
    }
}
