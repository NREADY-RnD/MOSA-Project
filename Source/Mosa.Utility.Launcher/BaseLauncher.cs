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
		public CompilerHook CompilerHook { get; }

		public List<string> Log { get; }

		public LauncherSettings LauncherSettings { get; }

		public Settings Settings { get { return LauncherSettings.Settings; } }

		public BaseLauncher(Settings settings, CompilerHook compilerHook)
		{
			CompilerHook = compilerHook;

			LauncherSettings = new LauncherSettings(settings);

			SetDefaultSettings();

			NormalizeSettings();

			Log = new List<string>();
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", 9999);
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Launcher.Advance.PlugKorlib", true);
			Settings.SetValue("Launcher.Advance.HuntForCorLib", true);
		}

		protected void NormalizeSettings()
		{
			// Normalize inputs
			LauncherSettings.ImageBootLoader = LauncherSettings.ImageBootLoader == null ? string.Empty : LauncherSettings.ImageBootLoader.ToLower();
			LauncherSettings.ImageFormat = LauncherSettings.ImageFormat == null ? string.Empty : LauncherSettings.ImageFormat.ToLower();
			LauncherSettings.FileSystem = LauncherSettings.FileSystem == null ? string.Empty : LauncherSettings.FileSystem.ToLower();
			LauncherSettings.EmulatorSerial = LauncherSettings.EmulatorSerial == null ? string.Empty : LauncherSettings.EmulatorSerial.ToLower();
			LauncherSettings.Emulator = LauncherSettings.Emulator == null ? string.Empty : LauncherSettings.Emulator.ToLower();
			LauncherSettings.Platform = LauncherSettings.Platform.ToLower();

			// Apply defaults
			var sourcefile = LauncherSettings.SourceFiles[0];

			if (string.IsNullOrEmpty(LauncherSettings.ImageDestination))
			{
				LauncherSettings.ImageDestination = Path.Combine(Path.GetTempPath(), "MOSA");
			}

			if (LauncherSettings.OutputFile == null || LauncherSettings.OutputFile == "%DEFAULT%")
			{
				LauncherSettings.OutputFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.bin");
			}

			if (LauncherSettings.MapFile == "%DEFAULT%")
			{
				LauncherSettings.MapFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-map.txt");
			}

			if (LauncherSettings.CompileTimeFile == "%DEFAULT%")
			{
				LauncherSettings.CompileTimeFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-time.txt");
			}

			if (LauncherSettings.DebugFile == "%DEFAULT%")
			{
				LauncherSettings.DebugFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.debug");
			}

			if (LauncherSettings.InlinedFile == "%DEFAULT%")
			{
				LauncherSettings.InlinedFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-inlined.txt");
			}

			if (LauncherSettings.PreLinkHashFile == "%DEFAULT%")
			{
				LauncherSettings.PreLinkHashFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-prelink-hash.txt");
			}

			if (LauncherSettings.PostLinkHashFile == "%DEFAULT%")
			{
				LauncherSettings.PostLinkHashFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}-postlink-hash.txt");
			}

			if (LauncherSettings.ImageFile == "%DEFAULT%")
			{
				LauncherSettings.ImageFile = Path.Combine(LauncherSettings.ImageDestination, $"{Path.GetFileNameWithoutExtension(sourcefile)}.{LauncherSettings.ImageFormat}");
			}
		}

		protected void AddOutput(string status)
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
