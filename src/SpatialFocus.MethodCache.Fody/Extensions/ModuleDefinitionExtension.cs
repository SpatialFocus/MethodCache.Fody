// <copyright file="ModuleDefinitionExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Mono.Cecil;

	public static class ModuleDefinitionExtension
	{
		public static ICollection<WeavingCandidate> GetWeavingCandidates(this ModuleDefinition moduleDefinition, References references)
		{
			if (moduleDefinition == null)
			{
				throw new ArgumentNullException(nameof(moduleDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			return moduleDefinition.Types.Select(type =>
				{
					if (type.HasCacheAttribute(references))
					{
						return new WeavingCandidate(type, type.Methods.ToList());
					}

					WeavingCandidate weavingCandidate =
						new WeavingCandidate(type, type.Methods.Where(method => method.HasCacheAttribute(references)).ToList());

					if (!weavingCandidate.MethodDefinitions.Any())
					{
						return null;
					}

					return weavingCandidate;
				})
				.Where(candidate => candidate != null)
				.ToList();
		}
	}
}