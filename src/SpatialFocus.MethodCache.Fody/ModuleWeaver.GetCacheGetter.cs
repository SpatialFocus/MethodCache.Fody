// <copyright file="ModuleWeaver.GetCacheGetter.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Fody;
	using Mono.Cecil;

	public partial class ModuleWeaver
	{
		protected static MethodDefinition GetCacheGetterMethod(ClassWeavingContext classWeavingContext)
		{
			if (classWeavingContext == null)
			{
				throw new ArgumentNullException(nameof(classWeavingContext));
			}

			// TODO: Check if this can be inherited, extract to class level
			List<PropertyDefinition> propertyDefinitions = classWeavingContext.TypeDefinition.Properties.Where(definition =>
				{
					TypeDefinition typeDefinition = definition.PropertyType.Resolve();
					TypeDefinition memoryCacheInterface = classWeavingContext.References.MemoryCacheInterface.Resolve();

					if (typeDefinition.IsInterface && typeDefinition.Equals(memoryCacheInterface))
					{
						return true;
					}

					if (typeDefinition.Interfaces.Any(x => x.InterfaceType == memoryCacheInterface))
					{
						return true;
					}

					return false;
				})
				.ToList();

			// TODO: Also check fields
			if (propertyDefinitions.Count != 1)
			{
				throw new WeavingException("Property not found");
			}

			return propertyDefinitions.Single().GetMethod;
		}
	}
}