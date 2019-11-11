// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.Compiler
{
	public static class CommandLineArguments
	{
		public static List<ArgumentMap> GetMap()
		{
			var map = new List<ArgumentMap>()
			{
				new ArgumentMap() { Argument = "--q", Setting = "Launcher.Exit", Value = "true"},
				new ArgumentMap() { Argument = "--autostart", Setting = "Launcher.Start", Value = "true"},
				new ArgumentMap() { Argument = "--threading", Setting = "Compiler.Multithreading", Value = "true"},
				new ArgumentMap() { Argument = "--threading-off", Setting = "Compiler.Multithreading", Value = "false"},
				new ArgumentMap() { Argument = "--settings", Setting = "Import", Value = null, IsList = true},

				new ArgumentMap() { Argument = null, Setting = "Compiler.SourceFiles", Value = null, IsList = true},

				new ArgumentMap() { Argument = "--base", Setting = "Compiler.BaseAddress"},
				new ArgumentMap() { Argument = "--scanner", Setting = "Compiler.MethodScanner", Value = "true"},
				new ArgumentMap() { Argument = "--no-code", Setting = "Compiler.EmitBinary", Value = "false"},

				new ArgumentMap() { Argument = "--output-map", Setting = "CompilerDebug.Emit.Map", Value = "true"},
				new ArgumentMap() { Argument = "--output-asm", Setting = "CompilerDebug.Emit.Asm", Value = "true"},
				new ArgumentMap() { Argument = "--output-debug", Setting = "CompilerDebug.Emit.Debug", Value = "true"},
				new ArgumentMap() { Argument = "--inline", Setting = "Optimizations.Inline", Value = "true"},
				new ArgumentMap() { Argument = "--inline-off", Setting = "Optimizations.Inline", Value = "false"},
				new ArgumentMap() { Argument = "--ssa", Setting = "Optimizations.SSA", Value = "true"},
				new ArgumentMap() { Argument = "--no-ssa", Setting = "Optimizations.SSA", Value = "false"},
				new ArgumentMap() { Argument = "--no-sparse", Setting = "Optimizations.SCCP", Value = "false"},
				new ArgumentMap() { Argument = "--sccp", Setting = "Optimizations.SCCP", Value = "true"},
				new ArgumentMap() { Argument = "--no-sccp", Setting = "Optimizations.SCCP", Value = "false"},
				new ArgumentMap() { Argument = "--no-ir-optimizations", Setting = "Optimizations.Basic", Value = "false"},
				new ArgumentMap() { Argument = "--ir-optimizations-off", Setting = "Optimizations.Basic", Value = "false"},
				new ArgumentMap() { Argument = "--inline-explicit", Setting = "Optimizations.Inline.ExplicitOnly", Value = "true"},
				new ArgumentMap() { Argument = "--long-expansion", Setting = "Optimizations.LongExpansion", Value = "true"},
				new ArgumentMap() { Argument = "--ir-long-expansion", Setting = "Optimizations.LongExpansion", Value = "true"},
				new ArgumentMap() { Argument = "--two-pass-optimizations", Setting = "Optimizations.TwoPass", Value = "true"},
				new ArgumentMap() { Argument = "--value-numbering", Setting = "Optimizations.Inline", Value = "true"},
				new ArgumentMap() { Argument = "--value-numbering-off", Setting = "Optimizations.Inline", Value = "false"},
				new ArgumentMap() { Argument = "--loop-invariant-code-motion", Setting = "Optimizations.LoopInvariantCodeMotion", Value = "true"},
				new ArgumentMap() { Argument = "--loop-invariant-code-motion-off", Setting = "Optimizations.LoopInvariantCodeMotion", Value = "false"},
				new ArgumentMap() { Argument = "--platform-optimizations", Setting = "Optimizations.Platform", Value = "true"},
				new ArgumentMap() { Argument = "--platform-optimizations-off", Setting = "Optimizations.Platform", Value = "false"},
				new ArgumentMap() { Argument = "--bit-tracker", Setting = "Optimizations.BitTracker", Value = "true"},
				new ArgumentMap() { Argument = "--bit-tracker-off", Setting = "Optimizations.BitTracker", Value = "false"},
				new ArgumentMap() { Argument = "--inline-level", Setting = "Optimizations.Inline.Maximum"},

				new ArgumentMap() { Argument = "--interrupt-method", Setting = "Advanced.InterruptMethodName"},

				new ArgumentMap() { Argument = "--output-nasm", Setting = "CompilerDebug.NasmFile", Value = "true"},
				new ArgumentMap() { Argument = "--output-asm", Setting = "CompilerDebug.AsmFile", Value = "true"},
				new ArgumentMap() { Argument = "--output-map", Setting = "CompilerDebug.MapFile", Value = "true"},
				new ArgumentMap() { Argument = "--output-time", Setting = "CompilerDebug.CompilerTimeFile", Value = "true"},
				new ArgumentMap() { Argument = "--output-debug", Setting = "CompilerDebug.DebugFile", Value = "true"},
				new ArgumentMap() { Argument = "--debugfile", Setting = "CompilerDebug.DebugFile.File"},

				new ArgumentMap() { Argument = "--platform", Setting = "Compiler.Platform"},
				new ArgumentMap() { Argument = "--x86", Setting = "Compiler.Platform", Value = "x86"},
				new ArgumentMap() { Argument = "--x64", Setting = "Compiler.Platform", Value = "x64"},
				new ArgumentMap() { Argument = "--armv8a32", Setting = "Compiler.Platform", Value = "armv8a32"},

				// Linker
				new ArgumentMap() { Argument = "--emit-all-symbols", Setting = "Linker.EmitAllSymbols", Value = "true"},
				new ArgumentMap() { Argument = "--emit-all-symbols-false", Setting = "Linker.EmitAllSymbols", Value = "false"},
				new ArgumentMap() { Argument = "--emit-relocations", Setting = "Linker.EmitStaticRelocations", Value = "true"},
				new ArgumentMap() { Argument = "--emit-relocations-false", Setting = "Linker.EmitStaticRelocations", Value = "false"},
				new ArgumentMap() { Argument = "--emit-static-relocations", Setting = "Linker.EmitStaticRelocations", Value = "true"},

				// Explorer:
				new ArgumentMap() { Argument = "--filter", Setting = "Explorer.Filter", Value = null},

				// Launched:
				new ArgumentMap() { Argument = "--dest", Setting = "Image.Destination"},
				new ArgumentMap() { Argument = "--destination-dir", Setting = "Image.Destination"},
				new ArgumentMap() { Argument = "--autostart", Setting = "Launcher.Start", Value="true"},
				new ArgumentMap() { Argument = "--launch", Setting = "Launcher.Launch", Value="true"},
				new ArgumentMap() { Argument = "--launch-off", Setting = "Launcher.Launch", Value="false"},
				new ArgumentMap() { Argument = "--e", Setting = "Launcher.Exit", Value="true"},
				new ArgumentMap() { Argument = "--q", Setting = "Launcher.Exit", Value="true"},

				// Launcher - Emulator
				new ArgumentMap() { Argument = "--emulator", Setting = "Emulator"},
				new ArgumentMap() { Argument = "--qemu", Setting = "Emulator", Value="qemu"},
				new ArgumentMap() { Argument = "--vmware", Setting = "Emulator", Value="vmware"},
				new ArgumentMap() { Argument = "--bochs", Setting = "Emulator", Value="bochs"},
				new ArgumentMap() { Argument = "--no-display", Setting = "Emulator.Display", Value = "off"},
				new ArgumentMap() { Argument = "--emulator-memory", Setting = "Emulator.Memory"},
				new ArgumentMap() { Argument = "--qemu-gdb", Setting = "Emulator.GDB", Value="false"},

				// Launcher - Image
				new ArgumentMap() { Argument = "--vhd", Setting = "Image.Format", Value="vhd"},
				new ArgumentMap() { Argument = "--img", Setting = "Image.Format", Value="img"},
				new ArgumentMap() { Argument = "--vdi", Setting = "Image.Format", Value="vdi"},
				new ArgumentMap() { Argument = "--iso", Setting = "Image.Format", Value="iso"},
				new ArgumentMap() { Argument = "--vmdk", Setting = "Image.Format", Value="vmdk"},
				new ArgumentMap() { Argument = "--image", Setting = "Image.ImageFile"},
				new ArgumentMap() { Argument = "--bootloader-image", Setting = "Image.ImageFile"},

				// Launcher - Boot
				new ArgumentMap() { Argument = "--multiboot-v1", Setting = "Multiboot.Version", Value = "v1"},
				new ArgumentMap() { Argument = "--multiboot-v2", Setting = "Multiboot.Version", Value = "v2"},
				new ArgumentMap() { Argument = "--multiboot-none", Setting = "Multiboot.Version", Value = ""},

				// Launcher - Serial
				new ArgumentMap() { Argument = "--serial-connection", Setting = "Emulator.Serial"},
				new ArgumentMap() { Argument = "--serial-pipe", Setting = "Emulator.Serial", Value = "pipe"},
				new ArgumentMap() { Argument = "--serial-tcpclient", Setting = "Emulator.Serial", Value = "tcpclient"},
				new ArgumentMap() { Argument = "--serial-tcpserver", Setting = "Emulator.Serial", Value = "tcpserver"},
				new ArgumentMap() { Argument = "--serial-connection-port", Setting = "Emulator.Serial.Port"},
				new ArgumentMap() { Argument = "--serial-connection-host", Setting = "Emulator.Serial.Host"},

				new ArgumentMap() { Argument = "--video", Setting = "Multiboot.Video", Value = "true"},
				new ArgumentMap() { Argument = "--video-width", Setting = "Multiboot.Video.Width"},
				new ArgumentMap() { Argument = "--video-height", Setting = "Multiboot.Video.Height"},
				new ArgumentMap() { Argument = "--video-depth", Setting = "Multiboot.Video.Depth"},

				new ArgumentMap() { Argument = "--gdb", Setting = "Launcher.Advance.LaunchGDBDebugger", Value="true"},
				new ArgumentMap() { Argument = "--gdb-port", Setting = "GDB.Port"},
				new ArgumentMap() { Argument = "--gdb-host", Setting = "GDB.Host"},

				new ArgumentMap() { Argument = "--launch-gdb-debugger", Setting = "Launcher.Advance.LaunchGDBDebugger", Value="true"},

				new ArgumentMap() { Argument = "--bootloader", Setting = "Image.BootLoader"},
				new ArgumentMap() { Argument = "--grub", Setting = "Image.BootLoader", Value = "grub_v0.97"},
				new ArgumentMap() { Argument = "--grub-0.97", Setting = "Image.BootLoader", Value = "grub_v0.97"},
				new ArgumentMap() { Argument = "--grub2", Setting = "Image.BootLoader", Value = "grub_v2.00"},
				new ArgumentMap() { Argument = "--syslinux", Setting = "Image.BootLoader", Value = "grub_v0.97"},
				new ArgumentMap() { Argument = "--syslinux-3.72", Setting = "Image.BootLoader", Value = "syslinux_v3.72"},
				new ArgumentMap() { Argument = "--syslinux-6.0", Setting = "Image.BootLoader", Value = "syslinux_v6.03"},

				// Launcher - Serial
				new ArgumentMap() { Argument = "--hunt-corlib", Setting = "Launcher.Advance.HuntForCorLib", Value = "true"},

				// Advance:
				new ArgumentMap() { Argument = "--plug-korlib", Setting = "Advanced.PlugKorlib", Value = "true"},

				// Debugger:
				new ArgumentMap() { Argument = "--breakpoints", Setting = "Debugger.BreakpointFile"},
				new ArgumentMap() { Argument = "--watch", Setting = "Debugger.WatchFile"},
			};

			return map;
		}
	}
}
