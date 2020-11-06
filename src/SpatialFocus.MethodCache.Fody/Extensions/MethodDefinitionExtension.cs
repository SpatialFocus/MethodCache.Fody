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

			bool isSpecialName = methodDefinition.IsSpecialName || methodDefinition.IsGetter || methodDefinition.IsSetter ||
				methodDefinition.IsConstructor;

			bool hasCompilerGeneratedAttribute =
				methodDefinition.CustomAttributes.Any(attribute => attribute.AttributeType.Resolve().Equals(typeDefinition));

			return !isSpecialName && !hasCompilerGeneratedAttribute;
		}
	}
}