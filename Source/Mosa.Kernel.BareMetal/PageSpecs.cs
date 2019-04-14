using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Kernel.BareMetal
{
	class PageSpecs
	{
		public static uint PageShift = Platform.GetPageShift();

		public static uint PageSize = (uint)(1 << (int)PageShift);

		public static ulong PageMask = (~(PageSize - 1));
	}
}
