// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class StubMethods
	{
		[IntrinsicMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
		private static void SizeOf(Context context, MethodCompiler methodCompiler)
		{
			return;
		}
	}
}
