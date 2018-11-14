// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Cdq
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class Cdq : X64Instruction
	{
		public override int ID { get { return 431; } }

		internal Cdq()
			: base(2, 1)
		{
		}

		public static readonly byte[] opcode = new byte[] { 0x99 };

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 2);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.Write(opcode);
		}
	}
}
