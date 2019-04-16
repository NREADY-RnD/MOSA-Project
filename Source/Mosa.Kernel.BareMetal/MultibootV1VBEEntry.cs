// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	/// <summary>
	/// Multiboot V1 VBE Entry
	/// </summary>
	public struct MultibootV1VBEEntry
	{
		private readonly IntPtr Entry;

		#region VBE Mode Info Offsets

		internal struct VBEModeInfoOffset
		{
			public const byte Attributes = 0;
			public const byte WindowA = 2;
			public const byte WindowB = 3;
			public const byte Granularity = 4;
			public const byte WindowSize = 6;
			public const byte SegmentA = 8;
			public const byte SegmentB = 10;
			public const byte WinFuncPtr = 12;
			public const byte Pitch = 16;
			public const byte ScreenWidth = 18;
			public const byte ScreenHeight = 20;
			public const byte WChar = 22;
			public const byte YChar = 23;
			public const byte Planes = 24;
			public const byte BitsPerPixel = 25;
			public const byte Banks = 26;
			public const byte MemoryModel = 27;
			public const byte BankSize = 28;
			public const byte ImagePages = 29;
			public const byte Reserved0 = 30;
			public const byte RedMask = 31;
			public const byte RedPosition = 32;
			public const byte GreenMask = 33;
			public const byte GreenPosition = 34;
			public const byte BlueMask = 35;
			public const byte BluePosition = 36;
			public const byte ReservedMask = 37;
			public const byte ReservedPosition = 38;
			public const byte DirectColorAttributes = 39;
			public const byte PhysBase = 40;
			public const byte OffScreenMemoryOff = 44;
			public const byte OffScreenMemorSize = 48;
			public const byte Reserved1 = 50;
		}

		#endregion VBE Mode Info Offsets

		/// <summary>
		/// Gets a value indicating whether VBE is available.
		/// </summary>
		public bool IsAvailable => !Entry.IsNull();

		/// <summary>
		/// Setup Multiboot V1 VBE Entry.
		/// </summary>
		public MultibootV1VBEEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public ushort Attributes { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.Attributes); } }

		public byte WindowA { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.WindowA); } }

		public byte WindowB { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.WindowB); } }

		public ushort Granularity { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.Granularity); } }

		public ushort WindowSize { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.WindowSize); } }

		public ushort SegmentA { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.SegmentA); } }

		public ushort SegmentB { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.SegmentB); } }

		public uint WinFuncPtr { get { return Intrinsic.Load32(Entry, VBEModeInfoOffset.WinFuncPtr); } }

		public ushort Pitch { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.Pitch); } }

		public ushort ScreenWidth { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.ScreenWidth); } }

		public ushort ScreenHeight { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.ScreenHeight); } }

		public byte WChar { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.WChar); } }

		public byte YChar { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.YChar); } }

		public byte Planes { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.Planes); } }

		public byte BitsPerPixel { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.BitsPerPixel); } }

		public byte Banks { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.Banks); } }

		public byte MemoryModel { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.MemoryModel); } }

		public byte BankSize { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.BankSize); } }

		public byte ImagePages { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.ImagePages); } }

		public byte Reserved0 { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.Reserved0); } }

		public byte RedMask { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.RedMask); } }

		public byte RedPosition { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.RedPosition); } }

		public byte GreenMask { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.GreenMask); } }

		public byte GreenPosition { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.GreenPosition); } }

		public byte BlueMask { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.BlueMask); } }

		public byte BluePosition { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.BluePosition); } }

		public byte ReservedMask { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.ReservedMask); } }

		public byte ReservedPosition { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.ReservedPosition); } }

		public byte DirectColorAttributes { get { return Intrinsic.Load8(Entry, VBEModeInfoOffset.DirectColorAttributes); } }

		public IntPtr MemoryPhysicalLocation { get { return Intrinsic.LoadPointer(Entry, VBEModeInfoOffset.PhysBase); } }

		public uint OffScreenMemoryOff { get { return Intrinsic.Load32(Entry, VBEModeInfoOffset.OffScreenMemoryOff); } }

		public ushort OffScreenMemorSize { get { return Intrinsic.Load16(Entry, VBEModeInfoOffset.OffScreenMemorSize); } }
	}
}
