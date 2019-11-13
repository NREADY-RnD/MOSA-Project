// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class LauncherOptionsV2
	{
		public Settings Settings { get; set; }

		public string DestinationDirectory
		{
			get { return Settings.GetValue("Image.Destination", null); }
			set { Settings.SetValue("Image.Destination", value); }
		}

		public bool AutoStart
		{
			get { return Settings.GetValue("Launcher.Start", false); }
			set { Settings.SetValue("Launcher.Start", value); }
		}

		public bool LaunchVM
		{
			get { return Settings.GetValue("Launcher.Launch", false); }
			set { Settings.SetValue("Launcher.Launch", value); }
		}

		public bool ExitOnLaunch
		{
			get { return Settings.GetValue("Launcher.Exit", false); }
			set { Settings.SetValue("Launcher.Exit", value); }
		}

		public string Emulator
		{
			get { return Settings.GetValue("Emulator", null); }
			set { Settings.SetValue("Emulator", value); }
		}

		public ImageFormat ImageFormat { get; set; }

		public int EmulatorMemoryInMB
		{
			get { return Settings.GetValue("Emulator.Memory", 128); }
			set { Settings.SetValue("Emulator.Memory", value); }
		}

		public string InterruptMethodName
		{
			get { return Settings.GetValue("X86.InterruptMethodName", null); }
			set { Settings.SetValue("X86.InterruptMethodName", value); }
		}

		public bool EnableSSA
		{
			get { return Settings.GetValue("Launcher.SSA", false); }
			set { Settings.SetValue("Launcher.SSA", value); }
		}

		public bool EnableBasicOptimizations
		{
			get { return Settings.GetValue("Optimizations.Basic", false); }
			set { Settings.SetValue("Optimizations.Basic", value); }
		}

		public bool EnableSparseConditionalConstantPropagation
		{
			get { return Settings.GetValue("Optimizations.SCCP", false); }
			set { Settings.SetValue("Optimizations.SCCP", value); }
		}

		public bool EnableInlineMethods
		{
			get { return Settings.GetValue("Optimizations.Inline", false); }
			set { Settings.SetValue("Optimizations.Inline", value); }
		}

		public bool InlineExplicitOnly
		{
			get { return Settings.GetValue("Optimizations.Inline.ExplicitOnly", false); }
			set { Settings.SetValue("Optimizations.Inline.ExplicitOnly", value); }
		}

		public bool EnableLongExpansion
		{
			get { return Settings.GetValue("Optimizations.LongExpansion", false); }
			set { Settings.SetValue("Optimizations.LongExpansion", value); }
		}

		public bool TwoPassOptimizations
		{
			get { return Settings.GetValue("Optimizations.TwoPass", false); }
			set { Settings.SetValue("Optimizations.TwoPass", value); }
		}

		public bool EnableValueNumbering
		{
			get { return Settings.GetValue("Optimizations.ValueNumbering", false); }
			set { Settings.SetValue("Optimizations.ValueNumbering", value); }
		}

		public bool EnableLoopInvariantCodeMotion
		{
			get { return Settings.GetValue("Optimizations.LoopInvariantCodeMotion", false); }
			set { Settings.SetValue("Optimizations.LoopInvariantCodeMotion", value); }
		}

		public bool EnablePlatformOptimizations
		{
			get { return Settings.GetValue("Optimizations.Platform", false); }
			set { Settings.SetValue("Optimizations.Platform", value); }
		}

		public bool EnableBitTracker
		{
			get { return Settings.GetValue("Optimizations.BitTracker", false); }
			set { Settings.SetValue("Optimizations.BitTracker", value); }
		}

		public int InlineMaximum
		{
			get { return Settings.GetValue("Optimizations.Inline.Maximum", 12); }
			set { Settings.SetValue("Optimizations.Inline.Maximum", value); }
		}

		public bool GenerateNASMFile
		{
			get { return Settings.GetValue("CompilerDebug.NasmFile", false); }
			set { Settings.SetValue("CompilerDebug.NasmFile", value); }
		}

		public bool GenerateASMFile
		{
			get { return Settings.GetValue("CompilerDebug.AsmFile", false); }
			set { Settings.SetValue("CompilerDebug.AsmFile", value); }
		}

		public bool GenerateMapFile
		{
			get { return Settings.GetValue("CompilerDebug.MapFile", false); }
			set { Settings.SetValue("CompilerDebug.MapFile", value); }
		}

		public bool GenerateDebugFile
		{
			get { return Settings.GetValue("CompilerDebug.DebugFile", false); }
			set { Settings.SetValue("CompilerDebug.DebugFile", value); }
		}

		public bool GenerateCompileTimeFile
		{
			get { return Settings.GetValue("CompilerDebug.CompileTimeFile", false); }
			set { Settings.SetValue("CompilerDebug.CompileTimeFile", value); }
		}

		public bool NoDisplay
		{
			get { return Settings.GetValue("Emulator.Display", false); }
			set { Settings.SetValue("Emulator.Display", value); }
		}

		public int SerialConnectionPort
		{
			get { return Settings.GetValue("Emulator.Serial.Port", 1234); }
			set { Settings.SetValue("Emulator.Serial.Port", value); }
		}

		public string SerialConnectionHost
		{
			get { return Settings.GetValue("Emulator.Serial.Host", "localhost"); }
			set { Settings.SetValue("Emulator.Serial.Host", value); }
		}

		public string SerialPipeName
		{
			get { return Settings.GetValue("Emulator.Serial.Pipe", "Mosa"); }
			set { Settings.SetValue("Emulator.Serial.Pipe", value); }
		}

		public bool EnableMultiThreading
		{
			get { return Settings.GetValue("Compiler.Multithreading", false); }
			set { Settings.SetValue("Compiler.Multithreading", value); }
		}

		public bool VBEVideo
		{
			get { return Settings.GetValue("Multiboot.Video", false); }
			set { Settings.SetValue("Multiboot.Video", value); }
		}

		public int Width
		{
			get { return Settings.GetValue("Multiboot.Width", 640); }
			set { Settings.SetValue("Multiboot.Width", value); }
		}

		public int Height
		{
			get { return Settings.GetValue("Multiboot.Height", 480); }
			set { Settings.SetValue("Multiboot.Height", value); }
		}

		public int Depth
		{
			get { return Settings.GetValue("Multiboot.Depth", 32); }
			set { Settings.SetValue("Multiboot.Depth", value); }
		}

		public long BaseAddress
		{
			get { return Settings.GetValue("Compiler.BaseAddress", 0x00400000); }
			set { Settings.SetValue("Compiler.BaseAddress", value); }
		}

		public bool EmitAllSymbols
		{
			get { return Settings.GetValue("Linker.EmitAllSymbols", false); }
			set { Settings.SetValue("Linker.EmitAllSymbols", value); }
		}

		public bool EmitStaticRelocations
		{
			get { return Settings.GetValue("Linker.EmitStaticRelocations", false); }
			set { Settings.SetValue("Linker.EmitStaticRelocations", value); }
		}

		public string BootLoaderImage
		{
			get { return Settings.GetValue("Image.BootLoader", null); }
			set { Settings.SetValue("Image.BootLoader", value); }
		}

		public bool EnableQemuGDB
		{
			get { return Settings.GetValue("Emulator.GDB", false); }
			set { Settings.SetValue("Emulator.GDB", value); }
		}

		public bool LaunchGDB
		{
			get { return Settings.GetValue("Launcher.Advance.LaunchGDB", false); }
			set { Settings.SetValue("Launcher.Advance.LaunchGDB", value); }
		}

		public bool LaunchGDBDebugger
		{
			get { return Settings.GetValue("Launcher.Advance.LaunchGDBDebugger", false); }
			set { Settings.SetValue("Launcher.Advance.LaunchGDBDebugger", value); }
		}

		public int GDBPort
		{
			get { return Settings.GetValue("GDB.Port", 1234); }
			set { Settings.SetValue("GDB.Port", value); }
		}

		public string GDBHost
		{
			get { return Settings.GetValue("GDB.Host", "localhost"); }
			set { Settings.SetValue("GDB.Host", value); }
		}

		private string _ImageFile;

		public string ImageFile
		{
			get { return Settings.GetValue("Image.ImageFile", null); }
			set { Settings.SetValue("Image.ImageFile", value); }
		}

		public string DebugFile
		{
			get { return Settings.GetValue("CompilerDebug.DebugFile.File", "%DEFAULT%"); }
			set { Settings.SetValue("CompilerDebug.DebugFile.File", value); }
		}

		public string BreakpointFile
		{
			get { return Settings.GetValue("Debugger.BreakpointFile", null); }
			set { Settings.SetValue("Debugger.BreakpointFile", value); }
		}

		public string WatchFile
		{
			get { return Settings.GetValue("Debugger.WatchFile", null); }
			set { Settings.SetValue("Debugger.WatchFile", value); }
		}

		public bool PlugKorlib
		{
			get { return Settings.GetValue("Compiler.Advanced.PlugKorlib", true); }
			set { Settings.SetValue("Compiler.Advanced.PlugKorlib", value); }
		}

		public bool EnableMethodScanner
		{
			get { return Settings.GetValue("Compiler.MethodScanner", true); }
			set { Settings.SetValue("Compiler.MethodScanner", value); }
		}

		public string LinkerFormat
		{
			get { return Settings.GetValue("Linker.Type", "elf32"); }
			set { Settings.SetValue("Linker.Type", value); }
		}

		public string MultibootSpecification
		{
			get { return Settings.GetValue("Multiboot.Version", string.Empty); }
			set { Settings.SetValue("Multiboot.Version", value); }
		}

		public string FileSystem
		{
			get { return Settings.GetValue("Image.FileSystem", string.Empty); }
			set { Settings.SetValue("Image.FileSystem", value); }
		}

		public PlatformType PlatformType { get; set; }

		public BootLoader BootLoader { get; set; }

		public SerialConnectionOption SerialConnectionOption { get; set; }

		public List<IncludeFile> IncludeFiles { get; set; }

		public List<string> Paths
		{
			get { return Settings.GetValueList("Compiler.MethodScanner"); }
		}

		public string SourceFile;

		public bool HuntForCorLib
		{
			get { return Settings.GetValue("Launcher.Advance.HuntForCorLib", false); }
			set { Settings.SetValue("Launcher.Advance.HuntForCorLib", value); }
		}

		public LauncherOptionsV2()
		{
			IncludeFiles = new List<IncludeFile>();
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			BootLoader = BootLoader.Syslinux_3_72; // Can't use the Default in the attribute because it would overwrite other bootloader options
			SerialConnectionOption = SerialConnectionOption.None;
			Emulator = "Qemu";
			ImageFormat = ImageFormat.IMG;
			LinkerFormat = "elf32";
			PlatformType = PlatformType.x86;
			FileSystem = "FAT16";
			BaseAddress = 0x00400000;
			SerialConnectionHost = "127.0.0.1";
			InlineMaximum = 12;
			LaunchVM = true;
			EnableLongExpansion = true;
			TwoPassOptimizations = true;
			EnableValueNumbering = true;
			LaunchGDB = false;
			GDBPort = 1234;
			GDBHost = "localhost";
			HuntForCorLib = true;
			EmulatorMemoryInMB = 256;
			SerialConnectionPort = 9999;
			GenerateASMFile = false;
			EnableSSA = true;
			EnableBasicOptimizations = true;
			EnableSparseConditionalConstantPropagation = true;
			EnableInlineMethods = true;
			EnableLongExpansion = true;
			EnableLoopInvariantCodeMotion = true;
			EnablePlatformOptimizations = true;
			TwoPassOptimizations = true;
			EnableValueNumbering = true;
			EnableBitTracker = true;
			MultibootSpecification = "v1";
			PlugKorlib = true;
		}
	}
}
