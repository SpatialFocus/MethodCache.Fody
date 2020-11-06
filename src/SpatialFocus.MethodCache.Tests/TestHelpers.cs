// <copyright file="TestHelpers.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests
{
	using System;
	using System.Reflection;

	public static class TestHelpers
	{
		public static dynamic CreateInstance<T>(Assembly assembly, object parameter)
		{
			return assembly != null ? Activator.CreateInstance(TestHelpers.CreateType<T>(assembly), parameter) : null;
		}

		private static Type CreateType<T>(Assembly assembly)
		{
			return assembly.GetType(typeof(T).FullName ?? string.Empty);
		}
	}
}