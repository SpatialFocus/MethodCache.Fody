// <copyright file="NoCacheAttribute.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache
{
	using System;

	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class NoCacheAttribute : Attribute
	{
		public NoCacheAttribute()
		{
		}
	}
}