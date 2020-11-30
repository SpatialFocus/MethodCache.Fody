// <copyright file="MemoryCacheEntryOptionsTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	public class MemoryCacheEntryOptionsTestClass
	{
		public MemoryCacheEntryOptionsTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0060 // Remove unused parameter
		[Cache]
		public int WithNoOption(int a)
		{
			return a;
		}

		[Cache(AbsoluteExpirationRelativeToNow = 1)]
		public int WithAbsoluteExpirationRelativeToNowOption(int a)
		{
			return a;
		}

		[Cache(SlidingExpiration = 1)]
		public int WithSlidingExpirationOption(int a)
		{
			return a;
		}

		[Cache(Priority = CacheItemPriority.High)]
		public int WithPriorityOption(int a)
		{
			return a;
		}

		[Cache(AbsoluteExpirationRelativeToNow = 1, SlidingExpiration = 2, Priority = CacheItemPriority.High)]
		public int WithAllOptions(int a)
		{
			return a;
		}
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0060 // Remove unused parameter
	}
}