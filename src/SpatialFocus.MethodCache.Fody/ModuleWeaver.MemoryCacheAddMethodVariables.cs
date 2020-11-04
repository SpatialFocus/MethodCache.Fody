// <copyright file="ModuleWeaver.MemoryCacheAddMethodVariables.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Mono.Cecil;
	using Mono.Cecil.Cil;

	public partial class ModuleWeaver
	{
		protected static void MemoryCacheAddMethodVariables(MethodWeavingContext methodWeavingContext)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			methodWeavingContext.MethodDefinition.Body.Variables.Add(
				new VariableDefinition(methodWeavingContext.MethodDefinition.ReturnType));
			methodWeavingContext.ResultVariableIndex = methodWeavingContext.MethodDefinition.Body.Variables.Count - 1;

			ICollection<TypeReference> tupleTypeReferences = new[] { methodWeavingContext.MethodDefinition.Module.TypeSystem.String }
				.Concat(Enumerable.Repeat(methodWeavingContext.ClassWeavingContext.References.TypeType,
					methodWeavingContext.MethodDefinition.DeclaringType.GenericParameters.Count))
				.Concat(Enumerable.Repeat(methodWeavingContext.ClassWeavingContext.References.TypeType,
					methodWeavingContext.MethodDefinition.GenericParameters.Count))
				.Concat(methodWeavingContext.MethodDefinition.Parameters.Select(x => x.ParameterType))
				.ToList();

			tupleTypeReferences.ToList().ForEach(tupleTypeReference => methodWeavingContext.CacheKeyParameterTypes.Add(tupleTypeReference));

			methodWeavingContext.CacheKeyType =
				methodWeavingContext.ClassWeavingContext.References.GetFragmentedSystemTupleType(tupleTypeReferences.ToList());

			methodWeavingContext.MethodDefinition.Body.Variables.Add(new VariableDefinition(methodWeavingContext.CacheKeyType));
			methodWeavingContext.CacheKeyVariableIndex = methodWeavingContext.MethodDefinition.Body.Variables.Count - 1;
		}
	}
}