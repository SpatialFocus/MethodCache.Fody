// <copyright file="MockMemoryCache.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests.Mock
{
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.DependencyInjection;

	public sealed class MockMemoryCache : IMemoryCache
	{
		public MockMemoryCache(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public static MockMemoryCache Default
		{
			get
			{
				ServiceCollection serviceCollection = new ServiceCollection();
				serviceCollection.AddMemoryCache();
				return new MockMemoryCache(serviceCollection.BuildServiceProvider().GetRequiredService<IMemoryCache>());
			}
		}

		public int CountGets { get; set; }

		public int CountSets { get; set; }

		public object LastCreatedEntryKey { get; set; }

		public ICacheEntry LastCreatedCacheEntry { get; set; }

		private IMemoryCache MemoryCache { get; }

		public ICacheEntry CreateEntry(object key)
		{
			CountSets++;

			ICacheEntry cacheEntry = MemoryCache.CreateEntry(key);
			LastCreatedCacheEntry = cacheEntry;
			LastCreatedEntryKey = cacheEntry.Key;

			return cacheEntry;
		}

		public void Dispose() => MemoryCache?.Dispose();

		public void Remove(object key)
		{
			MemoryCache.Remove(key);
		}

		public bool TryGetValue(object key, out object value)
		{
			CountGets++;

			return MemoryCache.TryGetValue(key, out value);
		}
	}
}