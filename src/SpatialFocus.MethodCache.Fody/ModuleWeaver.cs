// <copyright file="ModuleWeaver.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System.Collections.Generic;
	using System.Linq;
	using global::Fody;
	using Mono.Cecil;
	using Mono.Cecil.Cil;
	using Mono.Cecil.Rocks;

	public class ModuleWeaver : BaseModuleWeaver
	{
		public override bool ShouldCleanReference => true;

		public override void Execute()
		{
			// Create type definition
			TypeDefinition attributeTypeDefinition = FindTypeDefinition("System.Attribute");
			TypeDefinition myAttributeTypeDefinition = new TypeDefinition("MyNamespace", "MyAttribute", TypeAttributes.Public,
				ModuleDefinition.ImportReference(attributeTypeDefinition));

			// Add constructor
			MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
			MethodDefinition method = new MethodDefinition(".ctor", attributes, TypeSystem.VoidReference);
			MethodReference objectConstructor = ModuleDefinition.ImportReference(attributeTypeDefinition.GetConstructors().First());
			ILProcessor processor = method.Body.GetILProcessor();
			processor.Emit(OpCodes.Ldarg_0);
			processor.Emit(OpCodes.Call, objectConstructor);
			processor.Emit(OpCodes.Ret);
			myAttributeTypeDefinition.Methods.Add(method);

			// Add definition to module definition
			ModuleDefinition.Types.Add(myAttributeTypeDefinition);
		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
		}
	}
}