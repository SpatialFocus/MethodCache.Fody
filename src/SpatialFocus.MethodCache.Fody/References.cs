// <copyright file="References.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Mono.Cecil;
	using Mono.Cecil.Rocks;

	public class References
	{
		protected References(ModuleWeaver moduleWeaver)
		{
			ModuleWeaver = moduleWeaver;
		}

		public TypeReference CacheExtensionsType { get; protected set; }

		public MethodReference GetTypeFromHandleMethod { get; protected set; }

		public TypeReference MemoryCacheInterface { get; protected set; }

		public MethodReference SetMethod { get; protected set; }

		public MethodReference TryGetValueMethod { get; protected set; }

		public TypeReference TypeType { get; protected set; }

		protected ModuleWeaver ModuleWeaver { get; }

		public static References Init(ModuleWeaver moduleWeaver)
		{
			if (moduleWeaver == null)
			{
				throw new ArgumentNullException(nameof(moduleWeaver));
			}

			References references = new References(moduleWeaver);

			TypeDefinition type = moduleWeaver.FindTypeDefinition(typeof(Type).FullName);
			references.TypeType = moduleWeaver.ModuleDefinition.ImportReference(type);

			references.GetTypeFromHandleMethod =
				moduleWeaver.ModuleDefinition.ImportReference(type.Methods.Single(x => x.Name == nameof(Type.GetTypeFromHandle)));

			TypeDefinition memoryCacheInterface = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.IMemoryCache");
			references.MemoryCacheInterface = moduleWeaver.ModuleDefinition.ImportReference(memoryCacheInterface);

			TypeDefinition cacheExtensions = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.CacheExtensions");
			references.CacheExtensionsType = moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions);

			references.TryGetValueMethod =
				moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.Single(x => x.Name == "TryGetValue"));

			// TODO: Find the "Single" Method with only TItem parameter (or set timeout, etc.)
			references.SetMethod = moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.First(x => x.Name == "Set"));

			return references;
		}

		public TypeReference GetFragmentedSystemTupleType(ICollection<TypeReference> typeReferences)
		{
			if (typeReferences == null)
			{
				throw new ArgumentNullException(nameof(typeReferences));
			}

			if (typeReferences.Count <= 8)
			{
				return GetSystemTupleType(typeReferences.ToArray());
			}

			return GetSystemTupleType(typeReferences.Take(7)
				.Concat(new[] { GetFragmentedSystemTupleType(typeReferences.Skip(7).ToList()) })
				.ToArray());
		}

		public MethodReference GetGenericSetMethod(TypeReference type) =>
			ModuleWeaver.ModuleDefinition.ImportReference(SetMethod.MakeGeneric(type));

		public MethodReference GetSystemTupleConstructor(params TypeReference[] types)
		{
			if (types.Length > 8)
			{
				throw new ArgumentException($"{nameof(Array.Length)} must be less or equal to 8", nameof(types));
			}

			TypeDefinition findTypeDefinition = ModuleWeaver.FindTypeDefinition($"System.Tuple`{types.Length}");
			MethodDefinition methodDefinition = findTypeDefinition.GetConstructors().Single();

			MethodReference makeHostInstanceGeneric = methodDefinition.MakeHostInstanceGeneric(types);
			return ModuleWeaver.ModuleDefinition.ImportReference(makeHostInstanceGeneric);
		}

		public TypeReference GetSystemTupleType(params TypeReference[] types)
		{
			if (types.Length > 8)
			{
				throw new ArgumentException($"{nameof(Array.Length)} must be less or equal to 8", nameof(types));
			}

			TypeDefinition findTypeDefinition = ModuleWeaver.FindTypeDefinition($"System.Tuple`{types.Length}");
			return ModuleWeaver.ModuleDefinition.ImportReference(findTypeDefinition.MakeGenericInstanceType(types));
		}

		public MethodReference GetTryGetValue(TypeReference type) =>
			ModuleWeaver.ModuleDefinition.ImportReference(TryGetValueMethod.MakeGeneric(type));
	}
}