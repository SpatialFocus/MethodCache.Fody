// <copyright file="TestHelpers.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Tests
{
	using System;
	using System.Reflection;

	public class TestHelpers
	{
		public static dynamic CreateInstance<T>(Assembly assembly)
		{
			return Activator.CreateInstance(TestHelpers.CreateType<T>(assembly));
		}

		public static dynamic CreateInstance<T>(Assembly assembly, object parameter)
		{
			return Activator.CreateInstance(TestHelpers.CreateType<T>(assembly), parameter);
		}

		public static dynamic CreateInstance<T>(Assembly assembly, object[] parameters)
		{
			return Activator.CreateInstance(TestHelpers.CreateType<T>(assembly), parameters);
		}

		public static Type CreateType<T>(Assembly assembly)
		{
			return assembly.GetType(typeof(T).FullName);
		}
	}
}