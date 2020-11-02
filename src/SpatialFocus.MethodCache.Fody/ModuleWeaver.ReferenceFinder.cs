// <copyright file="ModuleWeaver.ReferenceFinder.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Mono.Cecil;
	using Mono.Cecil.Rocks;

	public partial class ModuleWeaver
	{
		protected MethodReference GetTypeFromHandle { get; set; }

		protected TypeReference Type { get; set; }

		public void FindCoreReferences()
		{
			TypeDefinition findTypeDefinition = FindTypeDefinition(typeof(Type).FullName);
			Type = ModuleDefinition.ImportReference(findTypeDefinition);

			GetTypeFromHandle =
				ModuleDefinition.ImportReference(findTypeDefinition.Methods.Single(x => x.Name == nameof(System.Type.GetTypeFromHandle)));
		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
		}

		public MethodReference GetSystemTupleConstructor(params TypeReference[] types)
		{
			if (types.Length > 8)
			{
				throw new ArgumentException($"{nameof(Array.Length)} must be less or equal to 8", nameof(types));
			}

			TypeDefinition findTypeDefinition = FindTypeDefinition($"System.Tuple`{types.Length}");
			MethodDefinition methodDefinition = findTypeDefinition.GetConstructors().Single();

			MethodReference makeHostInstanceGeneric = methodDefinition.MakeHostInstanceGeneric(types);
			return ModuleDefinition.ImportReference(makeHostInstanceGeneric);
		}

		public TypeReference GetSystemTupleType(params TypeReference[] types)
		{
			if (types.Length > 8)
			{
				throw new ArgumentException($"{nameof(Array.Length)} must be less or equal to 8", nameof(types));
			}

			TypeDefinition findTypeDefinition = FindTypeDefinition($"System.Tuple`{types.Length}");
			return ModuleDefinition.ImportReference(findTypeDefinition.MakeGenericInstanceType(types));
		}

		protected TypeReference GetFragmentedSystemTupleType(ICollection<TypeReference> typeReferences)
		{
			if (typeReferences.Count <= 8)
			{
				return GetSystemTupleType(typeReferences.ToArray());
			}

			return GetSystemTupleType(typeReferences.Take(7)
				.Concat(new[] { GetFragmentedSystemTupleType(typeReferences.Skip(7).ToList()) })
				.ToArray());
		}
	}
}