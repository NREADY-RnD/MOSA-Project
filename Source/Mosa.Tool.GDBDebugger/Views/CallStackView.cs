// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class CallStackView : DebugDockContent
	{
		private class CallStackEntry : TreeNode
		{
			public string MethodName { get { return (Symbol == null) ? "Unknown" : Symbol.Name; } }

			public string HexAddress { get { return Address.ToString((Address <= uint.MaxValue) ? "X4" : "X8"); } }

			public ulong Address { get; private set; }

			private readonly SymbolInfo Symbol = null;

			public CallStackEntry(SymbolInfo symbol, ulong address)
			{
				Symbol = symbol;
				Address = address;

				Text = "[0x" + HexAddress + "] " + (string.IsNullOrEmpty(MethodName) ? "Unknown" : MethodName);
			}
		}

		public CallStackView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			treeView1.MouseDown += (sender, args) => treeView1.SelectedNode = treeView1.GetNodeAt(args.X, args.Y);
		}

		public override void OnRunning()
		{
			treeView1.Nodes.Clear();
		}

		public override void OnPause()
		{
			treeView1.Nodes.Clear();

			if (StackFrame == 0 || InstructionPointer == 0 || StackPointer == 0)
				return;

			AddSymbol(InstructionPointer);

			// FIXME: x86 specific implementation
			var symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer);

			if (symbol != null)
			{
				// new stack frame has not been setup
				MemoryCache.ReadMemory(StackPointer, 8, OnMemoryReadPrologue);
				return;
			}

			symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer - 2);

			if (symbol != null)
			{
				// new stack frame has not been setup
				MemoryCache.ReadMemory(StackPointer + 4, 8, OnMemoryReadPrologue);
				return;
			}

			MemoryCache.ReadMemory(StackFrame, 8, OnMemoryRead);
		}

		private void AddSymbol(ulong ip)
		{
			var symbol = DebugSource.GetFirstSymbol(ip);

			treeView1.Nodes.Add(new CallStackEntry(symbol, ip));
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void OnMemoryReadPrologue(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplayPrologue(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			if (memory.Length < 8)
				return; // something went wrong!

			if (treeView1.Nodes.Count == 0)
				return; // race condition

			// FIXME: x86 specific implementation
			var ebp = MainForm.ToLong(memory, 0, 4);
			var ip = MainForm.ToLong(memory, 4, 4);

			if (ip == 0)
				return;

			AddSymbol(ip);

			if (ebp != 0)
			{
				if (treeView1.Nodes.Count > 20)
					return;

				MemoryCache.ReadMemory(ebp, 8, OnMemoryRead);
			}
		}

		private void UpdateDisplayPrologue(ulong address, byte[] memory)
		{
			if (memory.Length < 8)
				return; // something went wrong!

			if (treeView1.Nodes.Count == 0)
				return; // race condition

			// FIXME: x86 specific implementation
			ulong ip = MainForm.ToLong(memory, 0, 4);

			if (ip == 0)
				return;

			AddSymbol(ip);

			MemoryCache.ReadMemory(StackFrame, 8, OnMemoryRead);
		}

		private void treeView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			var node = treeView1.SelectedNode;

			if (node == null)
				return;

			var clickedEntry = node as CallStackEntry;

			var relativeMousePosition = treeView1.PointToClient(Cursor.Position);

			var menu = new MenuItem(clickedEntry.Text);
			menu.Enabled = false;

			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.HexAddress });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.Address) });
			m.Show(treeView1, relativeMousePosition);
		}
	}
}
