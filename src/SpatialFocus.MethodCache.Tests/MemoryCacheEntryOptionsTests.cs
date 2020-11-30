// <copyright file="MemoryCacheEntryOptionsTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests
{
	using System;
	using System.Threading;
	using global::Fody;
	using Microsoft.Extensions.Caching.Memory;
	using SpatialFocus.MethodCache.Fody;
	using SpatialFocus.MethodCache.TestAssembly;
	using SpatialFocus.MethodCache.Tests.Mock;
	using Xunit;

	[Collection("TestAssembly")]
	public class MemoryCacheEntryOptionsTests
	{
		static MemoryCacheEntryOptionsTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			MemoryCacheEntryOptionsTests.TestResult =
				weavingTask.ExecuteTestRun("SpatialFocus.MethodCache.TestAssembly.dll", ignoreCodes: new[] { "0x80131869" });
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void WithAbsoluteExpirationRelativeToNowOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithAbsoluteExpirationRelativeToNowOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Equal(TimeSpan.FromSeconds(1), lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Null(lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);

			result = instance.WithAbsoluteExpirationRelativeToNowOption(1);
			Thread.Sleep(1000);
			result = instance.WithAbsoluteExpirationRelativeToNowOption(1);

			Assert.Equal(2, mockMemoryCache.CountSets);
			Assert.Equal(3, mockMemoryCache.CountGets);
		}

		[Fact]
		public void WithAllOptions()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithAllOptions(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Equal(TimeSpan.FromSeconds(1), lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Equal(TimeSpan.FromSeconds(2), lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.High, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithClassLevelOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsClassLevelTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithClassLevelOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Equal(TimeSpan.FromSeconds(1), lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Null(lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithMethodLevelNoOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsClassLevelTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithMethodLevelNoOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Null(lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Null(lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithMethodLevelOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsClassLevelTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithMethodLevelOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Null(lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Equal(TimeSpan.FromSeconds(2), lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithNoEntryOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithNoOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Null(lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Null(lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithPriorityOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithPriorityOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Null(lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Null(lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.High, lastCreatedCacheEntry.Priority);
		}

		[Fact]
		public void WithSlidingExpirationOption()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<MemoryCacheEntryOptionsTestClass>(MemoryCacheEntryOptionsTests.TestResult.Assembly,
					mockMemoryCache);

			dynamic result = instance.WithSlidingExpirationOption(1);
			ICacheEntry lastCreatedCacheEntry = mockMemoryCache.LastCreatedCacheEntry;

			Assert.Null(lastCreatedCacheEntry.AbsoluteExpirationRelativeToNow);
			Assert.Equal(TimeSpan.FromSeconds(1), lastCreatedCacheEntry.SlidingExpiration);
			Assert.Equal(CacheItemPriority.Normal, lastCreatedCacheEntry.Priority);

			Thread.Sleep(500);
			result = instance.WithSlidingExpirationOption(1);
			Thread.Sleep(500);
			result = instance.WithSlidingExpirationOption(1);
			Thread.Sleep(1000);
			result = instance.WithSlidingExpirationOption(1);

			Assert.Equal(2, mockMemoryCache.CountSets);
			Assert.Equal(4, mockMemoryCache.CountGets);
		}
	}
}