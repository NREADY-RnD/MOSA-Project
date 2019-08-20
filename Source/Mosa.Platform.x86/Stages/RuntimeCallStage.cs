// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class RuntimeCallStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.DivSigned64, DivSigned64);     // sdiv64
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64); // udiv64
			AddVisitation(IRInstruction.RemFloatR4, RemFloatR4);
			AddVisitation(IRInstruction.RemFloatR8, RemFloatR8);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);     // smod64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
		}

		#region Visitation Methods

		private void DivSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "sdiv64");
		}

		private void DivUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "udiv64");
		}

		private void RemFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR4");
		}

		private void RemFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR8");
		}

		private void RemSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "smod64");
		}

		private void RemUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "umod64");
		}

		#endregion Visitation Methods

		private void ReplaceWithCall(Context context, string namespaceName, string typeName, string methodName)
		{
			var method = GetMethod(namespaceName, typeName, methodName);

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			// FUTURE: throw compiler exception

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
