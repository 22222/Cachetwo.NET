using LatticeObjectTree;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cachetwo.Memory
{
    public sealed class MemoryCacheExtensionsTest : IDisposable
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheExtensionsTest()
        {
            this.memoryCache = new MemoryCache(Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions()));
        }

        public void Dispose()
        {
            memoryCache.Dispose();
        }

        [Fact]
        public void GetOrCreate_IdNamePair_AbsoluteExpiration_CreatesThenGets()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            ObjectTreeAssert.AreEqual(input, memoryCache.GetOrCreate<IdNamePair>(key, () => input, absoluteExpiration: DateTimeOffset.MaxValue));
            ObjectTreeAssert.AreEqual(input, memoryCache.GetOrCreate<IdNamePair>(key, () => new IdNamePair { Id = 3, Name = "world" }, absoluteExpiration: DateTimeOffset.MaxValue));
        }

        [Fact]
        public async Task GetOrCreateAsync_IdNamePair_AbsoluteExpiration_CreatesThenGets()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            ObjectTreeAssert.AreEqual(input, await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult(input), absoluteExpiration: DateTimeOffset.MaxValue));
            ObjectTreeAssert.AreEqual(input, await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult(new IdNamePair { Id = 3, Name = "world" }), absoluteExpiration: DateTimeOffset.MaxValue));
        }

        [Fact]
        public void GetOrCreate_IdNamePair_AbsoluteExpirationRelativeToNow_CreatesThenGets()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            ObjectTreeAssert.AreEqual(input, memoryCache.GetOrCreate<IdNamePair>(key, () => input, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, memoryCache.GetOrCreate<IdNamePair>(key, () => new IdNamePair { Id = 3, Name = "world" }, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
        }

        [Fact]
        public async Task GetOrCreateAsync_IdNamePair_AbsoluteExpirationRelativeToNow_CreatesThenGets()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            ObjectTreeAssert.AreEqual(input, await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult(input), absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult(new IdNamePair { Id = 3, Name = "world" }), absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
        }

        [Fact]
        public void GetOrCreate_IdNamePair_AbsoluteExpiration_NullCreates()
        {
            const string key = "test";
            Assert.Null(memoryCache.GetOrCreate<IdNamePair>(key, () => null, absoluteExpiration: DateTimeOffset.MaxValue));
            Assert.True(memoryCache.TryGetValue<IdNamePair>(key, out var actual));
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetOrCreateAsync_IdNamePair_AbsoluteExpiration_NullCreates()
        {
            const string key = "test";
            Assert.Null(await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult<IdNamePair>(null), absoluteExpiration: DateTimeOffset.MaxValue));
            Assert.True(memoryCache.TryGetValue<IdNamePair>(key, out var actual));
            Assert.Null(actual);
        }

        [Fact]
        public void GetOrCreate_IdNamePair_AbsoluteExpirationRelativeToNow_NullCreates()
        {
            const string key = "test";
            Assert.Null(memoryCache.GetOrCreate<IdNamePair>(key, () => null, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            Assert.True(memoryCache.TryGetValue<IdNamePair>(key, out var actual));
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetOrCreateAsync_IdNamePair_AbsoluteExpirationRelativeToNow_NullCreates()
        {
            const string key = "test";
            Assert.Null(await memoryCache.GetOrCreateAsync<IdNamePair>(key, () => Task.FromResult<IdNamePair>(null), absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            Assert.True(memoryCache.TryGetValue<IdNamePair>(key, out var actual));
            Assert.Null(actual);
        }
    }
}