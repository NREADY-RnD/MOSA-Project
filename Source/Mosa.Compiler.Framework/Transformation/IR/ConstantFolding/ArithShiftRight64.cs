﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class ArithShiftRight64 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.ArithShiftRight64; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (ulong)(((long)context.Operand1.ConstantUnsignedLongInteger) >> (int)context.Operand2.ConstantUnsignedLongInteger));
		}
	}
}
