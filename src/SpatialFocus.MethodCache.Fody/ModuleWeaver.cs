// <copyright file="ModuleWeaver.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System.Collections.Generic;
	using System.Linq;
	using global::Fody;
	using Mono.Cecil;
	using SpatialFocus.MethodCache.Fody.Extensions;

	public partial class ModuleWeaver : BaseModuleWeaver
	{
		public override bool ShouldCleanReference => false;

		public override void Execute()
		{
			References references = Fody.References.Init(this);

			foreach (WeavingCandidate weavingCandidate in ModuleDefinition.GetWeavingCandidates(references))
			{
				if (weavingCandidate.ClassDefinition.HasCacheAttribute(references) && !weavingCandidate.MethodDefinitions
					.Where(x => !x.HasNoCacheAttribute(references))
					.Any(x => x.IsEligibleForWeaving(references)))
				{
					WriteWarning(
						$"Class {weavingCandidate.ClassDefinition.Resolve().FullName} contains [Cache] attribute but does not contain eligible methods for caching");
					continue;
				}

				if (!weavingCandidate.ClassDefinition.IsEligibleForWeaving(references))
				{
					WriteWarning(
						$"Class {weavingCandidate.ClassDefinition.Name} contains [Cache] attribute but does not contain a single non-inherited property implementing IMemoryCache interface");
					continue;
				}

				ClassWeavingContext classWeavingContext = new ClassWeavingContext(weavingCandidate.ClassDefinition, references);
				classWeavingContext.CacheGetterMethod = MemoryCache.GetCacheGetterMethod(classWeavingContext);

				foreach (MethodDefinition methodDefinition in weavingCandidate.MethodDefinitions)
				{
					if (!methodDefinition.IsEligibleForWeaving(references))
					{
						// Show warning if test was marked explicitly
						if (methodDefinition.HasCacheAttribute(references))
						{
							WriteWarning($"Method {methodDefinition.FullName} contains [Cache] attribute but is not eligible for weaving");
							break;
						}

						continue;
					}

					if (methodDefinition.HasNoCacheAttribute(references))
					{
						continue;
					}

					MethodWeavingContext methodWeavingContext = new MethodWeavingContext(classWeavingContext, methodDefinition);

					MemoryCache.AddMethodVariables(methodWeavingContext);
					ILProcessorContext processorContext = MemoryCache.WeaveCreateKey(methodWeavingContext);
					MemoryCache.WeaveTryGetValueAndReturn(methodWeavingContext, processorContext);
					MemoryCache.WeaveSetBeforeReturns(methodWeavingContext);
				}
			}
		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
			yield return "Microsoft.Extensions.Caching.Abstractions";
			yield return "SpatialFocus.MethodCache";
		}
	}
}