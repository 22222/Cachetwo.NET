using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cachetwo.Distributed
{
    /// <summary>
    /// Extension methods for <see cref="IDistributedCache"/> that handle conversion to and from byte arrays using <see cref="BinaryConvert"/>.
    /// </summary>
    public static class BinaryConvertDistributedCacheExtensions
    {
        /// <summary>
        /// Gets a <typeparamref name="TItem"/> value from the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <returns>The <typeparamref name="TItem"/> value from the stored cache key.</returns>
        public static TItem Get<TItem>(this IDistributedCache cache, string key)
        {
            var valueBytes = cache.Get(key);
            if (valueBytes == null)
            {
                return default(TItem);
            }

            try
            {
                return BinaryConvert.Deserialize<TItem>(valueBytes);
            }
            catch (Exception)
            {
                return default(TItem);
            }
        }

        /// <summary>
        /// Gets a <typeparamref name="TItem"/> value from the specified cache with the specified key if present.
        /// </summary>
        /// <remarks>
        /// This method is primarily intended for non-nullable value types.
        /// For nullable types, it's probably easier to use the normal <see cref="Get"/> method and check the result for null.
        /// </remarks>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="value">The <typeparamref name="TItem"/> value from the stored cache key.</param>
        /// <returns>True if the key was found.</returns>
        public static bool TryGet<TItem>(this IDistributedCache cache, string key, out TItem value)
        {
            var valueBytes = cache.Get(key);
            if (valueBytes == null)
            {
                value = default(TItem);
                return false;
            }

            try
            {
                value = BinaryConvert.Deserialize<TItem>(valueBytes);
                return true;
            }
            catch (Exception)
            {
                value = default(TItem);
                return false;
            }
        }

        /// <summary>
        /// Asynchronously gets a <typeparamref name="TItem"/> value from the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that gets the <typeparamref name="TItem"/> value from the stored cache key.</returns>
        public static async Task<TItem> GetAsync<TItem>(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken))
        {
            var valueBytes = await cache.GetAsync(key, token);
            if (valueBytes == null)
            {
                return default(TItem);
            }

            try
            {
                return BinaryConvert.Deserialize<TItem>(valueBytes);
            }
            catch (Exception)
            {
                return default(TItem);
            }
        }

        private static async Task<(bool, TItem)> TryGetAsync<TItem>(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken))
        {
            var valueBytes = await cache.GetAsync(key, token);
            if (valueBytes == null)
            {
                return (false, default(TItem));
            }

            try
            {
                TItem value = BinaryConvert.Deserialize<TItem>(valueBytes);
                return (true, value);
            }
            catch (Exception)
            {
                return (false, default(TItem));
            }
        }

        /// <summary>
        /// Sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value)
        {
            cache.Set<TItem>(key, value, new DistributedCacheEntryOptions());
        }

        /// <summary>
        /// Asynchronously sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken token = default(CancellationToken))
        {
            return cache.SetAsync<TItem>(key, value, new DistributedCacheEntryOptions(), token);
        }

        /// <summary>
        /// Sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="absoluteExpiration"><see cref="DistributedCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value, DateTimeOffset absoluteExpiration)
        {
            cache.Set<TItem>(key, value, new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration });
        }

        /// <summary>
        /// Asynchronously sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="absoluteExpiration"><see cref="DistributedCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, DateTimeOffset absoluteExpiration, CancellationToken token = default(CancellationToken))
        {
            return cache.SetAsync<TItem>(key, value, options: new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration }, token: token);
        }

        /// <summary>
        /// Sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="absoluteExpirationRelativeToNow"><see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            cache.Set<TItem>(key, value, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow });
        }

        /// <summary>
        /// Asynchronously sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="absoluteExpirationRelativeToNow"><see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default(CancellationToken))
        {
            return cache.SetAsync(key, value, options: new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow }, token: token);
        }

        /// <summary>
        /// Sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="options">The cache options for the entry.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            byte[] valueBytes;
            try
            {
                valueBytes = BinaryConvert.Serialize(value);
            }
            catch (Exception)
            {
                return;
            }
            cache.Set(key, valueBytes, options);
        }

        /// <summary>
        /// Asynchronously sets a <typeparamref name="TItem"/> in the specified cache with the specified key.
        /// </summary>
        /// <typeparam name="TItem">The type of value to store.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="options">The cache options for the entry.</param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
        public static Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            byte[] valueBytes;
            try
            {
                valueBytes = BinaryConvert.Serialize(value);
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
            return cache.SetAsync(key, valueBytes, options, token);
        }

        /// <summary>
        /// Gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key identifying the entry.</param>
        /// <param name="factory">Generates the value to set if there is no existing cache entry.</param>
        /// <param name="absoluteExpiration"><see cref="DistributedCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <returns>The existing or newly set value.</returns>
        public static TItem GetOrCreateIfNotDefault<TItem>(this IDistributedCache cache, string key, Func<TItem> factory, DateTimeOffset absoluteExpiration)
        {
            if (!cache.TryGet(key, out TItem result))
            {
                result = factory();
                if (!Equals(result, default(TItem)))
                {
                    cache.Set<TItem>(key, result, absoluteExpiration);
                }
            }
            return result;
        }

        /// <summary>
        /// Asynchronously gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key identifying the entry.</param>
        /// <param name="factory">Generates the value to set if there is no existing cache entry.</param>
        /// <param name="absoluteExpiration"><see cref="DistributedCacheEntryOptions.AbsoluteExpiration"/></param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that gets the existing or newly set value.</returns>
        public static async Task<TItem> GetOrCreateIfNotDefaultAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory, DateTimeOffset absoluteExpiration, CancellationToken token = default(CancellationToken))
        {
            var (found, result) = await cache.TryGetAsync<TItem>(key, token: token);
            if (!found)
            {
                result = await factory();
                if (!Equals(result, default(TItem)))
                {
                    await cache.SetAsync<TItem>(key, result, absoluteExpiration, token: token);
                }
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
        /// <param name="absoluteExpirationRelativeToNow"><see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <returns>The existing or newly set value.</returns>
        public static TItem GetOrCreateIfNotDefault<TItem>(this IDistributedCache cache, string key, Func<TItem> factory, TimeSpan absoluteExpirationRelativeToNow)
        {
            if (!cache.TryGet(key, out TItem result))
            {
                result = factory();
                if (!Equals(result, default(TItem)))
                {
                    cache.Set(key, result, absoluteExpirationRelativeToNow);
                }
            }
            return result;
        }

        /// <summary>
        /// Asynchronously gets or sets an entry in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of value to get.</typeparam>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key identifying the entry.</param>
        /// <param name="factory">Generates the value to set if there is no existing cache entry.</param>
        /// <param name="absoluteExpirationRelativeToNow"><see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/></param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that gets the existing or newly set value.</returns>
        public static async Task<TItem> GetOrCreateIfNotDefaultAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory, TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default(CancellationToken))
        {
            var (found, result) = await cache.TryGetAsync<TItem>(key, token: token);
            if (!found)
            {
                result = await factory();
                if (!Equals(result, default(TItem)))
                {
                    await cache.SetAsync(key, result, absoluteExpirationRelativeToNow, token: token);
                }
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
        /// <returns>The existing or newly set value.</returns>
        public static TItem GetOrCreateIfNotDefault<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, TItem> factory)
        {
            if (!cache.TryGet(key, out TItem result))
            {
                var options = new DistributedCacheEntryOptions();
                result = factory(options);
                if (!Equals(result, default(TItem)))
                {
                    cache.Set(key, result, options);
                }
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
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that gets the existing or newly set value.</returns>
        public static async Task<TItem> GetOrCreateIfNotDefaultAsync<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, Task<TItem>> factory, CancellationToken token = default(CancellationToken))
        {
            var (found, result) = await cache.TryGetAsync<TItem>(key, token: token);
            if (!found)
            {
                var options = new DistributedCacheEntryOptions();
                result = await factory(options);
                if (!Equals(result, default(TItem)))
                {
                    await cache.SetAsync(key, result, options, token: token);
                }
            }
            return result;
        }
    }
}
