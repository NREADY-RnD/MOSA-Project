// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Movups
	/// </summary>
	/// <seealso cref="Mosa.Platform.X64.X64Instruction" />
	public sealed class Movups : X64Instruction
	{
		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x10 } );

		internal Movups()
			: base(1, 1)
		{
		}
	}
}
