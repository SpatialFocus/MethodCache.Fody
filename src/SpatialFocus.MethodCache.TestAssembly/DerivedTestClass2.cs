// <copyright file="DerivedTestClass2.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class DerivedTestClass2 : TestAssembly2.BaseClass
	{
		public DerivedTestClass2(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

#pragma warning disable CA1822 // Mark members as static
		public int Add(int a, int b)
		{
			return a + b;
		}
#pragma warning restore CA1822 // Mark members as static
	}
}