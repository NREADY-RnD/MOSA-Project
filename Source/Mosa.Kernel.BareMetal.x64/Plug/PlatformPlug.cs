// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.Plug.BareMetal.x64
{
	public static class PlatformPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
		public static uint GetPageShift() { return 12; }
	}
}
