// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Mosa.Utility.Launcher
{
	public class BaseLauncher
	{
		public Settings Settings { get; }

		public CompilerHook CompilerHook { get; }

		public List<string> Log { get; }

		public BaseLauncher(Settings settings, CompilerHook compilerHook)
		{
			CompilerHook = compilerHook;

			Settings = new Settings();

			SetDefaultSettings();

			Settings.Merge(settings);

			Log = new List<string>();
		}

		private void SetDefaultSettings()
		{
			//Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			//Settings.SetValue("Compiler.Binary", true);
			//Settings.SetValue("Compiler.MethodScanner", false);
			//Settings.SetValue("Compiler.Multithreading", true);
			//Settings.SetValue("Compiler.Platform", "x86");
			//Settings.SetValue("Compiler.TraceLevel", 10);
			//Settings.SetValue("Compiler.Multithreading", true);
			//Settings.SetValue("Compiler.Advanced.PlugKorlib", true);
			//Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			//Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			//Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			//Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			//Settings.SetValue("Optimizations.Basic", true);
			//Settings.SetValue("Optimizations.BitTracker", true);
			//Settings.SetValue("Optimizations.Inline", true);
			//Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			//Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			//Settings.SetValue("Optimizations.Inline.Maximum", 12);
			//Settings.SetValue("Optimizations.LongExpansion", true);
			//Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			//Settings.SetValue("Optimizations.Platform", true);
			//Settings.SetValue("Optimizations.SCCP", true);
			//Settings.SetValue("Optimizations.SSA", true);
			//Settings.SetValue("Optimizations.TwoPass", true);
			//Settings.SetValue("Optimizations.ValueNumbering", true);
			//Settings.SetValue("Image.BootLoader", "syslinux3.72");
			//Settings.SetValue("Image.Destination", Path.Combine(Path.GetTempPath(), "MOSA"));
			//Settings.SetValue("Image.Format", "IMG");
			//Settings.SetValue("Image.FileSystem", "FAT16");
			//Settings.SetValue("Multiboot.Version", "v1");
			//Settings.SetValue("Multiboot.Video", false);
			//Settings.SetValue("Multiboot.Video.Width", 640);
			//Settings.SetValue("Multiboot.Video.Height", 480);
			//Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", 9999);
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");

			//Settings.SetValue("Launcher.Start", false);
			//Settings.SetValue("Launcher.Launch", false);
			//Settings.SetValue("Launcher.Exit", false);
			//Settings.SetValue("Launcher.Advance.HuntForCorLib", true);
		}

		public void AddOutput(string status)
		{
			if (status == null)
				return;

			Log.Add(status);

			OutputEvent(status);
		}

		protected virtual void OutputEvent(string status)
		{
			CompilerHook.NotifyStatus?.Invoke(status);
		}

		static protected byte[] GetResource(string path, string name)
		{
			var newname = path.Replace(".", "._").Replace(@"\", "._").Replace("/", "._").Replace("-", "_") + "." + name;
			return GetResource(newname);
		}

		static protected byte[] GetResource(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var stream = assembly.GetManifestResourceStream("Mosa.Utility.Launcher.Resources." + name);
			var binary = new BinaryReader(stream);
			return binary.ReadBytes((int)stream.Length);
		}

		static protected string Quote(string location)
		{
			return '"' + location + '"';
		}

		protected Process LaunchApplication(string app, string args)
		{
			AddOutput("Launching Application: " + app);
			AddOutput("Arguments: " + args);

			var start = new ProcessStartInfo
			{
				FileName = app,
				Arguments = args,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			return Process.Start(start);
		}

		protected Process LaunchConsoleApplication(string app, string args)
		{
			AddOutput("Launching Application: " + app);
			AddOutput("Arguments: " + args);

			var start = new ProcessStartInfo
			{
				FileName = app,
				Arguments = args,
				UseShellExecute = false,
				CreateNoWindow = false,
				RedirectStandardOutput = false,
				RedirectStandardError = false
			};

			return Process.Start(start);
		}

		protected string GetOutput(Process process)
		{
			var output = process.StandardOutput.ReadToEnd();

			process.WaitForExit();

			var error = process.StandardError.ReadToEnd();

			return output + error;
		}

		protected Process LaunchApplication(string app, string arg, bool getOutput)
		{
			var process = LaunchApplication(app, arg);

			if (getOutput)
			{
				var output = GetOutput(process);
				AddOutput(output);
			}

			return process;
		}
	}
}
