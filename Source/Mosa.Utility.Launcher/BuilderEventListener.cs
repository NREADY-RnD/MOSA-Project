// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Utility.Launcher
{
	internal class BuilderEventListener
	{
		private readonly Builder builder;
		private readonly object _lock = new object();

		public BuilderEventListener(Builder builder)
		{
			this.builder = builder;
		}

		private void OnProgress(int totalMethods, int completedMethods)
		{
			builder.CompilerHook?.NotifyProgress(totalMethods, completedMethods);
		}

		private void OnCompilerEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent == CompilerEvent.CompilerStart
				|| compilerEvent == CompilerEvent.CompilerEnd
				|| compilerEvent == CompilerEvent.CompilingMethods
				|| compilerEvent == CompilerEvent.CompilingMethodsCompleted
				|| compilerEvent == CompilerEvent.InlineMethodsScheduled
				|| compilerEvent == CompilerEvent.LinkingStart
				|| compilerEvent == CompilerEvent.LinkingEnd
				|| compilerEvent == CompilerEvent.Warning
				|| compilerEvent == CompilerEvent.Error
				|| compilerEvent == CompilerEvent.Exception)
			{
				string status = $"Compiling: {$"{(DateTime.Now - builder.CompileStartTime).TotalSeconds:0.00}"} secs: {compilerEvent.ToText()}";

				if (!string.IsNullOrEmpty(message))
					status += $"- { message}";

				lock (_lock)
				{
					builder.AddOutput(status);
				}
			}
			else if (compilerEvent == CompilerEvent.Counter)
			{
				lock (_lock)
				{
					builder.AddCounters(message);
				}
			}
		}
	}
}
