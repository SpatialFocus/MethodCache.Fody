// <copyright file="MemoryCacheKeyTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class MemoryCacheKeyTestClass
	{
		public MemoryCacheKeyTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1822 // Mark members as static
		public int Parameterless()
		{
			return 1;
		}

		public int With6Parameters(int a1, int a2, int a3, int a4, int a5, int a6)
		{
			return a1 + a2 + a3 + a4 + a5 + a6;
		}

		public int With7Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7;
		}

		public int With8Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8;
		}

		public int With9Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9;
		}

		public int With13Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12,
			int a13)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13;
		}

		public int With14Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12,
			int a13, int a14)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14;
		}

		public int With15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12,
			int a13, int a14, int a15)
		{
			return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15;
		}

		public int WithParameter(int a)
		{
			return a;
		}
#pragma warning restore CA1822 // Mark members as static
	}
}