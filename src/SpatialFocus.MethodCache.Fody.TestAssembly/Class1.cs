// <copyright file="Class1.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.TestAssembly
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	public class Class1
	{
		public Class1(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

		public int Method1(int a, int b)
		{
			return a + b;
		}

		public int Method2<TMethod>(int a, string b, double c, float d, object e, Type f)
		{
			return 2;
		}
	}
}