﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ConvertInt64ToFloatR4 : BaseTransformation
	{
		public ConvertInt64ToFloatR4() : base(IRInstruction.ConvertInt64ToFloatR4, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (float)context.Operand1.ConstantUnsigned64);
		}
	}
}
