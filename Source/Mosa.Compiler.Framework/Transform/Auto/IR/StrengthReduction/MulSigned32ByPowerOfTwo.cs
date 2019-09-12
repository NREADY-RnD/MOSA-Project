// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// MulSigned32ByPowerOfTwo
	/// </summary>
	public sealed class MulSigned32ByPowerOfTwo : BaseTransformation
	{
		public MulSigned32ByPowerOfTwo() : base(IRInstruction.MulSigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsPowerOfTwo32(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(GetPowerOfTwo(And32(To32(t2), 31u)));

			context.SetInstruction(IRInstruction.ShiftLeft32, result, t1, e1);
		}
	}
}
