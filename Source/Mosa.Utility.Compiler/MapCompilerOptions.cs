// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Compiler.Common;

using Mosa.Compiler.Framework;

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Utility.Compiler
{
	public static class MapCompilerOptions
	{
		public static void Map(Settings settings, CompilerOptions compilerOptions)
		{
			compilerOptions.TraceLevel = settings.GetValueAsInteger("Compiler.TraceLevel", compilerOptions.TraceLevel);
			compilerOptions.EmitBinary = settings.GetValueAsBoolean("Compiler.EmitBinary", compilerOptions.EmitBinary);
			compilerOptions.EnableMethodScanner = settings.GetValueAsBoolean("Compiler.MethodScanner", compilerOptions.EnableMethodScanner);

			//compilerOptions.EnableThreading = settings.GetValueAsBoolean("Compiler.Multithreading", compilerOptions.EnableThreading);

			compilerOptions.EnableSSA = settings.GetValueAsBoolean("Optimizations.SSA", compilerOptions.EnableSSA);
			compilerOptions.EnableBasicOptimizations = settings.GetValueAsBoolean("Optimizations.Basic", compilerOptions.EnableBasicOptimizations);
			compilerOptions.EnableValueNumbering = settings.GetValueAsBoolean("Optimizations.ValueNumbering", compilerOptions.EnableValueNumbering);
			compilerOptions.EnableSparseConditionalConstantPropagation = settings.GetValueAsBoolean("Optimizations.SCCP", compilerOptions.EnableSparseConditionalConstantPropagation);
			compilerOptions.EnableBitTracker = settings.GetValueAsBoolean("Optimizations.BitTracker", compilerOptions.EnableBitTracker);
			compilerOptions.EnableLoopInvariantCodeMotion = settings.GetValueAsBoolean("Optimizations.LoopInvariantCodeMotion", compilerOptions.EnableLoopInvariantCodeMotion);
			compilerOptions.TwoPassOptimizations = settings.GetValueAsBoolean("Optimizations.TwoPass", compilerOptions.TwoPassOptimizations);
			compilerOptions.EnableLongExpansion = settings.GetValueAsBoolean("Optimizations.LongExpansion", compilerOptions.EnableLongExpansion);
			compilerOptions.EnablePlatformOptimizations = settings.GetValueAsBoolean("Optimizations.Platform", compilerOptions.EnablePlatformOptimizations);
			compilerOptions.EnableInlineMethods = settings.GetValueAsBoolean("Optimizations.Inline", compilerOptions.EnableInlineMethods);
			compilerOptions.InlineMaximum = settings.GetValueAsInteger("Optimizations.Inline.Maximum", compilerOptions.InlineMaximum);
			compilerOptions.InlineAggressiveMaximum = settings.GetValueAsInteger("Optimizations.Inline.AggressiveMaximum", compilerOptions.InlineAggressiveMaximum);
			compilerOptions.InlineExplicitOnly = settings.GetValueAsBoolean("Optimizations.Inline.ExplicitOnly", compilerOptions.InlineExplicitOnly);

			var platform = settings.GetValue("Compiler.Platform");
			if (platform != null)
				compilerOptions.Platform = GetPlatform(platform);
		}

		private static BaseArchitecture GetPlatform(string platform)
		{
			switch (platform.ToLower())
			{
				case "x86": return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "x64": return Platform.x64.Architecture.CreateArchitecture(Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				case "armv8a32": return Platform.ARMv8A32.Architecture.CreateArchitecture(Platform.ARMv8A32.ArchitectureFeatureFlags.AutoDetect);
				default: return null;
			}
		}
	}
}
