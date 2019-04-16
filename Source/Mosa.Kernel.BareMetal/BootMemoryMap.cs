// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class BootMemoryMap
	{
		private static IntPtr Map;

		public static void Initialize()
		{
			Map = Platform.GetMemoryMapLocation();

			Intrinsic.Store64(Map, 0);
		}

		private static IntPtr GetEntryAddress(uint index)
		{
			var offset = sizeof(int) + (BootMemoryMapEntry.EntrySize * index);
			return Map + (int)offset;
		}

		public static void SetMemoryMap(IntPtr address, ulong size, BootMemoryMapType type)
		{
			var count = GetMemoryMapIndexCount();
			var entryPtr = GetEntryAddress(count);

			var entry = new BootMemoryMapEntry(entryPtr)
			{
				Address = address,
				Size = size,
				Type = type
			};

			Map.Store32(count + 1);
		}

		public static uint GetMemoryMapIndexCount()
		{
			return Map.Load32(0);
		}

		public static BootMemoryMapEntry GetMemoryMap(uint index)
		{
			var entry = GetEntryAddress(index);

			return new BootMemoryMapEntry(entry);
		}
	}
}
