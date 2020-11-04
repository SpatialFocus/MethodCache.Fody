// <copyright file="MockMemoryCache.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Tests.Mock
{
	using System.Collections.Generic;
	using Microsoft.Extensions.Caching.Memory;

	public class MockMemoryCache : IMemoryCache
	{
		public int CountGets { get; set; }

		public int CountSets { get; set; }

		private Dictionary<object, ICacheEntry> Storage { get; } = new Dictionary<object, ICacheEntry>();

		public ICacheEntry CreateEntry(object key)
		{
			CountSets++;

			if (Storage.ContainsKey(key))
			{
				return Storage[key];
			}

			MockCacheEntry cacheEntry = new MockCacheEntry();
			Storage.Add(key, cacheEntry);

			return cacheEntry;
		}

		public void Dispose()
		{
		}

		public void Remove(object key)
		{
			Storage.Remove(key);
		}

		public bool TryGetValue(object key, out object value)
		{
			CountGets++;

			if (Storage.ContainsKey(key))
			{
				value = Storage[key].Value;
				return true;
			}

			value = null;
			return false;
		}
	}
}