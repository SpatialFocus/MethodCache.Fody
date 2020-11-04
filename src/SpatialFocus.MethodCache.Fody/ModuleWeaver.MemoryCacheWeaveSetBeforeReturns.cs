// <copyright file="ModuleWeaver.MemoryCacheWeaveSetBeforeReturns.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System.Collections.Generic;
	using System.Linq;
	using Mono.Cecil.Cil;

	public partial class ModuleWeaver
	{
		protected static void MemoryCacheWeaveSetBeforeReturns(MethodWeavingContext methodWeavingContext)
		{
			MethodBody methodDefinitionBody = methodWeavingContext.MethodDefinition.Body;

			List<Instruction> returns = methodDefinitionBody.Instructions.Where(x => x.OpCode == OpCodes.Ret).ToList();
			ILProcessor processor = methodDefinitionBody.GetILProcessor();

			// TODO: Skip "ret" created above
			foreach (Instruction returnInstruction in returns.Skip(1))
			{
				ILProcessorContext processorContext = processor.Before(returnInstruction)
					.Append(x => x.Create(OpCodes.Stloc, methodWeavingContext.ResultVariableIndex.Value));

				processorContext = processorContext.Append(x => x.Create(OpCodes.Ldarg_0))
					.Append(x => x.Create(OpCodes.Call, methodWeavingContext.ClassWeavingContext.CacheGetterMethod))
					.Append(x => x.Create(OpCodes.Ldloc, methodWeavingContext.CacheKeyVariableIndex.Value))
					.Append(x => x.Create(OpCodes.Ldloc, methodWeavingContext.ResultVariableIndex.Value))
					.Append(x => x.Create(OpCodes.Call,
						methodWeavingContext.ClassWeavingContext.References.GetGenericSetMethod(methodWeavingContext.MethodDefinition
							.ReturnType)));

				// Not necessary, just return the return value from .Set
				////.Append(x => x.Create(OpCodes.Pop))
				////.Append(x => x.Create(OpCodes.Ldloc, index));
			}
		}
	}
}