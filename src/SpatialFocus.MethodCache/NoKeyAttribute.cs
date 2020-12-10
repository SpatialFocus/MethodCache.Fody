// <copyright file="NoKeyAttribute.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache
{
	using System;

	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class NoKeyAttribute : Attribute
	{
		public NoKeyAttribute()
		{
		}
	}
}