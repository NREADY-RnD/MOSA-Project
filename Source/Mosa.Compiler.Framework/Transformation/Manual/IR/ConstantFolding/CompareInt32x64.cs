﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.Manual.IR.ConstantFolding
{
	public sealed class CompareInt32x64 : BaseTransformation
	{
		public CompareInt32x64() : base(IRInstruction.CompareInt32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: return true;
				case ConditionCode.NotEqual: return true;
				case ConditionCode.GreaterOrEqual: return true;
				case ConditionCode.GreaterThan: return true;
				case ConditionCode.LessOrEqual: return true;
				case ConditionCode.LessThan: return true;

				case ConditionCode.UnsignedGreaterThan: return true;
				case ConditionCode.UnsignedGreaterOrEqual: return true;
				case ConditionCode.UnsignedLessThan: return true;
				case ConditionCode.UnsignedLessOrEqual: return true;
				default: return false;
			}
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = context.Operand1.ConstantUnsigned64 == context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.NotEqual: compareResult = context.Operand1.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.GreaterOrEqual: compareResult = context.Operand1.ConstantUnsigned64 >= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.GreaterThan: compareResult = context.Operand1.ConstantUnsigned64 > context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.LessOrEqual: compareResult = context.Operand1.ConstantUnsigned64 <= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.LessThan: compareResult = context.Operand1.ConstantUnsigned64 < context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedGreaterThan: compareResult = context.Operand1.ConstantUnsigned64 > context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedGreaterOrEqual: compareResult = context.Operand1.ConstantUnsigned64 >= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedLessThan: compareResult = context.Operand1.ConstantUnsigned64 < context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedLessOrEqual: compareResult = context.Operand1.ConstantUnsigned64 <= context.Operand2.ConstantUnsigned64; break;
			}

			transformContext.SetResultToConstant(context, compareResult ? 1 : 0);
		}
	}
}
