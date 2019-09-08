// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.Simplification
{
	public sealed class Phi : BaseTransformation
	{
		public Phi() : base(IRInstruction.Phi)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return (context.OperandCount == 1);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(GetMove(context.Result), context.Result, context.Operand1);
		}

		private static BaseInstruction GetMove(Operand operand)
		{
			if (operand.IsR4)
				return IRInstruction.MoveFloatR4;
			else if (operand.IsR8)
				return IRInstruction.MoveFloatR8;
			else if (operand.Is64BitInteger)
				return IRInstruction.MoveInt64;
			else
				return IRInstruction.MoveInt32;
		}
	}
}
