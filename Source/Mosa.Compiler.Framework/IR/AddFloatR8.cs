// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// AddFloatR8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class AddFloatR8 : BaseIRInstruction
	{
		public AddFloatR8()
			: base(2, 1)
		{
		}

		public override bool IsCommutative { get { return true; } }
	}
}
