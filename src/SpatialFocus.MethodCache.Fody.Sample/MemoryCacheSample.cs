// <copyright file="MemoryCacheSample.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Sample
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	public class MemoryCacheSample
	{
		public MemoryCacheSample(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		protected IMemoryCache MemoryCache { get; }

		public int Add(int a, int b)
		{
			return a + b;
		}

		public int AddCached(int a, int b)
		{
			Tuple<string, int, int> key = new Tuple<string, int, int>($"SpatialFocus.MethodCache.Fody.Sample.MemoryCacheSample.AddCached", a, b);
			int result;

			if (MemoryCache.TryGetValue(key, out result))
			{
				return result;
			}

			result = a + b;

			MemoryCache.Set<int>(key, result);

			return result;
		}
	}
}