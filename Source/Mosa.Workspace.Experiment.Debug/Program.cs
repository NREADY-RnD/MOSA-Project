// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			//Compile();

			var args = new string[] { "--q", "--autostart", "--output-map", "--output-asm", "--output-debug", "--threading-off", "--inline-off", "Mosa.CoolWorld.x86.exe" };

			List<Argument> map = new List<Argument>()
			{
				new Argument(){ Name = "--q", Setting = "Launcher.Exit", Value = "true"},
				new Argument(){ Name = "--autostart", Setting = "Launcher.Start", Value = "true"},
				new Argument(){ Name = "--output-map", Setting = "CompilerDebug.Emit.Map", Value = "true"},
				new Argument(){ Name = "--output-asm", Setting = "CompilerDebug.Emit.Asm", Value = "true"},
				new Argument(){ Name = "--output-debug", Setting = "CompilerDebug.Emit.Debug", Value = "true"},
				new Argument(){ Name = "--threading-off", Setting = "Compiler.Multithreading", Value = "false"},
				new Argument(){ Name = "--inline-off", Setting = "Optimizations.Inline", Value = "false"},
				new Argument(){ Name = null, Setting = "Compiler.SourceFile", Value = null},
				new Argument(){ Name = "--settings", Setting = "Import", Value = null, IsList = true},
			};

			var argumentSettings = Reader.ParseArguments(args, map);

			var fileSettings = Reader.Import(@".mosa-global.txt");

			var settings = Settings.Merge(fileSettings, argumentSettings);
		}

		private static void Compile()
		{
			const string platform = "x86";

			var settings = new Settings();

			settings.SetValue("Compiler.MethodScanner", false);
			settings.SetValue("Compiler.Binary", true);
			settings.SetValue("Compiler.TraceLevel", 0);
			settings.SetValue("Compiler.Platform", platform);
			settings.SetValue("Compiler.Multithreading", true);
			settings.SetValue("Optimizations.SSA", true);
			settings.SetValue("Optimizations.Basic", true);
			settings.SetValue("Optimizations.ValueNumbering", true);
			settings.SetValue("Optimizations.SCCP", true);
			settings.SetValue("Optimizations.BitTracker", true);
			settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			settings.SetValue("Optimizations.LongExpansion", true);
			settings.SetValue("Optimizations.TwoPass", true);
			settings.SetValue("Optimizations.Platform", true);
			settings.SetValue("Optimizations.Inline", true);
			settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			settings.SetValue("Optimizations.Inline.Maximum", 12);
			settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			settings.SetValue("Multiboot.Version", "v1");
			settings.SetValue("Compiler.Platform", "x86");

			var stopwatch = new Stopwatch();

			var compiler = new MosaCompiler(settings);

			compiler.Load();
			compiler.Initialize();
			compiler.Setup();

			stopwatch.Start();

			//MeasureCompileTime(stopwatch, compiler, "Mosa.Kernel.x86.IDT::SetTableEntries");
			//MeasureCompileTime(stopwatch, compiler, "System.Void Mosa.TestWorld.x86.Boot::Thread1");
			//MeasureCompileTime(stopwatch, compiler, "System.String System.Int32::CreateString(System.UInt32, System.Boolean, System.Boolean)");

			//compiler.ScheduleAll();

			var start = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine("Threaded Execution Time:");

			compiler.ThreadedCompile();

			//compiler.Execute();

			Console.WriteLine($"Elapsed: {(stopwatch.Elapsed.TotalSeconds - start).ToString("F2")} secs");

			Console.ReadKey();
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, string methodName)
		{
			var method = GetMethod(methodName, compiler.TypeSystem);

			MeasureCompileTime(stopwatch, compiler, method);
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, MosaMethod method)
		{
			Console.WriteLine($"Method: {method}");

			double min = double.MaxValue;

			for (int i = 0; i < 6; i++)
			{
				var start = stopwatch.Elapsed.TotalMilliseconds;

				compiler.CompileSingleMethod(method);

				var elapsed = stopwatch.Elapsed.TotalMilliseconds - start;

				min = Math.Min(min, elapsed);

				//Console.WriteLine($"Elapsed: {elapsed.ToString("F2")} ms");
			}

			Console.WriteLine($"Elapsed: {min.ToString("F2")} ms (best)");
		}

		private static MosaMethod GetMethod(string partial, TypeSystem typeSystem)
		{
			foreach (var type in typeSystem.AllTypes)
			{
				foreach (var method in type.Methods)
				{
					if (method.FullName.Contains(partial))
						return method;
				}
			}

			return null;
		}

		private static BaseArchitecture SelectPlatform(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return new Platform.x86.Architecture();
				case "x64": return new Platform.x64.Architecture();
				case "armv8a32": return new Platform.ARMv8A32.Architecture();
				default: throw new NotImplementCompilerException($"Unknown or unsupported Architecture {platform}.");
			}
		}
	}
}
