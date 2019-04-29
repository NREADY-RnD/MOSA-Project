﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public static class Platform
	{
		// These methods will be plugged and implemented elsewhere in the platform specific implementation

		public static uint GetPageShift()
		{
			return 0;
		}

		public static void EntryPoint()
		{
		}

		public static IntPtr GetMemoryMapAddress()
		{
			return IntPtr.Zero;
		}

		public static void UpdateBootMemoryMap()
		{
		}

		public static (IntPtr pool, int size) GetInitialGCMemoryPool()
		{
			return (new IntPtr(0x0), 0);
		}
	}
}
