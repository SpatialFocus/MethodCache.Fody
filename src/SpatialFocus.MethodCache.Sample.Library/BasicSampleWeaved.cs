// <copyright file="BasicSampleWeaved.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	public class BasicSampleWeaved
	{
		public BasicSampleWeaved(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		protected IMemoryCache MemoryCache { get; }

		public int Add(int a, int b)
		{
			// Create a unique cache key, based on namespace, class name and method name as first parameter and corresponding
			// generic class parameters, generic method parameters and method parameters
			Tuple<string, int, int> key = new Tuple<string, int, int>("SpatialFocus.MethodCache.Sample.Library.BasicSample.Add", a, b);

			// Check and return if a cached value exists for key
			if (MemoryCache.TryGetValue(key, out int value))
			{
				return value;
			}

			// Before each return statement, save the value that would be returned in the cache
			value = a + b;
			MemoryCache.Set<int>(key, value);
			return value;
		}
	}
}