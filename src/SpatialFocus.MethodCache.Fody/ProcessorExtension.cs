// <copyright file="ProcessorExtension.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Linq;
	using Mono.Cecil.Cil;

	public static class ProcessorExtension
	{
		public static (ILProcessor Processor, Instruction Instruction) Append(this (ILProcessor Processor, Instruction Instruction) tuple,
			Func<ILProcessor, Instruction> action)
		{
			Instruction instruction = action(tuple.Processor);

			if (tuple.Instruction == null)
			{
				if (tuple.Processor.Body.Instructions.Count == 0)
				{
					tuple.Processor.Append(instruction);
				}
				else
				{
					tuple.Processor.InsertBefore(tuple.Processor.Body.Instructions.First(), instruction);
				}
			}
			else
			{
				tuple.Processor.InsertAfter(tuple.Instruction, instruction);
			}

			return (tuple.Processor, instruction);
		}

		public static (ILProcessor Processor, Instruction Instruction) Start(this ILProcessor processor) => (processor, processor.Body.Instructions.First());
	}
}