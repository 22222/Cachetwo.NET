using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cachetwo.Memory
{
    /// <summary>
    /// Extension methods for <see cref="IMemoryCache"/> (to augment the ones in <see cref="Microsoft.Extensions.Caching.Memory.CacheExtensions"/>).
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// Gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key identifying the entry.</param>
        /// <param name="factory">Generates the value to set if there is no existing cache entry.</param>
        /// <param name="absoluteExpiration"><see cref="MemoryCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <returns>The existing or newly set value.</returns>
        public static TItem GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<TItem> factory, DateTimeOffset absoluteExpiration)
        {
            if (!cache.TryGetValue(key, out TItem result))
            {
                result = factory();
                cache.Set<TItem>(key, result, absoluteExpiration);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">An object identifying the entry.</param>
        /// <param name="factory">A factory that returns the value to set and/or modifies the cache entry options.</param>
        /// <param name="absoluteExpiration"><see cref="MemoryCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <returns>A task that gets the existing or newly set value.</returns>
        public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<Task<TItem>> factory, DateTimeOffset absoluteExpiration)
        {
            if (!cache.TryGetValue(key, out TItem result))
            {
                result = await factory();
                cache.Set<TItem>(key, result, absoluteExpiration);
            }
            return result;
        }

        /// <summary>
        /// Gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key identifying the entry.</param>
        /// <param name="factory">Generates the value to set if there is no existing cache entry.</param>
        /// <param name="absoluteExpirationRelativeToNow"><see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <returns>The existing or newly set value.</returns>
        public static TItem GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<TItem> factory, TimeSpan absoluteExpirationRelativeToNow)
        {
            if (!cache.TryGetValue(key, out TItem result))
            {
                result = factory();
                cache.Set<TItem>(key, result, absoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">An object identifying the entry.</param>
        /// <param name="factory">A factory that returns the value to set and/or modifies the cache entry options.</param>
        /// <param name="absoluteExpirationRelativeToNow"><see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <returns>A task that gets the existing or newly set value.</returns>
        public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<Task<TItem>> factory, TimeSpan absoluteExpirationRelativeToNow)
        {
            if (!cache.TryGetValue(key, out TItem result))
            {
                result = await factory();
                cache.Set<TItem>(key, result, absoluteExpirationRelativeToNow);
            }
            return result;
        }
    }
}
