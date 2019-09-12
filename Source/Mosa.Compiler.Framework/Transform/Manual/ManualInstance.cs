// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualInstance
	{
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt32x32 = new IR.ConstantFolding.CompareInt32x32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt32x64 = new IR.ConstantFolding.CompareInt32x64();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt64x32 = new IR.ConstantFolding.CompareInt64x32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt64x64 = new IR.ConstantFolding.CompareInt64x64();

		public static readonly BaseTransformation IR_ConstantFolding_CompareIntBranch32 = new IR.ConstantFolding.CompareIntBranch32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareIntBranch64 = new IR.ConstantFolding.CompareIntBranch64();

		public static readonly BaseTransformation IR_ConstantMove_CompareInt32x32 = new IR.ConstantMove.CompareInt32x32();
		public static readonly BaseTransformation IR_ConstantMove_CompareInt32x64 = new IR.ConstantMove.CompareInt32x64();
		public static readonly BaseTransformation IR_ConstantMove_CompareInt64x32 = new IR.ConstantMove.CompareInt64x32();
		public static readonly BaseTransformation IR_ConstantMove_CompareInt64x64 = new IR.ConstantMove.CompareInt64x64();

		public static readonly BaseTransformation IR_Rewrite_CompareInt32x32 = new IR.Rewrite.CompareInt32x32GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_CompareInt32x64 = new IR.Rewrite.CompareInt32x64GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_CompareInt64x32 = new IR.Rewrite.CompareInt64x32GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_CompareInt64x64 = new IR.Rewrite.CompareInt64x64GreaterThanZero();

		public static readonly BaseTransformation IR_LowerTo32_Add64 = new IR.LowerTo32.Add64();
		public static readonly BaseTransformation IR_Special_CodeInDeadBlock = new Transform.IR.Special.CodeInDeadBlock();
		public static readonly BaseTransformation IR_Special_Deadcode = new Transform.IR.Special.Deadcode();

		public static readonly BaseTransformation IR_Simplification_Phi32 = new IR.Simplification.Phi32();
		public static readonly BaseTransformation IR_Simplification_Phi64 = new IR.Simplification.Phi64();
		public static readonly BaseTransformation IR_Simplification_PhiR4 = new IR.Simplification.PhiR4();
		public static readonly BaseTransformation IR_Simplification_PhiR8 = new IR.Simplification.PhiR8();

		public static readonly BaseTransformation IR_Simplification_AddCarryOut32CarryNotUsed = new IR.Simplification.AddCarryOut32CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_AddCarryOut64CarryNotUsed = new IR.Simplification.AddCarryOut64CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_SubCarryOut32CarryNotUsed = new IR.Simplification.SubCarryOut32CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_SubCarryOut64CarryNotUsed = new IR.Simplification.SubCarryOut64CarryNotUsed();

		public static readonly BaseTransformation IR_Simplification_CompareInt32x32Same = new IR.Simplification.CompareInt32x32Same();
		public static readonly BaseTransformation IR_Simplification_CompareInt32x64Same = new IR.Simplification.CompareInt32x64Same();
		public static readonly BaseTransformation IR_Simplification_CompareInt64x32Same = new IR.Simplification.CompareInt64x32Same();
		public static readonly BaseTransformation IR_Simplification_CompareInt64x64Same = new IR.Simplification.CompareInt64x64Same();

		public static readonly BaseTransformation IR_Simplification_CompareInt32x32NotSame = new IR.Simplification.CompareInt32x32NotSame();
		public static readonly BaseTransformation IR_Simplification_CompareInt32x64NotSame = new IR.Simplification.CompareInt32x64NotSame();
		public static readonly BaseTransformation IR_Simplification_CompareInt64x32NotSame = new IR.Simplification.CompareInt64x32NotSame();
		public static readonly BaseTransformation IR_Simplification_CompareInt64x64NotSame = new IR.Simplification.CompareInt64x64NotSame();

		public static readonly BaseTransformation IR_Special_Move32Propagate = new Transform.IR.Special.Move32Propagate();
		public static readonly BaseTransformation IR_Special_Move64Propagate = new Transform.IR.Special.Move64Propagate();
		public static readonly BaseTransformation IR_Special_MoveR4Propagate = new Transform.IR.Special.MoveR4Propagate();
		public static readonly BaseTransformation IR_Special_MoveR8Propagate = new Transform.IR.Special.MoveR8Propagate();

		public static readonly BaseTransformation IR_Special_Phi32Propagate = new IR.Special.Phi32Propagate();
		public static readonly BaseTransformation IR_Special_Phi64Propagate = new IR.Special.Phi64Propagate();
		public static readonly BaseTransformation IR_Special_PhiR4Propagate = new IR.Special.PhiR4Propagate();
		public static readonly BaseTransformation IR_Special_PhiR8Propagate = new IR.Special.PhiR8Propagate();

		public static readonly BaseTransformation IR_Special_Phi32Dead = new IR.Special.Phi32Dead();
		public static readonly BaseTransformation IR_Special_Phi64Dead = new IR.Special.Phi64Dead();
		public static readonly BaseTransformation IR_Special_PhiR4Dead = new IR.Special.PhiR4Dead();
		public static readonly BaseTransformation IR_Special_PhiR8Dead = new IR.Special.PhiR8Dead();

		public static readonly BaseTransformation IR_Simplification_CompareIntBranch32OnlyOnceExit = new IR.Simplification.CompareIntBranch32OnlyOnceExit();
		public static readonly BaseTransformation IR_Simplification_CompareIntBranch64OnlyOnceExit = new IR.Simplification.CompareIntBranch64OnlyOnceExit();

		public static readonly BaseTransformation IR_ConstantMove_CompareIntBranch32 = new IR.ConstantMove.CompareIntBranch32();
		public static readonly BaseTransformation IR_ConstantMove_CompareIntBranch64 = new IR.ConstantMove.CompareIntBranch64();

		public static readonly BaseTransformation IR_Rewrite_CompareIntBranch32 = new IR.Rewrite.CompareIntBranch32();
		public static readonly BaseTransformation IR_Rewrite_CompareIntBranch64 = new IR.Rewrite.CompareIntBranch64();

		public static readonly BaseTransformation IR_Rewrite_CompareIntBranch32From64 = new IR.Rewrite.CompareIntBranch32From64();
		public static readonly BaseTransformation IR_Rewrite_CompareIntBranch64From32 = new IR.Rewrite.CompareIntBranch64From32();

		public static readonly BaseTransformation IR_Special_MoveCompoundPropagate = new IR.Special.MoveCompoundPropagate();

		public static readonly BaseTransformation IR_Special_Move32PropagateConstant = new Transform.IR.Special.Move32PropagateConstant();
		public static readonly BaseTransformation IR_Special_Move64PropagateConstant = new Transform.IR.Special.Move64PropagateConstant();
	}
}
