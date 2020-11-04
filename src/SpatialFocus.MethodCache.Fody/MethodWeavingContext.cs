// <copyright file="MethodWeavingContext.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System.Collections.Generic;
	using Mono.Cecil;

	public class MethodWeavingContext
	{
		public MethodWeavingContext(ClassWeavingContext classWeavingContext, MethodDefinition methodDefinition)
		{
			ClassWeavingContext = classWeavingContext;
			MethodDefinition = methodDefinition;
		}

		public ICollection<TypeReference> CacheKeyParameterTypes { get; } = new List<TypeReference>();

		public ClassWeavingContext ClassWeavingContext { get; }

		public MethodDefinition MethodDefinition { get; }

		public TypeReference CacheKeyType { get; set; }

		public int? CacheKeyVariableIndex { get; set; }

		public int? ResultVariableIndex { get; set; }
	}
}