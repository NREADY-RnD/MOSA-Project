// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.ConstantFolding
{
	/// <summary>
	/// AddWithCarry64
	/// </summary>
	public sealed class AddWithCarry64 : BaseTransformation
	{
		public AddWithCarry64() : base(IRInstruction.AddWithCarry64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand3))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;
			var t3 = context.Operand3;

			var e1 = transformContext.CreateConstant(Add64(Add64(To32(t1), To64(t2)), BoolTo64(To64(t3))));

			context.SetInstruction(IRInstruction.Move64, result, e1);
		}
	}
}
