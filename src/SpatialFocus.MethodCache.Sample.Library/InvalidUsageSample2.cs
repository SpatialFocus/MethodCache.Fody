// <copyright file="InvalidUsageSample2.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample.Library
{
	// MethodCache.Fody will emit warning messages for invalid use of Cache attribute
	[Cache]
	public class InvalidUsageSample2
	{
#pragma warning disable CA1822 // Mark members as static
		public void A()
		{
		}
#pragma warning restore CA1822 // Mark members as static
	}
}