// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.Compiler
{
	public static class CommandLineArguments
	{
		public static List<ArgumentMap> GetMap()
		{
			var map = new List<ArgumentMap>()
			{
				new ArgumentMap() { Argument = "--q", Setting = "Launcher.Exit", Value = "true"},
				new ArgumentMap() { Argument = "--autostart", Setting = "Launcher.Start", Value = "true"},
				new ArgumentMap() { Argument = "--threading-off", Setting = "Compiler.Multithreading", Value = "false"},
				new ArgumentMap() { Argument = "--settings", Setting = "Import", Value = null, IsList = true},

				new ArgumentMap() { Argument = null, Setting = "Compiler.SourceFiles", Value = null, IsList = true},

				new ArgumentMap() { Argument = "--scanner", Setting = "Compiler.MethodScanner", Value = "true"},
				new ArgumentMap() { Argument = "--no-code", Setting = "Compiler.EmitBinary", Value = "false"},

				new ArgumentMap() { Argument = "--output-map", Setting = "CompilerDebug.Emit.Map", Value = "true"},
				new ArgumentMap() { Argument = "--output-asm", Setting = "CompilerDebug.Emit.Asm", Value = "true"},
				new ArgumentMap() { Argument = "--output-debug", Setting = "CompilerDebug.Emit.Debug", Value = "true"},
				new ArgumentMap() { Argument = "--inline-off", Setting = "Optimizations.Inline", Value = "false"},
				new ArgumentMap() { Argument = "--no-ssa", Setting = "Optimizations.SSA", Value = "false"},
				new ArgumentMap() { Argument = "--no-sparse", Setting = "Optimizations.SCCP", Value = "false"},
				new ArgumentMap() { Argument = "--no-ir-optimizations", Setting = "Optimizations.Basic", Value = "false"},

				new ArgumentMap() { Argument = "--x86", Setting = "Compiler.Platform", Value = "x86"},
				new ArgumentMap() { Argument = "--x64", Setting = "Compiler.Platform", Value = "x64"},
				new ArgumentMap() { Argument = "--armv8a32", Setting = "Compiler.Platform", Value = "armv8a32"},

				new ArgumentMap() { Argument = "--filter", Setting = "Explorer.Filter", Value = null},
			};

			return map;
		}
	}
}
