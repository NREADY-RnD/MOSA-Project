// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Transformation;

namespace Mosa.Compiler.Framework.Transformation.IR2.ConstantFolding
{
	/// <summary>
	/// SubWithCarry32
	/// </summary>
	public sealed class SubWithCarry32 : BaseTransformation
	{
		public SubWithCarry32() : base(IRInstruction.SubWithCarry32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand3))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;
			var t3 = context.Operand3;

			var e1 = transformContext.CreateConstant(Sub32(Sub32(ToInt32(t1), ToInt32(t2)), BoolToInt32(ToInt32(t3))));

			context.SetInstruction(IRInstruction.MoveInt32, result, e1);
		}
	}
}
