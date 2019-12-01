// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.Linker;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class LinkerSettings
	{
		#region Properties

		public Settings Settings { get; } = new Settings();

		public ulong BaseAddress { get { return (ulong)Settings.GetValue("Compiler.BaseAddress", 0x00400000); } }

		public string OutputFile { get { return Settings.GetValue("Compiler.OutputFile", "_main.exe"); } }

		public string LinkerFormat { get { return Settings.GetValue("Linker.Format", "elf32"); } }

		public bool EmitBinary { get { return Settings.GetValue("Compiler.EmitBinary", true); } }

		public bool EmitAllSymbols { get { return Settings.GetValue("Linker.EmitAllSymbols", false); } }

		public bool EmitStaticRelocations { get { return Settings.GetValue("Linker.EmitStaticRelocations", false); } }

		public bool EmitShortSymbolNames { get { return Settings.GetValue("Linker.EmitShortSymbolNames", false); } }

		public bool EmitDrawf { get { return Settings.GetValue("Linker.EmitDrawf", false); } }

		public bool Statistics { get { return Settings.GetValue("CompilerDebug.EnableStatistics", true); } }

		public int TraceLevel { get { return Settings.GetValue("Compiler.TraceLevel", 0); } }

		public bool EmitInline { get { return Settings.GetValue("Compiler.EmitInline", false); } }

		#endregion Properties

		public LinkerSettings(Settings settings)
		{
			// defaults

			Settings.SetValue("Compiler.EmitBinary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.Multithreading", true);

			Settings.Merge(settings);
		}
	}
}
