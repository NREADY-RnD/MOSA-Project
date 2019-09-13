// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.ConstantMove
{
	/// <summary>
	/// MulUnsigned32
	/// </summary>
	public sealed class MulUnsigned32 : BaseTransformation
	{
		public MulUnsigned32() : base(IRInstruction.MulUnsigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t2, t1);
		}
	}
}
