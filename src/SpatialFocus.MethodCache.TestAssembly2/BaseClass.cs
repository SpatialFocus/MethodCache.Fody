// <copyright file="BaseClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly2
{
	using Microsoft.Extensions.Caching.Memory;

	public class BaseClass
	{
		public BaseClass(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		public IMemoryCache MemoryCache { get; }
	}
}