// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.API
{
	public class CompilerHook
	{
		#region Delegates definitions

		public delegate void CompilerProgressHandler(int totalMethods, int completedMethods);

		public delegate void CompilerEventHandler(CompilerEvent compilerEvent, string message, int threadID);

		public delegate void CompilerTraceLogHandler(TraceLog traceLog);

		public delegate void MethodCompiledHandler(MosaMethod method);

		public delegate IList<CustomELFSection> CustomElfSectionsHandler();

		public delegate IList<CustomProgramHeader> CustomProgramHeadersHandler();

		public delegate CompilerTraceLogHandler MethodInstructionTraceHandler(MosaMethod method);

		public delegate void ExtendCompilerPipelineHandler(Pipeline<BaseCompilerStage> pipeline);

		public delegate void ExtendMethodCompilerPipelineHandler(Pipeline<BaseMethodCompilerStage> pipeline);

		#endregion Delegates definitions

		#region Events

		public event CompilerProgressHandler CompilerProgress;

		public event CompilerEventHandler CompilerEvent;

		public event CompilerTraceLogHandler CompilerTraceLog;

		public event MethodCompiledHandler MethodCompiled;

		#endregion Events

		public CustomElfSectionsHandler CustomElfSections;

		public CustomProgramHeadersHandler CustomProgramHeaders;

		public MethodInstructionTraceHandler MethodInstructionTrace;

		public ExtendCompilerPipelineHandler ExtendCompilerPipeline;

		public ExtendMethodCompilerPipelineHandler ExtendMethodCompilerPipeline;
	}
}
