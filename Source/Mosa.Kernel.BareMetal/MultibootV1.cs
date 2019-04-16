﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public struct MultibootV1
	{
		private static IntPtr Entry;

		/// <summary>
		/// Magic value that indicates that kernel was loaded by a Multiboot V1 compliant boot loader
		/// </summary>
		public const uint MultibootMagic = 0x2BADB002;

		#region Multiboot Info Offsets

		private struct MultiBootInfoOffset
		{
			public const byte Flags = 0;
			public const byte MemLower = 4;
			public const byte MemUpper = 8;
			public const byte BootDevice = 12;
			public const byte CommandLine = 16;
			public const byte ModuleCount = 20;
			public const byte ModuleAddress = 24;
			public const byte Syms1 = 28;
			public const byte Syms2 = 32;
			public const byte Syms3 = 36;
			public const byte Syms4 = 40;
			public const byte MemMapLength = 44;
			public const byte MemMapAddress = 48;
			public const byte DriveLength = 52;
			public const byte DriveAddress = 56;
			public const byte ConfigTable = 60;
			public const byte BootLoaderName = 64;
			public const byte ApmTable = 68;
			public const byte VbeControlInfo = 72;
			public const byte VbeModeInfo = 76;
			public const byte VbeMode = 80;
			public const byte VbeInterfaceSeg = 84;
			public const byte VbeInterfaceOff = 88;
			public const byte VbeInterfaceLength = 92;
		}

		#endregion Multiboot Info Offsets

		public MultibootV1(IntPtr entry)
		{
			Entry = entry;
		}

		/// <summary>
		/// Gets a value indicating whether multiboot v1 is available.
		/// </summary>
		public bool IsAvailable => !Entry.IsNull();

		/// <summary>
		/// Gets the flags.
		/// </summary>
		public uint Flags { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.Flags); } }

		/// <summary>
		/// Gets the memory lower.
		/// </summary>
		public IntPtr MemoryLower { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.MemLower); } }

		/// <summary>
		/// Gets the memory upper.
		/// </summary>
		public IntPtr MemoryUpper { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.MemUpper); } }

		/// <summary>
		/// Gets the boot device.
		/// </summary>
		public uint BootDevice { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.BootDevice); } }

		/// <summary>
		/// Gets the command line address.
		/// </summary>
		public IntPtr CommandLineAddress { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.CommandLine); } }

		/// <summary>
		/// Gets the module count.
		/// </summary>
		public uint ModuleCount { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.ModuleCount); } }

		/// <summary>
		/// Gets the module start.
		/// </summary>
		public IntPtr ModuleStart { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.ModuleAddress); } }

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		public uint MemoryMapLength { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.MemMapLength); } }

		/// <summary>
		/// Gets the memory map start.
		/// </summary>
		public IntPtr MemoryMapStart { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.MemMapAddress); } }

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		public uint DriveLength { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.DriveLength); } }

		/// <summary>
		/// Gets the drive start.
		/// </summary>
		public uint DriveStart { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.DriveAddress); } }

		/// <summary>
		/// Gets the configuration table.
		/// </summary>
		public uint ConfigurationTable { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.ConfigTable); } }

		/// <summary>
		/// Gets the name of the boot loader address.
		/// </summary>
		public uint BootLoaderName { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.BootLoaderName); } }

		/// <summary>
		/// Gets the APM table.
		/// </summary>
		public IntPtr APMTable { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.ApmTable); } }

		/// <summary>
		/// Gets the VBE control information.
		/// </summary>
		public uint VBEControlInformation { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.VbeControlInfo); } }

		/// <summary>
		/// Gets the VBE mode info.
		/// </summary>
		public IntPtr VBEModeInfo { get { return Intrinsic.LoadPointer(Entry, MultiBootInfoOffset.VbeModeInfo); } }

		/// <summary>
		/// Gets the VBE mode.
		/// </summary>
		public uint VBEMode { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.VbeMode); } }

		/// <summary>
		/// Gets the VBE interface seg.
		/// </summary>
		public uint VBEInterfaceSeg { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.VbeInterfaceSeg); } }

		/// <summary>
		/// Gets the VBE interface off.
		/// </summary>
		public uint VBEInterfaceOff { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.VbeInterfaceOff); } }

		/// <summary>
		/// Gets the VBE interface len.
		/// </summary>
		public uint VBEInterfaceLen { get { return Intrinsic.Load32(Entry, MultiBootInfoOffset.VbeInterfaceLength); } }
	}
}
