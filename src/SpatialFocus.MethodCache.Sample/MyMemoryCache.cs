// <copyright file="MyMemoryCache.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample
{
	using System;
	using System.Runtime.CompilerServices;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Logging;

	public class MyMemoryCache : IMemoryCache
	{
		public MyMemoryCache(IMemoryCache memoryCache, ILogger<MyMemoryCache> logger)
		{
			MemoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected ILogger<MyMemoryCache> Logger { get; }

		protected IMemoryCache MemoryCache { get; }

		public ICacheEntry CreateEntry(object key)
		{
			Logger.LogInformation("Creating entry with key {key}", key);
			return MemoryCache.CreateEntry(key);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Remove(object key) => throw new NotImplementedException();

		public bool TryGetValue(object key, out object value)
		{
			if (!MemoryCache.TryGetValue(key, out value))
			{
				Logger.LogInformation("Value for {key} not found", key);
				return false;
			}

			Logger.LogInformation("Value for {key} found: {value}", key, value);
			return true;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				MemoryCache?.Dispose();
			}
		}
	}
}