Extension methods that expand on [Microsoft.Extension.Caching](https://github.com/aspnet/Caching).

The main focus of this library is on type-specific serialization in a distributed cache using [Json.NET BSON](https://github.com/JamesNK/Newtonsoft.Json.Bson).

Installation
============
There are a couple ways to install this package:

* Install the [NuGet package](https://www.nuget.org/packages/Cachetwo.NET/)
* Download the assembly from the [latest release](https://github.com/22222/Cachetwo.NET/releases/latest) and reference it manually
* Copy the source code directly into your project

This project is available under either of two licenses: [MIT](LICENSE) or [The Unlicense](UNLICENSE).  So if you're able to use the Unlicense, you don't have to provide any kind of attribution if you copy anything into your own project.


Getting Started
===============
This library mostly just adds more extension methods for [IDistributedCache](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache) or [IMemoryCache](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.memory.imemorycache).

The main feature is handling object serialization with a distributed cache.  For example:

```c#
using Cachetwo.Distributed;
using Microsoft.Extensions.Caching.Distributed;

public static void Sample(IDistributedCache distributedCache)
{
	int[] value = new[] { 1, 2 };
	distributedCache.Set("sample", value, System.TimeSpan.FromMinutes(15));

	int[] storedValue = distributedCache.Get<int[]>("sample");
	System.Diagnostics.Debug.Assert(System.Linq.Enumerable.SequenceEqual(value, storedValue));
}
```

There are also methods similar to the `GetOrCreate` memory cache extension methods:

```c#
using Cachetwo.Distributed;
using Microsoft.Extensions.Caching.Distributed;

public static void Sample2(IDistributedCache distributedCache)
{
	int[] value = new[] { 1, 2 };
	int[] storedValue = distributedCache.GetOrCreateIfNotDefault("sample", () => value, System.TimeSpan.FromMinutes(15));
	System.Diagnostics.Debug.Assert(System.Linq.Enumerable.SequenceEqual(value, storedValue));
}
```

And a few bonus memory cache extension methods are included for specifying options like `absoluteExpirationRelativeToNow`:

```c#
using Cachetwo.Memory;
using Microsoft.Extensions.Caching.Memory;

public static void Sample(IDistributedCache distributedCache)
{
	int[] value = new[] { 1, 2 };
	distributedCache.Set("sample", value, System.TimeSpan.FromMinutes(15));

	int[] storedValue = distributedCache.Get<int[]>("sample");
	System.Diagnostics.Debug.Assert(System.Linq.Enumerable.SequenceEqual(value, storedValue));
}
```


Object Serialization
====================
Normal objects are serialized using [Json.NET BSON](https://github.com/JamesNK/Newtonsoft.Json.Bson) to convert the objects to and from BSON.  The same limitations apply that you'd have with using [Json.NET] to serialize any object, including:

* Only public properties with getters and setters will be stored in the cache
* A self-referencing loop may cause serialization to fail

Also, custom (non-BSON) serialization is not used for primitive types or for common value types including `DateTime`, `DateTimeOffset`, `TimeSpan`, `Guid`.  Values of type `string` and `byte[]` also have special handling.

But as much as possible, you should avoid depending on the exact serialization format.  It's possible that it will change if the future if a better serialization strategy comes along.

There is not currently any customization available for the serialization strategy.  If you need a custom strategy, your best bet is probably to just make your own copy of this library and modify or replace the [BinaryConvert class](Cachetwo/Distributed/BinaryConvert.cs).
