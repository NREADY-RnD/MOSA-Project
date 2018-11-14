// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// CMovZero32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class CMovZero32 : X64Instruction
	{
		public override int ID { get { return 664; } }

		internal CMovZero32()
			: base(1, 1)
		{
		}

		public override string AlternativeName { get { return "CMovZ32"; } }

		public override bool IsZeroFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.CMovNotZero32;
		}
	}
}
