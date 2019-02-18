﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	internal class MoveHint
	{
		public readonly SlotIndex Slot;
		public readonly VirtualRegister From;
		public readonly VirtualRegister To;

		public readonly int Bonus;

		public LiveInterval FromInterval;
		public LiveInterval ToInterval;

		public PhysicalRegister FromRegister
		{
			get
			{
				if (From.IsPhysicalRegister)
					return From.PhysicalRegister;

				if (FromInterval == null)
					return null;

				// lazy updates are allowed, so if interval doesn't touch move slot (anymore), return null
				if (FromInterval.StartSlot != Slot)
					return null;

				return FromInterval.AssignedPhysicalRegister;
			}
		}

		public PhysicalRegister ToRegister
		{
			get
			{
				if (To.IsPhysicalRegister)
					return To.PhysicalRegister;

				if (ToInterval == null)
					return null;

				// lazy updates are allowed, so if interval doesn't touch move slot (anymore), return null
				if (ToInterval.EndSlot != Slot)
					return null;

				return ToInterval.AssignedPhysicalRegister;
			}
		}

		public MoveHint(SlotIndex slot, VirtualRegister from, VirtualRegister to, int bonus)
		{
			Slot = slot;
			From = from;
			To = to;
			Bonus = bonus;
		}

		public void Update(LiveInterval interval)
		{
			LiveInterval updateInterval = interval.AssignedPhysicalRegister == null ? null : interval;

			if (interval.VirtualRegister == From)
			{
				FromInterval = updateInterval;
			}

			if (interval.VirtualRegister == To)
			{
				ToInterval = updateInterval;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Slot.ToString());

			sb.Append(" FROM: ");
			if (From.IsPhysicalRegister)
			{
				sb.Append(From.PhysicalRegister.ToString());
			}
			else
			{
				sb.AppendFormat("V_{0}", From.VirtualRegisterOperand.Index);

				if (FromRegister != null)
				{
					sb.Append(" [");
					sb.Append(FromRegister.ToString());
					sb.Append("]");
				}
			}

			sb.Append(" TO: ");
			if (To.IsPhysicalRegister)
			{
				sb.Append(To.PhysicalRegister.ToString());
			}
			else
			{
				sb.AppendFormat("V_{0}", To.VirtualRegisterOperand.Index);

				if (ToRegister != null)
				{
					sb.Append(" [");
					sb.Append(ToRegister.ToString());
					sb.Append("]");
				}
			}

			return sb.ToString();
		}
	}
}
