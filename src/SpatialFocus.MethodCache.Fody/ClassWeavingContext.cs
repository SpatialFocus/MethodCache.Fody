// <copyright file="ClassWeavingContext.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using Mono.Cecil;

	public class ClassWeavingContext
	{
		public ClassWeavingContext(TypeDefinition typeDefinition, References references)
		{
			TypeDefinition = typeDefinition;
			References = references;
		}

		public References References { get; }

		public TypeDefinition TypeDefinition { get; }

		public MethodReference CacheGetterMethod { get; set; }
	}
}