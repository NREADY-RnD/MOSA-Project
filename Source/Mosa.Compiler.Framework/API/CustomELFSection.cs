// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker.Elf;
using System.IO;

namespace Mosa.Compiler.Framework.API
{
	public class CustomELFSection
	{
		public string Name { get; set; }

		public ulong VirtualAddress { get; set; } = 0;

		public ulong PhysicalAddress { get; set; } = 0;

		public long Size { get { return Stream.Length; } }

		public bool Read { get; set; } = true;

		public bool Write { get; set; } = false;

		public bool Execute { get; set; } = false;

		public Stream Stream { get; set; }
	}
}
