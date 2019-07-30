// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// CMov32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class CMov32 : X86Instruction
	{
		public override int ID { get { return 314; } }

		internal CMov32()
			: base(1, 1)
		{
		}

		public override string AlternativeName { get { return "CMov"; } }

		public override bool IsZeroFlagUsed { get { return true; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override bool IsSignFlagUsed { get { return true; } }

		public override bool IsOverflowFlagUsed { get { return true; } }

		public override bool IsParityFlagUsed { get { return true; } }

		public override bool AreFlagUseConditional { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.OpcodeEncoder.AppendByte(0x0F);
			emitter.OpcodeEncoder.AppendNibble(0b0100);
			emitter.OpcodeEncoder.AppendNibble(GetConditionCode(node.ConditionCode));
			emitter.OpcodeEncoder.Append2Bits(0b11);
			emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		}
	}
}