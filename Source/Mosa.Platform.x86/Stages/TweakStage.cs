// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class TweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.CallReg, CallReg);
			AddVisitation(X86.Cmp32, Cmp32);
			AddVisitation(X86.Sar32, ConvertShiftConstantToByte);
			AddVisitation(X86.Shl32, ConvertShiftConstantToByte);
			AddVisitation(X86.Shr32, ConvertShiftConstantToByte);
			AddVisitation(X86.SarConst32, ConvertShiftConstantToByte);
			AddVisitation(X86.ShlConst32, ConvertShiftConstantToByte);
			AddVisitation(X86.ShrConst32, ConvertShiftConstantToByte);
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void CallReg(Context context)
		{
			// FIXME: Result operand should be used instead of Operand1 for the result
			// FIXME: Move to FixedRegisterAssignmentStage
			if (!context.Operand1.IsCPURegister)
			{
				Context before = context.InsertBefore();
				Operand v1 = AllocateVirtualRegister(context.Operand1.Type);

				before.SetInstruction(X86.Mov, v1, context.Operand1);
				context.Operand1 = v1;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Cmp32(Context context)
		{
			Operand left = context.Operand1;

			if (left.IsConstant)
			{
				Operand v1 = AllocateVirtualRegister(left.Type);

				context.InsertBefore().AppendInstruction(X86.Mov, v1, left);
				context.Operand1 = v1;
			}
		}

		#endregion Visitation Methods

		/// <summary>
		/// Adjusts the shift constant.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConvertShiftConstantToByte(Context context)
		{
			if (!context.Operand2.IsConstant || context.Operand2.IsByte)
				return;

			context.Operand2 = CreateConstant(TypeSystem.BuiltIn.U1, context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}