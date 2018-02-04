// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// MovssStore
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class MovssStore : X86Instruction
	{
		internal MovssStore()
			: base(0, 3)
		{
		}

		public override bool IsMemoryWrite { get { return true; } }

		public override bool ThreeTwoAddressConversion { get { return false; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);
			System.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);

			StaticEmitters.EmitMovssStore(node, emitter);
		}

		// The following is used by the automated code generator.

		public override string __staticEmitMethod { get { return "StaticEmitters.Emit%"; } }
	}
}
