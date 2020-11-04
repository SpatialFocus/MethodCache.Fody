// <copyright file="ModuleWeaver.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
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

			// TODO: Extract to method
			var elementsToCache = ModuleDefinition.Types.Select(type =>
				{
					if (type.HasCacheAttribute(references))
					{
						return new { Type = type, Methods = type.Methods.ToList() };
					}

					return new { Type = type, Methods = type.Methods.Where(method => method.HasCacheAttribute(references)).ToList(), };
				})
				.Where(x => x.Methods.Any())
				.ToList();

			foreach (var elementToCache in elementsToCache)
			{
				if (!elementToCache.Type.IsEligibleForWeaving(references))
				{
					// TODO: Create warning
					continue;
				}

				ClassWeavingContext classWeavingContext = new ClassWeavingContext(elementToCache.Type, references);
				classWeavingContext.CacheGetterMethod = MemoryCache.GetCacheGetterMethod(classWeavingContext);

				foreach (MethodDefinition methodDefinition in elementToCache.Methods)
				{
					if (!methodDefinition.IsEligibleForWeaving(references))
					{
						// TODO: Create warning if marked explicit for weaving
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