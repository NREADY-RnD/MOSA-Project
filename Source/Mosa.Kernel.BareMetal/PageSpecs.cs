// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	internal class PageSpecs
	{
		public static uint PageShift = Platform.GetPageShift();

		public static uint PageSize = (uint)(1 << (int)PageShift);

		public static ulong PageMask = (~(PageSize - 1));
	}
}
