// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	public delegate Stream SectionEmitter();

	public sealed class ElfLinker
	{
		#region Data Members

		private readonly MosaLinker Linker;

		private readonly LinkerFormatType LinkerFormatType;
		private readonly ElfHeader elfheader = new ElfHeader();

		private readonly List<Section> sections = new List<Section>();
		private readonly Dictionary<string, Section> sectionByName = new Dictionary<string, Section>();

		public Section nullSection = new Section();
		private readonly Section sectionHeaderStringSection = new Section();
		private readonly Section stringSection = new Section();
		private readonly Section symbolSection = new Section();

		private readonly List<byte> sectionHeaderStringTable = new List<byte>();

		private readonly List<byte> stringTable = new List<byte>(4096);

		private readonly Dictionary<LinkerSymbol, uint> symbolTableOffset = new Dictionary<LinkerSymbol, uint>();

		private BinaryWriter writer;

		private static readonly string[] SectionNames = { ".text", ".data", ".rodata", ".bss" };

		private MachineType MachineType;

		#endregion Data Members

		#region Properties

		public uint BaseFileOffset { get; }

		public uint SectionAlignment { get; }

		#endregion Properties

		public ElfLinker(MosaLinker linker, LinkerFormatType linkerFormatType, MachineType machineType)
		{
			Linker = linker;
			LinkerFormatType = linkerFormatType;
			MachineType = machineType;

			sectionHeaderStringTable.Add((byte)'\0');
			stringTable.Add((byte)'\0');

			BaseFileOffset = 0x1000;   // required by ELF
			SectionAlignment = 0x1000; // default 1K
		}

		#region Helpers

		private void AddSection(Section section)
		{
			Debug.Assert(section != null);

			var index = (ushort)sections.Count;
			section.Index = index;

			sections.Add(section);

			if (section.Name != null)
			{
				sectionByName.Add(section.Name, section);

				section.NameIndex = AddToSectionHeaderStringTable(section.Name);
			}
		}

		private Section GetSection(string name)
		{
			return sectionByName[name];
		}

		private Section GetSection(SectionKind sectionKind)
		{
			Debug.Assert(sectionKind != SectionKind.Unknown, "sectionKind != SectionKind.Unknown");
			return GetSection(SectionNames[(int)sectionKind]);
		}

		private void ResolveSectionOffset(Section section)
		{
			if (section.Type == SectionType.NoBits || section.Type == SectionType.Null)
				return;

			// Already resolved?
			if (section.Offset != 0)
				return;

			var offset = BaseFileOffset;

			foreach (var entry in sections)
			{
				if (entry.Offset == 0)
					continue;

				offset = Math.Max(offset, entry.Offset + Alignment.AlignUp(entry.Size, SectionAlignment));
			}

			section.Offset = offset;
		}

		#endregion Helpers

		public void Emit(Stream stream)
		{
			writer = new BinaryWriter(stream, Encoding.Unicode);

			// Create the sections headers
			CreateSections();

			// Write Sections
			WriteSections();

			// Write program headers -- must be called before writing Elf header
			WriteProgramHeaders();

			// Write section headers
			WriteSectionHeaders();

			// Write ELF header
			WriteElfHeader();
		}

		private void CreateSections()
		{
			CreateNullSection();

			CreateStandardSections();

			CreateSymbolSection();

			CreateStringSection();

			// FIXME: set by condition
			CreateRelocationSections();

			CreateSectionHeaderStringSection();
		}

		private void CreateStandardSections()
		{
			var previous = nullSection;

			foreach (var linkerSection in Linker.Sections)
			{
				if (linkerSection.Size == 0 && linkerSection.SectionKind != SectionKind.BSS)
					continue;

				var section = new Section()
				{
					Name = SectionNames[(int)linkerSection.SectionKind],
					Address = linkerSection.VirtualAddress,
					Size = Alignment.AlignUp(linkerSection.Size, SectionAlignment),
					AddressAlignment = SectionAlignment,
					EmitMethod = WriteLinkerSection,
					SectionKind = linkerSection.SectionKind
				};

				switch (linkerSection.SectionKind)
				{
					case SectionKind.Text:
						section.Type = SectionType.ProgBits;
						section.Flags = SectionAttribute.AllocExecute;
						break;

					case SectionKind.Data:
						section.Type = SectionType.ProgBits;
						section.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
						break;

					case SectionKind.ROData:
						section.Type = SectionType.ProgBits;
						section.Flags = SectionAttribute.Alloc;
						break;

					case SectionKind.BSS:
						section.Type = SectionType.NoBits;
						section.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
						break;
				}

				section.AddDependency(previous);

				AddSection(section);

				previous = section;
			}
		}

		private void CreateNullSection()
		{
			nullSection = new Section()
			{
				Name = null,
				Type = SectionType.Null
			};
			AddSection(nullSection);
		}

		private void CreateSectionHeaderStringSection()
		{
			sectionHeaderStringSection.Name = ".shstrtab";
			sectionHeaderStringSection.Type = SectionType.StringTable;
			sectionHeaderStringSection.AddressAlignment = SectionAlignment;
			sectionHeaderStringSection.EmitMethod = WriteSectionHeaderStringSection;

			AddSection(sectionHeaderStringSection);
		}

		private void CreateStringSection()
		{
			stringSection.Name = ".strtab";
			stringSection.Type = SectionType.StringTable;
			stringSection.AddressAlignment = SectionAlignment;
			stringSection.EmitMethod = WriteStringSection;

			AddSection(stringSection);

			sectionHeaderStringSection.AddDependency(stringSection);
		}

		private void CreateSymbolSection()
		{
			symbolSection.Name = ".symtab";
			symbolSection.Type = SectionType.SymbolTable;
			symbolSection.AddressAlignment = SectionAlignment;
			symbolSection.EntrySize = SymbolEntry.GetEntrySize(LinkerFormatType);
			symbolSection.Link = stringSection;
			symbolSection.EmitMethod = WriteSymbolSection;

			AddSection(symbolSection);

			stringSection.AddDependency(symbolSection);
			sectionHeaderStringSection.AddDependency(symbolSection);
		}

		private void CreateRelocationSections()
		{
			foreach (var kind in MosaLinker.SectionKinds)
			{
				bool reloc = false;
				bool relocAddend = false;

				foreach (var symbol in Linker.Symbols)
				{
					if (symbol.SectionKind != kind)
						continue;

					if (symbol.IsExternalSymbol)
						continue;

					foreach (var patch in symbol.LinkRequests)
					{
						if (patch.LinkType == LinkType.Size)
							continue;

						if (!patch.ReferenceSymbol.IsExternalSymbol)
							continue;

						if (patch.ReferenceOffset == 0)
							reloc = true;
						else
							relocAddend = true;

						if (reloc && relocAddend)
							break;
					}

					if (reloc && relocAddend)
						break;
				}

				if (reloc)
				{
					CreateRelocationSection(kind, false);
				}

				if (relocAddend)
				{
					CreateRelocationSection(kind, true);
				}
			}
		}

		private void CreateRelocationSection(SectionKind kind, bool addend)
		{
			var relocationSection = new Section()
			{
				Name = (addend ? ".rela" : ".rel") + SectionNames[(int)kind],
				Type = addend ? SectionType.RelocationA : SectionType.Relocation,
				Link = symbolSection,
				Info = GetSection(kind),
				AddressAlignment = SectionAlignment,
				EntrySize = addend ? RelocationAddendEntry.GetEntrySize(LinkerFormatType) : RelocationEntry.GetEntrySize(LinkerFormatType),
				EmitMethod = WriteRelocationSections
			};

			AddSection(relocationSection);

			relocationSection.AddDependency(symbolSection);
			relocationSection.AddDependency(GetSection(kind));
		}

		private void WriteSections()
		{
			var completed = new HashSet<Section>();

			for (int i = 0; i < sections.Count;)
			{
				var section = sections[i];

				if (completed.Contains(section))
				{
					i++;
					continue;
				}

				bool dependency = false;

				foreach (var dep in section.Dependencies)
				{
					if (!completed.Contains(dep))
					{
						dependency = true;
						break;
					}
				}

				if (!dependency)
				{
					WriteSection(section);
					completed.Add(section);
					i = 0;
					continue;
				}

				i++;
			}
		}

		private void WriteSection(Section section)
		{
			if (section.Type == SectionType.NoBits || section.Type == SectionType.Null)
				return;

			if (section.EmitMethod == null)
				return;

			// Set the next available offset
			ResolveSectionOffset(section);
			writer.SetPosition(section.Offset);

			section.EmitMethod.Invoke(section, writer);
		}

		private void WriteElfHeader()
		{
			writer.SetPosition(0);

			elfheader.Type = FileType.Executable;
			elfheader.Machine = MachineType;
			elfheader.EntryAddress = (uint)Linker.EntryPoint.VirtualAddress;
			elfheader.CreateIdent((LinkerFormatType == LinkerFormatType.Elf32) ? IdentClass.Class32 : IdentClass.Class64, IdentData.Data2LSB);
			elfheader.SectionHeaderNumber = (ushort)sections.Count;
			elfheader.SectionHeaderStringIndex = sectionHeaderStringSection.Index;

			elfheader.Write(LinkerFormatType, writer);
		}

		private void WriteProgramHeaders()
		{
			elfheader.ProgramHeaderOffset = ElfHeader.GetEntrySize(LinkerFormatType);

			writer.SetPosition(elfheader.ProgramHeaderOffset);

			elfheader.ProgramHeaderNumber = 0;

			foreach (var section in sections)
			{
				if (section.Address == 0 || section.SectionKind == SectionKind.Unknown)
					continue;

				if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
					continue;

				var programHeader = new ProgramHeader
				{
					Alignment = SectionAlignment,
					FileSize = Alignment.AlignUp(section.Size, SectionAlignment),
					MemorySize = Alignment.AlignUp(section.Size, SectionAlignment),
					Offset = section.Offset,
					VirtualAddress = section.Address,
					PhysicalAddress = section.Address,
					Type = ProgramHeaderType.Load,
					Flags =
						(section.SectionKind == SectionKind.Text) ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
						(section.SectionKind == SectionKind.ROData) ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
				};

				programHeader.Write(LinkerFormatType, writer);

				elfheader.ProgramHeaderNumber++;
			}
		}

		private void WriteSectionHeaders()
		{
			elfheader.SectionHeaderOffset = elfheader.ProgramHeaderOffset + (ProgramHeader.GetEntrySize(LinkerFormatType) * elfheader.ProgramHeaderNumber);

			writer.SetPosition(elfheader.SectionHeaderOffset);

			foreach (var section in sections)
			{
				section.WriteSectionHeader(LinkerFormatType, writer);
			}
		}

		private void WriteLinkerSection(Section section, BinaryWriter writer)
		{
			var linkerSection = Linker.Sections[(int)section.SectionKind];

			writer.SetPosition(section.Offset);

			Linker.WriteLinkerSectionTo(writer.BaseStream, linkerSection, section.Offset);
		}

		private Stream WriteLinkerSectionV2(Section section)
		{
			var linkerSection = Linker.Sections[(int)section.SectionKind];

			var stream = new MemoryStream();

			Linker.WriteLinkerSectionTo(stream, linkerSection, section.Offset);

			return stream;
		}

		private void WriteSectionHeaderStringSection(Section section, BinaryWriter writer)
		{
			Debug.Assert(section == sectionHeaderStringSection);

			section.Size = (uint)sectionHeaderStringTable.Count;
			writer.Write(sectionHeaderStringTable.ToArray());
		}

		private void WriteStringSection(Section section, BinaryWriter writer)
		{
			Debug.Assert(section == stringSection);

			writer.Write(stringTable.ToArray());

			section.Size = (uint)stringTable.Count;
		}

		private void WriteSymbolSection(Section section, BinaryWriter writer)
		{
			Debug.Assert(section == symbolSection);

			var emitSymbols = Linker.LinkerSettings.Symbols;

			// first entry is completely filled with zeros
			writer.WriteZeroBytes(SymbolEntry.GetEntrySize(LinkerFormatType));

			uint count = 1;

			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.SectionKind == SectionKind.Unknown && symbol.LinkRequests.Count == 0)
					continue;

				Debug.Assert(symbol.SectionKind != SectionKind.Unknown, "symbol.SectionKind != SectionKind.Unknown");

				if (!(symbol.IsExternalSymbol || emitSymbols))
					continue;

				if (symbol.VirtualAddress == 0)
					continue;

				var name = GetFinalSymboName(symbol);

				var symbolEntry = new SymbolEntry()
				{
					Name = AddToStringTable(name),
					Value = symbol.VirtualAddress,
					Size = symbol.Size,
					SymbolBinding = SymbolBinding.Global,
					SymbolVisibility = SymbolVisibility.Default,
					SymbolType = symbol.SectionKind == SectionKind.Text ? SymbolType.Function : SymbolType.Object,
					SectionHeaderTableIndex = GetSection(symbol.SectionKind).Index
				};

				symbolEntry.Write(LinkerFormatType, writer);
				symbolTableOffset.Add(symbol, count);

				count++;
			}

			section.Size = count * SymbolEntry.GetEntrySize(LinkerFormatType);
		}

		private void WriteRelocationSections(Section section, BinaryWriter writer)
		{
			if (section.SectionKind == SectionKind.BSS)
				return;

			if (!ContainsKind(section.SectionKind))
				return;

			if (section.Type == SectionType.Relocation)
			{
				WriteRelocationSection(section, writer);
			}
			else if (section.Type == SectionType.RelocationA)
			{
				WriteRelocationAddendSection(section, writer);
			}
		}

		private void WriteRelocationSection(Section section, BinaryWriter writer)
		{
			int count = 0;

			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.IsExternalSymbol)
					continue;

				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset != 0)
						continue;

					if (patch.ReferenceSymbol.SectionKind != section.SectionKind)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
						continue;

					//Debug.Assert(symbolTableOffset.ContainsKey(patch.ReferenceSymbol));

					var relocationEntry = new RelocationEntry()
					{
						RelocationType = ConvertType(MachineType, patch.LinkType, patch.PatchType),
						Symbol = symbolTableOffset[patch.ReferenceSymbol],
						Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
					};

					relocationEntry.Write(LinkerFormatType, writer);
					count++;
				}

				section.Size = (uint)(count * RelocationEntry.GetEntrySize(LinkerFormatType));
			}
		}

		private void WriteRelocationAddendSection(Section section, BinaryWriter writer)
		{
			int count = 0;

			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.IsExternalSymbol)
					continue;

				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset == 0)
						continue;

					if (patch.ReferenceSymbol.SectionKind != section.SectionKind)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
						continue;

					var relocationAddendEntry = new RelocationAddendEntry()
					{
						RelocationType = ConvertType(MachineType, patch.LinkType, patch.PatchType),
						Symbol = symbolTableOffset[patch.ReferenceSymbol],
						Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
						Addend = (ulong)patch.ReferenceOffset,
					};

					relocationAddendEntry.Write(LinkerFormatType, writer);

					count++;
				}
			}

			section.Size = (uint)(count * RelocationAddendEntry.GetEntrySize(LinkerFormatType));
		}

		private uint AddToStringTable(string text)
		{
			if (text.Length == 0)
				return 0;

			uint index = (uint)stringTable.Count;

			foreach (char c in text)
			{
				stringTable.Add((byte)c);
			}

			stringTable.Add((byte)'\0');

			return index;
		}

		private uint AddToSectionHeaderStringTable(string text)
		{
			if (text.Length == 0)
				return 0;

			uint index = (uint)sectionHeaderStringTable.Count;

			foreach (char c in text)
			{
				sectionHeaderStringTable.Add((byte)c);
			}

			sectionHeaderStringTable.Add((byte)'\0');

			return index;
		}

		private string GetFinalSymboName(LinkerSymbol symbol)
		{
			if (symbol.ExternalSymbolName != null)
				return symbol.ExternalSymbolName;

			if (symbol.SectionKind != SectionKind.Text)
				return symbol.Name;

			if (!Linker.EmitShortSymbolName)
				return symbol.Name;

			int pos = symbol.Name.LastIndexOf(") ");

			if (pos < 0)
				return symbol.Name;

			var shortname = symbol.Name.Substring(0, pos + 1);

			return shortname;
		}

		private bool ContainsKind(SectionKind kind)
		{
			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.SectionKind == kind)
					return true;
			}

			return false;
		}

		private static RelocationType ConvertType(MachineType machineType, LinkType linkType, PatchType patchType)
		{
			if (machineType == MachineType.Intel386)
			{
				if (linkType == LinkType.AbsoluteAddress)
					return RelocationType.R_386_32;
				else if (linkType == LinkType.RelativeOffset)
					return RelocationType.R_386_PC32;
				else if (linkType == LinkType.Size)
					return RelocationType.R_386_COPY;
			}
			else if (machineType == MachineType.ARM)
			{
				if (linkType == LinkType.AbsoluteAddress)
					return RelocationType.R_ARM_ABS16;
				else if (linkType == LinkType.RelativeOffset)
					return RelocationType.R_ARM_PC24;
				else if (linkType == LinkType.Size)
					return RelocationType.R_ARM_COPY;
			}

			return RelocationType.R_386_NONE;
		}
	}
}
