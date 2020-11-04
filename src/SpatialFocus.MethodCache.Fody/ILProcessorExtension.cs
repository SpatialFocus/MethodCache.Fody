// <copyright file="ILProcessorExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Linq;
	using Mono.Cecil.Cil;

	public static class ILProcessorExtension
	{
		public static ILProcessorContext Before(this ILProcessor processor, Instruction instruction)
		{
			if (processor == null)
			{
				throw new ArgumentNullException(nameof(processor));
			}

			if (instruction == null)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			return new ILProcessorContext(processor, instruction.Previous);
		}

		public static ILProcessorContext Start(this ILProcessor processor)
		{
			if (processor == null)
			{
				throw new ArgumentNullException(nameof(processor));
			}

			return new ILProcessorContext(processor, processor.Body.Instructions.First());
		}
	}
}