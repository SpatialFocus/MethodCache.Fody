// <copyright file="ModuleWeaver.cs" company="Spatial Focus GmbH">
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

	public partial class ModuleWeaver : BaseModuleWeaver
	{
		public override bool ShouldCleanReference => true;

		public override void Execute()
		{
			FindCoreReferences();

			////TypeDefinition findTypeDefinition = FindTypeDefinition("System.Tuple`1");
			////MethodDefinition methodDefinition = findTypeDefinition.GetConstructors().Single();
			////MethodReference makeHostInstanceGeneric = methodDefinition.MakeHostInstanceGeneric(TypeSystem.StringReference);
			////MethodReference importReference = ModuleDefinition.ImportReference(makeHostInstanceGeneric);

			////TypeDefinition typeDefinition = ModuleDefinition.Types.Single(x => x.Name == "Class1");
			////MethodBody methodBody = typeDefinition.Methods.Single(x => x.Name == "Method2").Body;
			////ILProcessor ilProcessor = methodBody.GetILProcessor();
			////Instruction instruction = methodBody.Instructions.Last();
			////ilProcessor.InsertBefore(instruction, ilProcessor.Create(OpCodes.Ldstr, "a"));
			////ilProcessor.InsertBefore(instruction, ilProcessor.Create(OpCodes.Newobj, importReference));
			////ilProcessor.InsertBefore(instruction, ilProcessor.Create(OpCodes.Pop));

			////// Create type definition
			////TypeDefinition attributeTypeDefinition = FindTypeDefinition("System.Attribute");
			////TypeDefinition myAttributeTypeDefinition = new TypeDefinition("MyNamespace", "MyAttribute", TypeAttributes.Public,
			////	ModuleDefinition.ImportReference(attributeTypeDefinition));

			////// Add constructor
			////MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
			////MethodDefinition method = new MethodDefinition(".ctor", attributes, TypeSystem.VoidReference);
			////MethodReference objectConstructor = ModuleDefinition.ImportReference(attributeTypeDefinition.GetConstructors().First());
			////ILProcessor processor = method.Body.GetILProcessor();
			////processor.Emit(OpCodes.Ldarg_0);
			////processor.Emit(OpCodes.Call, objectConstructor);
			////processor.Emit(OpCodes.Ret);
			////myAttributeTypeDefinition.Methods.Add(method);

			////// Add definition to module definition
			////ModuleDefinition.Types.Add(myAttributeTypeDefinition);

			foreach (MethodDefinition definition in ModuleDefinition.Types.SelectMany(x => x.Methods))
			{
				CreateCacheKey(definition, definition.Body.GetILProcessor());
			}
		}

		protected void CreateCacheKey(MethodDefinition methodDefinition, ILProcessor processor)
		{
			ICollection<GenericParameter> declaringTypeGenericParameters = methodDefinition.DeclaringType.GenericParameters ??
				(ICollection<GenericParameter>)Array.Empty<GenericParameter>();
			ICollection<GenericParameter> methodDefinitionGenericParameters =
				methodDefinition.GenericParameters ?? (ICollection<GenericParameter>)Array.Empty<GenericParameter>();
			ICollection<ParameterDefinition> methodDefinitionParameters =
				methodDefinition.Parameters ?? (ICollection<ParameterDefinition>)Array.Empty<ParameterDefinition>();

			ICollection<TypeReference> tupleTypeReferences = new[] { ModuleDefinition.TypeSystem.String }
				.Concat(Enumerable.Repeat(Type, declaringTypeGenericParameters.Count))
				.Concat(Enumerable.Repeat(Type, methodDefinitionGenericParameters.Count))
				.Concat(methodDefinitionParameters.Select(x => x.ParameterType))
				.ToList();

			TypeReference tupleType = GetFragmentedSystemTupleType(tupleTypeReferences.ToList());

			methodDefinition.Body.Variables.Add(new VariableDefinition(tupleType));
			int index = methodDefinition.Body.Variables.Count - 1;

			(ILProcessor Processor, Instruction Instruction) instructions = processor.Start();

			string methodName = CreateCacheKeyMethodName(methodDefinition);
			instructions = instructions.Append(x => x.Create(OpCodes.Ldstr, methodName));

			foreach (GenericParameter genericParameter in declaringTypeGenericParameters)
			{
				instructions = instructions.Append(x => x.Create(OpCodes.Ldtoken, genericParameter));
				instructions = instructions.Append(x => x.Create(OpCodes.Call, GetTypeFromHandle));
			}

			foreach (GenericParameter genericParameter in methodDefinitionGenericParameters)
			{
				instructions = instructions.Append(x => x.Create(OpCodes.Ldtoken, genericParameter));
				instructions = instructions.Append(x => x.Create(OpCodes.Call, GetTypeFromHandle));
			}

			for (var i = 0; i < methodDefinitionParameters.Count; i++)
			{
				int value = i;
				instructions = instructions.Append(x => x.Create(OpCodes.Ldarg, value));
			}

			instructions = CreateTupleConstructorCalls(instructions, (GenericInstanceType)tupleType, tupleTypeReferences.Count); 
			instructions.Append(x => x.Create(OpCodes.Stloc, index));
		}

		protected (ILProcessor Processor, Instruction Instruction) CreateTupleConstructorCalls((ILProcessor Processor, Instruction Instruction) instructions, GenericInstanceType tupleType, int remainingTupleTypeReferencesCount)
		{
			TypeReference[] typeReferences = tupleType.GenericArguments.Cast<TypeReference>().ToArray();

			if (typeReferences.Length == 8 && remainingTupleTypeReferencesCount > 8)
			{
				remainingTupleTypeReferencesCount -= 7;
				return CreateTupleConstructorCalls(instructions, (GenericInstanceType)typeReferences.Last(), remainingTupleTypeReferencesCount);
			}

			MethodReference tupleConstructor = GetSystemTupleConstructor(typeReferences);
			return instructions.Append(x => x.Create(OpCodes.Newobj, tupleConstructor));
		}


		protected string CreateCacheKeyMethodName(MethodDefinition methodDefinition)
		{
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
				builder.Append("<");
				builder.Append(string.Join(", ", declaringType.GenericParameters.Select(x => x.FullName)));
				builder.Append(">");
			}

			builder.Append(".");
			builder.Append(methodDefinition.Name);

			if (methodDefinition.GenericParameters.Any())
			{
				builder.Append("<");
				builder.Append(string.Join(", ", methodDefinition.GenericParameters.Select(x => x.FullName)));
				builder.Append(">");
			}

			return builder.ToString();
		}
	}
}