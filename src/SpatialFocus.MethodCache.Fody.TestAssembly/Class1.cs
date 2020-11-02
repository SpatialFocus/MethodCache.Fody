// <copyright file="Class1.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.TestAssembly
{
	using System;

	public class Class1<TClass>
	{
		public void Method1()
		{
			new Tuple<string>("1");
		}

		public void Method2<TMethod>(int a, string b, double c, float d, object e, Type f)
		{
		}
	}
}