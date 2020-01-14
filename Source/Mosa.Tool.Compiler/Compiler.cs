// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.API;
using Mosa.Compiler.Framework.Linker.Elf.Dwarf;
using Mosa.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Data

		protected MosaCompiler compiler;

		protected Settings Settings = new Settings();

		private readonly int majorVersion = 1;
		private readonly int minorVersion = 4;
		private readonly string codeName = "Neptune";

		/// <summary>
		/// A string holding a simple usage description.
		/// </summary>
		private readonly string usageString;

		#endregion Data

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Compiler class.
		/// </summary>
		public Compiler()
		{
			usageString = @"Usage: Mosa.Tool.Compiler.exe -o outputfile --achitecture=[x86|x64] --format=[ELF32|ELF64] {--boot=[mb0.7]} {additional options} inputfiles.

Example: Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin -a x86 --mboot v1 --x86-irq-methods --base-address 0x00500000 Mosa.HelloWorld.x86.exe mscorlib.dll Mosa.Plug.Korlib.dll Mosa.Plug.Korlib.x86.dll";
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Runs the command line parser and the compilation process.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public void Run(string[] args)
		{
			RegisterPlatforms();

			// always print header with version information
			Console.WriteLine("MOSA AOT Compiler, Version {0}.{1} '{2}'", majorVersion, minorVersion, codeName);
			Console.WriteLine("Copyright 2019 by the MOSA Project. Licensed under the New BSD License.");

			Console.WriteLine();
			Console.WriteLine("Parsing options...");

			try
			{
				LoadArguments(args);

				var sourceFiles = Settings.GetValueList("Compiler.SourceFiles");

				if (sourceFiles == null && sourceFiles.Count == 0)
				{
					throw new Exception("No input file(s) specified.");
				}

				compiler = new MosaCompiler(Settings, new CompilerHook());

				Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
				Debug.AutoFlush = true;

				if (string.IsNullOrEmpty(compiler.CompilerSettings.OutputFile))
				{
					throw new Exception("No output file specified.");
				}

				if (compiler.CompilerSettings.Platform == null)
				{
					throw new Exception("No Architecture specified.");
				}
			}
			catch (Exception e)
			{
				ShowError(e.Message);
				Environment.Exit(1);
				return;
			}

			Console.WriteLine(ToString());

			Console.WriteLine("Compiling ...");

			DateTime start = DateTime.Now;

			try
			{
				Compile();
			}
			catch (CompilerException ce)
			{
				ShowError(ce.Message);
				Environment.Exit(1);
				return;
			}

			DateTime end = DateTime.Now;

			TimeSpan time = end - start;
			Console.WriteLine();
			Console.WriteLine("Compilation time: " + time);
		}

		/// <summary>
		/// Returns a string representation of the current options.
		/// </summary>
		/// <returns>A string containing the options.</returns>
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(" > Output file: ").AppendLine(Settings.GetValue("Compiler.OutputFile", string.Empty));
			sb.Append(" > Input file(s): ").AppendLine(string.Join(", ", new List<string>(Settings.GetValueList("Compiler.SourceFiles").ToArray())));
			sb.Append(" > Platform: ").AppendLine(Settings.GetValue("Compiler.Platform", string.Empty));
			sb.Append(" > Binary format: ").AppendLine(Settings.GetValue("Linker.Format", string.Empty));
			sb.Append(" > Boot spec: ").AppendLine(Settings.GetValue("Multiboot.Version", string.Empty));
			return sb.ToString();
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		#endregion Public Methods

		#region Private Methods

		private void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Launcher.Advance.PlugKorlib", true);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.Devirtualization", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Destination", Path.Combine(Path.GetTempPath(), "MOSA"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", 9999);
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Launcher.Start", false);
			Settings.SetValue("Launcher.Launch", false);
			Settings.SetValue("Launcher.Exit", false);
			Settings.SetValue("Launcher.Advance.HuntForCorLib", true);
		}

		private void Compile()
		{
			compiler.Load();

			compiler.ThreadedCompile();
		}

		/// <summary>
		/// Shows an error and a short information text.
		/// </summary>
		/// <param name="message">The error message to show.</param>
		private void ShowError(string message)
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.Write("Error: ");
			Console.WriteLine(message);
			Console.WriteLine();
			Console.WriteLine("Execute 'Mosa.Tool.Compiler.exe --help' for more information.");
			Console.WriteLine();
		}

		/// <summary>
		/// Shows a short help text pointing to the '--help' option.
		/// </summary>
		private void ShowShortHelp()
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.WriteLine("Execute 'Mosa.Tool.Compiler.exe --help' for more information.");
		}

		#endregion Private Methods
	}
}
