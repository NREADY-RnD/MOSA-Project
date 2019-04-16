// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System;

namespace Mosa.Kernel.BareMetal.x64
{
	public static class PlatformPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
		public static uint GetPageShift()
		{
			return 12;
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::EntryPoint")]
		public static void EntryPoint()
		{
			// TODO: Get EAX and EBX
			Multiboot.Setup(IntPtr.Zero, 0);

			// TODO: SSE
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetMemoryMapLocation")]
		public static IntPtr GetMemoryMapLocation()
		{
			return new IntPtr(0x00007E00);
		}
	}
}
