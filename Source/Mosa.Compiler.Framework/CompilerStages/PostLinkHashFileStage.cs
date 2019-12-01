// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class PostLinkHashFileStage : PreLinkHashFileStage
	{
		protected override void Finalization()
		{
			Generate(CompilerOptions.PostLinkHashFile);
		}
	}
}
