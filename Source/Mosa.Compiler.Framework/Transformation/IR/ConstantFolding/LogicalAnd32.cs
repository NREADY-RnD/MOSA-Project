﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class LogicalAnd32 : BaseTransformation
	{
		public LogicalAnd32() : base(IRInstruction.LogicalAnd32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantUnsignedLongInteger & context.Operand2.ConstantUnsignedLongInteger & 0xFFFFFFFF);
		}
	}
}
