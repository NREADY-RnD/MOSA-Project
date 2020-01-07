// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.API;
using Mosa.Compiler.Framework.Linker;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public class Starter : BaseLauncher
	{
		public MosaLinker Linker { get; }

		public Starter(Settings settings, CompilerHook compilerHook)
			: base(settings, compilerHook)
		{
		}

		public Starter(Settings settings, CompilerHook compilerHook, MosaLinker linker)
			: base(settings, compilerHook)
		{
			Linker = linker;
		}

		public Process Launch()
		{
			var process = LaunchVM();

			if (Settings.GetValue("Launcher.Advance.LaunchGDBDebugger", false))
			{
				LaunchGDBDebugger();
			}

			if (Settings.GetValue("Launcher.Advance.LaunchGDB", false))
			{
				LaunchGDB();
			}

			if (!Settings.GetValue("Launcher.Exit", false))
			{
				var output = GetOutput(process);
				AddOutput(output);
			}

			return process;
		}

		public Process LaunchVM()
		{
			switch (Settings.GetValue("Emulator", string.Empty).ToLower())
			{
				case "qemu": return LaunchQemu(false);
				case "bochs": return LaunchBochs(false);
				case "vmware": return LaunchVMwarePlayer(false);
				default: throw new InvalidOperationException();
			}
		}

		private Process LaunchQemu(bool getOutput)
		{
			var qemubios = Settings.GetValue("AppLocation.Qemu.BIOS", null);

			string arg = " -L " + Quote(qemubios);

			var imageformat = Settings.GetValue("Image.Format", string.Empty).ToUpper();
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var qemu = Settings.GetValue("AppLocation.Qemu", null);

			if (Settings.GetValue("Compiler.Platform", string.Empty).ToLower() == "x86")
			{
				arg += " -cpu qemu32,+sse4.1";
			}

			//arg = arg + " -vga vmware";

			if (!Settings.GetValue("Emulator.Display", false))
			{
				arg += " -display none";
			}

			// We need as least 2 COM Ports:
			// COM1 = Kernel log
			// COM2 = MosaDebugger

			arg += " -serial null"; // TODO: Redirect to file

			var serial = Settings.GetValue("Emulator.Serial", string.Empty).ToLower();

			if (!string.IsNullOrWhiteSpace(serial))
			{
				switch (serial)
				{
					case "pipe": arg = $"{arg} -serial pipe:{Settings.GetValue("Emulator.Serial.Pipe", "MOSA")}"; break;
					case "tcpserver": arg = $"{arg} -serial tcp:{Settings.GetValue("Emulator.Serial.Host", "localhost")}:{Settings.GetValue("Emulator.Serial.Port", 0)},server,nowait"; break;
					case "tcpclient": arg = $"{arg} -serial tcp:{Settings.GetValue("Emulator.Serial.Host", "localhost")}:{Settings.GetValue("Emulator.Serial.Port", 0)},client,nowait"; break;
				}
			}

			if (Settings.GetValue("Emulator.GDB", false))
			{
				arg += $" -S -gdb tcp::{Settings.GetValue("GDB.Port", 0)}";
			}

			if (imageformat == "ISO")
			{
				arg = arg + " -cdrom " + Quote(imagefile);
			}
			else
			{
				if (imageformat == "BIN")
					arg = $"{arg} -kernel {Quote(imagefile)}";
				else
					arg = $"{arg} -hda {Quote(imagefile)}";
			}

			return LaunchApplication(qemu, arg, getOutput);
		}

		private Process LaunchBochs(bool getOutput)
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var imageformat = Settings.GetValue("Image.Format", string.Empty).ToUpper();
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var bochs = Settings.GetValue("AppLocation.Bochs", null);

			var bochsdirectory = Path.GetDirectoryName(bochs);

			var logfile = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + "-bochs.log");
			var configfile = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".bxrc");
			var exeDir = Path.GetDirectoryName(bochs);

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(bochs);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine($"megs: {Settings.GetValue("Emulator.Memory", 128)}");
			sb.AppendLine($"ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine($"cpuid: mmx=1,sep=1,{simd}=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine($"boot: cdrom,disk");
			sb.AppendLine($"log: {Quote(logfile)}");
			sb.AppendLine($"romimage: file={Quote(Path.Combine(bochsdirectory, "BIOS-bochs-latest"))}");
			sb.AppendLine($"vgaromimage: file={Quote(Path.Combine(bochsdirectory, "VGABIOS-lgpl-latest"))}");
			sb.AppendLine($"display_library: x, options={Quote("gui_debug")}");

			if (imageformat == "ISO")
			{
				sb.AppendLine($"ata0-master: type=cdrom,path={Quote(imagefile)},status=inserted");
			}
			else
			{
				sb.AppendLine($"ata0-master: type=disk,path={Quote(imagefile)},biosdetect=none,cylinders=0,heads=0,spt=0");
			}

			sb.AppendLine(@"com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA1");

			if (Settings.GetValue("Emulator.Serial", string.Empty).ToLower() == "pipe")
			{
				sb.AppendLine(@"com2: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA2");
			}

			string arg = "-q -f " + Quote(configfile);

			File.WriteAllText(configfile, sb.ToString());

			return LaunchApplication(bochs, arg, getOutput);
		}

		private Process LaunchVMwarePlayer(bool getOutput)
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var vmwareplayer = Settings.GetValue("AppLocation.Vmware.Player", null);

			var logfile = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + "-vmx.log");
			var configfile = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".vmx");

			var imageformat = Settings.GetValue("Image.Format", string.Empty).ToUpper();

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine($"memsize = {Quote(Settings.GetValue("Emulator.Memory", 128).ToString())}");
			sb.AppendLine($"displayName = \"MOSA - {Path.GetFileNameWithoutExtension(sourcefile)}\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
			sb.AppendLine("ide0:0.present = \"TRUE\"");
			sb.AppendLine($"ide0:0.fileName = {Quote(imagefile)}");

			if (imageformat == "ISO")
			{
				sb.AppendLine("ide0:0.deviceType = \"cdrom-image\"");
			}

			sb.AppendLine("floppy0.present = \"FALSE\"");

			sb.AppendLine("serial0.present = \"TRUE\"");
			sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
			sb.AppendLine("serial0.fileType = \"pipe\"");
			sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA1\"");
			sb.AppendLine("serial0.pipe.endPoint = \"server\"");
			sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");

			if (Settings.GetValue("Emulator.Serial", string.Empty) == "pipe")
			{
				sb.AppendLine("serial1.present = \"TRUE\"");
				sb.AppendLine("serial1.yieldOnMsrRead = \"FALSE\"");
				sb.AppendLine("serial1.fileType = \"pipe\"");
				sb.AppendLine("serial1.fileName = \"\\\\.\\pipe\\MOSA2\"");
				sb.AppendLine("serial1.pipe.endPoint = \"server\"");
				sb.AppendLine("serial1.tryNoRxLoss = \"FALSE\"");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg = Quote(configfile);

			return LaunchApplication(vmwareplayer, arg, getOutput);
		}

		private void LaunchGDBDebugger()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];

			var arg = $" -debugfile {Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".debug")}";
			arg += $" -port {Settings.GetValue("GDB.Port", 0)}";
			arg += $" -connect";
			arg += $" -image {Quote(imagefile)}";

			LaunchApplication("Mosa.Tool.GDBDebugger.exe", arg);
		}

		private void LaunchGDB()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var gdb = Settings.GetValue("AppLocation.GDB", null);

			var gdbscript = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".gdb");

			var arg = $" -d {Quote(destination)}";
			arg = $"{arg} -s {Quote(Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".bin"))}";
			arg = $"{arg} -x {Quote(gdbscript)}";

			// FIXME!
			ulong startingAddress = Linker.Sections[(int)SectionKind.Text].VirtualAddress + Builder.MultibootHeaderLength;

			var sb = new StringBuilder();

			sb.AppendLine($"target remote localhost:{Settings.GetValue("GDB.Port", 0)}");
			sb.AppendLine($"set confirm off ");
			sb.AppendLine($"set disassemble-next-line on");
			sb.AppendLine($"set disassembly-flavor intel");
			sb.AppendLine($"set pagination off");
			sb.AppendLine($"break *0x{startingAddress.ToString("x")}");
			sb.AppendLine($"c");

			File.WriteAllText(gdbscript, sb.ToString());

			AddOutput("Created configuration file: " + gdbscript);
			AddOutput("==================");
			AddOutput(sb.ToString());
			AddOutput("==================");

			LaunchConsoleApplication(gdb, arg);
		}
	}
}
