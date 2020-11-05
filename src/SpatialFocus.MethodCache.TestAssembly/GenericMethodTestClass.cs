// <copyright file="GenericMethodTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class GenericMethodTestClass
	{
		public GenericMethodTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

#pragma warning disable CA1822 // Mark members as static
		public T GenericReturn<T>(T a)
		{
			return a;
		}

		public int NonGenericReturn(int a)
		{
			return a;
		}
#pragma warning restore CA1822 // Mark members as static
	}
}