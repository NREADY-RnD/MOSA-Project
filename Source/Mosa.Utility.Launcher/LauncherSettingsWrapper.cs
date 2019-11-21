// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class LauncherSettingsWrapper
	{
		public Settings Settings { get; set; }

		public List<IncludeFile> IncludeFiles { get; set; }

		public LauncherSettingsWrapper(Settings settings)
		{
			Settings = settings;

			IncludeFiles = new List<IncludeFile>();

			//DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");

			//BootLoader = "syslinux3.72"; // Can't use the Default in the attribute because it would overwrite other bootloader options
			//Settings.SetValue("Emulator.Serial", "None");
			//Emulator = "Qemu";
			//ImageFormat = "IMG";

			//LinkerFormat = "elf32";
			//PlatformType = "x86";

			//FileSystem = "FAT16";
			//BaseAddress = 0x00400000;
			//SerialConnectionHost = "127.0.0.1";
			//InlineMaximum = 12;
			//LaunchVM = true;
			//EnableLongExpansion = true;
			//TwoPassOptimizations = true;
			//EnableValueNumbering = true;
			//LaunchGDB = false;
			//GDBPort = 1234;
			//GDBHost = "localhost";
			//HuntForCorLib = true;
			//EmulatorMemoryInMB = 256;
			//SerialConnectionPort = 9999;
			//GenerateASMFile = false;
			//EnableSSA = true;
			//EnableBasicOptimizations = true;
			//EnableSparseConditionalConstantPropagation = true;
			//EnableInlineMethods = true;
			//EnableLongExpansion = true;
			//EnableLoopInvariantCodeMotion = true;
			//EnablePlatformOptimizations = true;
			//TwoPassOptimizations = true;
			//EnableValueNumbering = true;
			//EnableBitTracker = true;
			//MultibootSpecification = "v1";
			//PlugKorlib = true;
		}
	}
}
