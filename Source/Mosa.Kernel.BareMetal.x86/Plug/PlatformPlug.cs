using Mosa.Runtime.Plug;

namespace Mosa.Kernel.Plug.BareMetal.x86
{
	public static class PlatformPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
		public static uint GetPageShift() { return 12; }
	}
}
