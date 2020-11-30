// <copyright file="BasicTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class BasicTestClass
	{
		public BasicTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1822 // Mark members as static
		public int Add(int a, int b)
		{
			return a + b;
		}

		public int AddAndSubtract(int a, int b, out int difference)
		{
			difference = a - b;
			return a + b;
		}

		[NoCache]
		public int UncachedAdd(int a, int b)
		{
			return a + b;
		}
#pragma warning restore CA1822 // Mark members as static
	}
}