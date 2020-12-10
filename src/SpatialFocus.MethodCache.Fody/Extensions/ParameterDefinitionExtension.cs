// <copyright file="ParameterDefinitionExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Extensions
{
	using System;
	using System.Linq;
	using Mono.Cecil;

	public static class ParameterDefinitionExtension
	{
		public static bool HasNoKeyAttribute(this ParameterDefinition parameterDefinition, References references)
		{
			if (parameterDefinition == null)
			{
				throw new ArgumentNullException(nameof(parameterDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			TypeReference noKeyAttributeType = references.NoKeyAttributeType.Resolve();

			return parameterDefinition.CustomAttributes.Any(classAttribute =>
				classAttribute.AttributeType.Resolve().Equals(noKeyAttributeType));
		}
	}
}