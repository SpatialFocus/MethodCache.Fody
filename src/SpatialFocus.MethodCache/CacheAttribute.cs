// <copyright file="CacheAttribute.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

using System;

[assembly: CLSCompliant(true)]

namespace SpatialFocus.MethodCache
{
	using System;

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute()
		{
		}
	}
}