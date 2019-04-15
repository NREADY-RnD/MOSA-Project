// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System;

namespace Mosa.Kernel.Plug.BareMetal.x86
{
	public static class BootMemoryMapPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetInitializeMemoryMapLocation")]
		public static IntPtr GetInitializeMemoryMapLocation()
		{
			return new IntPtr(0x00007E00);
		}
	}
}
