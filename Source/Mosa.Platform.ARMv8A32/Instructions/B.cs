// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// B - Call a subroutine
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class B : ARMv8A32Instruction
	{
		internal B()
			: base(0, 1)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			if (node.Operand1.IsConstant)
			{
				emitter.OpcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				emitter.OpcodeEncoder.Append4Bits(0b1010);
				emitter.OpcodeEncoder.EmitRelative24(node.BranchTargets[0].Label);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
