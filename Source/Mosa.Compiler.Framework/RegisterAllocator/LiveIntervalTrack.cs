﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersections, and add & evicting intervals.
	public class LiveIntervalTrack
	{
		private readonly IntervalTree<LiveInterval> intervals = new IntervalTree<LiveInterval>();

		public readonly bool IsReserved;

		public readonly PhysicalRegister Register;

		public bool IsFloatingPoint { get { return Register.IsFloatingPoint; } }

		public bool IsInteger { get { return Register.IsInteger; } }

		public LiveIntervalTrack(PhysicalRegister register, bool reserved)
		{
			Register = register;
			IsReserved = reserved;
		}

		public void Add(LiveInterval liveInterval)
		{
			Debug.Assert(!Intersects(liveInterval));

			intervals.Add(liveInterval);

			liveInterval.LiveIntervalTrack = this;
		}

		public void Evict(LiveInterval liveInterval)
		{
			intervals.Remove(liveInterval);

			liveInterval.LiveIntervalTrack = null;
		}

		public void Evict(List<LiveInterval> liveIntervals)
		{
			foreach (var interval in liveIntervals)
			{
				Evict(interval);
			}
		}

		public bool Intersects(LiveInterval liveInterval)
		{
			return intervals.Contains(liveInterval);
		}

		public bool Intersects(SlotIndex slotIndex)
		{
			return intervals.Contains(slotIndex.SlotNumber);
		}

		public LiveInterval GetLiveIntervalAt(SlotIndex slotIndex)
		{
			return intervals.SearchFirstOverlapping(slotIndex.SlotNumber);
		}

		/// <summary>
		/// Gets the intersections.
		/// </summary>
		/// <param name="liveInterval">The live interval.</param>
		/// <returns></returns>
		public List<LiveInterval> GetIntersections(LiveInterval liveInterval)
		{
			return intervals.Search(liveInterval);
		}

		public override string ToString()
		{
			return Register.ToString();
		}

		public string ToString2()
		{
			var sb = new StringBuilder();

			sb.Append(Register.ToString());
			sb.Append(' ');

			// FIX ME
			//foreach (var interval in liveIntervals)
			//{
			//	sb.Append(interval.ToString());
			//	sb.Append(", ");
			//}

			sb.Length -= 2;

			return sb.ToString();
		}
	}
}
