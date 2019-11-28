// Copyright (c) MOSA Project. Licensed under the New BSD License.

using DiscUtils.Fat;
using DiscUtils.Partitions;
using DiscUtils.Raw;
using DiscUtils.Streams;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.BootImage;
using SharpDisasm;
using SharpDisasm.Translators;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public class Builder : BaseLauncher
	{
		public List<string> Counters { get; }

		public DateTime CompileStartTime { get; private set; }

		public IBuilderEvent BuilderEvent { get; }

		public bool HasCompileError { get; private set; }

		public MosaLinker Linker { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public const uint MultibootHeaderLength = 3 * 16;

		public List<IncludeFile> IncludeFiles = new List<IncludeFile>();

		protected ITraceListener traceListener;

		public Builder(Settings settings, AppLocations appLocations, IBuilderEvent builderEvent)
			: base(settings, appLocations)
		{
			Counters = new List<string>();
			traceListener = new BuilderEventListener(this);
			BuilderEvent = builderEvent;
		}

		protected override void OutputEvent(string status)
		{
			BuilderEvent?.NewStatus(status);
		}

		public void AddCounters(string data)
		{
			if (data == null)
				return;

			Counters.Add(data);
		}

		private List<BaseCompilerExtension> GetCompilerExtensions()
		{
			return new List<BaseCompilerExtension>();
		}

		public void Build()
		{
			Log.Clear();
			Counters.Clear();
			HasCompileError = true;

			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var destination = Settings.GetValue("Image.Destination", null);
			var output = Settings.GetValue("Compiler.OutputFile", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var imageformat = Settings.GetValue("Image.Format", string.Empty).ToUpper();
			var bootloader = Settings.GetValue("Image.BootLoader", string.Empty).ToLower();

			try
			{
				CompileStartTime = DateTime.Now;

				if (string.IsNullOrEmpty(destination))
				{
					destination = Path.Combine(Path.GetTempPath(), "MOSA");
					Settings.SetValue("Image.Destination", destination);
				}

				if (output == null || output == "%DEFAULT%")
				{
					Settings.SetValue("Compiler.OutputFile", Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.bin"));
				}

				if (Settings.GetValue("CompilerDebug.MapFile", string.Empty) == "%DEFAULT%")
				{
					Settings.SetValue("CompilerDebug.MapFile", Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-map.txt"));
				}

				if (Settings.GetValue("CompilerDebug.CompileTimeFile", string.Empty) == "%DEFAULT%")
				{
					Settings.SetValue("CompilerDebug.CompileTimeFile", Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-time.txt"));
				}

				if (Settings.GetValue("CompilerDebug.DebugFile", string.Empty) == "%DEFAULT%")
				{
					Settings.SetValue("CompilerDebug.DebugFile", Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.debug"));
				}

				if (Settings.GetValue("CompilerDebug.InlinedFile", string.Empty) == "%DEFAULT%")
				{
					Settings.SetValue("CompilerDebug.InlinedFile", Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-inlined.txt"));
				}

				if (imagefile == null || imagefile == "%DEFAULT")
				{
					var vmext = GetImageFileExtension(imageformat);
					imagefile = Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.{vmext}");
					Settings.SetValue("Image.ImageFile", imagefile);
				}

				if (!Directory.Exists(destination))
				{
					Directory.CreateDirectory(destination);
				}

				if (string.IsNullOrEmpty(sourcefile))
				{
					AddOutput("Please select a source file");
					return;
				}
				else if (!File.Exists(sourcefile))
				{
					AddOutput($"File {sourcefile} does not exists");
					return;
				}

				var fileHunter = new FileHunter(Path.GetDirectoryName(sourcefile));

				if (Settings.GetValue("Launcher.Advance.HuntForCorLib", false))
				{
					var fileCorlib = fileHunter.HuntFile("mscorlib.dll");

					if (fileCorlib != null)
					{
						Settings.AddPropertyListValue("Compiler.SourceFiles", fileCorlib.FullName);
					}
				}

				if (Settings.GetValue("Compiler.Advanced.PlugKorlib", false))
				{
					var fileKorlib = fileHunter.HuntFile("Mosa.Plug.Korlib.dll");

					if (fileKorlib != null)
					{
						Settings.AddPropertyListValue("Compiler.SourceFiles", fileKorlib.FullName);
					}

					string platform = Settings.GetValue("Compiler.Platform", string.Empty).ToLower();

					if (platform == "armv8a32")
						platform = "ARMv8A32";

					var fileKorlibPlatform = fileHunter.HuntFile($"Mosa.Plug.Korlib.{platform}.dll");

					if (fileKorlibPlatform != null)
					{
						Settings.AddPropertyListValue("Compiler.SourceFiles", fileKorlibPlatform.FullName);
					}
				}

				var compiler = new MosaCompiler(Settings, GetCompilerExtensions());

				compiler.CompilerTrace.SetTraceListener(traceListener);

				compiler.Load();
				compiler.Initialize();
				compiler.Setup();

				if (Settings.GetValue("Compiler.Multithreading", false))
				{
					compiler.ThreadedCompile();
				}
				else
				{
					compiler.Compile();
				}

				Linker = compiler.Linker;
				TypeSystem = compiler.TypeSystem;

				if (imageformat == "ISO")
				{
					if (bootloader == "grub0.97" || bootloader == "grub2.00")
					{
						CreateISOImageWithGrub();
					}
					else // assuming syslinux
					{
						CreateISOImageWithSyslinux();
					}
				}
				else
				{
					if (imageformat == "VMDK")
					{
						var tmpimagefile = Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.img");

						CreateDiskImage(tmpimagefile);

						CreateVMDK(tmpimagefile);
					}
					else
					{
						CreateDiskImage(imagefile);
					}
				}

				if (!string.IsNullOrWhiteSpace(Settings.GetValue("CompilerDebug.NasmFile", string.Empty)))
				{
					LaunchNDISASM();
				}

				if (!string.IsNullOrWhiteSpace(Settings.GetValue("CompilerDebug.AsmFile", string.Empty)))
				{
					GenerateASMFile();
				}

				HasCompileError = false;
			}
			catch (Exception e)
			{
				HasCompileError = true;
				AddOutput(e.ToString());
			}
			finally
			{
				//compiler = null;
			}
		}

		private void CreateDiskImage(string imagefile)
		{
			var bootImageOptions = new BootImageOptions();
			var output = Settings.GetValue("Compiler.OutputFile", null);
			var bootloader = Settings.GetValue("Image.BootLoader", string.Empty).ToLower();
			var imageformat = Settings.GetValue("Image.Format", string.Empty).ToUpper();
			var filesystem = Settings.GetValue("Image.FileSystem", string.Empty).ToLower();

			if (bootloader == "syslinux6.03")
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\6.03", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (bootloader == "syslinux3.72")
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\3.72", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(output, "main.exe"));

			bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in IncludeFiles)
			{
				bootImageOptions.IncludeFiles.Add(include);
			}

			bootImageOptions.VolumeLabel = "MOSABOOT";
			bootImageOptions.PatchSyslinuxOption = true;
			bootImageOptions.DiskImageFileName = imagefile;

			switch (bootloader)
			{
				case "syslinux3.72": bootImageOptions.BootLoader = BootLoader.Syslinux_3_72; break;
				case "syslinux6.03": bootImageOptions.BootLoader = BootLoader.Syslinux_6_03; break;
				case "grub0.97": bootImageOptions.BootLoader = BootLoader.Grub_0_97; break;
				case "grub2.00": bootImageOptions.BootLoader = BootLoader.Grub_2_00; break;
				default: break;
			}

			switch (imageformat)
			{
				case "IMG": bootImageOptions.ImageFormat = ImageFormat.IMG; break;
				case "ISO": bootImageOptions.ImageFormat = ImageFormat.ISO; break;
				case "VHD": bootImageOptions.ImageFormat = ImageFormat.VHD; break;
				case "VDI": bootImageOptions.ImageFormat = ImageFormat.VDI; break;
				case "VMDK": bootImageOptions.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (filesystem)
			{
				case "fat12": bootImageOptions.FileSystem = BootImage.FileSystem.FAT12; break;
				case "fat16": bootImageOptions.FileSystem = BootImage.FileSystem.FAT16; break;
				case "fat32": bootImageOptions.FileSystem = BootImage.FileSystem.FAT32; break;
				default: throw new NotImplementCompilerException("unknown file system");
			}

			Generator.Create(bootImageOptions);
		}

		private static string GetImageFileExtension(string imageformat)
		{
			switch (imageformat.ToUpper())
			{
				case "VHD": return "vhd";
				case "VDI": return "vdi";
				case "ISO": return "iso";
				case "IMG": return "img";
				case "VMDK": return "vmdk";
				default: return "img";
			}
		}

		private void CreateDiskImageV2(string compiledFile)
		{
			var SectorSize = 512;
			var files = new List<IncludeFile>();
			byte[] mbr = null;
			byte[] fatBootCode = null;

			var destination = Settings.GetValue("Image.Destination", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var imagefile = Settings.GetValue("Image.ImageFile", null);

			if (File.Exists(imagefile))
			{
				File.Delete(imagefile);
			}

			var bootloader = Settings.GetValue("Image.BootLoader", string.Empty).ToLower();

			// Get Files
			if (bootloader == "syslinux6.03")
			{
				mbr = GetResource(@"syslinux\6.03", "mbr.bin");
				fatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (bootloader == "syslinux3.72")
			{
				mbr = GetResource(@"syslinux\3.72", "mbr.bin");
				fatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			files.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			files.Add(new IncludeFile(compiledFile, "main.exe"));

			files.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in IncludeFiles)
			{
				files.Add(include);
			}

			// Estimate file system size
			var blockCount = 8400 + 1;
			foreach (var file in files)
			{
				blockCount += (file.Content.Length / SectorSize) + 1;
			}

			using (var imageStream = new MemoryStream())
			{
				var disk = Disk.Initialize(imageStream, Ownership.Dispose, blockCount * SectorSize);

				BiosPartitionTable.Initialize(disk, WellKnownPartitionType.WindowsFat);

				disk.SetMasterBootRecord(mbr);

				using (var fs = FatFileSystem.FormatPartition(disk, 0, null))
				{
					foreach (var file in files)
					{
						var directory = Path.GetFullPath(file.Filename);

						if (!string.IsNullOrWhiteSpace(directory) && !fs.DirectoryExists(directory))
						{
							fs.CreateDirectory(directory);
						}

						using (var f = fs.OpenFile(file.Filename, FileMode.Create))
						{
							f.Write(file.Content);
							f.Close();
						}
					}
				}

				using (var partition = disk.Partitions.Partitions[0].Open())
				{
					partition.Seek(0x03, SeekOrigin.Begin);
					partition.Write(fatBootCode, 0, 3);
					partition.Seek(0x3E, SeekOrigin.Begin);
					partition.Write(fatBootCode, 0x3E, Math.Max(448, fatBootCode.Length - 0x3E));
					partition.Close();
				}

				imageStream.WriteTo(File.Create(imagefile));
			}
		}

		private void CreateISOImageWithSyslinux()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var output = Settings.GetValue("Compiler.OutputFile", null);

			string isoDirectory = Path.Combine(destination, "iso");
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);

			var bootloader = Settings.GetValue("Image.BootLoader", string.Empty).ToLower();

			if (bootloader == "syslinux6.03")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\6.03", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\6.03", "mboot.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "ldlinux.c32"), GetResource(@"syslinux\6.03", "ldlinux.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "libcom32.c32"), GetResource(@"syslinux\6.03", "libcom32.c32"));
			}
			else if (bootloader == "Syslinux3.72")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\3.72", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\3.72", "mboot.c32"));
			}

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource("syslinux", "syslinux.cfg"));

			foreach (var include in IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(output, Path.Combine(isoDirectory, "main.exe"));

			string arg = $"-relaxed-filenames -J -R -o {Quote(imagefile)} -b isolinux.bin -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateISOImageWithGrub()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var output = Settings.GetValue("Compiler.OutputFile", null);
			var imagefile = Settings.GetValue("Image.ImageFile", null);

			string isoDirectory = Path.Combine(destination, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot"));
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			Directory.CreateDirectory(isoDirectory);

			string loader = string.Empty;

			var bootloader = Settings.GetValue("Image.BootLoader", string.Empty).ToLower();

			if (bootloader == "grub0.97")
			{
				loader = "boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (bootloader == "grub2.00")
			{
				loader = "boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			foreach (var include in IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(output, Path.Combine(isoDirectory, "boot", "main.exe"));

			string arg = $"-relaxed-filenames -J -R -o {Quote(imagefile)} -b {Quote(loader)} -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateVMDK(string source)
		{
			var imagefile = Settings.GetValue("Image.ImageFile", null);

			string arg = $"convert -f raw -O vmdk {Quote(source)} {Quote(imagefile)}";

			LaunchApplication(AppLocations.QEMUImg, arg, true);
		}

		private void LaunchNDISASM()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var output = Settings.GetValue("Compiler.OutputFile", null);

			var textSection = Linker.Sections[(int)SectionKind.Text];

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;

			string arg = $"-b 32 -o0x{startingAddress.ToString("x")} -e0x{fileOffset.ToString("x")} {Quote(output)}";

			var nasmfile = Path.Combine(destination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.nasm");

			var process = LaunchApplication(AppLocations.NDISASM, arg);

			var processoutput = GetOutput(process);

			File.WriteAllText(nasmfile, processoutput);
		}

		private void GenerateASMFile()
		{
			var destination = Settings.GetValue("Image.Destination", null);
			var sourcefile = Settings.GetValueList("Compiler.SourceFiles")[0];
			var imagefile = Settings.GetValue("Image.ImageFile", null);
			var output = Settings.GetValue("Compiler.OutputFile", null);

			var translator = new IntelTranslator()
			{
				IncludeAddress = true,
				IncludeBinary = true
			};

			var asmfile = Path.Combine(destination, Path.GetFileNameWithoutExtension(sourcefile) + ".asm");

			var textSection = Linker.Sections[(int)SectionKind.Text];

			var map = new Dictionary<ulong, List<string>>();

			foreach (var symbol in Linker.Symbols)
			{
				if (!map.TryGetValue(symbol.VirtualAddress, out List<string> list))
				{
					list = new List<string>();
					map.Add(symbol.VirtualAddress, list);
				}

				list.Add(symbol.Name);
			}

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;
			uint length = textSection.Size;

			var code2 = File.ReadAllBytes(output);

			var code = new byte[code2.Length];

			for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			{
				code[i - fileOffset] = code2[i];
			}

			var mode = ArchitectureMode.x86_32;

			switch (Settings.GetValue("Compiler.Platform", string.Empty).ToLower())
			{
				case "x86": mode = ArchitectureMode.x86_32; break;
				case "x64": mode = ArchitectureMode.x86_64; break;
			}

			using (var disasm = new Disassembler(code, mode, startingAddress, true, Vendor.Any))
			{
				using (var dest = File.CreateText(asmfile))
				{
					if (map.TryGetValue(startingAddress, out List<string> list))
					{
						foreach (var entry in list)
						{
							dest.WriteLine($"; {entry}");
						}
					}

					foreach (var instruction in disasm.Disassemble())
					{
						var inst = translator.Translate(instruction);
						dest.WriteLine(inst);

						if (map.TryGetValue(instruction.PC, out List<string> list2))
						{
							foreach (var entry in list2)
							{
								dest.WriteLine($"; {entry}");
							}
						}

						if (instruction.PC > startingAddress + length)
							break;
					}
				}
			}
		}
	}
}
