// <copyright file="GenericTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using Microsoft.Extensions.Caching.Memory;

	[Cache]
	public class GenericTestClass<TClass>
	{
		public GenericTestClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }

		public TClass GenericTClassReturn(TClass a)
		{
			return a;
		}

		public TClass GenericTClassTMethodReturn<TMethod>(TMethod a)
		{
			return default(TClass);
		}

		public TMethod GenericTMethodReturn<TMethod>(TMethod a)
		{
			return a;
		}

		public int NonGenericReturn(int a)
		{
			return a;
		}
	}
}