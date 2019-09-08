// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Transformation;

namespace Mosa.Compiler.Framework.Transformation.IR2.ConstantFolding
{
	/// <summary>
	/// LogicalNot64
	/// </summary>
	public sealed class LogicalNot64 : BaseTransformation
	{
		public LogicalNot64() : base(IRInstruction.LogicalNot64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			var e1 = transformContext.CreateConstant(Not64(ToInt64(t1)));

			context.SetInstruction(IRInstruction.MoveInt64, result, e1);
		}
	}
}
