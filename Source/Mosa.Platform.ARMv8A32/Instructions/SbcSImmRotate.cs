// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// SbcSImmRotate - Subtract with Carry
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class SbcSImmRotate : ARMv8A32Instruction
	{
		public override int ID { get { return 638; } }

		internal SbcSImmRotate()
			: base(1, 4)
		{
		}

		public override bool IsZeroFlagModified { get { return true; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagModified { get { return true; } }

		public override bool IsOverflowFlagModified { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 4);

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister && node.Operand3.IsConstant && node.GetOperand(3).IsConstant)
			{
				emitter.OpcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append1Bit(0b1);
				emitter.OpcodeEncoder.Append4Bits(0b0110);
				emitter.OpcodeEncoder.Append1Bit(0b1);
				emitter.OpcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				emitter.OpcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append4BitImmediate(node.Operand1);
				emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
