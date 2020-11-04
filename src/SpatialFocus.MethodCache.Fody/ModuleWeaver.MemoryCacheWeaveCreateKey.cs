// <copyright file="ModuleWeaver.MemoryCacheWeaveCreateKey.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Linq;
	using System.Text;
	using Mono.Cecil;
	using Mono.Cecil.Cil;

	public partial class ModuleWeaver
	{
		protected static ILProcessorContext CreateTupleConstructorCalls(MethodWeavingContext methodWeavingContext,
			ILProcessorContext processorContext, GenericInstanceType tupleType, int remainingTupleTypeReferencesCount)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			if (processorContext == null)
			{
				throw new ArgumentNullException(nameof(processorContext));
			}

			if (tupleType == null)
			{
				throw new ArgumentNullException(nameof(tupleType));
			}

			TypeReference[] typeReferences = ((GenericInstanceType)methodWeavingContext.CacheKeyType).GenericArguments.Cast<TypeReference>()
				.ToArray();

			if (typeReferences.Length == 8 && remainingTupleTypeReferencesCount > 8)
			{
				remainingTupleTypeReferencesCount -= 7;
				return ModuleWeaver.CreateTupleConstructorCalls(methodWeavingContext, processorContext,
					(GenericInstanceType)typeReferences.Last(), remainingTupleTypeReferencesCount);
			}

			MethodReference tupleConstructor =
				methodWeavingContext.ClassWeavingContext.References.GetSystemTupleConstructor(typeReferences);

			return processorContext.Append(x => x.Create(OpCodes.Newobj, tupleConstructor));
		}

		protected static string MemoryCacheCreateCacheKeyMethodName(MethodDefinition methodDefinition)
		{
			if (methodDefinition == null)
			{
				throw new ArgumentNullException(nameof(methodDefinition));
			}

			StringBuilder builder = new StringBuilder();

			TypeDefinition declaringType = methodDefinition.DeclaringType;

			if (declaringType.HasGenericParameters)
			{
				builder.Append(declaringType.FullName.Substring(0, declaringType.FullName.IndexOf('`')));
			}
			else
			{
				builder.Append(declaringType.FullName);
			}

			if (declaringType.GenericParameters.Any())
			{
				builder.Append('<');
				builder.Append(string.Join(", ", declaringType.GenericParameters.Select(x => x.FullName)));
				builder.Append('>');
			}

			builder.Append('.');
			builder.Append(methodDefinition.Name);

			if (methodDefinition.GenericParameters.Any())
			{
				builder.Append('<');
				builder.Append(string.Join(", ", methodDefinition.GenericParameters.Select(x => x.FullName)));
				builder.Append('>');
			}

			return builder.ToString();
		}

		protected static ILProcessorContext MemoryCacheWeaveCreateKey(MethodWeavingContext methodWeavingContext)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			ILProcessorContext processorContext = methodWeavingContext.MethodDefinition.Body.GetILProcessor().Start();

			string methodName = ModuleWeaver.MemoryCacheCreateCacheKeyMethodName(methodWeavingContext.MethodDefinition);
			processorContext = processorContext.Append(x => x.Create(OpCodes.Ldstr, methodName));

			foreach (GenericParameter genericParameter in methodWeavingContext.ClassWeavingContext.TypeDefinition.GenericParameters)
			{
				processorContext = processorContext.Append(x => x.Create(OpCodes.Ldtoken, genericParameter))
					.Append(x => x.Create(OpCodes.Call, methodWeavingContext.ClassWeavingContext.References.GetTypeFromHandleMethod));
			}

			foreach (GenericParameter genericParameter in methodWeavingContext.MethodDefinition.GenericParameters)
			{
				processorContext = processorContext.Append(x => x.Create(OpCodes.Ldtoken, genericParameter))
					.Append(x => x.Create(OpCodes.Call, methodWeavingContext.ClassWeavingContext.References.GetTypeFromHandleMethod));
			}

			for (int i = 0; i < methodWeavingContext.MethodDefinition.Parameters.Count; i++)
			{
				int value = i;

				processorContext = processorContext.Append(x => x.Create(OpCodes.Ldarg, value + 1));
			}

			return ModuleWeaver
				.CreateTupleConstructorCalls(methodWeavingContext, processorContext, (GenericInstanceType)methodWeavingContext.CacheKeyType,
					methodWeavingContext.CacheKeyParameterTypes.Count)
				.Append(x => x.Create(OpCodes.Stloc, methodWeavingContext.CacheKeyVariableIndex.Value));
		}
	}
}