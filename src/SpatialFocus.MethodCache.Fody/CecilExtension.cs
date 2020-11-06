// <copyright file="CecilExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using Mono.Cecil;
	using Mono.Cecil.Rocks;

	public static class CecilExtension
	{
		public static MethodReference MakeGeneric(this MethodReference method, params TypeReference[] arguments)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			if (method.GenericParameters.Count != arguments.Length)
			{
				throw new ArgumentException("Invalid number of generic type arguments supplied");
			}

			if (arguments.Length == 0)
			{
				return method;
			}

			GenericInstanceMethod genericTypeReference = new GenericInstanceMethod(method);

			foreach (TypeReference argument in arguments)
			{
				genericTypeReference.GenericArguments.Add(argument);
			}

			return genericTypeReference;
		}

		public static MethodReference MakeHostInstanceGeneric(this MethodReference self, params TypeReference[] args)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

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