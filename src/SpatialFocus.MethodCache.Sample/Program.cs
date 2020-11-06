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
		private static void Main()
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging(builder => builder.AddConsole());
			ServiceProvider buildServiceProvider = serviceCollection.BuildServiceProvider();

			using MyMemoryCache memoryCacheBasicSample = new MyMemoryCache(new MemoryCache(new MemoryCacheOptions()),
				buildServiceProvider.GetRequiredService<ILogger<MyMemoryCache>>());
			BasicSample basicSample = new BasicSample(memoryCacheBasicSample);

			basicSample.Add(1, 2);
			basicSample.Add(1, 2);
			basicSample.Add(3, 4);

			using MyMemoryCache memoryCacheGenericSample = new MyMemoryCache(new MemoryCache(new MemoryCacheOptions()),
				buildServiceProvider.GetRequiredService<ILogger<MyMemoryCache>>());
			GenericSample<int, object> genericSample = new GenericSample<int, object>(memoryCacheGenericSample);

			genericSample.Add<string, Attribute>(1, 2, 3, 4);
			genericSample.Add<string, Attribute>(1, 2, 3, 4);
			genericSample.Add<string, Attribute>(5, 6, 7, 8);
		}
	}
}