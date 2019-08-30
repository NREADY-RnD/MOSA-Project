﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ShiftLeft64 : BaseTransformation
	{
		public ShiftLeft64() : base(IRInstruction.ShiftLeft64, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantUnsignedLongInteger << (int)context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}
