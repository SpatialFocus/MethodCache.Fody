// <copyright file="InvalidUsageSample4.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	using Microsoft.Extensions.Caching.Memory;

	// MethodCache.Fody will emit warning messages for invalid use of Cache attribute
	public class InvalidUsageSample4
	{
		public InvalidUsageSample4(IMemoryCache memoryCache)
		{
			MemoryCache = memoryCache;
		}

		protected IMemoryCache MemoryCache { get; }

		[Cache]
#pragma warning disable CA1822 // Mark members as static
		public void A()
		{
		}
#pragma warning restore CA1822 // Mark members as static
	}
}