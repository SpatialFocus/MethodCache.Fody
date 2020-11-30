// <copyright file="MemoryCacheGenericTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests
{
	using global::Fody;
	using SpatialFocus.MethodCache.Fody;
	using SpatialFocus.MethodCache.TestAssembly;
	using SpatialFocus.MethodCache.TestAssembly.Models;
	using SpatialFocus.MethodCache.Tests.Mock;
	using Xunit;

	[Collection("TestAssembly")]
	public class MemoryCacheGenericTests
	{
		static MemoryCacheGenericTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			MemoryCacheGenericTests.TestResult =
				weavingTask.ExecuteTestRun("SpatialFocus.MethodCache.TestAssembly.dll", ignoreCodes: new[] { "0x80131869" });
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void GenericClassTest1NonGenericMethod()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericTestClass<int>>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.NonGenericReturn(1);

			Assert.Equal(1, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericClassTest2GenericTClassMethod()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericTestClass<int>>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.GenericTClassReturn(1);

			Assert.Equal(1, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericMethodTest1NonGenericMethod()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericMethodTestClass>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.NonGenericReturn(1);

			Assert.Equal(1, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericMethodTest2GenericMethodInt()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericMethodTestClass>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.GenericReturn<int>(1);

			// {(SpatialFocus.MethodCache.TestAssembly.GenericTestClass.GenericReturn<T>, System.Int32, 1)}
			Assert.Equal(1, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericMethodTest3GenericMethodString()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericMethodTestClass>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.GenericReturn<string>("Test");

			Assert.Equal("Test", result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericMethodTest4GenericMethodCustomObjectFromTestAssembly()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericMethodTestClass>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.GenericReturn<CustomObject>(new CustomObject("Tester", 42));

			Assert.Equal(new CustomObject("Tester", 42), result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}

		[Fact]
		public void GenericMethodTest5GenericClassNonGenericMethod()
		{
			using MockMemoryCache mockMemoryCache = MockMemoryCache.Default;

			dynamic instance =
				TestHelpers.CreateInstance<GenericTestClass<int>>(MemoryCacheGenericTests.TestResult.Assembly, mockMemoryCache);

			dynamic result = instance.NonGenericReturn(1);

			Assert.Equal(1, result);
			Assert.Equal(1, mockMemoryCache.CountSets);
			Assert.Equal(1, mockMemoryCache.CountGets);
		}
	}
}