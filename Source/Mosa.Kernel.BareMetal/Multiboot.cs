// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public static class Multiboot
	{
		/// <summary>
		/// Location of the Multiboot Structure
		/// </summary>
		public static MultibootV1 MultibootV1 { get; private set; }

		public static void Setup(IntPtr location, int magic)
		{
			if (magic == MultibootV1.MultibootMagic)
			{
				MultibootV1 = new MultibootV1(location);
			}
			else
			{
				MultibootV1 = new MultibootV1(IntPtr.Zero);
			}
		}

		/// <summary>
		/// Gets a value indicating whether multiboot is available.
		/// </summary>
		public static bool IsAvailable => MultibootV1.IsAvailable;
	}
}
