// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transformation.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualTransformation
	{
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt32x32 = new IR.ConstantFolding.CompareInt32x32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt32x64 = new IR.ConstantFolding.CompareInt32x64();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt64x32 = new IR.ConstantFolding.CompareInt64x32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareInt64x64 = new IR.ConstantFolding.CompareInt64x64();

		public static readonly BaseTransformation IR_LowerTo32_Add64 = new IR.LowerTo32.Add64();
		public static readonly BaseTransformation IR_Simplification_Phi32 = new IR.Simplification.Phi32();
		public static readonly BaseTransformation IR_Special_CodeInDeadBlock = new Transformation.IR.Special.CodeInDeadBlock();
		public static readonly BaseTransformation IR_Special_Deadcode = new Transformation.IR.Special.Deadcode();
	}
}