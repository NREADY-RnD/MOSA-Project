// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal
{
	public struct BootMemoryMapEntry
	{
		private readonly IntPtr Entry;

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

		public BootMemoryMapType Type
		{
			get { return (BootMemoryMapType)Intrinsic.Load32(Entry, IntPtr.Size + sizeof(ulong)); }
			set { Intrinsic.Store32(Entry, IntPtr.Size, (int)value); }
		}

		public static uint EntrySize = (uint)IntPtr.Size + sizeof(ulong) + sizeof(int);
	}
}
