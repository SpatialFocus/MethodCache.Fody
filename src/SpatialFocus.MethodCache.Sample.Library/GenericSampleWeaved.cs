// <copyright file="GenericSampleWeaved.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	public class GenericSampleWeaved<TClass1, TClass2>
	{
		public GenericSampleWeaved(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		protected IMemoryCache MemoryCache { get; }

		public int Add<TMethod1, TMethod2>(int a, int b, int c, int d)
		{
			int value;

			// Create a unique cache key, based on namespace, class name and method name as first parameter and corresponding
			// generic class parameters,generic method parameters and method parameters
			Tuple<string, Type, Type, Type, Type, int, int, Tuple<int, int>> key =
				new Tuple<string, Type, Type, Type, Type, int, int, Tuple<int, int>>(
					"SpatialFocus.MethodCache.Sample.Library.BasicSample.Add", typeof(TClass1), typeof(TClass2), typeof(TMethod1),
					typeof(TMethod2), a, b, new Tuple<int, int>(c, d));

			// Check and return if a cached value exists for key
			if (MemoryCache.TryGetValue(key, out value))
			{
				return value;
			}

			// Before each return statement, save the value that would be returned in the cache
			value = a + b + c + d;
			MemoryCache.Set<int>(key, value);
			return value;
		}
	}
}