// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// MovLoad16
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class MovLoad16 : X64Instruction
	{
		public override int ID { get { return 398; } }

		internal MovLoad16()
			: base(1, 2)
		{
		}

		public override bool IsMemoryRead { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 5) && node.Operand2.IsConstantZero)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b01);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b101);
				emitter.OpcodeEncoder.Append8Bits(0x00);
				return;
			}

			if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 5) && node.Operand2.IsCPURegister)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit((node.Operand2.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b01);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b101);
				emitter.OpcodeEncoder.Append8Bits(0x00);
				return;
			}

			if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 4) && node.Operand2.IsConstantZero)
			{
				return;
			}

			if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 4) && (node.Operand2.IsConstant && node.Operand2.ConstantSignedInteger >= -128 && node.Operand2.ConstantSignedInteger <= 127))
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b01);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
				return;
			}

			if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 4) && node.Operand2.IsConstant)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b10);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsCPURegister)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b100);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsConstantZero)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			if (node.Operand1.IsCPURegister && (node.Operand2.IsConstant && node.Operand2.ConstantSignedInteger >= -128 && node.Operand2.ConstantSignedInteger <= 127))
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b01);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
				return;
			}

			if (node.Operand1.IsCPURegister && node.Operand2.IsConstant)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Operand1.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b10);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				emitter.OpcodeEncoder.Append32BitImmediate(node.Operand2);
				return;
			}

			if (node.Operand1.IsConstant && node.Operand2.IsConstantZero)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b101);
				emitter.OpcodeEncoder.Append32BitImmediate(node.Operand1);
				return;
			}

			if (node.Operand1.IsConstant && node.Operand2.IsConstant)
			{
				emitter.OpcodeEncoder.Append8Bits(0x66);
				emitter.OpcodeEncoder.SuppressByte(0x40);
				emitter.OpcodeEncoder.Append4Bits(0b0100);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit((node.Result.Register.RegisterCode >> 3) & 0x1);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append1Bit(0b0);
				emitter.OpcodeEncoder.Append8Bits(0x8B);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.Append3Bits(0b101);
				emitter.OpcodeEncoder.Append32BitImmediateWithOffset(node.Operand1, node.Operand2);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
