// <copyright file="MethodDefinitionExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Extensions
{
	using System;
	using System.Linq;
	using Mono.Cecil;

	public static class MethodDefinitionExtension
	{
		public static bool HasCacheAttribute(this MethodDefinition methodDefinition, References references)
		{
			if (methodDefinition == null)
			{
				throw new ArgumentNullException(nameof(methodDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference cacheAttributeType = references.CacheAttributeType.Resolve();

			return methodDefinition.CustomAttributes.Any(
				classAttribute => classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}

		public static bool HasNoCacheAttribute(this MethodDefinition methodDefinition, References references)
		{
			if (methodDefinition == null)
			{
				throw new ArgumentNullException(nameof(methodDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference noCacheAttributeType = references.NoCacheAttributeType.Resolve();

			return methodDefinition.CustomAttributes.Any(classAttribute =>
				classAttribute.AttributeType.Resolve().Equals(noCacheAttributeType));
		}

		public static bool IsEligibleForWeaving(this MethodDefinition methodDefinition, References references)
		{
			if (methodDefinition == null)
			{
				throw new ArgumentNullException(nameof(methodDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeDefinition typeDefinition = references.CompilerGeneratedAttributeType.Resolve();

			if (methodDefinition.ReturnType.Equals(methodDefinition.Module.TypeSystem.Void))
			{
				return false;
			}

			bool hasOutParameter = methodDefinition.Parameters.Any(x => x.IsOut);

			bool isSpecialName = methodDefinition.IsSpecialName || methodDefinition.IsGetter || methodDefinition.IsSetter ||
				methodDefinition.IsConstructor;

			bool hasCompilerGeneratedAttribute =
				methodDefinition.CustomAttributes.Any(attribute => attribute.AttributeType.Resolve().Equals(typeDefinition));

			return !hasOutParameter && !isSpecialName && !hasCompilerGeneratedAttribute;
		}

		public static CustomAttribute TryGetCacheAttribute(this MethodDefinition methodDefinition, References references)
		{
			if (methodDefinition == null)
			{
				throw new ArgumentNullException(nameof(methodDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference cacheAttributeType = references.CacheAttributeType.Resolve();

			return methodDefinition.CustomAttributes.SingleOrDefault(classAttribute =>
				classAttribute.AttributeType.Resolve().Equals(cacheAttributeType));
		}
	}
}