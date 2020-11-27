// <copyright file="References.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using Mono.Cecil;
	using Mono.Cecil.Rocks;
	using SpatialFocus.MethodCache.Fody.Extensions;

	public class References
	{
		protected References(ModuleWeaver moduleWeaver)
		{
			ModuleWeaver = moduleWeaver;
		}

		public TypeReference CacheAttributeType { get; set; }

		public TypeReference CacheExtensionsType { get; protected set; }

		public TypeReference CompilerGeneratedAttributeType { get; set; }

		public MethodReference GetTypeFromHandleMethod { get; protected set; }

		public TypeReference MemoryCacheInterface { get; protected set; }

		public TypeReference NoCacheAttributeType { get; set; }

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

			TypeDefinition cacheAttributeType = moduleWeaver.FindTypeDefinition("SpatialFocus.MethodCache.CacheAttribute");
			references.CacheAttributeType = moduleWeaver.ModuleDefinition.ImportReference(cacheAttributeType);

			TypeDefinition noCacheAttributeType = moduleWeaver.FindTypeDefinition("SpatialFocus.MethodCache.NoCacheAttribute");
			references.NoCacheAttributeType = moduleWeaver.ModuleDefinition.ImportReference(noCacheAttributeType);

			TypeDefinition type = moduleWeaver.FindTypeDefinition(typeof(Type).FullName);
			references.TypeType = moduleWeaver.ModuleDefinition.ImportReference(type);

			TypeDefinition compilerGeneratedAttributeType = moduleWeaver.FindTypeDefinition(typeof(CompilerGeneratedAttribute).FullName);
			references.CompilerGeneratedAttributeType = moduleWeaver.ModuleDefinition.ImportReference(compilerGeneratedAttributeType);

			references.GetTypeFromHandleMethod =
				moduleWeaver.ModuleDefinition.ImportReference(type.Methods.Single(x => x.Name == nameof(Type.GetTypeFromHandle)));

			TypeDefinition memoryCacheInterface = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.IMemoryCache");
			references.MemoryCacheInterface = moduleWeaver.ModuleDefinition.ImportReference(memoryCacheInterface);

			TypeDefinition cacheExtensions = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.CacheExtensions");
			references.CacheExtensionsType = moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions);

			references.TryGetValueMethod =
				moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.Single(x => x.Name == "TryGetValue"));

			references.SetMethod =
				moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.Single(x =>
					x.Name == "Set" && x.HasParameters && x.Parameters.Count == 3));

			return references;
		}

		public TypeReference GetFragmentedSystemTupleType(ICollection<TypeReference> typeReferences)
		{
			if (typeReferences == null)
			{
				throw new ArgumentNullException(nameof(typeReferences));
			}

			if (typeReferences.Count <= 7)
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

			return ModuleWeaver.ModuleDefinition.ImportReference(methodDefinition).MakeHostInstanceGeneric(types);
		}

		public TypeReference GetSystemTupleType(params TypeReference[] types)
		{
			if (types.Length > 8)
			{
				throw new ArgumentException($"{nameof(Array.Length)} must be less or equal to 8", nameof(types));
			}

			TypeDefinition findTypeDefinition = ModuleWeaver.FindTypeDefinition($"System.Tuple`{types.Length}");
			return ModuleWeaver.ModuleDefinition.ImportReference(findTypeDefinition).MakeGenericInstanceType(types);
		}

		public MethodReference GetTryGetValue(TypeReference type) =>
			ModuleWeaver.ModuleDefinition.ImportReference(TryGetValueMethod.MakeGeneric(type));
	}
}