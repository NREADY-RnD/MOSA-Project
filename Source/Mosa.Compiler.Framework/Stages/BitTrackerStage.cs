// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class BitTrackerStage : BaseMethodCompilerStage
	{
		private const int MaxInstructions = 1024;

		private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;
		private const ulong Upper48BitsSet = ~(ulong)ushort.MaxValue;
		private const ulong Upper56BitsSet = ~(ulong)byte.MaxValue;

		#region Internal Value Class

		private struct Value
		{
			public static Value Indeterminate = new Value() { IsRangeValueIndeterminate = true, BitsSet = 0, BitsClear = 0, IsEvaluated = true };

			public bool IsEvaluated;

			public ulong MaxValue;

			public ulong MinValue;

			public ulong BitsSet;

			public ulong BitsClear;

			public bool IsRangeValueIndeterminate;

			public ulong BitsKnown { get { return BitsSet | BitsClear; } }

			public bool AreLower32BitsKnown { get { return (BitsKnown & uint.MaxValue) == uint.MaxValue; } }

			public bool AreAll64BitsKnown { get { return (BitsKnown & ulong.MaxValue) == ulong.MaxValue; } }

			public bool AreLower5BitsKnown { get { return (BitsKnown & 0b11111) == 0b11111; } }
			public bool AreLower6BitsKnown { get { return (BitsKnown & 0b111111) == 0b111111; } }
			public bool AreLower8BitsKnown { get { return (BitsKnown & byte.MaxValue) == byte.MaxValue; } }
			public bool AreLower16BitsKnown { get { return (BitsKnown & ushort.MaxValue) == ushort.MaxValue; } }

			public bool IsBitsIndeterminate { get { return BitsClear == 0 && BitsSet == 0; } }

			public bool IsIndeterminate { get { return IsRangeValueIndeterminate && IsBitsIndeterminate; } }

			public Value(ulong value)
			{
				MaxValue = value;
				MinValue = value;
				BitsSet = value;
				BitsClear = ~value;
				IsEvaluated = true;
				IsRangeValueIndeterminate = true;
			}

			public override string ToString()
			{
				if (!IsEvaluated)
					return "Not Evaluated";

				var sb = new StringBuilder();

				if (!IsRangeValueIndeterminate)
				{
					sb.Append($" MaxValue: {MaxValue.ToString()}");
					sb.Append($" MinValue: {MinValue.ToString()}");
				}

				if (BitsKnown != 0)
				{
					sb.Append($" BitsSet: {Convert.ToString((long)BitsSet, 2).PadLeft(64, '0')}");
					sb.Append($" BitsClear: {Convert.ToString((long)BitsClear, 2).PadLeft(64, '0')}");
					sb.Append($" BitsKnown: {Convert.ToString((long)BitsKnown, 2).PadLeft(64, '0')}");
				}

				return sb.ToString();
			}
		}

		#endregion Internal Value Class

		private Value[] Values;

		private TraceLog trace;

		private Counter InstructionUpdateCount = new Counter("BitTrackerStage.InstructionUpdate");

		private delegate Value NodeVisitationDelegate(InstructionNode node);

		private NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];

		protected override void Initialize()
		{
			visitation = new NodeVisitationDelegate[MaxInstructions];

			Register(InstructionUpdateCount);

			Register(IRInstruction.Phi, Phi);

			Register(IRInstruction.MoveInt32, MoveInt32);
			Register(IRInstruction.MoveInt64, MoveInt64);

			Register(IRInstruction.Truncation64x32, Truncation64x32);

			Register(IRInstruction.GetLow64, GetLow64);
			Register(IRInstruction.GetHigh64, GetHigh64);
			Register(IRInstruction.To64, To64);

			Register(IRInstruction.LogicalOr32, LogicalOr32);
			Register(IRInstruction.LogicalOr64, LogicalOr64);
			Register(IRInstruction.LogicalAnd32, LogicalAnd32);
			Register(IRInstruction.LogicalAnd64, LogicalAnd64);
			Register(IRInstruction.LogicalXor32, LogicalXor32);
			Register(IRInstruction.LogicalXor64, LogicalXor64);

			Register(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			Register(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);

			Register(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			Register(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);

			Register(IRInstruction.ShiftRight32, ShiftRight32);
			Register(IRInstruction.ShiftRight64, ShiftRight64);

			Register(IRInstruction.ShiftLeft32, ShiftLeft32);
			Register(IRInstruction.ShiftLeft64, ShiftLeft64);

			Register(IRInstruction.CompareInt32x32, CompareInt32x32);

			Register(IRInstruction.MulUnsigned32, MulUnsigned32);
			Register(IRInstruction.MulUnsigned64, MulUnsigned64);
			Register(IRInstruction.Add32, Add32);
			Register(IRInstruction.Add64, Add64);

			Register(IRInstruction.SignExtend16x32, SignExtend16x32);
			Register(IRInstruction.SignExtend8x32, SignExtend8x32);
			Register(IRInstruction.SignExtend16x64, SignExtend16x64);
			Register(IRInstruction.SignExtend8x64, SignExtend8x64);
			Register(IRInstruction.SignExtend32x64, SignExtend32x64);

			Register(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			Register(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			Register(IRInstruction.ZeroExtend16x64, ZeroExtend16x64);
			Register(IRInstruction.ZeroExtend8x64, ZeroExtend8x64);
			Register(IRInstruction.ZeroExtend32x64, ZeroExtend32x64);
		}

		private void Register(BaseInstruction instruction, NodeVisitationDelegate method)
		{
			visitation[instruction.ID] = method;
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			Values = new Value[MethodCompiler.VirtualRegisters.Count + 1];  // 0 entry is not used

			EvaluateVirtualRegisters();

			//DumpValues();

			//UpdateInstructions();
		}

		private void DumpValues()
		{
			var valueTrace = CreateTraceLog("Values", 5);

			if (valueTrace == null)
				return;

			int count = MethodCompiler.VirtualRegisters.Count;

			for (int i = 0; i < count; i++)
			{
				var virtualRegister = MethodCompiler.VirtualRegisters[i];
				var value = Values[virtualRegister.Index];

				if (value.IsIndeterminate || !value.IsEvaluated)
					continue;

				valueTrace?.Log($"Node: {virtualRegister.Definitions[0].ToString()}");

				if (!value.IsRangeValueIndeterminate)
				{
					valueTrace?.Log($"  MaxValue:  {value.MaxValue.ToString()}");
					valueTrace?.Log($"  MinValue:  {value.MinValue.ToString()}");
				}

				if (value.BitsKnown != 0)
				{
					valueTrace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
					valueTrace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
					valueTrace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");
				}

				valueTrace?.Log();
			}
		}

		private void EvaluateVirtualRegisters()
		{
			int count = MethodCompiler.VirtualRegisters.Count;
			int evaluated = 0;

			while (evaluated != count)
			{
				int startedAt = evaluated;

				for (int i = 0; i < count; i++)
				{
					if (Evaluate(MethodCompiler.VirtualRegisters[i]))
					{
						evaluated++;

						if (evaluated == count)
							return;
					}
				}

				// cycle detected - exit
				if (startedAt == evaluated)
					return;
			}
		}

		private bool Evaluate(Operand virtualRegister)
		{
			int index = virtualRegister.Index;

			if (Values[index].IsEvaluated)
				return false;

			if (virtualRegister.Uses.Count == 0 || virtualRegister.IsR || virtualRegister.Definitions.Count != 1 || virtualRegister.Definitions.Count == 0)
			{
				Values[index] = Value.Indeterminate;
				return true;
			}

			var node = virtualRegister.Definitions[0];

			// check dependencies
			foreach (var operand in node.Operands)
			{
				if (operand.IsVirtualRegister)
				{
					var operandValue = Values[operand.Index];

					if (operandValue.IsEvaluated)
						continue;

					return false; // can not evaluate yet
				}

				if (operand.IsResolvedConstant && operand.IsInteger)
					continue;

				// everything else we assume is indeterminate
				Values[index] = Value.Indeterminate;

				return true;
			}

			var method = visitation[node.Instruction.ID];

			if (method != null)
			{
				var value = method.Invoke(node);
				Values[index] = value;

				if (!value.IsIndeterminate)
				{
					UpdateInstruction(node.Result, value);
				}
			}
			else
			{
				Values[index] = Value.Indeterminate;
			}

			return true;
		}

		protected override void Finish()
		{
			Values = null;
			trace = null;
		}

		private void UpdateInstruction(Operand virtualRegister, Value value)
		{
			Debug.Assert(!value.IsIndeterminate);
			Debug.Assert(value.IsEvaluated);
			Debug.Assert(!virtualRegister.IsR);
			Debug.Assert(virtualRegister.Definitions.Count == 1);

			var node = virtualRegister.Definitions[0];

			ulong replaceValue;

			if (virtualRegister.Is64BitInteger)
			{
				if (!value.IsRangeValueIndeterminate && value.MaxValue == value.MinValue)
				{
					replaceValue = value.MaxValue;
				}
				else if ((value.BitsSet | value.BitsClear) == ulong.MaxValue)
				{
					replaceValue = value.BitsSet;
				}
				else
				{
					return;
				}
			}
			else
			{
				if (!value.IsRangeValueIndeterminate && (value.MaxValue & uint.MaxValue) == (value.MinValue & uint.MaxValue))
				{
					replaceValue = value.MaxValue & uint.MaxValue;
				}
				else if (((value.BitsSet | value.BitsClear) & uint.MaxValue) == uint.MaxValue)
				{
					replaceValue = value.BitsSet & uint.MaxValue;
				}
				else
				{
					return;
				}
			}

			var instruction = virtualRegister.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInt64 : IRInstruction.MoveInt32;
			var constant = virtualRegister.Is64BitInteger ? node.Operand1.ConstantUnsignedLongInteger : node.Operand1.ConstantUnsignedLongInteger & uint.MaxValue;

			if (node.Instruction == instruction && constant == replaceValue)
				return;

			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(instruction, virtualRegister, CreateConstant(node.Result.Type, replaceValue));
			trace?.Log($"AFTER: \t{node}");

			if (!value.IsRangeValueIndeterminate)
			{
				trace?.Log($"  MaxValue:  {value.MaxValue.ToString()}");
				trace?.Log($"  MinValue:  {value.MinValue.ToString()}");
			}

			if (value.BitsKnown != 0)
			{
				trace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
				trace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
				trace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");
			}

			trace?.Log();

			InstructionUpdateCount++;
		}

		#region Helpers

		private static bool IsAddOverflow(ulong a, ulong b)
		{
			return ((b > 0) && (a > (ulong.MaxValue - b)));
		}

		private static bool IsAddOverflow(uint a, uint b)
		{
			return ((b > 0) && (a > (uint.MaxValue - b)));
		}

		private static bool IsMultiplyOverflow(uint a, uint b)
		{
			var r = (ulong)a * (ulong)b;

			return r > uint.MaxValue;
		}

		private static bool IsMultiplyOverflow(ulong a, ulong b)
		{
			if (a == 0 | b == 0)
				return false;

			var r = a * b;
			var r2 = r / b;

			return r2 == a;
		}

		#endregion Helpers

		#region IR Instructions

		private Value MoveInt64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			return value1;
		}

		private Value MoveInt32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet & uint.MaxValue);
			}

			return value1;
		}

		private Value Phi(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != 0);

			var value = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			for (int i = 1; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);
				var value2 = operand.IsConstant ? new Value(operand.ConstantUnsignedLongInteger) : Values[operand.Index];

				value.MaxValue = Math.Max(value.MaxValue, value2.MaxValue);
				value.MinValue = Math.Min(value.MinValue, value2.MinValue);
				value.IsRangeValueIndeterminate = value.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate;
				value.BitsSet &= value2.BitsSet;
				value.BitsClear &= value2.BitsClear;
			}

			value.IsEvaluated = true;

			if (!node.Result.Is64BitInteger)
			{
				value.MaxValue &= uint.MaxValue;
				value.MinValue &= uint.MaxValue;
				value.BitsSet &= uint.MaxValue;
				value.BitsClear |= Upper32BitsSet;
			}

			return value;
		}

		private Value Truncation64x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			return new Value()
			{
				MaxValue = value1.MaxValue & uint.MaxValue,
				MinValue = value1.MinValue & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & uint.MaxValue,
				BitsClear = value1.BitsClear | Upper32BitsSet,
				IsEvaluated = true
			};
		}

		private Value GetLow64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			return new Value()
			{
				MaxValue = value1.MaxValue & uint.MaxValue,
				MinValue = value1.MinValue & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & uint.MaxValue,
				BitsClear = value1.BitsClear | Upper32BitsSet,
				IsEvaluated = true
			};
		}

		private Value GetHigh64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			return new Value()
			{
				MaxValue = (value1.MaxValue >> 32) & uint.MaxValue,
				MinValue = (value1.MinValue >> 32) & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = (value1.BitsSet >> 32) & uint.MaxValue,
				BitsClear = (value1.BitsClear >> 32) | Upper32BitsSet,
				IsEvaluated = true
			};
		}

		private Value To64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			return new Value()
			{
				MaxValue = (value2.MaxValue << 32) | (value1.MaxValue & uint.MaxValue),
				MinValue = (value2.MinValue << 32) | (value1.MinValue & uint.MaxValue),
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = (value2.BitsSet << 32) | (value1.BitsSet & uint.MaxValue),
				BitsClear = (value2.BitsClear << 32) | (value1.BitsClear & uint.MaxValue),
				IsEvaluated = true
			};
		}

		private Value ShiftRight64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return new Value(value1.BitsSet >> (int)(value2.BitsSet & 0b111111));
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower6BitsKnown)
			{
				var shift = (int)(value2.BitsSet & 0b111111);

				if (shift == 0)
				{
					return value1;
				}

				return new Value()
				{
					MaxValue = value1.MaxValue >> shift,
					MinValue = value1.MinValue >> shift,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet >> shift,
					BitsClear = value1.BitsClear >> shift | ~(ulong.MaxValue >> shift),
					IsEvaluated = true
				};
			}

			return Value.Indeterminate;
		}

		private Value ShiftRight32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return new Value((value1.BitsSet & uint.MaxValue) >> (int)(value2.BitsSet & 0b11111));
			}

			if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower5BitsKnown)
			{
				var shift = (int)(value2.BitsSet & 0b11111);

				if (shift == 0)
				{
					return value1;
				}

				return new Value()
				{
					MaxValue = value1.MaxValue >> shift,
					MinValue = value1.MinValue >> shift,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet >> shift,
					BitsClear = (value1.BitsClear >> shift) | (ulong)(~(uint.MaxValue >> shift)) | Upper32BitsSet,
					IsEvaluated = true
				};
			}

			return Value.Indeterminate;
		}

		private Value ShiftLeft64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return new Value((value1.BitsSet & uint.MaxValue) << (int)(value2.BitsSet & 0b111111));
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower6BitsKnown)
			{
				var shift = (int)(value2.BitsSet & 0b111111);

				if (shift == 0)
				{
					return value1;
				}

				return new Value()
				{
					MaxValue = value1.MaxValue << shift,
					MinValue = value1.MinValue << shift,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet << shift,
					BitsClear = (value1.BitsClear << shift) | ~(ulong.MaxValue << shift),
					IsEvaluated = true
				};
			}

			return Value.Indeterminate;
		}

		private Value ShiftLeft32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return new Value((value1.BitsSet & uint.MaxValue) << (int)(value2.BitsSet & 0b11111));
			}

			if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower5BitsKnown)
			{
				var shift = (int)(value2.BitsSet & 0b11111);

				if (shift == 0)
				{
					return value1;
				}

				return new Value()
				{
					MaxValue = value1.MaxValue << shift,
					MinValue = value1.MinValue << shift,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet << shift,
					BitsClear = value1.BitsClear << shift | ~(ulong.MaxValue << shift) | Upper32BitsSet,
					IsEvaluated = true
				};
			}

			return Value.Indeterminate;
		}

		private Value LogicalOr32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			return new Value()
			{
				MaxValue = (value1.MaxValue | value2.MaxValue) & uint.MaxValue,
				MinValue = (value1.MinValue | value2.MinValue) & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = (value1.BitsSet | value2.BitsSet) & uint.MaxValue,
				BitsClear = (value2.BitsClear & value1.BitsClear) & uint.MaxValue,
				IsEvaluated = true
			};
		}

		private Value LogicalOr64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			return new Value()
			{
				MaxValue = value1.MaxValue | value2.MaxValue,
				MinValue = value1.MinValue | value2.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet | value2.BitsSet,
				BitsClear = value2.BitsClear & value1.BitsClear,
				IsEvaluated = true
			};
		}

		private Value LogicalAnd32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			return new Value()
			{
				MaxValue = (value1.MaxValue & value2.MaxValue) & uint.MaxValue,
				MinValue = (value1.MinValue & value2.MinValue) & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = (value1.BitsSet & value2.BitsSet) & uint.MaxValue,
				BitsClear = (value2.BitsClear | value1.BitsClear) & uint.MaxValue,
				IsEvaluated = true
			};
		}

		private Value LogicalAnd64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			return new Value()
			{
				MaxValue = value1.MaxValue & value2.MaxValue,
				MinValue = value1.MinValue & value2.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & value2.BitsSet,
				BitsClear = value2.BitsClear | value1.BitsClear,
				IsEvaluated = true
			};
		}

		private Value LogicalXor32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			ulong bitsKnown = value1.BitsKnown & value2.BitsKnown & uint.MaxValue;

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				IsRangeValueIndeterminate = true,
				BitsSet = (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
				BitsClear = (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
				IsEvaluated = true
			};
		}

		private Value LogicalXor64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			ulong bitsKnown = value1.BitsKnown & value2.BitsKnown;

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate,
				BitsSet = (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
				BitsClear = (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
				IsEvaluated = true
			};
		}

		private Value LoadZeroExtend8x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				IsRangeValueIndeterminate = false,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				IsEvaluated = true,
			};
		}

		private Value LoadZeroExtend16x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				IsRangeValueIndeterminate = false,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				IsEvaluated = true
			};
		}

		private Value LoadParamZeroExtend8x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				IsRangeValueIndeterminate = false,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				IsEvaluated = true
			};
		}

		private Value LoadParamZeroExtend16x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				IsRangeValueIndeterminate = false,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				IsEvaluated = true
			};
		}

		private Value CompareInt32x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			var conditionCode = node.ConditionCode;

			bool hasResult = false;
			bool result = false;

			ulong maxValue1 = value1.MaxValue & uint.MaxValue;
			ulong maxValue2 = value2.MaxValue & uint.MaxValue;
			ulong minValue1 = value1.MinValue & uint.MaxValue;
			ulong minValue2 = value2.MinValue & uint.MaxValue;

			ulong bitsSet1 = value1.BitsSet & uint.MaxValue;
			ulong bitsSet2 = value2.BitsSet & uint.MaxValue;

			ulong bitsClear1 = value1.BitsClear & uint.MaxValue;
			ulong bitsClear2 = value2.BitsClear & uint.MaxValue;

			if (conditionCode == ConditionCode.Equal)
			{
				if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
				{
					hasResult = true;
					result = maxValue1 == maxValue2;
				}

				// if MaxValue(s) == MinValues(s) and nothing is indeterminate, then must be equal
				else if (!value1.IsRangeValueIndeterminate && !value2.IsRangeValueIndeterminate && maxValue1 == minValue1 && maxValue1 == maxValue2 && minValue1 == minValue2)
				{
					hasResult = true;
					result = true;
				}

				// if one bit doesn't match, then can not be equal (regardless of the other bits)
				else if ((((bitsSet1 & bitsSet2) != bitsSet1) || ((bitsClear1 & bitsClear2) != bitsClear1)) && value1.IsBitsIndeterminate && value2.IsBitsIndeterminate)
				{
					hasResult = true;
					result = false;
				}
			}
			else if (conditionCode == ConditionCode.NotEqual)
			{
				if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
				{
					hasResult = true;
					result = maxValue1 != maxValue2;
				}
				else if (!value1.IsRangeValueIndeterminate && !value2.IsRangeValueIndeterminate && maxValue1 == minValue1 && maxValue1 == maxValue2 && minValue1 == minValue2)
				{
					hasResult = true;
					result = false;
				}
				else if (value1.AreLower32BitsKnown && maxValue1 == 0 && bitsSet2 != 0)
				{
					hasResult = true;
					result = true;
				}
				else if (value2.AreLower32BitsKnown && maxValue2 == 0 && bitsSet1 != 0)
				{
					hasResult = true;
					result = true;
				}
			}
			else if (conditionCode == ConditionCode.UnsignedGreaterThan)
			{
				if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
				{
					hasResult = true;
					result = maxValue1 > maxValue2;
				}
				else if (value2.AreLower32BitsKnown && maxValue2 == 0 && bitsSet1 != 0)
				{
					hasResult = true;
					result = true;
				}
			}
			else if (conditionCode == ConditionCode.UnsignedLessThan)
			{
				if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
				{
					hasResult = true;
					result = maxValue1 < maxValue2;
				}
				else if (value1.AreLower32BitsKnown && maxValue1 == 0 && bitsSet2 != 0)
				{
					hasResult = true;
					result = true;
				}
			}

			return new Value()
			{
				MaxValue = result ? 1u : 0u,
				MinValue = result ? 1u : 0u,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate || !hasResult,
				BitsSet = hasResult ? (result ? 1u : 0u) : 0,
				BitsClear = hasResult ? ~(result ? 1u : 0u) : 0,
				IsEvaluated = true
			};
		}

		private Value MulUnsigned32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value((value1.BitsSet * value2.BitsSet) & uint.MaxValue);
			}

			if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower32BitsKnown && (value2.BitsSet & uint.MaxValue) == 0)
			{
				return new Value(0);
			}

			if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 1)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && (value2.BitsSet & uint.MaxValue) == 1)
			{
				return value1;
			}

			// TODO: Special power of two handling for bits, handle similar to shift left

			return new Value()
			{
				MaxValue = (value1.MaxValue * value2.MaxValue) & uint.MaxValue,
				MinValue = (value1.MinValue * value2.MinValue) & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate || IsMultiplyOverflow((uint)value1.MaxValue, (uint)value2.MaxValue),
				BitsSet = 0,
				BitsClear = Upper32BitsSet | ((value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate) ? 0 : BitTwiddling.GetClearBits(value1.MaxValue * value2.MaxValue)),
				IsEvaluated = true
			};
		}

		private Value MulUnsigned64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value(value1.BitsSet * value2.BitsSet);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
			{
				return value2;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
			{
				return value1;
			}

			// TODO: Special power of two handling for bits, handle similar to shift left
			return new Value()
			{
				MaxValue = value1.MaxValue * value2.MaxValue,
				MinValue = value1.MinValue * value2.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate || IsMultiplyOverflow(value1.MaxValue, value2.MaxValue),
				BitsSet = 0,
				BitsClear = (value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate) ? 0 : BitTwiddling.GetClearBits(value1.MaxValue * value2.MaxValue),
				IsEvaluated = true
			};
		}

		private Value Add32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value((value1.BitsSet + value2.BitsSet) & uint.MaxValue);
			}

			if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && (value2.BitsSet & uint.MaxValue) == 0)
			{
				return value1;
			}

			return new Value()
			{
				MaxValue = (value1.MaxValue + value2.MaxValue) & uint.MaxValue,
				MinValue = (value1.MinValue + value2.MinValue) & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate || IsAddOverflow((uint)value1.MaxValue, (uint)value2.MaxValue),
				BitsSet = 0,
				BitsClear = Upper32BitsSet | ((value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate) ? 0 : BitTwiddling.GetClearBits((value1.MaxValue + value2.MaxValue) & uint.MaxValue)),
				IsEvaluated = true
			};
		}

		private Value Add64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsignedLongInteger) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value(value1.BitsSet + value2.BitsSet);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return value2;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return value2;
			}

			return new Value()
			{
				MaxValue = value1.MaxValue + value2.MaxValue,
				MinValue = value1.MinValue + value2.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate || IsAddOverflow(value1.MaxValue, value2.MaxValue),
				BitsSet = 0,
				BitsClear = (value1.IsRangeValueIndeterminate || value2.IsRangeValueIndeterminate) ? 0 : BitTwiddling.GetClearBits(value1.MaxValue + value2.MaxValue),
				IsEvaluated = true
			};
		}

		private Value SignExtend16x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet & ushort.MaxValue,
					BitsClear = value1.BitsClear & ushort.MaxValue,
					IsEvaluated = true
				};
			}

			bool signedA = ((value1.BitsSet >> 15) & 1) == 1;
			bool signedB = ((value1.BitsClear >> 15) & 1) != 1;
			bool signed = signedA || signedB;

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & ushort.MaxValue | (signed ? Upper48BitsSet : 0),
				BitsClear = value1.BitsClear & ushort.MaxValue | (signed ? 0 : Upper48BitsSet),
				IsEvaluated = true
			};
		}

		private Value SignExtend16x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet & ushort.MaxValue,
					BitsClear = value1.BitsClear & ushort.MaxValue,
					IsEvaluated = true
				};
			}

			bool signedA = ((value1.BitsSet >> 15) & 1) == 1;
			bool signedB = ((value1.BitsClear >> 15) & 1) != 1;
			bool signed = signedA || signedB;

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & ushort.MaxValue | (signed ? Upper48BitsSet : 0),
				BitsClear = value1.BitsClear & ushort.MaxValue | (signed ? 0 : Upper48BitsSet),
				IsEvaluated = true
			};
		}

		private Value SignExtend8x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet & byte.MaxValue,
					BitsClear = value1.BitsClear & byte.MaxValue,
					IsEvaluated = true
				};
			}

			bool signedA = ((value1.BitsSet >> 7) & 1) == 1;
			bool signedB = ((value1.BitsClear >> 7) & 1) != 1;
			bool signed = signedA || signedB;

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & byte.MaxValue | (signed ? Upper56BitsSet : 0),
				BitsClear = value1.BitsClear & byte.MaxValue | (signed ? 0 : Upper56BitsSet),
				IsEvaluated = true
			};
		}

		private Value SignExtend8x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet & byte.MaxValue,
					BitsClear = value1.BitsClear & byte.MaxValue,
					IsEvaluated = true
				};
			}

			bool signedA = ((value1.BitsSet >> 7) & 1) == 1;
			bool signedB = ((value1.BitsClear >> 7) & 1) != 1;
			bool signed = signedA || signedB;

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & byte.MaxValue | (signed ? Upper56BitsSet : 0),
				BitsClear = value1.BitsClear & byte.MaxValue | (signed ? 0 : Upper56BitsSet),
				IsEvaluated = true
			};
		}

		private Value SignExtend32x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			bool knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
					BitsSet = value1.BitsSet & uint.MaxValue,
					BitsClear = value1.BitsClear & uint.MaxValue,
					IsEvaluated = true
				};
			}

			bool signedA = ((value1.BitsSet >> 31) & 1) == 1;
			bool signedB = ((value1.BitsClear >> 31) & 1) != 1;
			bool signed = signedA || signedB;

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & uint.MaxValue | (signed ? Upper32BitsSet : 0),
				BitsClear = value1.BitsClear & uint.MaxValue | (signed ? 0 : Upper32BitsSet),
				IsEvaluated = true
			};
		}

		private Value ZeroExtend16x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			if (value1.AreLower16BitsKnown)
			{
				return new Value(value1.MaxValue & ushort.MaxValue);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & ushort.MaxValue,
				MinValue = value1.MinValue & ushort.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & ushort.MaxValue,
				BitsClear = value1.BitsClear | Upper48BitsSet,
				IsEvaluated = true
			};
		}

		private Value ZeroExtend16x64(InstructionNode node)
		{
			return ZeroExtend16x32(node);
		}

		private Value ZeroExtend8x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			if (value1.AreLower8BitsKnown)
			{
				return new Value(value1.MaxValue & byte.MaxValue);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & byte.MaxValue,
				MinValue = value1.MinValue & byte.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & byte.MaxValue,
				BitsClear = value1.BitsClear | Upper56BitsSet,
				IsEvaluated = true
			};
		}

		private Value ZeroExtend8x64(InstructionNode node)
		{
			return ZeroExtend8x32(node);
		}

		private Value ZeroExtend32x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsignedLongInteger) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.MaxValue & uint.MaxValue);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & uint.MaxValue,
				MinValue = value1.MinValue & uint.MaxValue,
				IsRangeValueIndeterminate = value1.IsRangeValueIndeterminate,
				BitsSet = value1.BitsSet & uint.MaxValue,
				BitsClear = value1.BitsClear | Upper32BitsSet,
				IsEvaluated = true
			};
		}

		#endregion IR Instructions
	}
}
