// <copyright file="Program.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample
{
	using System;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using SpatialFocus.MethodCache.Sample.Library;

	internal class Program
	{
		private static void Main(string[] args)
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging(builder => builder.AddConsole());
			ServiceProvider buildServiceProvider = serviceCollection.BuildServiceProvider();

			BasicSample basicSample = new BasicSample(new MyMemoryCache(new MemoryCache(new MemoryCacheOptions()),
				buildServiceProvider.GetRequiredService<ILogger<MyMemoryCache>>()));

			basicSample.Add(1, 2);
			basicSample.Add(1, 2);
			basicSample.Add(3, 4);

			GenericSample<int, object> genericSample = new GenericSample<int, object>(
				new MyMemoryCache(new MemoryCache(new MemoryCacheOptions()),
					buildServiceProvider.GetRequiredService<ILogger<MyMemoryCache>>()));

			genericSample.Add<string, Attribute>(1, 2, 3, 4);
			genericSample.Add<string, Attribute>(1, 2, 3, 4);
			genericSample.Add<string, Attribute>(5, 6, 7, 8);
		}
	}
}