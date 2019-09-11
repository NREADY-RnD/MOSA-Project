// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualTransformations
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			ManualTransformation.IR_ConstantFolding_CompareInt32x32,
			ManualTransformation.IR_ConstantFolding_CompareInt32x64,
			ManualTransformation.IR_ConstantFolding_CompareInt64x32,
			ManualTransformation.IR_ConstantFolding_CompareInt64x64,

			ManualTransformation.IR_ConstantMove_CompareInt32x32,
			ManualTransformation.IR_ConstantMove_CompareInt32x64,
			ManualTransformation.IR_ConstantMove_CompareInt64x32,
			ManualTransformation.IR_ConstantMove_CompareInt64x64,

			ManualTransformation.IR_Rewrite_CompareInt32x32,
			ManualTransformation.IR_Rewrite_CompareInt32x64,
			ManualTransformation.IR_Rewrite_CompareInt64x32,
			ManualTransformation.IR_Rewrite_CompareInt64x64,

			//ManualTransformation.IR_LowerTo32_Add64,
			ManualTransformation.IR_Special_CodeInDeadBlock,
			ManualTransformation.IR_Special_Deadcode,

			ManualTransformation.IR_Simplification_AddCarryOut32CarryNotUsed,
			ManualTransformation.IR_Simplification_AddCarryOut64CarryNotUsed,
			ManualTransformation.IR_Simplification_SubCarryOut32CarryNotUsed,
			ManualTransformation.IR_Simplification_SubCarryOut64CarryNotUsed,

			ManualTransformation.IR_Simplification_CompareInt32x32Same,
			ManualTransformation.IR_Simplification_CompareInt32x64Same,
			ManualTransformation.IR_Simplification_CompareInt64x32Same,
			ManualTransformation.IR_Simplification_CompareInt64x64Same,

			ManualTransformation.IR_Simplification_CompareInt32x32NotSame,
			ManualTransformation.IR_Simplification_CompareInt32x64NotSame,
			ManualTransformation.IR_Simplification_CompareInt64x32NotSame,
			ManualTransformation.IR_Simplification_CompareInt64x64NotSame,

			ManualTransformation.IR_Special_PropagateMove32,
			ManualTransformation.IR_Special_PropagateMove64,
			ManualTransformation.IR_Special_PropagateMoveR4,
			ManualTransformation.IR_Special_PropagateMoveR8,

			ManualTransformation.IR_Special_PropagatePhi32,
			ManualTransformation.IR_Special_PropagatePhi64,
			ManualTransformation.IR_Special_PropagatePhiR4,
			ManualTransformation.IR_Special_PropagatePhiR8,

			//ManualTransformation.IR_Simplification_Phi32,
			//ManualTransformation.IR_Simplification_Phi64,
			//ManualTransformation.IR_Simplification_PhiR4,
			//ManualTransformation.IR_Simplification_PhiR8,
		};
	}
}
