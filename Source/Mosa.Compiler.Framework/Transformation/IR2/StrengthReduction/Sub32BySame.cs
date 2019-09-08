// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Transformation;

namespace Mosa.Compiler.Framework.Transformation.IR2.StrengthReduction
{
	/// <summary>
	/// Sub32BySame
	/// </summary>
	public sealed class Sub32BySame : BaseTransformation
	{
		public Sub32BySame() : base(IRInstruction.Sub32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			var c1 = transformContext.CreateConstant(0L);

			context.SetInstruction(IRInstruction.MoveInt32, result, c1);
		}
	}
}
