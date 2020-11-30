// <copyright file="CacheAttribute.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

using System;

[assembly: CLSCompliant(false)]

namespace SpatialFocus.MethodCache
{
	using System;
	using Microsoft.Extensions.Caching.Memory;

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute()
		{
		}

		public double AbsoluteExpirationRelativeToNow { get; set; }

		public CacheItemPriority Priority { get; set; }

		public double SlidingExpiration { get; set; }
	}
}