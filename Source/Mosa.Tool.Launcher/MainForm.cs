// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using MetroFramework.Forms;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.BootImage;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : MetroForm, IBuilderEvent, IStarterEvent
	{
		private Settings Settings = new Settings();

		private LauncherSettingsWrapper LauncherSettingsWrapper;

		public Builder Builder;

		public AppLocations AppLocations { get; set; }

		private int TotalMethods = 0;
		private int CompletedMethods = 0;

		private readonly BindingList<IncludedEntry> includedEntries = new BindingList<IncludedEntry>();

		private class IncludedEntry
		{
			[Browsable(false)]
			public IncludeFile IncludeFile { get; }

			public int Size { get { return IncludeFile.Length; } }
			public string Destination { get { return IncludeFile.Filename; } }
			public string Source { get { return IncludeFile.SourceFileName; } }

			public IncludedEntry(IncludeFile file)
			{
				IncludeFile = file;
			}
		}

		public MainForm()
		{
			InitializeComponent();

			LauncherSettingsWrapper = new LauncherSettingsWrapper(Settings);

			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			dataGridView1.DataSource = includedEntries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 175;
			dataGridView1.Columns[2].Width = 500;

			AddOutput("Current Directory: " + Environment.CurrentDirectory);

			cbBootFormat.SelectedIndex = 0;
		}

		public void UpdateStatusLabel(string msg)
		{
			tsStatusLabel.Text = msg;
		}

		private void NewStatus(string info)
		{
			AddOutput(info);
		}

		void IBuilderEvent.NewStatus(string status)
		{
			Invoke((MethodInvoker)(() => NewStatus(status)));
		}

		void IStarterEvent.NewStatus(string status)
		{
			Invoke((MethodInvoker)(() => NewStatus(status)));
		}

		private void UpdateProgress()
		{
			progressBar1.Maximum = TotalMethods;
			progressBar1.Value = CompletedMethods;
		}

		void IBuilderEvent.UpdateProgress(int total, int at)
		{
			TotalMethods = total;
			CompletedMethods = at;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			UpdateInterfaceAppLocations();

			Refresh();

			if (LauncherSettingsWrapper.AutoStart)
			{
				CompileBuildAndStart();
			}
		}

		public void AddOutput(string data)
		{
			if (data == null)
				return;

			rtbOutput.AppendText(data);
			rtbOutput.AppendText("\n");
			rtbOutput.Update();
		}

		public void AddCounters(string data)
		{
			rtbCounters.AppendText(data);
			rtbCounters.AppendText("\n");
			rtbCounters.Update();
		}

		private void BtnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var filename = openFileDialog1.FileName;

				UpdateSettings();

				Settings.ClearProperty("Compiler.SourceFiles");
				Settings.AddPropertyListValue("Compiler.SourceFiles", filename);

				Settings.ClearProperty("SearchPaths");
				Settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(filename));

				UpdateDisplay();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text = "MOSA Launcher v" + CompilerVersion.VersionString;
			tbApplicationLocations.SelectedTab = tabOptions;

			foreach (var includeFile in LauncherSettingsWrapper.IncludeFiles)
			{
				includedEntries.Add(new IncludedEntry(includeFile));
			}
		}

		private void BtnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				LauncherSettingsWrapper.DestinationDirectory = folderBrowserDialog1.SelectedPath;

				lbDestinationDirectory.Text = LauncherSettingsWrapper.DestinationDirectory;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F5)
			{
				CompileBuildAndStart();
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CompileBuildAndStart()
		{
			rtbOutput.Clear();
			rtbCounters.Clear();

			if (CheckKeyPressed())
				return;

			tbApplicationLocations.SelectedTab = tabOutput;

			ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
			{
				try
				{
					Builder = new Builder(Settings, AppLocations, this);

					Builder.Compile();
				}
				catch (Exception e)
				{
					OnException(e.ToString());
				}
				finally
				{
					if (!Builder.HasCompileError)
					{
						OnCompileCompleted();
					}
				}
			}));
		}

		private void OnException(string data)
		{
			MethodInvoker method = () => AddOutput(data);

			Invoke(method);
		}

		private void OnCompileCompleted()
		{
			MethodInvoker method = CompileCompleted;

			Invoke(method);
		}

		private void CompileCompleted()
		{
			if (Builder.LauncherOptions.LaunchVM)
			{
				foreach (var line in Builder.Counters)
				{
					AddCounters(line);
				}

				if (CheckKeyPressed())
					return;

				LauncherSettingsWrapper.ImageFile = LauncherSettingsWrapper.ImageFile ?? Builder.ImageFile;

				var starter = new Starter(LauncherSettingsWrapper.Settings, AppLocations, this, Builder.Linker);

				starter.Launch();
			}

			if (LauncherSettingsWrapper.ExitOnLaunch)
			{
				Application.Exit();
			}
		}

		private bool CheckKeyPressed()
		{
			return ((ModifierKeys & Keys.Shift) != 0) || ((ModifierKeys & Keys.Control) != 0);
		}

		private void Btn1_Click(object sender, EventArgs e)
		{
			UpdateSettings();

			var result = CheckOptions.Verify(LauncherSettingsWrapper);

			if (result == null)
			{
				CompileBuildAndStart();
			}
			else
			{
				UpdateStatusLabel("ERROR: " + result);
				AddOutput(result);
			}
		}

		private void CbVBEVideo_CheckedChanged(object sender, EventArgs e)
		{
			tbMode.Enabled = cbVBEVideo.Checked;
		}

		private void BtnAddFiles_Click(object sender, EventArgs e)
		{
			using (var open = new OpenFileDialog())
			{
				open.Multiselect = true;
				open.Filter = "All files (*.*)|*.*";

				if (open.ShowDialog(this) == DialogResult.OK)
				{
					foreach (var file in open.FileNames)
					{
						var includeFile = new IncludeFile(file, Path.GetFileName(file));

						includedEntries.Add(new IncludedEntry(includeFile));
					}
				}
			}
		}

		private void BtnRemoveFiles_Click(object sender, EventArgs e)
		{
			while (dataGridView1.SelectedRows.Count > 0)
			{
				dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgress();
		}

		public void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);

			UpdateDisplay();
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.EmitBinary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Platform", cbPlatform.Text);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Multiboot.Version", "v2");
			Settings.SetValue("Compiler.Platform", "x86");
		}

		private void UpdateInterfaceAppLocations()
		{
			lbBOCHSExecutable.Text = AppLocations.BOCHS;
			lbNDISASMExecutable.Text = AppLocations.NDISASM;
			lbQEMUExecutable.Text = AppLocations.QEMU;
			lbQEMUBIOSDirectory.Text = AppLocations.QEMUBIOSDirectory;
			lbQEMUImgApplication.Text = AppLocations.QEMUImg;
			lbVMwarePlayerExecutable.Text = AppLocations.VMwarePlayer;
			lbmkisofsExecutable.Text = AppLocations.Mkisofs;
		}

		private void UpdateSettings()
		{
			LauncherSettingsWrapper.EnableSSA = cbEnableSSA.Checked;
			LauncherSettingsWrapper.EnableBasicOptimizations = cbEnableIROptimizations.Checked;
			LauncherSettingsWrapper.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			LauncherSettingsWrapper.GenerateNASMFile = cbGenerateNASMFile.Checked;
			LauncherSettingsWrapper.GenerateASMFile = cbGenerateASMFile.Checked;
			LauncherSettingsWrapper.GenerateMapFile = cbGenerateMapFile.Checked;
			LauncherSettingsWrapper.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			LauncherSettingsWrapper.ExitOnLaunch = cbExitOnLaunch.Checked;
			LauncherSettingsWrapper.EnableQemuGDB = cbEnableQemuGDB.Checked;
			LauncherSettingsWrapper.LaunchGDB = cbLaunchGDB.Checked;
			LauncherSettingsWrapper.LaunchGDBDebugger = cbLaunchMosaDebugger.Checked;
			LauncherSettingsWrapper.MultiThreading = cbCompilerUsesMultipleThreads.Checked;
			LauncherSettingsWrapper.EmulatorMemoryInMB = (int)nmMemory.Value;
			LauncherSettingsWrapper.EnableInlineMethods = cbInline.Checked;
			LauncherSettingsWrapper.InlineExplicitOnly = cbInlineExplicitOnly.Checked;
			LauncherSettingsWrapper.VBEVideo = cbVBEVideo.Checked;
			LauncherSettingsWrapper.EnableLongExpansion = cbLongExpansion.Checked;
			LauncherSettingsWrapper.TwoPassOptimizations = cbTwoPassOptimizations.Checked;
			LauncherSettingsWrapper.EnableLongExpansion = cbLongExpansion.Checked;
			LauncherSettingsWrapper.EnableValueNumbering = cbValueNumbering.Checked;
			LauncherSettingsWrapper.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			LauncherSettingsWrapper.BaseAddress = tbBaseAddress.Text.ParseHexOrInteger();
			LauncherSettingsWrapper.EmitAllSymbols = cbEmitSymbolTable.Checked;
			LauncherSettingsWrapper.EmitStaticRelocations = cbRelocationTable.Checked;
			LauncherSettingsWrapper.EnableMethodScanner = cbEnableMethodScanner.Checked;
			LauncherSettingsWrapper.GenerateCompileTimeFile = cbGenerateCompilerTime.Checked;
			LauncherSettingsWrapper.EnableBitTracker = cbBitTracker.Checked;
			LauncherSettingsWrapper.EnablePlatformOptimizations = cbPlatformOptimizations.Checked;
			LauncherSettingsWrapper.EnableLoopInvariantCodeMotion = cbLoopInvariantCodeMotion.Checked;

			if (LauncherSettingsWrapper.VBEVideo)
			{
				var Mode = tbMode.Text.Split('x');

				if (Mode.Length == 3)
				{
					try
					{
						LauncherSettingsWrapper.Width = int.Parse(Mode[0]);
						LauncherSettingsWrapper.Height = int.Parse(Mode[1]);
						LauncherSettingsWrapper.Depth = int.Parse(Mode[2]);
					}
					catch (Exception e)
					{
						throw new Exception("An error occurred while parsing VBE Mode: " + e.Message);
					}
				}
				else
				{
					throw new Exception("An error occurred while parsing VBE Mode: There wasn't 3 arguments");
				}
			}

			switch (cbImageFormat.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.ImageFormat = "IMG"; break;
				case 1: LauncherSettingsWrapper.ImageFormat = "ISO"; break;
				case 2: LauncherSettingsWrapper.ImageFormat = "VHD"; break;
				case 3: LauncherSettingsWrapper.ImageFormat = "VDI"; break;
				case 4: LauncherSettingsWrapper.ImageFormat = "VMDK"; break;
				default: break;
			}

			switch (cbEmulator.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.Emulator = "Qemu"; break;
				case 1: LauncherSettingsWrapper.Emulator = "Bochs"; break;
				case 2: LauncherSettingsWrapper.Emulator = "VMware"; break;
				default: break;
			}

			switch (cbDebugConnectionOption.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.SerialConnection = "None"; break;
				case 1: LauncherSettingsWrapper.SerialConnection = "Pipe"; break;
				case 2: LauncherSettingsWrapper.SerialConnection = "TCPServer"; break;
				case 3: LauncherSettingsWrapper.SerialConnection = "TCPClient"; break;
				default: break;
			}

			switch (cbBootFileSystem.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.FileSystem = "FAT12"; break;
				case 1: LauncherSettingsWrapper.FileSystem = "FAT16"; break;
				default: break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.PlatformType = "x86"; break;
				case 1: LauncherSettingsWrapper.PlatformType = "x64"; break;
				default: LauncherSettingsWrapper.PlatformType = "x86"; break;
			}

			switch (cbBootLoader.SelectedIndex)
			{
				case 0: LauncherSettingsWrapper.BootLoader = "syslinux3.72"; break;
				case 1: LauncherSettingsWrapper.BootLoader = "syslinux6.03"; break;
				case 2: LauncherSettingsWrapper.BootLoader = "grub0.97"; break;
				case 3: LauncherSettingsWrapper.BootLoader = "grub2.00"; break;
				default: break;
			}

			LauncherSettingsWrapper.IncludeFiles.Clear();

			foreach (var entry in includedEntries)
			{
				LauncherSettingsWrapper.IncludeFiles.Add(entry.IncludeFile);
			}
		}

		private void UpdateDisplay()
		{
			cbEnableSSA.Checked = LauncherSettingsWrapper.EnableSSA;
			cbEnableIROptimizations.Checked = LauncherSettingsWrapper.EnableBasicOptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = LauncherSettingsWrapper.EnableSparseConditionalConstantPropagation;
			cbGenerateNASMFile.Checked = LauncherSettingsWrapper.GenerateNASMFile;
			cbGenerateASMFile.Checked = LauncherSettingsWrapper.GenerateASMFile;
			cbGenerateMapFile.Checked = LauncherSettingsWrapper.GenerateMapFile;
			cbGenerateDebugInfoFile.Checked = LauncherSettingsWrapper.GenerateDebugFile;
			cbExitOnLaunch.Checked = LauncherSettingsWrapper.ExitOnLaunch;
			cbEnableQemuGDB.Checked = LauncherSettingsWrapper.EnableQemuGDB;
			cbLaunchGDB.Checked = LauncherSettingsWrapper.LaunchGDB;
			cbLaunchMosaDebugger.Checked = LauncherSettingsWrapper.LaunchGDBDebugger;
			cbInline.Checked = LauncherSettingsWrapper.EnableInlineMethods;
			cbInlineExplicitOnly.Checked = LauncherSettingsWrapper.InlineExplicitOnly;
			cbCompilerUsesMultipleThreads.Checked = LauncherSettingsWrapper.MultiThreading;
			nmMemory.Value = LauncherSettingsWrapper.EmulatorMemoryInMB;
			cbVBEVideo.Checked = LauncherSettingsWrapper.VBEVideo;
			tbBaseAddress.Text = "0x" + LauncherSettingsWrapper.BaseAddress.ToString("x8");
			cbRelocationTable.Checked = LauncherSettingsWrapper.EmitStaticRelocations;
			cbEmitSymbolTable.Checked = LauncherSettingsWrapper.EmitAllSymbols;
			tbMode.Text = LauncherSettingsWrapper.Width + "x" + LauncherSettingsWrapper.Height + "x" + LauncherSettingsWrapper.Depth;
			cbLongExpansion.Checked = LauncherSettingsWrapper.EnableLongExpansion;
			cbTwoPassOptimizations.Checked = LauncherSettingsWrapper.TwoPassOptimizations;
			cbValueNumbering.Checked = LauncherSettingsWrapper.EnableValueNumbering;
			cbEnableMethodScanner.Checked = LauncherSettingsWrapper.EnableMethodScanner;
			cbGenerateCompilerTime.Checked = LauncherSettingsWrapper.GenerateCompileTimeFile;
			cbBitTracker.Checked = LauncherSettingsWrapper.EnableBitTracker;
			cbPlatformOptimizations.Checked = LauncherSettingsWrapper.EnablePlatformOptimizations;
			cbLoopInvariantCodeMotion.Checked = LauncherSettingsWrapper.EnableLoopInvariantCodeMotion;

			switch (LauncherSettingsWrapper.ImageFormat.ToUpper())
			{
				case "IMG": cbImageFormat.SelectedIndex = 0; break;
				case "ISO": cbImageFormat.SelectedIndex = 1; break;
				case "VHD": cbImageFormat.SelectedIndex = 2; break;
				case "VDI": cbImageFormat.SelectedIndex = 3; break;
				case "VMDK": cbImageFormat.SelectedIndex = 4; break;
				default: break;
			}

			switch (LauncherSettingsWrapper.Emulator.ToLower())
			{
				case "qemu": cbEmulator.SelectedIndex = 0; break;
				case "bochs": cbEmulator.SelectedIndex = 1; break;
				case "vmware": cbEmulator.SelectedIndex = 2; break;
				default: break;
			}

			switch (LauncherSettingsWrapper.FileSystem.ToLower())
			{
				case "fat12": cbBootFileSystem.SelectedIndex = 0; break;
				case "fat16": cbBootFileSystem.SelectedIndex = 1; break;
				default: break;
			}

			switch (LauncherSettingsWrapper.BootLoader.ToLower())
			{
				case "syslinux3.72": cbBootLoader.SelectedIndex = 0; break;
				case "syslinux6.03": cbBootLoader.SelectedIndex = 1; break;
				case "grub_0_97": cbBootLoader.SelectedIndex = 2; break;
				case "grub_2_00": cbBootLoader.SelectedIndex = 3; break;
				default: break;
			}

			switch (LauncherSettingsWrapper.PlatformType.ToLower())
			{
				case "x86": cbPlatform.SelectedIndex = 0; break;
				case "x64": cbPlatform.SelectedIndex = 1; break;
				default: cbPlatform.SelectedIndex = 0; break;
			}

			if (LauncherSettingsWrapper.SerialConnection != null)
			{
				switch (LauncherSettingsWrapper.SerialConnection.ToLower())
				{
					case "none": cbDebugConnectionOption.SelectedIndex = 0; break;
					case "pipe": cbDebugConnectionOption.SelectedIndex = 1; break;
					case "tcpserver": cbDebugConnectionOption.SelectedIndex = 2; break;
					case "tcpclient": cbDebugConnectionOption.SelectedIndex = 3; break;
					default: cbDebugConnectionOption.SelectedIndex = 0; break;
				}
			}
			else
			{
				cbDebugConnectionOption.SelectedIndex = 0;
			}

			lbDestinationDirectory.Text = LauncherSettingsWrapper.DestinationDirectory ?? string.Empty;

			string filename = LauncherSettingsWrapper.SourceFiles != null && LauncherSettingsWrapper.SourceFiles.Count >= 1 ? LauncherSettingsWrapper.SourceFiles[0] : null;

			lbSourceDirectory.Text = (filename != null) ? Path.GetDirectoryName(filename) : string.Empty;
			lbSource.Text = (filename != null) ? Path.GetFileName(filename) : string.Empty;
		}
	}
}
