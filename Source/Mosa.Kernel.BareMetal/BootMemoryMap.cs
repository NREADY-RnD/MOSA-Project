// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class BootMemoryMap
	{
		private static IntPtr Map;

		public static IntPtr GetInitializeMemoryMapLocation()
		{
			return IntPtr.Zero;
		}

		public static void Initialize()
		{
			Map = GetInitializeMemoryMapLocation();
			Intrinsic.Store64(Map, 0);
		}

		private static IntPtr GetEntryAddress(uint index)
		{
			var offset = sizeof(int) + (BootMemoryMapEntry.EntrySize * index);
			return Map + (int)offset;
		}

		public static void SetMemoryMap(IntPtr address, ulong size, uint type)
		{
			var count = GetMemoryMapIndexCount();
			var entryPtr = GetEntryAddress(count);

			var entry = new BootMemoryMapEntry(entryPtr)
			{
				Address = address,
				Size = size,
				Type = type
			};

			Intrinsic.Store32(Map, count + 1);
		}

		public static uint GetMemoryMapIndexCount()
		{
			return Intrinsic.Load32(Map, 0);
		}

		public static BootMemoryMapEntry GetMemoryMap(uint index)
		{
			var entry = GetEntryAddress(index);

			return new BootMemoryMapEntry(entry);
		}
	}
}
