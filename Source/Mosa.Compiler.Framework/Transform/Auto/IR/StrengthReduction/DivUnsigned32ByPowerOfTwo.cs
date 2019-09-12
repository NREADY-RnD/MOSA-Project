// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// DivUnsigned32ByPowerOfTwo
	/// </summary>
	public sealed class DivUnsigned32ByPowerOfTwo : BaseTransformation
	{
		public DivUnsigned32ByPowerOfTwo() : base(IRInstruction.DivUnsigned32)
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

			context.SetInstruction(IRInstruction.ShiftRight32, result, t1, e1);
		}
	}
}
