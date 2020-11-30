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

		public MethodReference MemoryCacheEntryOptionsAbsoluteExpirationRelativeToNowSetter { get; set; }

		public MethodReference MemoryCacheEntryOptionsConstructor { get; set; }

		public MethodReference MemoryCacheEntryOptionsPrioritySetter { get; set; }

		public MethodReference MemoryCacheEntryOptionsSlidingExpirationSetter { get; set; }

		public TypeReference MemoryCacheEntryOptionsType { get; set; }

		public TypeReference MemoryCacheInterface { get; protected set; }

		public TypeReference NoCacheAttributeType { get; set; }

		public MethodReference NullableTimeSpanConstructor { get; set; }

		public MethodReference SetMethod { get; protected set; }

		public MethodReference SetMethodWithMemoryCacheEntryOptions { get; protected set; }

		public MethodReference TimeSpanFromSecondsMethod { get; set; }

		public TypeReference TimeSpanType { get; set; }

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

			TypeDefinition timeSpanType = moduleWeaver.FindTypeDefinition(typeof(TimeSpan).FullName);
			references.TimeSpanType = moduleWeaver.ModuleDefinition.ImportReference(timeSpanType);
			references.TimeSpanFromSecondsMethod =
				moduleWeaver.ModuleDefinition.ImportReference(timeSpanType.Methods.Single(x => x.Name == nameof(TimeSpan.FromSeconds)));

			TypeDefinition nullableType = moduleWeaver.FindTypeDefinition(typeof(Nullable<>).FullName);
			references.NullableTimeSpanConstructor =
				moduleWeaver.ModuleDefinition.ImportReference(nullableType.GetConstructors()
					.Single()
					.MakeHostInstanceGeneric(references.TimeSpanType));

			TypeDefinition memoryCacheInterface = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.IMemoryCache");
			references.MemoryCacheInterface = moduleWeaver.ModuleDefinition.ImportReference(memoryCacheInterface);

			TypeDefinition cacheExtensions = moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.CacheExtensions");
			references.CacheExtensionsType = moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions);

			TypeDefinition memoryCacheEntryOptions =
				moduleWeaver.FindTypeDefinition("Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions");
			references.MemoryCacheEntryOptionsType = moduleWeaver.ModuleDefinition.ImportReference(memoryCacheEntryOptions);
			references.MemoryCacheEntryOptionsConstructor =
				moduleWeaver.ModuleDefinition.ImportReference(memoryCacheEntryOptions.GetConstructors().Single(x => !x.Parameters.Any()));
			references.MemoryCacheEntryOptionsAbsoluteExpirationRelativeToNowSetter =
				moduleWeaver.ModuleDefinition.ImportReference(memoryCacheEntryOptions.Properties
					.Single(x => x.Name == "AbsoluteExpirationRelativeToNow")
					.SetMethod);
			references.MemoryCacheEntryOptionsSlidingExpirationSetter =
				moduleWeaver.ModuleDefinition.ImportReference(memoryCacheEntryOptions.Properties.Single(x => x.Name == "SlidingExpiration")
					.SetMethod);
			references.MemoryCacheEntryOptionsPrioritySetter =
				moduleWeaver.ModuleDefinition.ImportReference(
					memoryCacheEntryOptions.Properties.Single(x => x.Name == "Priority").SetMethod);

			references.TryGetValueMethod =
				moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.Single(x => x.Name == "TryGetValue"));

			references.SetMethod =
				moduleWeaver.ModuleDefinition.ImportReference(cacheExtensions.Methods.Single(x =>
					x.Name == "Set" && x.HasParameters && x.Parameters.Count == 3));

			references.SetMethodWithMemoryCacheEntryOptions = moduleWeaver.ModuleDefinition.ImportReference(
				cacheExtensions.Methods.Single(x =>
					x.Name == "Set" && x.HasParameters && x.Parameters.Count == 4 &&
					moduleWeaver.ModuleDefinition.ImportReference(x.Parameters.Last().ParameterType).Resolve() ==
					references.MemoryCacheEntryOptionsType.Resolve()));

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

		public MethodReference GetGenericSetMethodWithMemoryCacheEntryOptions(TypeReference type) =>
			ModuleWeaver.ModuleDefinition.ImportReference(SetMethodWithMemoryCacheEntryOptions.MakeGeneric(type));

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