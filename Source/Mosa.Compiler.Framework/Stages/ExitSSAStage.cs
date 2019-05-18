// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Leave SSA Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public class ExitSSAStage : BaseMethodCompilerStage
	{
		private Counter InstructionCount = new Counter("ExitSSAStage.IRInstructions");

		protected override void Initialize()
		{
			Register(InstructionCount);
		}

		protected override void Run()
		{
			if (!HasCode)
				return;

			if (HasProtectedRegions)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					InstructionCount++;

					if (node.Instruction != IRInstruction.Phi)
						break;

					Debug.Assert(node.OperandCount == node.Block.PreviousBlocks.Count);

					ProcessPhiInstruction(node);
				}
			}

			MethodCompiler.IsInSSAForm = false;
		}

		/// <summary>
		/// Processes the phi instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void ProcessPhiInstruction(InstructionNode node)
		{
			var sourceBlocks = node.PhiBlocks;

			for (var index = 0; index < node.Block.PreviousBlocks.Count; index++)
			{
				var operand = node.GetOperand(index);
				var predecessor = sourceBlocks[index];

				InsertCopyStatement(predecessor, node.Result, operand);
			}

			node.Empty();
		}

		/// <summary>
		/// Inserts the copy statement.
		/// </summary>
		/// <param name="predecessor">The predecessor.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand">The operand.</param>
		private void InsertCopyStatement(BasicBlock predecessor, Operand result, Operand operand)
		{
			// TODO: Generalize
			var context = new Context(predecessor.Last);

			context.GotoPrevious();

			while (context.IsEmpty
				|| context.Instruction == IRInstruction.CompareIntBranch32
				|| context.Instruction == IRInstruction.CompareIntBranch64
				|| context.Instruction == IRInstruction.Jmp)
			{
				context.GotoPrevious();
			}

			var source = operand;
			var destination = result;

			if (destination != source)
			{
				if (MosaTypeLayout.IsStoredOnStack(destination.Type))
				{
					context.AppendInstruction(IRInstruction.MoveCompound, destination, source);
					context.MosaType = destination.Type;
				}
				else
				{
					var moveInstruction = GetMoveInstruction(destination.Type);
					context.AppendInstruction(moveInstruction, destination, source);
				}
			}
		}
	}
}
