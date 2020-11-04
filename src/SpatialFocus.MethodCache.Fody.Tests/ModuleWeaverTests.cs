// <copyright file="ModuleWeaverTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Tests
{
	using System;
	using global::Fody;
	using SpatialFocus.MethodCache.Fody.Tests.Mock;
	using Xunit;

	public class ModuleWeaverTests
	{
		static ModuleWeaverTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			ModuleWeaverTests.TestResult =
				weavingTask.ExecuteTestRun("SpatialFocus.MethodCache.Fody.TestAssembly.dll", ignoreCodes: new[] { "0x80131869" });
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void BasicTest1CreateAndGet()
		{
			using MockMemoryCache mockMemoryCache = new MockMemoryCache();

			Type type = ModuleWeaverTests.TestResult.Assembly.GetType("SpatialFocus.MethodCache.Fody.TestAssembly.BasicTestClass");
			dynamic instance = (dynamic)Activator.CreateInstance(type, mockMemoryCache);

			dynamic result = instance.Add(1, 2);

			Assert.Equal(3, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest2CreateAndGet2()
		{
			using MockMemoryCache mockMemoryCache = new MockMemoryCache();

			Type type = ModuleWeaverTests.TestResult.Assembly.GetType("SpatialFocus.MethodCache.Fody.TestAssembly.BasicTestClass");
			dynamic instance = (dynamic)Activator.CreateInstance(type, mockMemoryCache);

			dynamic result = instance.Add(1, 2);
			result = instance.Add(1, 2);

			Assert.Equal(3, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(2, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest3Create2AndGet2()
		{
			using MockMemoryCache mockMemoryCache = new MockMemoryCache();

			Type type = ModuleWeaverTests.TestResult.Assembly.GetType("SpatialFocus.MethodCache.Fody.TestAssembly.BasicTestClass");
			dynamic instance = (dynamic)Activator.CreateInstance(type, mockMemoryCache);

			dynamic result1 = instance.Add(1, 2);
			dynamic result2 = instance.Add(2, 2);

			Assert.Equal(3, result1);
			Assert.Equal(4, result2);
			Assert.Equal(2, mockMemoryCache.CountSets);
			Assert.Equal(2, mockMemoryCache.CountGets);
		}
	}
}