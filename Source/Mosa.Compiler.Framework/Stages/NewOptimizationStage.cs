// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transformation;
using Mosa.Compiler.Framework.Transformation.Auto;
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

		private List<Operand> operandWorklist = new List<Operand>();
		private List<InstructionNode> nodeWorklist = new List<InstructionNode>();

		protected override void Initialize()
		{
			transformations = new List<BaseTransformation>[BaseInstruction.MaximumInstructionID];

			Register(OptimizationsCount);

			foreach (var transformation in AutoTransformations.List)
			{
				if (transformations[transformation.Instruction.ID] == null)
				{
					transformations[transformation.Instruction.ID] = new List<BaseTransformation>();
				}

				transformations[transformation.Instruction.ID].Add(transformation);
			}
		}

		protected override void Finish()
		{
			trace = null;
			operandWorklist.Clear();
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

			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					context.Node = node;

					Do(context, transformContext);

					ProcessWorklist();
				}
			}
		}

		private void ProcessWorklist()
		{
			foreach (var operand in operandWorklist)
			{
				foreach (var use in operand.Uses)
				{
					nodeWorklist.AddIfNew(use);
				}

				foreach (var def in operand.Definitions)
				{
					nodeWorklist.AddIfNew(def);
				}
			}

			operandWorklist.Clear();
		}

		private void Do(Context context, TransformContext transformContext)
		{
			var instructionTransformations = transformations[context.Instruction.ID];
			int count = instructionTransformations.Count;

			for (int i = 0; i < count; i++)
			{
				var transformation = instructionTransformations[i];

				var changed = transformContext.ApplyTransform(context, transformation, operandWorklist);

				if (!changed)
					continue;

				if (context.IsEmptyOrNop)
					return;

				i = 0;
			}
		}
	}
}
