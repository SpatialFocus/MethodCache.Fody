// <copyright file="MemoryCacheEntryOptionsClassLevelTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache(AbsoluteExpirationRelativeToNow = 1)]
	public class MemoryCacheEntryOptionsClassLevelTestClass
	{
		public MemoryCacheEntryOptionsClassLevelTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0060 // Remove unused parameter
		[Cache]
		public int WithMethodLevelNoOption(int a)
		{
			return a;
		}

		public int WithClassLevelOption(int a)
		{
			return a;
		}

		[Cache(SlidingExpiration = 2)]
		public int WithMethodLevelOption(int a)
		{
			return a;
		}
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0060 // Remove unused parameter
	}
}