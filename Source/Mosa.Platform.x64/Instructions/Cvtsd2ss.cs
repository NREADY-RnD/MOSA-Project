// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Cvtsd2ss
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class Cvtsd2ss : X64Instruction
	{
		public override int ID { get { return 347; } }

		internal Cvtsd2ss()
			: base(1, 1)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.OpcodeEncoder.SuppressByte(0x40);
			emitter.OpcodeEncoder.Append4Bits(0b0100);
			emitter.OpcodeEncoder.Append1Bit(0b0);
			emitter.OpcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3) & 0x1);
			emitter.OpcodeEncoder.Append1Bit(0b0);
			emitter.OpcodeEncoder.Append1Bit((node.Operand2.Register.RegisterCode >> 3) & 0x1);
			emitter.OpcodeEncoder.Append8Bits(0xF2);
			emitter.OpcodeEncoder.Append8Bits(0x0F);
			emitter.OpcodeEncoder.Append8Bits(0x5A);
			emitter.OpcodeEncoder.Append2Bits(0b11);
			emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			emitter.OpcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
		}
	}
}
