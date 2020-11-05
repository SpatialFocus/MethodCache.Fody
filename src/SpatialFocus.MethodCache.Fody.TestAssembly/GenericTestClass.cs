// <copyright file="GenericTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class GenericTestClass
	{
		public GenericTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

		public int Add(int a, int b)
		{
			return a + b;
		}

		public T GenericReturn<T>(T a)
		{
			return a;
		}
	}
}