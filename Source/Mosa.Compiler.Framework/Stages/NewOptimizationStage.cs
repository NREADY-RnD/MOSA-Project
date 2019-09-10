// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transformation;
using Mosa.Compiler.Framework.Transformation.Auto;
using Mosa.Compiler.Framework.Transformation.Manual;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public class NewOptimizationStage : BaseMethodCompilerStage
	{
		private Counter OptimizationsCount = new Counter("FastSimplification.Optimizations");

		private TraceLog trace;

		private List<BaseTransformation>[] transformations;

		protected override void Initialize()
		{
			transformations = new List<BaseTransformation>[BaseInstruction.MaximumInstructionID];

			Register(OptimizationsCount);

			CreateTransformationList(AutoTransformations.List);
			CreateTransformationList(ManualTransformations.List);
		}

		private void CreateTransformationList(List<BaseTransformation> list)
		{
			foreach (var transformation in list)
			{
				int id = transformation.Instruction != null ? transformation.Instruction.ID : 0;

				if (transformations[id] == null)
				{
					transformations[id] = new List<BaseTransformation>();
				}

				transformations[id].Add(transformation);
			}
		}

		protected override void Finish()
		{
			trace = null;
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			Optimize();
		}

		private void Optimize()
		{
			var context = new Context(BasicBlocks.PrologueBlock);
			var transformContext = new TransformContext(MethodCompiler, trace);

			var updated = false;
			var changed = true;
			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
					{
						updated = true;

						while (updated)
						{
							if (node.IsEmptyOrNop)
								break;

							context.Node = node;

							updated = ApplyTransformations(context, transformContext);

							changed |= updated;
						}
					}
				}
			}
		}

		private bool ApplyTransformations(Context context, TransformContext transformContext)
		{
			if (ApplyTransformations(context, transformContext, context.Instruction.ID))
				return true;

			return ApplyTransformations(context, transformContext, 0);
		}

		private bool ApplyTransformations(Context context, TransformContext transformContext, int id)
		{
			var instructionTransformations = transformations[id];

			if (instructionTransformations == null)
				return false;

			int count = instructionTransformations.Count;

			for (int i = 0; i < count; i++)
			{
				var transformation = instructionTransformations[i];

				var updated = transformContext.ApplyTransform(context, transformation);

				if (updated)
					return true;
			}

			return false;
		}
	}
}
