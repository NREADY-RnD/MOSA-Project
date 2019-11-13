// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class LauncherOptions
	{
		[Option("dest")]
		public string DestinationDirectory { get; set; }

		[Option("destination-dir")]
		public string DestinationDirectoryAlt { set { DestinationDirectory = value; } }

		[Option("autostart")]
		public bool AutoStart { get; set; }

		[Option('l', "launch")]
		public bool LaunchVM { get; set; }

		[Option("launch-off")]
		public bool LaunchVMFalse { set { LaunchVM = false; } }

		[Option('e')]
		public bool ExitOnLaunch { get; set; }

		[Option('q')]
		public bool ExitOnLaunchFalse { set { ExitOnLaunch = value; } }

		[Option("emulator")]
		public string Emulator { get; set; }

		[Option("qemu")]
		public bool EmulatorQEMU { set { Emulator = "Qemu"; } }

		[Option("vmware")]
		public bool EmulatorVMware { set { Emulator = "VMware"; } }

		[Option("bochs")]
		public bool EmulatorBochs { set { Emulator = "Bochs"; } }

		[Option("image-format")]
		public string ImageFormat { get; set; }

		[Option("vhd")]
		public bool ImageFormatVHD { set { ImageFormat = "VHD"; } }

		[Option("img")]
		public bool ImageFormatIMG { set { ImageFormat = "IMG"; } }

		[Option("vdi")]
		public bool ImageFormatVDI { set { ImageFormat = "VDI"; } }

		[Option("iso")]
		public bool ImageFormatISO { set { ImageFormat = "ISO"; } }

		[Option("vmdk")]
		public bool ImageFormatVMDK { set { ImageFormat = "VMDK"; } }

		[Option("emulator-memory", HelpText = "Emulator memory in megabytes.")]
		public uint EmulatorMemoryInMB { get; set; }

		[Option("interrupt-method")]
		public string InterruptMethodName { get; set; }

		[Option("ssa")]
		public bool EnableSSA { get; set; }

		[Option("ir-optimizations")]
		public bool EnableBasicOptimizations { get; set; }

		[Option("ir-optimizations-off")]
		public bool IROptimizationsFalse { set { EnableBasicOptimizations = false; } }

		[Option("sccp")]
		public bool EnableSparseConditionalConstantPropagation { get; set; }

		[Option("sccp-off")]
		public bool EnableSparseConditionalConstantPropagationFalse { set { EnableSparseConditionalConstantPropagation = false; } }

		[Option("inline")]
		public bool EnableInlineMethods { get; set; }

		[Option("inline-off")]
		public bool EnableInlineFalse { set { EnableInlineMethods = false; } }

		[Option("inline-explicit")]
		public bool InlineExplicitOnly { get; set; }

		[Option("long-expansion")]
		public bool EnableLongExpansion { get; set; }

		[Option("ir-long-expansion")]
		public bool EnableLongExpansion2 { set { EnableLongExpansion = true; } }

		// Legacy - will be removed in the future
		[Option("two-pass-optimizations")]
		public bool TwoPassOptimizations { get; set; }

		[Option("value-numbering")]
		public bool EnableValueNumbering { get; set; }

		[Option("value-numbering-off")]
		public bool ValueNumberingFalse { set { EnableValueNumbering = false; } }

		[Option("loop-invariant-code-motion")]
		public bool EnableLoopInvariantCodeMotion { get; set; }

		[Option("loop-invariant-code-motion-off")]
		public bool EnableLoopInvariantCodeMotionFalse { set { EnableLoopInvariantCodeMotion = false; } }

		[Option("platform-optimizations")]
		public bool EnablePlatformOptimizations { get; set; }

		[Option("platform-optimizations-off")]
		public bool EnablePlatformOptimizationsFalse { set { EnablePlatformOptimizations = false; } }

		[Option("bit-tracker")]
		public bool EnableBitTracker { get; set; }

		[Option("bit-tracker-off")]
		public bool EnableBitTrackerFalse { set { EnableBitTracker = false; } }

		public int InlineMaximum { get; set; }

		[Option("inline-level")]
		public string InlinedMaximumHelper { set { InlineMaximum = (int)value.ParseHexOrInteger(); } }

		[Option("all-optimizations-off")]
		public bool AllOptimizationsOff
		{
			set
			{
				EnableSSA = false;
				EnableBasicOptimizations = false;
				EnableInlineMethods = false;
				TwoPassOptimizations = false;
				EnableLongExpansion = false;
				EnableSparseConditionalConstantPropagation = false;
				EnableValueNumbering = false;
				EnableBitTracker = false;
				EnableLoopInvariantCodeMotion = false;
				EnablePlatformOptimizations = false;
			}
		}

		[Option("output-nasm")]
		public bool GenerateNASMFile { get; set; }

		[Option("output-asm")]
		public bool GenerateASMFile { get; set; }

		[Option("output-map")]
		public bool GenerateMapFile { get; set; }

		[Option("output-debug")]
		public bool GenerateDebugFile { get; set; }

		[Option("output-time")]
		public bool GenerateCompileTimeFile { get; set; }

		[Option("no-display")]
		public bool NoDisplay { get; set; }

		[Option("platform")]
		public PlatformType PlatformType { get; set; }

		[Option("file-system")]
		public string FileSystem { get; set; }

		[Option("serial-connection")]
		public string SerialConnection { get; set; }

		[Option("serial-pipe")]
		public bool SerialConnectionPipe { set { SerialConnection = "Pipe"; } }

		[Option("serial-tcpclient")]
		public bool SerialConnectionTCPClient { set { SerialConnection = "TCPClient"; } }

		[Option("serial-tcpserver")]
		public bool SerialConnectionTCPServer { set { SerialConnection = "TCPServer"; } }

		[Option("serial-connection-port")]
		public int SerialConnectionPort { get; set; }

		[Option("serial-connection-host")]
		public string SerialConnectionHost { get; set; }

		[Option("serial-pipe-name")]
		public string SerialPipeName { get; set; } = "MOSA";

		[Option("threading")]
		public bool MultiThreading { get; set; } = true;

		[Option("threading-off")]
		public bool UseMultiThreadingCompilerFalse { set { MultiThreading = false; } }

		[Option("bootloader")]
		public BootLoader BootLoader { get; set; }

		[Option("grub")]
		public bool BootLoaderGRUB { set { BootLoader = BootLoader.Grub_0_97; } }

		[Option("grub-0.97")]
		public bool BootLoaderGRUB97 { set { BootLoader = BootLoader.Grub_0_97; } }

		[Option("grub2")]
		public bool BootLoaderGRUB2 { set { BootLoader = BootLoader.Grub_2_00; } }

		[Option("syslinux")]
		public bool BootLoaderSyslinux { set { BootLoader = BootLoader.Syslinux_6_03; } }

		[Option("syslinux-6.03")]
		public bool BootLoaderSyslinux603 { set { BootLoader = BootLoader.Syslinux_6_03; } }

		[Option("syslinux-3.72")]
		public bool BootLoaderSyslinux372 { set { BootLoader = BootLoader.Syslinux_3_72; } }

		[Option("video")]
		public bool VBEVideo { get; set; }

		[Option("video-width")]
		public int Width { get; set; } = 640;

		[Option("video-height")]
		public int Height { get; set; } = 480;

		[Option("video-depth")]
		public int Depth { get; set; } = 32;

		public ulong BaseAddress { get; set; }

		[Option("base")]
		public string BaseAddressHelper { set { BaseAddress = value.ParseHexOrInteger(); } }

		[Option("emit-all-symbols")]
		public bool EmitAllSymbols { get; set; } = false;

		[Option("emit-all-symbols-false")]
		public bool EmitAllSymbolsFalse { set { EmitAllSymbols = false; } }

		[Option("emit-static-relocations")]
		public bool EmitStaticRelocations { get; set; }

		[Option("emit-relocations-false")]
		public bool EmitRelocationsFalse { set { EmitStaticRelocations = false; } }

		[Option("bootloader-image")]
		public string BootLoaderImage { get; set; }

		[Option("qemu-gdb")]
		public bool EnableQemuGDB { get; set; }

		[Option("gdb")]
		public bool LaunchGDB { get; set; }

		[Option("gdb-port")]
		public int GDBPort { get; set; }

		[Option("gdb-host")]
		public string GDBHost { get; set; }

		[Option("launch-gdb-debugger")]
		public bool LaunchGDBDebugger { get; set; }

		private string _ImageFile;

		[Option("image")]
		public string ImageFile
		{
			get
			{
				return _ImageFile;
			}
			set
			{
				_ImageFile = value;

				if (value.EndsWith(".bin"))
				{
					ImageFormat = "BIN";
				}
			}
		}

		[Option("debugfile")]
		public string DebugFile { get; set; }

		[Option("breakpoints")]
		public string BreakpointFile { get; set; }

		[Option("watch")]
		public string WatchFile { get; set; }

		[Option("plug-korlib")]
		public bool PlugKorlib { get; set; }

		[Option("scanner")]
		public bool EnableMethodScanner { get; set; }

		public List<IncludeFile> IncludeFiles { get; set; }

		public List<string> Paths { get; set; }

		[Option("file", HelpText = "Path to a file which contains files to be included in the generated image file.")]
		public string IncludeFileHelper
		{
			set
			{
				if (!File.Exists(value))
				{
					Console.WriteLine("File doesn't exist \"" + value + "\"");
					return;
				}

				ReadIncludeFile(value);
			}
		}

		[Option("path")]
		public string PathHelper
		{
			set
			{
				if (!Paths.Contains(value))
				{
					Paths.Add(value);
				}
			}
		}

		public string SourceFile;

		[Value(0)]
		public string SourceFileHelper
		{
			set
			{
				if (value.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					SourceFile = value;
				}
				else
				{
					SourceFile = Path.Combine(Directory.GetCurrentDirectory(), value);
				}
			}
		}

		[Option("hunt-corlib")]
		public bool HuntForCorLib { get; set; }

		public MosaLinker.CreateExtraSectionsDelegate CreateExtraSections { get; set; }

		public MosaLinker.CreateExtraProgramHeaderDelegate CreateExtraProgramHeaders { get; set; }

		public LauncherOptions()
		{
			IncludeFiles = new List<IncludeFile>();
			Paths = new List<string>();
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			BootLoader = BootLoader.Syslinux_3_72; // Can't use the Default in the attribute because it would overwrite other bootloader options
			SerialConnection = "None";
			Emulator = "Qemu";
			ImageFormat = "IMG";
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
			EmulatorMemoryInMB = 256U;
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
			PlugKorlib = true;
		}

		private void ReadIncludeFile(string file)
		{
			string line;

			using (var reader = new StreamReader(file))
			{
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine();

					if (string.IsNullOrEmpty(line))
						continue;

					var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (parts.Length == 0)
						continue;

					if (!File.Exists(parts[0]))
					{
						Console.WriteLine("File not found \"" + parts[0] + "\"");
						continue;
					}

					if (parts.Length > 1)
					{
						IncludeFiles.Add(new IncludeFile(parts[0], parts[1]));
					}
					else
					{
						IncludeFiles.Add(new IncludeFile(parts[0]));
					}
				}
			}
		}
	}
}
