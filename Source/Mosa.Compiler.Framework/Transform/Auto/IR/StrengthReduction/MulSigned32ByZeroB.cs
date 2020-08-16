// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// MulSigned32ByZeroB
	/// </summary>
	public sealed class MulSigned32ByZeroB : BaseTransformation
	{
		public MulSigned32ByZeroB() : base(IRInstruction.MulSigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var e1 = transformContext.CreateConstant(To32(0));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}
}
