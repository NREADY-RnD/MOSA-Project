using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Kernel.BareMetal
{
	public static class Platform
	{
		// GetPageShift method will be plugged elsewhere
		public static uint GetPageShift() { return 0; }
	}
}
