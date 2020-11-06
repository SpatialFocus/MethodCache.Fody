// <copyright file="ILProcessorContext.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody
{
	using System;
	using System.Linq;
	using Mono.Cecil.Cil;

	public class ILProcessorContext
	{
		public ILProcessorContext(ILProcessor processor, Instruction currentInstruction)
		{
			Processor = processor;
			CurrentInstruction = currentInstruction;
		}

		public Instruction CurrentInstruction { get; }

		public ILProcessor Processor { get; }

		public ILProcessorContext Append(Func<ILProcessor, Instruction> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			Instruction instruction = action(Processor);

			if (CurrentInstruction == null)
			{
				if (Processor.Body.Instructions.Count == 0)
				{
					Processor.Append(instruction);
				}
				else
				{
					Processor.InsertBefore(Processor.Body.Instructions.First(), instruction);
				}
			}
			else
			{
				Processor.InsertAfter(CurrentInstruction, instruction);
			}

			return new ILProcessorContext(Processor, instruction);
		}
	}
}