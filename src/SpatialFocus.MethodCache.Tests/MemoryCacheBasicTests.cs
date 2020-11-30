// <copyright file="MemoryCacheBasicTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests
{
	using global::Fody;
	using SpatialFocus.MethodCache.Fody;
	using SpatialFocus.MethodCache.TestAssembly;
	using SpatialFocus.MethodCache.Tests.Mock;
	using Xunit;

	[Collection("TestAssembly")]
	public class MemoryCacheBasicTests
	{
		static MemoryCacheBasicTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			MemoryCacheBasicTests.TestResult =
				weavingTask.ExecuteTestRun("SpatialFocus.MethodCache.TestAssembly.dll", ignoreCodes: new[] { "0x80131869" });
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void BasicTest1CreateAndGet()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance = TestHelpers.CreateInstance<BasicTestClass>(MemoryCacheBasicTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.Add(1, 2);

			Assert.Equal(3, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest2CreateAndGet2()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance = TestHelpers.CreateInstance<BasicTestClass>(MemoryCacheBasicTests.TestResult.Assembly, mockMemoryCache);

			_ = instance.Add(1, 2);
			dynamic result = instance.Add(1, 2);

			Assert.Equal(3, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(2, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest3Create2AndGet2()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance = TestHelpers.CreateInstance<BasicTestClass>(MemoryCacheBasicTests.TestResult.Assembly, mockMemoryCache);

			dynamic result1 = instance.Add(1, 2);
			dynamic result2 = instance.Add(2, 2);

			Assert.Equal(3, result1);
			Assert.Equal(4, result2);
			Assert.Equal(2, mockMemoryCache.CountSets);
			Assert.Equal(2, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest4NoCache()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance = TestHelpers.CreateInstance<BasicTestClass>(MemoryCacheBasicTests.TestResult.Assembly, mockMemoryCache);

			dynamic result1 = instance.UncachedAdd(1, 2);
			dynamic result2 = instance.UncachedAdd(2, 2);

			Assert.Equal(3, result1);
			Assert.Equal(4, result2);
			Assert.Equal(0, mockMemoryCache.CountSets);
			Assert.Equal(0, mockMemoryCache.CountGets);
		}

		[Fact]
		public void BasicTest5DerivedClass()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance = TestHelpers.CreateInstance<DerivedTestClass>(MemoryCacheBasicTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.Add(1, 2);

			Assert.Equal(3, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}
	}
}