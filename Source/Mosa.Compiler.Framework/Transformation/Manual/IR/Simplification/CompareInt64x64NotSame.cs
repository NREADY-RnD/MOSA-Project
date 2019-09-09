// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.Manual.IR.Simplification
{
	public sealed class CompareInt64x64NotSame : BaseTransformation
	{
		public CompareInt64x64NotSame() : base(IRInstruction.CompareInt64x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			var condition = context.ConditionCode;

			return (condition == ConditionCode.NotEqual || condition == ConditionCode.GreaterThan || condition == ConditionCode.LessThan || condition == ConditionCode.UnsignedGreaterThan || condition == ConditionCode.UnsignedLessThan);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var operand1 = transformContext.CreateConstant(0);
			context.SetInstruction(IRInstruction.MoveInt64, context.Result, operand1);
		}
	}
}
