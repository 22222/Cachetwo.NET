using LatticeObjectTree;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cachetwo.Distributed
{
    public class BinaryConvertDistributedCacheExtensionsTest
    {
        private readonly IDistributedCache distributedCache;

        public BinaryConvertDistributedCacheExtensionsTest()
        {
            this.distributedCache = new MemoryDistributedCache(Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));
        }

        [Fact]
        public void Get_IdNamePair_Null()
        {
            Assert.Null(distributedCache.Get<IdNamePair>("test"));
        }

        [Fact]
        public async Task GetAsync_IdNamePair_Null()
        {
            Assert.Null(await distributedCache.GetAsync<IdNamePair>("test"));
        }

        [Fact]
        public void Get_Int_Zero()
        {
            Assert.Equal(0, distributedCache.Get<int>("test"));
        }

        [Fact]
        public async Task GetAsync_Int_Zero()
        {
            Assert.Equal(0, await distributedCache.GetAsync<int>("test"));
        }

        [Fact]
        public void TryGet_Int_DoesNotExist_ReturnsFalse()
        {
            Assert.False(distributedCache.TryGet("test", out int actual));
            Assert.Equal(0, actual);
        }

        [Fact]
        public void SetThenGet_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            distributedCache.Set(key, input);
            ObjectTreeAssert.AreEqual(input, distributedCache.Get<IdNamePair>(key));
        }

        [Fact]
        public async Task SetAsyncThenGetAsync_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            await distributedCache.SetAsync(key, input);
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetAsync<IdNamePair>(key));
        }

        [Fact]
        public void SetThenGet_AbsoluteExpiration_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            distributedCache.Set(key, input, absoluteExpiration: DateTimeOffset.MaxValue);
            ObjectTreeAssert.AreEqual(input, distributedCache.Get<IdNamePair>(key));
        }

        [Fact]
        public async Task SetAsyncThenGetAsync_AbsoluteExpiration_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            await distributedCache.SetAsync(key, input, absoluteExpiration: DateTimeOffset.MaxValue);
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetAsync<IdNamePair>(key));
        }

        [Fact]
        public void SetThenGet_AbsoluteExpirationRelativeToNow_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            distributedCache.Set(key, input, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2));
            ObjectTreeAssert.AreEqual(input, distributedCache.Get<IdNamePair>(key));
        }

        [Fact]
        public async Task SetThenGetAsync_AbsoluteExpirationRelativeToNow_IdNamePair()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            await distributedCache.SetAsync(key, input, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetAsync<IdNamePair>(key));
        }

        [Fact]
        public void GetOrCreateIfNotDefault_Int_CreatesOnFirstCallThenGetsOnSecondCall()
        {
            const string key = "test";
            var input = 2;
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<int>(key, (options) => input));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<int>(key, (options) => 3));
        }

        [Fact]
        public async Task GetOrCreateIfNotDefaultAsync_Int_CreatesOnFirstCallThenGetsOnSecondCall()
        {
            const string key = "test";
            var input = 2;
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<int>(key, (options) => Task.FromResult(input)));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<int>(key, (options) => Task.FromResult(3)));
        }

        [Fact]
        public void GetOrCreateIfNotDefault_Int_DefaultValueThenNonDefault()
        {
            const string key = "test";
            ObjectTreeAssert.AreEqual(0, distributedCache.GetOrCreateIfNotDefault<int>(key, (options) => 0));
            ObjectTreeAssert.AreEqual(2, distributedCache.GetOrCreateIfNotDefault<int>(key, (options) => 2));
        }

        [Fact]
        public async Task GetOrCreateIfNotDefaultAsync_Int_DefaultValueThenNonDefault()
        {
            const string key = "test";
            ObjectTreeAssert.AreEqual(0, await distributedCache.GetOrCreateIfNotDefaultAsync<int>(key, (options) => Task.FromResult(0)));
            ObjectTreeAssert.AreEqual(2, await distributedCache.GetOrCreateIfNotDefaultAsync<int>(key, (options) => Task.FromResult(2)));
        }

        [Fact]
        public void GetOrCreateIfNotDefault_IdNamePair_CreatesOnFirstCallThenGetsOnSecondCall()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, (options) => input));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, (options) => new IdNamePair { Id = 3, Name = "world" }));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, () => new IdNamePair { Id = 3, Name = "world" }, absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, () => new IdNamePair { Id = 3, Name = "world" }, absoluteExpiration: DateTimeOffset.MaxValue));
        }

        [Fact]
        public async Task GetOrCreateIfNotDefaultAsync_IdNamePair_CreatesOnFirstCallThenGetsOnSecondCall()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, (options) => Task.FromResult(input)));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, (options) => Task.FromResult(new IdNamePair { Id = 3, Name = "world" })));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, () => Task.FromResult(new IdNamePair { Id = 3, Name = "world" }), TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, () => Task.FromResult(new IdNamePair { Id = 3, Name = "world" }), DateTimeOffset.MaxValue));
        }

        [Fact]
        public void GetOrCreateIfNotDefault_IdNamePair_DoesNotCallFactoryIfAlreadyExists()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            distributedCache.Set(key, input);

            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, new Func<DistributedCacheEntryOptions, IdNamePair>((options) => throw new Exception("Test"))));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, new Func<IdNamePair>(() => throw new Exception("Test2")), absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, distributedCache.GetOrCreateIfNotDefault<IdNamePair>(key, new Func<IdNamePair>(() => throw new Exception("Test3")), absoluteExpiration: DateTimeOffset.MaxValue));
        }

        [Fact]
        public async Task GetOrCreateIfNotDefaultAsync_IdNamePair_DoesNotCallFactoryIfAlreadyExists()
        {
            const string key = "test";
            var input = new IdNamePair { Id = 2, Name = "hello" };

            await distributedCache.SetAsync(key, input);

            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, new Func<DistributedCacheEntryOptions, Task<IdNamePair>>((options) => throw new Exception("Test"))));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, new Func<Task<IdNamePair>>(() => throw new Exception("Test2")), absoluteExpirationRelativeToNow: TimeSpan.FromDays(2)));
            ObjectTreeAssert.AreEqual(input, await distributedCache.GetOrCreateIfNotDefaultAsync<IdNamePair>(key, new Func<Task<IdNamePair>>(() => throw new Exception("Test3")), absoluteExpiration: DateTimeOffset.MaxValue));
        }

        [Theory]
        [MemberData(nameof(TestData.PrimitiveValueCases), MemberType = typeof(TestData))]
        public void PrimitiveValues_SetThenGet(object input)
        {
            var inputType = input.GetType();
            const string key = "test";

            var getMethod = ReflectionUtils.GetMethodInfo(() =>
                BinaryConvertDistributedCacheExtensions.Get<object>(distributedCache, key)
            );
            getMethod = getMethod.GetGenericMethodDefinition().MakeGenericMethod(inputType);

            var setMethod = ReflectionUtils.GetMethodInfo(() =>
                BinaryConvertDistributedCacheExtensions.Set<object>(distributedCache, key, input)
            );
            setMethod = setMethod.GetGenericMethodDefinition().MakeGenericMethod(inputType);

            setMethod.Invoke(null, new object[] { distributedCache, key, input });
            var actual = getMethod.Invoke(null, new object[] { distributedCache, key });

            Assert.Equal(input, actual);
        }

        [Theory]
        [MemberData(nameof(TestData.PrimitiveValueCases), MemberType = typeof(TestData))]
        public async Task PrimitiveValues_SetAsyncThenGetAsync(object input)
        {
            var inputType = input.GetType();
            const string key = "test";
            var cancellationToken = CancellationToken.None;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            var getAsyncMethod = ReflectionUtils.GetMethodInfo(() =>
                BinaryConvertDistributedCacheExtensions.GetAsync<object>(distributedCache, key, cancellationToken)
            );
            getAsyncMethod = getAsyncMethod.GetGenericMethodDefinition().MakeGenericMethod(inputType);

            var setAsyncMethod = ReflectionUtils.GetMethodInfo(() =>
                BinaryConvertDistributedCacheExtensions.SetAsync<object>(distributedCache, key, input, cancellationToken)
            );
            setAsyncMethod = setAsyncMethod.GetGenericMethodDefinition().MakeGenericMethod(inputType);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await (Task)setAsyncMethod.Invoke(null, new object[] { distributedCache, key, input, cancellationToken });
            var getTask = (Task)getAsyncMethod.Invoke(null, new object[] { distributedCache, key, cancellationToken });
            await getTask;

            var getTaskResultProperty = typeof(Task<>).MakeGenericType(inputType).GetProperty(nameof(Task<object>.Result));
            var actual = getTaskResultProperty.GetValue(getTask);
            Assert.Equal(input, actual);
        }
    }
}
