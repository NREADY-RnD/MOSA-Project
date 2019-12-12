// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Trace
{
	public sealed class CompilerTrace
	{
		private ITraceListener TraceListener;

		public int TraceLevel { get; }

		public CompilerTrace(int traceLevel)
		{
			TraceLevel = traceLevel;
			TraceListener = null;
		}

		public bool IsTraceable(int traceLevel)
		{
			return TraceLevel != 0 && TraceLevel >= traceLevel;
		}

		public void PostMethodCompiled(MosaMethod method)
		{
			TraceListener?.OnMethodCompiled(method);
		}

		public void PostTraceLog(TraceLog traceLog, bool signalStatusUpdate = false)
		{
			if (traceLog == null)
				return;

			TraceListener?.OnTraceLog(traceLog);
		}

		public void PostCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			TraceListener?.OnCompilerEvent(compilerEvent, message, threadID);
		}

		public void UpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			TraceListener?.OnProgress(totalMethods, completedMethods);
		}
	}
}
