// <copyright file="ModuleWeaverTests.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Tests
{
	using System;
	using global::Fody;
	using Xunit;

	public class ModuleWeaverTests
	{
		static ModuleWeaverTests()
		{
			ModuleWeaver weavingTask = new ModuleWeaver();

			ModuleWeaverTests.TestResult = weavingTask.ExecuteTestRun("MethodCache.Fody.TestAssembly.dll");
		}

		private static TestResult TestResult { get; }

		[Fact]
		public void ValidateMyAttributeIsInjected()
		{
			Type type = ModuleWeaverTests.TestResult.Assembly.GetType("MyNamespace.MyAttribute");

			Assert.NotNull(type);
		}
	}
}