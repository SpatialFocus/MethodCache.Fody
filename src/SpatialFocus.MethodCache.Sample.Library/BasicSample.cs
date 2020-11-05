// <copyright file="BasicSample.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	using Microsoft.Extensions.Caching.Memory;

	// MethodCache.Fody will look for classes and methods decorated with the Cache attribute
	[Cache]
	public class BasicSample
	{
		public BasicSample(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		// MethodCache.Fody will look for a property implementing the IMemoryCache
		protected IMemoryCache MemoryCache { get; }

		public int Add(int a, int b)
		{
			return a + b;
		}
	}
}