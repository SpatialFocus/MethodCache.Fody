// <copyright file="BasicTestClass.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly
{
	using System;
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

		[NoCache]
		public int GetRandomNumber(Random random)
		{
			if (random == null)
			{
				throw new ArgumentNullException(nameof(random));
			}

			return random.Next();
		}
#pragma warning restore CA1822 // Mark members as static
	}
}