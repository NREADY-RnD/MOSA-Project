// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.Linker;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class CompilerOptions
	{
		public Settings Settings { get; } = new Settings();

		#region Properties

		/// <summary>
		/// Gets or sets the base address.
		/// </summary>
		public ulong BaseAddress { get; set; }

		/// <summary>
		/// Gets or sets the architecture.
		/// </summary>
		public BaseArchitecture Platform { get; set; }

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		public string OutputFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		public string MapFile { get; set; }

		/// <summary>
		/// Gets or sets the compile time file.
		/// </summary>
		public string CompileTimeFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		public string DebugFile { get; set; }

		/// <summary>
		/// Gets or sets interrupt method name to override the architecture specific default method
		/// </summary>
		/// <example>Mosa.Kernel.x86.IDT::ProcessInterrupt</example>
		public string InterruptMethodName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether SSA is enabled.
		/// </summary>
		public bool SSA { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable IR optimizations].
		/// </summary>
		public bool BasicOptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable value numbering].
		/// </summary>
		public bool ValueNumbering { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable conditional constant propagation].
		/// </summary>
		public bool SparseConditionalConstantPropagation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable loop invariant code motion].
		/// </summary>
		public bool LoopInvariantCodeMotion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable inline methods].
		/// </summary>
		public bool InlineMethods { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether only methods are inline marked as AgressiveInlining.
		/// </summary>
		public bool InlineExplicitOnly { get; set; }

		/// <summary>
		/// Gets or sets the maximum IR instructions for inline optimization.
		/// </summary>
		public int InlineMaximum { get; set; }

		/// <summary>
		/// Gets or sets the maximum IR instructions for aggressive inline optimization.
		/// </summary>
		public int InlineAggressiveMaximum { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable IR long operand conversion].
		/// </summary>
		public bool LongExpansion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable bit estimator].
		/// </summary>
		public bool BitTracker { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable platform optimizations].
		/// </summary>
		public bool PlatformOptimizations { get; set; }

		/// <summary>
		/// Gets or sets the type of the elf.
		/// </summary>
		public string LinkerFormat { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit binary].
		/// </summary>
		public bool EmitBinary { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit symbols].
		/// </summary>
		public bool EmitAllSymbols { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit relocations].
		/// </summary>
		public bool EmitStaticRelocations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit short symbol names].
		/// </summary>
		public bool EmitShortSymbolNames { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit dwarf].
		/// </summary>
		public bool EmitDrawf { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [aggressive optimizations].
		/// </summary>
		public bool TwoPass { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable statistics].
		/// </summary>
		public bool Statistics { get; set; }

		/// <summary>
		/// Gets or sets the trace level.
		/// </summary>
		public int TraceLevel { get; set; }

		/// <summary>
		/// Gets or sets the include paths.
		/// </summary>
		public List<string> SearchPaths { get; set; } = new List<string>();

		/// <summary>
		/// Gets or sets the source files.
		/// </summary>
		public List<string> SourceFiles { get; set; } = new List<string>();

		/// <summary>
		/// Gets or sets a value indicating whether [enable method scanner].
		/// </summary>
		public bool MethodScanner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit inline methods].
		/// </summary>
		public bool EmitInline { get; set; }

		/// <summary>
		/// Adds additional sections to the Elf-File.
		/// </summary>
		public MosaLinker.CreateExtraSectionsDelegate CreateExtraSections { get; set; }

		/// <summary>
		/// Adds additional program headers to the Elf-File.
		/// </summary>
		public MosaLinker.CreateExtraProgramHeaderDelegate CreateExtraProgramHeaders { get; set; }

		public List<string> InlineMethodsList { get; set; } = new List<string>();

		public List<string> DoNotInlineMethodsList { get; set; } = new List<string>();

		#endregion Properties

		/// <summary>
		/// Adds the search path.
		/// </summary>
		/// <param name="path">The path.</param>
		public void AddSearchPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			SearchPaths.AddIfNew(path);
		}

		/// <summary>
		/// Adds the search paths.
		/// </summary>
		/// <param name="files">The files.</param>
		public void AddSearchPaths(IEnumerable<FileInfo> files)
		{
			foreach (var file in files)
			{
				if (file == null)
					continue;

				AddSearchPath(Path.GetDirectoryName(file.FullName));
			}
		}

		/// <summary>
		/// Adds the search paths.
		/// </summary>
		/// <param name="paths">The paths.</param>
		public void AddSearchPaths(IList<string> paths)
		{
			if (paths == null)
				return;

			foreach (var path in paths)
			{
				AddSearchPath(Path.GetDirectoryName(path));
			}
		}

		/// <summary>
		/// Adds the source file.
		/// </summary>
		/// <param name="path">The path.</param>
		public void AddSourceFile(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			SourceFiles.AddIfNew(path);
		}

		public void AddSourceFiles(IList<string> files)
		{
			foreach (var file in files)
			{
				if (file == null)
					continue;

				AddSourceFile(file);
			}
		}

		/// <summary>
		/// Adds the source files.
		/// </summary>
		/// <param name="files">The files.</param>
		public void AddSourceFiles(IEnumerable<FileInfo> files)
		{
			foreach (var file in files)
			{
				if (file == null)
					continue;

				AddSourceFile(file.FullName);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerOptions"/> class.
		/// </summary>
		public CompilerOptions()
		{
			TraceLevel = 0;
			SSA = true;
			BasicOptimizations = true;
			SparseConditionalConstantPropagation = true;
			InlineMethods = false;
			EmitBinary = true;
			InlineMaximum = 12;
			InlineAggressiveMaximum = 24;
			TwoPass = true;
			LongExpansion = true;
			ValueNumbering = true;
			LoopInvariantCodeMotion = true;
			PlatformOptimizations = true;
			MethodScanner = false;
			BitTracker = true;
			EmitInline = false;
			BaseAddress = 0x00400000; // todo
			Statistics = true;  // todo
			EmitShortSymbolNames = true; // todo
			EmitDrawf = false; // todo
			LinkerFormat = "elf32"; // todo
			EmitAllSymbols = true; // todo
			EmitStaticRelocations = true; // todo

			// defaults
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.EmitBinary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Compiler.Platform", "x86");
		}
	}
}
