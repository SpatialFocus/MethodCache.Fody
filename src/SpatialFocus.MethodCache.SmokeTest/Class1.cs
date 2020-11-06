// <copyright file="Class1.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

using System;

[assembly: CLSCompliant(false)]

namespace SpatialFocus.MethodCache.SmokeTest
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class Class1
	{
		public Class1(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0060 // Remove unused parameter
		public int Method1(int a, int b)
		{
			return a + b;
		}

		public int Method2<TMethod>(int a, string b, double c, float d, object e, Type f)
		{
			return 2;
		}
#pragma warning restore CA1801 // Review unused parameters
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0060 // Remove unused parameter
	}
}