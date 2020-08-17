// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.Algebraic
{
	/// <summary>
	/// Signed64AABBPlus2AB
	/// </summary>
	public sealed class Signed64AABBPlus2AB : BaseTransformation
	{
		public Signed64AABBPlus2AB() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Add64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv1
	/// </summary>
	public sealed class Signed64AABBPlus2ABv1 : BaseTransformation
	{
		public Signed64AABBPlus2ABv1() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Add64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv2
	/// </summary>
	public sealed class Signed64AABBPlus2ABv2 : BaseTransformation
	{
		public Signed64AABBPlus2ABv2() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Add64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv3
	/// </summary>
	public sealed class Signed64AABBPlus2ABv3 : BaseTransformation
	{
		public Signed64AABBPlus2ABv3() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Add64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv4
	/// </summary>
	public sealed class Signed64AABBPlus2ABv4 : BaseTransformation
	{
		public Signed64AABBPlus2ABv4() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Add64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv5
	/// </summary>
	public sealed class Signed64AABBPlus2ABv5 : BaseTransformation
	{
		public Signed64AABBPlus2ABv5() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Add64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv6
	/// </summary>
	public sealed class Signed64AABBPlus2ABv6 : BaseTransformation
	{
		public Signed64AABBPlus2ABv6() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Add64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AABBPlus2ABv7
	/// </summary>
	public sealed class Signed64AABBPlus2ABv7 : BaseTransformation
	{
		public Signed64AABBPlus2ABv7() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);
			var v2 = transformContext.AllocateVirtualRegister(transformContext.I8);

			context.SetInstruction(IRInstruction.Add64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Add64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}
}
