// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Extension;

namespace Mosa.Kernel.BareMetal
{
	public static class Boot
	{
		public static void EntryPoint()
		{
			Platform.EntryPoint();

			BootMemoryMap.Initialize();

			ImportMultibootMemoryMap();
		}

		private static void ImportMultibootMemoryMap()
		{
			if (!Multiboot.IsAvailable)
				return;

			if (Multiboot.MultibootV1.MemoryMapStart.IsNull())
				return;

			var memoryMapEnd = Multiboot.MultibootV1.MemoryMapStart + (int)Multiboot.MultibootV1.MemoryMapLength;

			var entry = new MultibootV1MemoryMapEntry(Multiboot.MultibootV1.MemoryMapStart);

			while (entry.IsAvailable)
			{
				BootMemoryMapType type = BootMemoryMapType.Reserved;

				switch (entry.Type)
				{
					case 1: type = BootMemoryMapType.Available; break;
					default: type = BootMemoryMapType.Reserved; break;
				}

				BootMemoryMap.SetMemoryMap(entry.BaseAddr, entry.Length, type);

				entry = entry.GetNext(memoryMapEnd);
			}
		}
	}
}
