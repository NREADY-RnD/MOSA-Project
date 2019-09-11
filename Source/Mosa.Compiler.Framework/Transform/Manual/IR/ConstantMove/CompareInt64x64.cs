﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.ConstantMove
{
	public sealed class CompareInt64x64 : BaseTransformation
	{
		public CompareInt64x64() : base(IRInstruction.CompareInt64x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (IsConstant(context.Operand2))
				return false;

			if (!IsConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.CompareInt64x64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
		}
	}
}
