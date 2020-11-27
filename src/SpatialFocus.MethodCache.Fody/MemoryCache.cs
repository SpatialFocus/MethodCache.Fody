// <copyright file="MemoryCache.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using global::Fody;
	using Mono.Cecil;
	using Mono.Cecil.Cil;
	using SpatialFocus.MethodCache.Fody.Extensions;

	public static class MemoryCache
	{
		public static void AddMethodVariables(MethodWeavingContext methodWeavingContext)
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

		public static MethodReference GetCacheGetterMethod(ClassWeavingContext classWeavingContext)
		{
			if (classWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(classWeavingContext));
			}

			// TODO: Check if this can be inherited, extract to class level
			List<PropertyDefinition> propertyDefinitions =
				classWeavingContext.TypeDefinition.TryGetCacheGetterProperty(classWeavingContext.References);

			// TODO: Also check fields
			if (propertyDefinitions == null || propertyDefinitions.Count != 1)
			{
				throw new WeavingException("Cache Property not found or multiple properties found.");
			}

			MethodDefinition methodDefinition = propertyDefinitions.Single().GetMethod;

			if (methodDefinition.DeclaringType.GenericParameters.Any())
			{
				return methodDefinition.MakeHostInstanceGeneric(methodDefinition.DeclaringType.GenericParameters.Cast<TypeReference>()
					.ToArray());
			}

			return methodDefinition;
		}

		public static ILProcessorContext WeaveCreateKey(MethodWeavingContext methodWeavingContext)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			ILProcessorContext processorContext = methodWeavingContext.MethodDefinition.Body.GetILProcessor().Start();

			string methodName = MemoryCache.CreateCacheKeyMethodName(methodWeavingContext.MethodDefinition);
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

			return MemoryCache
				.CreateTupleConstructorCalls(methodWeavingContext, processorContext, (GenericInstanceType)methodWeavingContext.CacheKeyType)
				.Append(x => x.Create(OpCodes.Stloc, methodWeavingContext.CacheKeyVariableIndex.Value));
		}

		public static void WeaveSetBeforeReturns(MethodWeavingContext methodWeavingContext)
		{
			if (methodWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(methodWeavingContext));
			}

			CustomAttribute cacheAttribute =
				methodWeavingContext.MethodDefinition.TryGetCacheAttribute(methodWeavingContext.ClassWeavingContext.References);

			if (cacheAttribute == null)
			{
				cacheAttribute =
					methodWeavingContext.ClassWeavingContext.TypeDefinition.TryGetCacheAttribute(methodWeavingContext.ClassWeavingContext
						.References);
			}

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
					.Append(x => x.Create(OpCodes.Ldloc, methodWeavingContext.ResultVariableIndex.Value));

				if (cacheAttribute.Properties.Any())
				{
					processorContext = processorContext.Append(x => x.Create(OpCodes.Newobj,
						methodWeavingContext.ClassWeavingContext.References.MemoryCacheEntryOptionsConstructor));

					foreach (CustomAttributeNamedArgument customAttributeNamedArgument in cacheAttribute.Properties)
					{
						switch (customAttributeNamedArgument.Name)
						{
							case "AbsoluteExpirationRelativeToNow":
								processorContext = processorContext.Append(x => x.Create(OpCodes.Dup))
									.Append(x => x.Create(OpCodes.Ldc_R8, (double)customAttributeNamedArgument.Argument.Value))
									.Append(x => x.Create(OpCodes.Call,
										methodWeavingContext.ClassWeavingContext.References.TimeSpanFromSecondsMethod))
									.Append(x => x.Create(OpCodes.Newobj,
										methodWeavingContext.ClassWeavingContext.References.NullableTimeSpanConstructor))
									.Append(x => x.Create(OpCodes.Callvirt,
										methodWeavingContext.ClassWeavingContext.References.MemoryCacheEntryOptionsAbsoluteExpirationRelativeToNowSetter));
								break;

							case "SlidingExpiration":
								processorContext = processorContext.Append(x => x.Create(OpCodes.Dup))
									.Append(x => x.Create(OpCodes.Ldc_R8, (double)customAttributeNamedArgument.Argument.Value))
									.Append(x => x.Create(OpCodes.Call,
										methodWeavingContext.ClassWeavingContext.References.TimeSpanFromSecondsMethod))
									.Append(x => x.Create(OpCodes.Newobj,
										methodWeavingContext.ClassWeavingContext.References.NullableTimeSpanConstructor))
									.Append(x => x.Create(OpCodes.Callvirt,
										methodWeavingContext.ClassWeavingContext.References.MemoryCacheEntryOptionsSlidingExpirationSetter));
								break;

							case "Priority":
								processorContext = processorContext.Append(x => x.Create(OpCodes.Dup))
									.Append(x => x.Create(OpCodes.Ldc_I4, (int)customAttributeNamedArgument.Argument.Value))
									.Append(x => x.Create(OpCodes.Callvirt,
										methodWeavingContext.ClassWeavingContext.References.MemoryCacheEntryOptionsPrioritySetter));
								break;
						}
					}

					processorContext = processorContext.Append(x => x.Create(OpCodes.Call,
						methodWeavingContext.ClassWeavingContext.References.GetGenericSetMethodWithMemoryCacheEntryOptions(
							methodWeavingContext.MethodDefinition.ReturnType)));
				}
				else
				{
					processorContext = processorContext.Append(x => x.Create(OpCodes.Call,
						methodWeavingContext.ClassWeavingContext.References.GetGenericSetMethod(methodWeavingContext.MethodDefinition
							.ReturnType)));
				}

				// Not necessary, just return the return value from .Set
				////.Append(x => x.Create(OpCodes.Pop))
				////.Append(x => x.Create(OpCodes.Ldloc, index));
			}
		}

		public static void WeaveTryGetValueAndReturn(MethodWeavingContext methodWeavingContext, ILProcessorContext processorContext)
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

		private static string CreateCacheKeyMethodName(MethodDefinition methodDefinition)
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

		private static ILProcessorContext CreateTupleConstructorCalls(MethodWeavingContext methodWeavingContext,
			ILProcessorContext processorContext, GenericInstanceType tupleType)
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

			TypeReference[] typeReferences = tupleType.GenericArguments.Cast<TypeReference>().ToArray();

			if (typeReferences.Length == 8)
			{
				processorContext = MemoryCache.CreateTupleConstructorCalls(methodWeavingContext, processorContext,
					(GenericInstanceType)typeReferences.Last());
			}

			MethodReference tupleConstructor =
				methodWeavingContext.ClassWeavingContext.References.GetSystemTupleConstructor(typeReferences);

			return processorContext.Append(x => x.Create(OpCodes.Newobj, tupleConstructor));
		}
	}
}