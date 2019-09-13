// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Phi64
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class Phi64 : BaseIRInstruction
	{
		public Phi64()
			: base(0, 0)
		{
		}

		public override bool VariableOperands { get { return true; } }
	}
}
