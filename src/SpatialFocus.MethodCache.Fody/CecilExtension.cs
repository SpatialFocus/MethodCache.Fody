// <copyright file="CecilExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using Mono.Cecil;
	using Mono.Cecil.Rocks;

	public static class CecilExtension
	{
		public static MethodReference MakeHostInstanceGeneric(this MethodReference self, params TypeReference[] args)
		{
			MethodReference reference = new MethodReference(self.Name, self.ReturnType, self.DeclaringType.MakeGenericInstanceType(args))
			{
				HasThis = self.HasThis, ExplicitThis = self.ExplicitThis, CallingConvention = self.CallingConvention,
			};

			foreach (ParameterDefinition parameter in self.Parameters)
			{
				reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
			}

			foreach (GenericParameter genericParam in self.GenericParameters)
			{
				reference.GenericParameters.Add(new GenericParameter(genericParam.Name, reference));
			}

			return reference;
		}
	}
}