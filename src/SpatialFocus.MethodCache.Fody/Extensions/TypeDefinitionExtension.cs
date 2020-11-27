// <copyright file="TypeDefinitionExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Extensions
{
	using System;
	using System.Linq;
	using Mono.Cecil;

	public static class TypeDefinitionExtension
	{
		public static bool HasCacheAttribute(this TypeDefinition typeDefinition, References references)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException(nameof(typeDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference cacheAttributeType = references.CacheAttributeType.Resolve();

			return typeDefinition.CustomAttributes.Any(classAttribute => classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}

		public static bool IsEligibleForWeaving(this TypeDefinition typeDefinition, References references)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException(nameof(typeDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			bool isEligibleForWeaving = typeDefinition.TryGetCacheGetterProperty(references) != null;

			return isEligibleForWeaving;
		}

		public static PropertyDefinition TryGetCacheGetterProperty(this TypeDefinition typeDefinition, References references)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException(nameof(typeDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			PropertyDefinition property = typeDefinition.Properties.Where(propertyDefinition =>
				{
					TypeDefinition propertyTypeDefinition = propertyDefinition.PropertyType.Resolve();
					TypeDefinition memoryCacheInterface = references.MemoryCacheInterface.Resolve();

					if (propertyDefinition.GetMethod.IsStatic)
					{
						return false;
					}

					if (propertyTypeDefinition.IsInterface && propertyTypeDefinition.Equals(memoryCacheInterface))
					{
						return true;
					}

					if (propertyTypeDefinition.Interfaces.Any(x => x.InterfaceType == memoryCacheInterface))
					{
						return true;
					}

					return false;
				})
				.SingleOrDefault();

			if (property == null && typeDefinition.BaseType != null)
			{
				TypeDefinition baseTypeDefinition = typeDefinition.BaseType.Resolve();
				property = baseTypeDefinition.TryGetCacheGetterProperty(references);
			}

			return property;
		}

		public static CustomAttribute TryGetCacheAttribute(this TypeDefinition typeDefinition, References references)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException(nameof(typeDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference cacheAttributeType = references.CacheAttributeType.Resolve();

			return typeDefinition.CustomAttributes.SingleOrDefault(classAttribute =>
				classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}
	}
}