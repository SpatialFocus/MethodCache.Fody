// <copyright file="InvalidUsageSample3.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	// MethodCache.Fody will emit warning messages for invalid use of Cache attribute
	[Cache]
	public class InvalidUsageSample3
	{
#pragma warning disable CA1822 // Mark members as static
		public int A() => 0;
#pragma warning restore CA1822 // Mark members as static
	}
}