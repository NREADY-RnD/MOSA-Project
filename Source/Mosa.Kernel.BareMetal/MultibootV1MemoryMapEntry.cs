// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	/// <summary>
	/// Multiboot V1 Memory Map
	/// </summary>
	public struct MultibootV1MemoryMapEntry
	{
		private readonly IntPtr Entry;

		#region Multiboot Memory Map Offsets

		private struct MultiBootMemoryMapOffset
		{
			public const byte Size = 0;
			public const byte BaseAddr = 4;
			public const byte Length = 12;
			public const byte Type = 20;
			public const byte Next = 24;
		}

		#endregion Multiboot Memory Map Offsets

		/// <summary>
		/// Gets a value indicating whether Multiboot V1 Memory Map is available.
		/// </summary>
		public bool IsAvailable => !Entry.IsNull();

		/// <summary>
		/// Setup Multiboot V1 Memory Map Entry.
		/// </summary>
		public MultibootV1MemoryMapEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public uint Size { get { return Intrinsic.Load32(Entry, MultiBootMemoryMapOffset.Size); } }

		public IntPtr BaseAddr { get { return Intrinsic.LoadPointer(Entry, MultiBootMemoryMapOffset.BaseAddr); } }

		public uint Length { get { return Intrinsic.Load32(Entry, MultiBootMemoryMapOffset.Length); } }

		public byte Type { get { return Intrinsic.Load8(Entry, MultiBootMemoryMapOffset.Type); } }

		public byte Next { get { return Intrinsic.Load8(Entry, MultiBootMemoryMapOffset.Next); } }

		public MultibootV1MemoryMapEntry GetNext(IntPtr memoryMapEnd)
		{
			var next = Entry + Next + sizeof(int);

			if (!next.LessThan(memoryMapEnd))
			{
				next = IntPtr.Zero;
			}

			return new MultibootV1MemoryMapEntry(next);
		}
	}
}
