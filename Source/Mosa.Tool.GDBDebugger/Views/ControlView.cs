// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class ControlView : DebugDockContent
	{
		public ulong ReturnAddress { get; private set; } = 0;

		public ControlView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnPause()
		{
			btnPause.Enabled = false;

			ReturnAddress = 0;

			ulong ebp = Platform.StackFrame.Value;
			ulong ip = Platform.InstructionPointer.Value;

			if (ebp == 0 || ip == 0)
				return;

			MemoryCache.ReadMemory(ebp, 8, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] memory)
		{
			if (memory.Length < 8)
				return; // something went wrong!

			ulong ebp = MainForm.ToLong(memory, 0, 4);
			ulong ip = MainForm.ToLong(memory, 4, 4);

			if (ip == 0)
				return;

			ReturnAddress = ip;
			btnPause.Enabled = true;
		}

		private void btnStep_Click(object sender, EventArgs e)
		{
			MemoryCache.Clear();
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step();
			MainForm.ResendBreakPoints();
		}

		private void btnStepN_Click(object sender, EventArgs e)
		{
			if (GDBConnector.IsRunning)
				return;

			uint steps;

			try
			{
				steps = Convert.ToUInt32(tbSteps.Text);
			}
			catch
			{
				MessageBox.Show($"Invalid input, '{tbSteps.Text}' is not a valid number.");
				return;
			}

			if (steps == 0)
				return;

			MemoryCache.Clear();

			if (MainForm.BreakPoints.Count != 0)
			{
				GDBConnector.ClearAllBreakPoints();
				GDBConnector.Step(true);

				while (GDBConnector.IsRunning)
				{
					Thread.Sleep(10);
				}

				MainForm.ResendBreakPoints();

				steps--;
			}

			if (steps == 0)
				return;

			GDBConnector.StepN(steps);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			// ???
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			Continue();
		}

		private void Continue()
		{
			if (GDBConnector == null)
				return;

			if (GDBConnector.IsRunning)
				return;

			MemoryCache.Clear();

			if (MainForm.BreakPoints.Count != 0)
			{
				GDBConnector.ClearAllBreakPoints();
				GDBConnector.Step(true);

				while (GDBConnector.IsRunning)
				{
					Thread.Sleep(10);
				}

				MainForm.ResendBreakPoints();
			}

			GDBConnector.Continue();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			GDBConnector.Break();
			GDBConnector.GetRegisters();
		}

		private void btnStepOut_Click(object sender, EventArgs e)
		{
			if (Platform == null)
				return;

			MainForm.AddBreakPoint(ReturnAddress, true);

			Continue();
		}
	}
}
