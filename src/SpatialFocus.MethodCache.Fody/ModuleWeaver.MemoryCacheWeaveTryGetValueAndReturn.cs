// <copyright file="ModuleWeaver.MemoryCacheWeaveTryGetValueAndReturn.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using Mono.Cecil.Cil;

	public partial class ModuleWeaver
	{
		protected static void MemoryCacheWeaveTryGetValueAndReturn(MethodWeavingContext methodWeavingContext,
			ILProcessorContext processorContext)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			if (processorContext == null)
			{
				throw new ArgumentNullException(nameof(processorContext));
			}

			processorContext = processorContext.Append(x => x.Create(OpCodes.Ldarg_0))
				.Append(x => x.Create(OpCodes.Call, methodWeavingContext.ClassWeavingContext.CacheGetterMethod))
				.Append(x => x.Create(OpCodes.Ldloc, methodWeavingContext.CacheKeyVariableIndex.Value))
				.Append(x => x.Create(OpCodes.Ldloca, methodWeavingContext.ResultVariableIndex.Value))
				.Append(x => x.Create(OpCodes.Call,
					methodWeavingContext.ClassWeavingContext.References.GetTryGetValue(methodWeavingContext.MethodDefinition.ReturnType)));

			Instruction instructionNext = processorContext.CurrentInstruction.Next;

			processorContext.Append(x => x.Create(OpCodes.Brfalse, instructionNext))
				.Append(x => x.Create(OpCodes.Ldloc, methodWeavingContext.ResultVariableIndex.Value))
				.Append(x => x.Create(OpCodes.Ret));
		}
	}
}