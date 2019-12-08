// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.API
{
	public class CompilerHooks
	{
		#region Delegates definitions

		public delegate void CompilerProgressHandler(int totalMethods, int completedMethods);

		public delegate void CompilerEventHandler(CompilerEvent compilerEvent, string message, int threadID);

		public delegate void CompilerTraceLogHandler(TraceLog traceLog);

		public delegate void MethodCompiledHandler(MosaMethod method);

		public delegate IList<CustomELFSection> CustomElfSectionsHandler();

		public delegate IList<CustomProgramHeader> CustomProgramHeadersHandler();

		public delegate CompilerTraceLogHandler MethodInstructionTraceHandler(MosaMethod method);

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
	}
}
