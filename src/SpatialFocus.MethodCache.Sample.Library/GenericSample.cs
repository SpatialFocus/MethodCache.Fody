// <copyright file="GenericSample.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	using Microsoft.Extensions.Caching.Memory;

	// MethodCache.Fody will look for classes and methods decorated with the Cache attribute
	[Cache]
	public class GenericSample<TClass1, TClass2>
	{
		public GenericSample(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		// MethodCache.Fody will look for a property implementing the IMemoryCache
		protected IMemoryCache MemoryCache { get; }

		[Cache]
		public int Add<TMethod1, TMethod2>(int a, int b, int c, int d)
		{
			return a + b + c + d;
		}
	}
}