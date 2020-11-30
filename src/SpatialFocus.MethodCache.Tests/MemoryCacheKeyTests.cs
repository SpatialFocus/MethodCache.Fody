// <copyright file="MemoryCacheKeyTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests
{
	using System.Runtime.CompilerServices;
	using global::Fody;
	using SpatialFocus.MethodCache.Fody;
	using SpatialFocus.MethodCache.TestAssembly;
	using SpatialFocus.MethodCache.Tests.Mock;
	using Xunit;

	[Collection("TestAssembly")]
	public class MemoryCacheKeyTests
	{
		static MemoryCacheKeyTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			MemoryCacheKeyTests.TestResult =
				weavingTask.ExecuteTestRun("SpatialFocus.MethodCache.TestAssembly.dll", ignoreCodes: new[] { "0x80131869" });
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void CacheKeyTest1Parameterless()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.Parameterless();

			Assert.Equal(1, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(1, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.Parameterless", key[0]);
		}

		[Fact]
		public void CacheKeyTest2WithParameter()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.WithParameter(1);

			Assert.Equal(1, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(2, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.WithParameter", key[0]);
			Assert.Equal(1, key[1]);
		}

		[Fact]
		public void CacheKeyTest3With6Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With6Parameters(1, 2, 3, 4, 5, 6);

			Assert.Equal(21, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(7, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With6Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
		}

		[Fact]
		public void CacheKeyTest4With7Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With7Parameters(1, 2, 3, 4, 5, 6, 7);

			Assert.Equal(28, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(8, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With7Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
		}

		[Fact]
		public void CacheKeyTest4With8Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With8Parameters(1, 2, 3, 4, 5, 6, 7, 8);

			Assert.Equal(36, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(9, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With8Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
			Assert.Equal(8, key[8]);
		}

		[Fact]
		public void CacheKeyTest5With9Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With9Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9);

			Assert.Equal(45, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(10, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With9Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
			Assert.Equal(8, key[8]);
			Assert.Equal(9, key[9]);
		}

		[Fact]
		public void CacheKeyTest6With13Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With13Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

			Assert.Equal(91, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(14, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With13Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
			Assert.Equal(8, key[8]);
			Assert.Equal(9, key[9]);
			Assert.Equal(10, key[10]);
			Assert.Equal(11, key[11]);
			Assert.Equal(12, key[12]);
			Assert.Equal(13, key[13]);
		}

		[Fact]
		public void CacheKeyTest7With14Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With14Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

			Assert.Equal(105, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(15, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With14Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
			Assert.Equal(8, key[8]);
			Assert.Equal(9, key[9]);
			Assert.Equal(10, key[10]);
			Assert.Equal(11, key[11]);
			Assert.Equal(12, key[12]);
			Assert.Equal(13, key[13]);
			Assert.Equal(14, key[14]);
		}

		[Fact]
		public void CacheKeyTest8With15Parameters()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheKeyTestClass>(MemoryCacheKeyTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.With15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

			Assert.Equal(120, result);

			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);

			ITuple key = (ITuple)mockMemoryCache.LastCreatedEntryKey;
			Assert.Equal(16, key.Length);
			Assert.Equal("SpatialFocus.MethodCache.TestAssembly.MemoryCacheKeyTestClass.With15Parameters", key[0]);
			Assert.Equal(1, key[1]);
			Assert.Equal(2, key[2]);
			Assert.Equal(3, key[3]);
			Assert.Equal(4, key[4]);
			Assert.Equal(5, key[5]);
			Assert.Equal(6, key[6]);
			Assert.Equal(7, key[7]);
			Assert.Equal(8, key[8]);
			Assert.Equal(9, key[9]);
			Assert.Equal(10, key[10]);
			Assert.Equal(11, key[11]);
			Assert.Equal(12, key[12]);
			Assert.Equal(13, key[13]);
			Assert.Equal(14, key[14]);
			Assert.Equal(15, key[15]);
		}
	}
}