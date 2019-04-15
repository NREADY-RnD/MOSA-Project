// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal
{
	public struct BootMemoryMapEntry
	{
		private IntPtr Entry;

		public BootMemoryMapEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public bool IsNull => Entry == IntPtr.Zero;

		public IntPtr Address
		{
			get { return Intrinsic.LoadPointer(Entry); }
			set { Intrinsic.Store(Entry, 0, value); }
		}

		public ulong Size
		{
			get { return Intrinsic.Load32(Entry, IntPtr.Size); }
			set { Intrinsic.Store64(Entry, IntPtr.Size, value); }
		}

		public uint Type
		{
			get { return Intrinsic.Load32(Entry, IntPtr.Size + sizeof(ulong)); }
			set { Intrinsic.Store64(Entry, IntPtr.Size, value); }
		}

		public void SetType(uint type)
		{
			Intrinsic.Store32(Entry, IntPtr.Size + sizeof(long), type);
		}

		public static uint EntrySize = (uint)IntPtr.Size + sizeof(ulong) + sizeof(int);
	}
}
