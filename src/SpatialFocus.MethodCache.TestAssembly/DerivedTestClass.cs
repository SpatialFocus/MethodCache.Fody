// <copyright file="DerivedTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class DerivedTestClass : BaseClass
	{
		public DerivedTestClass(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

#pragma warning disable CA1822 // Mark members as static
		public int Add(int a, int b)
		{
			return a + b;
		}
#pragma warning restore CA1822 // Mark members as static
	}

#pragma warning disable SA1402 // File may only contain a single type
	public class BaseClass
	{
		public BaseClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }
	}
#pragma warning restore SA1402 // File may only contain a single type
}