// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// StoreParam64
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class StoreParam64 : BaseIRInstruction
	{
		public StoreParam64()
			: base(2, 0)
		{
		}

		public override bool IsMemoryWrite { get { return true; } }

		public override bool IsParameterStore { get { return true; } }
	}
}
