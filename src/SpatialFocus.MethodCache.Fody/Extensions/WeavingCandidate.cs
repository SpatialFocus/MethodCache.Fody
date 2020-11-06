// <copyright file="WeavingCandidate.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Extensions
{
	using System.Collections.Generic;
	using Mono.Cecil;

	public class WeavingCandidate
	{
		public WeavingCandidate(TypeDefinition classDefinition, ICollection<MethodDefinition> methodDefinitions)
		{
			ClassDefinition = classDefinition;
			MethodDefinitions = methodDefinitions;
		}

		public TypeDefinition ClassDefinition { get; }

		public ICollection<MethodDefinition> MethodDefinitions { get; }
	}
}