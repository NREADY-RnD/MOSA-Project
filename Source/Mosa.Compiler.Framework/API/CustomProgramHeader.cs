// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker.Elf;

namespace Mosa.Compiler.Framework.API
{
	public class CustomProgramHeader
	{
		public ProgramHeaderType Type;

		public ulong Offset;

		public ulong VirtualAddress;

		public ulong PhysicalAddress;

		public ulong FileSize;

		public ulong MemorySize;

		public ProgramHeaderFlags Flags;
	}
}
