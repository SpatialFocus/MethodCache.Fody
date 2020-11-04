// <copyright file="ModuleWeaver.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Fody;
	using Mono.Cecil;

	public partial class ModuleWeaver : BaseModuleWeaver
	{
		public override bool ShouldCleanReference => true;

		public override void Execute()
		{
			References references = Fody.References.Init(this);

			foreach (TypeDefinition typeDefinition in ModuleDefinition.Types.Where(x =>
				x.Name.StartsWith("Class1", StringComparison.Ordinal)))
			{
				ClassWeavingContext classWeavingContext = new ClassWeavingContext(typeDefinition, references);

				classWeavingContext.CacheGetterMethod = ModuleWeaver.GetCacheGetterMethod(classWeavingContext);

				foreach (MethodDefinition methodDefinition in ModuleDefinition.Types.SelectMany(x => x.Methods))
				{
					bool isSpecialName = methodDefinition.IsSpecialName || methodDefinition.IsGetter || methodDefinition.IsSetter ||
						methodDefinition.IsConstructor;

					// TODO: Check resolve bla bla
					//bool isCompilerGenerated = definition.CustomAttributes.Any(x => x.)

					if (isSpecialName)
					{
						continue;
					}

					MethodWeavingContext methodWeavingContext = new MethodWeavingContext(classWeavingContext, methodDefinition);

					ModuleWeaver.MemoryCacheAddMethodVariables(methodWeavingContext);
					ILProcessorContext processorContext = ModuleWeaver.MemoryCacheWeaveCreateKey(methodWeavingContext);
					ModuleWeaver.MemoryCacheWeaveTryGetValueAndReturn(methodWeavingContext, processorContext);
					ModuleWeaver.MemoryCacheWeaveSetBeforeReturns(methodWeavingContext);
				}
			}
		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
			yield return "Microsoft.Extensions.Caching.Abstractions";
		}
	}
}