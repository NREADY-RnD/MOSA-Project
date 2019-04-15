// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	public static class Platform
	{
		// GetPageShift method will be plugged elsewhere
		public static uint GetPageShift() { return 0; }
	}
}
