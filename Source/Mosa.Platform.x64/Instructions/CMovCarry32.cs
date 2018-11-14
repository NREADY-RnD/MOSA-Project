// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// CMovCarry32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class CMovCarry32 : X64Instruction
	{
		public override int ID { get { return 659; } }

		internal CMovCarry32()
			: base(1, 1)
		{
		}

		public override string AlternativeName { get { return "CMovC32"; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.CMovNoCarry32;
		}
	}
}
