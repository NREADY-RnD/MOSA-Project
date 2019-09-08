// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Transformation;

namespace Mosa.Compiler.Framework.Transformation.IR2.ConstantFolding
{
	/// <summary>
	/// DivUnsigned64
	/// </summary>
	public sealed class DivUnsigned64 : BaseTransformation
	{
		public DivUnsigned64() : base(IRInstruction.DivUnsigned64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(DivUnsigned64(ToInt64(t1), ToInt64(t2)));

			context.SetInstruction(IRInstruction.MoveInt64, result, e1);
		}
	}
}
