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
using Mosa.Utility.Configuration;
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

		public string CompiledFile { get; private set; }

		public string ImageFile { get; private set; }

		public MosaLinker Linker { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public const uint MultibootHeaderLength = 3 * 16;

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

		public void Compile()
		{
			Log.Clear();
			Counters.Clear();
			HasCompileError = true;

			var compiler = new MosaCompiler(GetCompilerExtensions());

			var sourceFile = LauncherOptions.SourceFiles[0];

			try
			{
				CompileStartTime = DateTime.Now;

				CompiledFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(sourceFile)}.bin");

				MapCompilerOptions.Set(LauncherOptions.Settings, compiler.CompilerOptions);

				if (LauncherOptions.GenerateMapFile)
				{
					compiler.CompilerOptions.MapFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(sourceFile)}.map");
				}

				if (LauncherOptions.GenerateCompileTimeFile)
				{
					compiler.CompilerOptions.CompileTimeFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(sourceFile)}-time.txt");
				}

				if (LauncherOptions.GenerateDebugFile)
				{
					var debugFile = LauncherOptions.DebugFile ?? Path.GetFileNameWithoutExtension(sourceFile) + ".debug";

					compiler.CompilerOptions.DebugFile = Path.Combine(LauncherOptions.DestinationDirectory, debugFile);
				}

				if (!Directory.Exists(LauncherOptions.DestinationDirectory))
				{
					Directory.CreateDirectory(LauncherOptions.DestinationDirectory);
				}

				compiler.CompilerTrace.SetTraceListener(traceListener);

				if (string.IsNullOrEmpty(sourceFile))
				{
					AddOutput("Please select a source file");
					return;
				}
				else if (!File.Exists(sourceFile))
				{
					AddOutput($"File {sourceFile} does not exists");
					return;
				}

				compiler.CompilerOptions.AddSourceFile(sourceFile);
				compiler.CompilerOptions.AddSearchPaths(LauncherOptions.Paths);

				var fileHunter = new FileHunter(Path.GetDirectoryName(sourceFile));

				string platform = LauncherOptions.PlatformType.ToLower();

				if (platform == "armv8a32")
					platform = "ARMv8A32";

				var inputFiles = new List<FileInfo>
				{
					(LauncherOptions.HuntForCorLib) ? fileHunter.HuntFile("mscorlib.dll") : null,
					(LauncherOptions.PlugKorlib) ? fileHunter.HuntFile("Mosa.Plug.Korlib.dll") : null,
					(LauncherOptions.PlugKorlib) ? fileHunter.HuntFile($"Mosa.Plug.Korlib.{platform}.dll"): null,
				};

				compiler.CompilerOptions.AddSourceFiles(inputFiles);
				compiler.CompilerOptions.AddSearchPaths(inputFiles);

				compiler.Load();
				compiler.Initialize();
				compiler.Setup();

				// TODO Include Unit Tests

				if (LauncherOptions.MultiThreading)
				{
					compiler.ThreadedCompile();
				}
				else
				{
					compiler.Compile();
				}

				Linker = compiler.Linker;
				TypeSystem = compiler.TypeSystem;

				if (LauncherOptions.ImageFormat.ToUpper() == "ISO")
				{
					if (LauncherOptions.BootLoader.ToLower() == "grub0.97" || LauncherOptions.BootLoader.ToLower() == "grub2.00")
					{
						CreateISOImageWithGrub(CompiledFile);
					}
					else // assuming syslinux
					{
						CreateISOImageWithSyslinux(CompiledFile);
					}
				}
				else
				{
					CreateDiskImage(CompiledFile);

					if (LauncherOptions.ImageFormat.ToUpper() == "VMDK")
					{
						CreateVMDK();
					}
				}

				if (LauncherOptions.GenerateNASMFile)
				{
					LaunchNDISASM();
				}

				if (LauncherOptions.GenerateASMFile)
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
				compiler = null;
			}
		}

		private void CreateDiskImage(string compiledFile)
		{
			var bootImageOptions = new BootImageOptions();

			if (LauncherOptions.BootLoader.ToLower() == "syslinux6.03")
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\6.03", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (LauncherOptions.BootLoader.ToLower() == "syslinux3.72")
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\3.72", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				bootImageOptions.IncludeFiles.Add(include);
			}

			bootImageOptions.VolumeLabel = "MOSABOOT";

			var vmext = ".img";
			switch (LauncherOptions.ImageFormat.ToUpper())
			{
				case "VHD": vmext = ".vhd"; break;
				case "VDI": vmext = ".vdi"; break;
				default: break;
			}

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0]) + vmext);

			bootImageOptions.DiskImageFileName = ImageFile;
			bootImageOptions.PatchSyslinuxOption = true;

			switch (LauncherOptions.BootLoader.ToLower())
			{
				case "syslinux3.72": bootImageOptions.BootLoader = BootLoader.Syslinux_3_72; break;
				case "syslinux6.03": bootImageOptions.BootLoader = BootLoader.Syslinux_6_03; break;
				case "grub0.97": bootImageOptions.BootLoader = BootLoader.Grub_0_97; break;
				case "grub2.00": bootImageOptions.BootLoader = BootLoader.Grub_2_00; break;
				default: break;
			}

			switch (LauncherOptions.ImageFormat.ToUpper())
			{
				case "IMG": bootImageOptions.ImageFormat = ImageFormat.IMG; break;
				case "ISO": bootImageOptions.ImageFormat = ImageFormat.ISO; break;
				case "VHD": bootImageOptions.ImageFormat = ImageFormat.VHD; break;
				case "VDI": bootImageOptions.ImageFormat = ImageFormat.VDI; break;
				case "VMDK": bootImageOptions.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (LauncherOptions.FileSystem.ToLower())
			{
				case "fat12": bootImageOptions.FileSystem = BootImage.FileSystem.FAT12; break;
				case "fat16": bootImageOptions.FileSystem = BootImage.FileSystem.FAT16; break;
				case "fat32": bootImageOptions.FileSystem = BootImage.FileSystem.FAT32; break;
				default: throw new NotImplementCompilerException("unknown file system");
			}

			Generator.Create(bootImageOptions);
		}

		private void CreateDiskImageV2(string compiledFile)
		{
			var SectorSize = 512;
			var files = new List<IncludeFile>();
			byte[] mbr = null;
			byte[] fatBootCode = null;

			// Determine Image File Name
			var vmext = ".img";

			//switch (LauncherOptions.ImageFormat)
			//{
			//	case ImageFormat.VHD: vmext = ".vhd"; break;
			//	case ImageFormat.VDI: vmext = ".vdi"; break;
			//	default: break;
			//}

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0]) + vmext);

			if (File.Exists(ImageFile))
			{
				File.Delete(ImageFile);
			}

			// Get Files
			if (LauncherOptions.BootLoader == "syslinux6.03")
			{
				mbr = GetResource(@"syslinux\6.03", "mbr.bin");
				fatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (LauncherOptions.BootLoader == "syslinux3.72")
			{
				mbr = GetResource(@"syslinux\3.72", "mbr.bin");
				fatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			files.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			files.Add(new IncludeFile(compiledFile, "main.exe"));

			files.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in LauncherOptions.IncludeFiles)
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

				imageStream.WriteTo(File.Create(ImageFile));
			}
		}

		private void CreateISOImageWithSyslinux(string compiledFile)
		{
			string isoDirectory = Path.Combine(LauncherOptions.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);

			if (LauncherOptions.BootLoader == "syslinux6.03")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\6.03", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\6.03", "mboot.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "ldlinux.c32"), GetResource(@"syslinux\6.03", "ldlinux.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "libcom32.c32"), GetResource(@"syslinux\6.03", "libcom32.c32"));
			}
			else if (LauncherOptions.BootLoader == "Syslinux3.72")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\3.72", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\3.72", "mboot.c32"));
			}

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource("syslinux", "syslinux.cfg"));

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "main.exe"));

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0])}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b isolinux.bin -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateISOImageWithGrub(string compiledFile)
		{
			string isoDirectory = Path.Combine(LauncherOptions.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot"));
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			Directory.CreateDirectory(isoDirectory);

			string loader = string.Empty;

			if (LauncherOptions.BootLoader.ToLower() == "grub0.97")
			{
				loader = "boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (LauncherOptions.BootLoader.ToLower() == "grub2.00")
			{
				loader = "boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "boot", "main.exe"));

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0])}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b {Quote(loader)} -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateVMDK()
		{
			var vmdkFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0])}.vmdk");

			string arg = $"convert -f raw -O vmdk {Quote(ImageFile)} {Quote(vmdkFile)}";

			ImageFile = vmdkFile;
			LaunchApplication(AppLocations.QEMUImg, arg, true);
		}

		private void LaunchNDISASM()
		{
			var textSection = Linker.Sections[(int)SectionKind.Text];

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;

			string arg = $"-b 32 -o0x{startingAddress.ToString("x")} -e0x{fileOffset.ToString("x")} {Quote(CompiledFile)}";

			var nasmfile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0])}.nasm");

			var process = LaunchApplication(AppLocations.NDISASM, arg);

			var output = GetOutput(process);

			File.WriteAllText(nasmfile, output);
		}

		private void GenerateASMFile()
		{
			var translator = new IntelTranslator()
			{
				IncludeAddress = true,
				IncludeBinary = true
			};

			var asmfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFiles[0]) + ".asm");

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

			var code2 = File.ReadAllBytes(CompiledFile);

			var code = new byte[code2.Length];

			for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			{
				code[i - fileOffset] = code2[i];
			}

			var mode = ArchitectureMode.x86_32;
			switch (LauncherOptions.PlatformType.ToLower())
			{
				case "x64":
					mode = ArchitectureMode.x86_64;
					break;
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

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="platformType">Type of the platform.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException">Unknown or unsupported Architecture</exception>
		private static BaseArchitecture SelectArchitecture(string platformType)
		{
			switch (platformType.ToLower())
			{
				case "x86": return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "x64": return Mosa.Platform.x64.Architecture.CreateArchitecture(Mosa.Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException("Unknown or unsupported Architecture");
			}
		}
	}
}
