// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			dataGridView1.DataSource = includedEntries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 175;
			dataGridView1.Columns[2].Width = 500;

			AddOutput("Current Directory: " + Environment.CurrentDirectory);

			cbBootFormat.SelectedIndex = 0;

			RegisterPlatforms();
		}

		private void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
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

			if (Settings.GetValue("Launcher.Start", false))
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
		}

		private void BtnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				Settings.SetValue("Image.Destination", folderBrowserDialog1.SelectedPath);

				lbDestinationDirectory.Text = Settings.GetValue("Image.Destination", string.Empty);
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

					Builder.IncludeFiles.Clear();

					foreach (var entry in includedEntries)
					{
						Builder.IncludeFiles.Add(entry.IncludeFile);
					}

					Builder.Build();
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
			if (Settings.GetValue("Launcher.Launch", false))
			{
				foreach (var line in Builder.Counters)
				{
					AddCounters(line);
				}

				if (CheckKeyPressed())
					return;

				var starter = new Starter(Builder.Settings, AppLocations, this, Builder.Linker);

				starter.Launch();
			}

			if (Settings.GetValue("Launcher.Exit", false))
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

			var result = CheckOptions.Verify(Settings);

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

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			if (sourcefiles != null)
			{
				foreach (var sourcefile in sourcefiles)
				{
					Settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(sourcefile));
				}
			}
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.EmitBinary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Advanced.PlugKorlib", true);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Destination", Path.Combine(Path.GetTempPath(), "MOSA"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Emulator.Display", true);
			Settings.SetValue("Launcher.Start", true);
			Settings.SetValue("Launcher.Launch", true);
			Settings.SetValue("Launcher.Exit", true);
			Settings.SetValue("Launcher.Advance.HuntForCorLib", true);
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
			Settings.SetValue("Launcher.SSA", cbEnableSSA.Checked);
			Settings.SetValue("Optimizations.Basic", cbEnableIROptimizations.Checked);
			Settings.SetValue("Optimizations.SCCP", cbEnableSparseConditionalConstantPropagation.Checked);
			Settings.SetValue("CompilerDebug.NasmFile", cbGenerateNASMFile.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", cbGenerateASMFile.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", cbGenerateMapFile.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.DebugFile", cbGenerateDebugInfoFile.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("Launcher.Exit", cbExitOnLaunch.Checked);
			Settings.SetValue("Emulator.GDB", cbEnableQemuGDB.Checked);
			Settings.SetValue("Launcher.Advance.LaunchGDB", cbLaunchGDB.Checked);
			Settings.SetValue("Launcher.Advance.LaunchGDBDebugger", cbLaunchMosaDebugger.Checked);
			Settings.SetValue("Compiler.Multithreading", cbCompilerUsesMultipleThreads.Checked);
			Settings.SetValue("Emulator.Memory", Settings.GetValue("Emulator.Memory", 128));
			Settings.SetValue("Optimizations.Inline", cbInline.Checked);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", cbInlineExplicitOnly.Checked);
			Settings.SetValue("Multiboot.Video", cbVBEVideo.Checked);
			Settings.SetValue("Optimizations.LongExpansion", cbLongExpansion.Checked);
			Settings.SetValue("Optimizations.TwoPass", cbTwoPassOptimizations.Checked);
			Settings.SetValue("Optimizations.ValueNumbering", cbValueNumbering.Checked);
			Settings.SetValue("Compiler.BaseAddress", (uint)tbBaseAddress.Text.ParseHexOrInteger());
			Settings.SetValue("Linker.EmitAllSymbols", cbEmitSymbolTable.Checked);
			Settings.SetValue("Linker.EmitStaticRelocations", cbRelocationTable.Checked);
			Settings.SetValue("Compiler.MethodScanner", cbEnableMethodScanner.Checked);
			Settings.SetValue("CompilerDebug.CompileTimeFile", cbGenerateCompilerTime.Checked);
			Settings.SetValue("Optimizations.BitTracker", cbBitTracker.Checked);
			Settings.SetValue("Optimizations.Platform", cbPlatformOptimizations.Checked);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", cbLoopInvariantCodeMotion.Checked);
			Settings.SetValue("Launcher.Launch", true);

			if (Settings.GetValue("Multiboot.Video", false))
			{
				var Mode = tbMode.Text.Split('x');

				if (Mode.Length == 3)
				{
					try
					{
						Settings.SetValue("Multiboot.Width", int.Parse(Mode[0]));
						Settings.SetValue("Multiboot.Height", int.Parse(Mode[1]));
						Settings.SetValue("Multiboot.Depth", int.Parse(Mode[2]));
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
				case 0: Settings.SetValue("Image.Format", "IMG"); break;
				case 1: Settings.SetValue("Image.Format", "ISO"); break;
				case 2: Settings.SetValue("Image.Format", "VHD"); break;
				case 3: Settings.SetValue("Image.Format", "VDI"); break;
				case 4: Settings.SetValue("Image.Format", "VMDK"); break;
				default: Settings.ClearProperty("Image.Format"); break;
			}

			switch (cbEmulator.SelectedIndex)
			{
				case 0: Settings.SetValue("Emulator", "Qemu"); break;
				case 1: Settings.SetValue("Emulator", "Bochs"); break;
				case 2: Settings.SetValue("Emulator", "VMware"); break;
				default: Settings.ClearProperty("Emulator"); break;
			}

			switch (cbDebugConnectionOption.SelectedIndex)
			{
				case 0: Settings.SetValue("Emulator.Serial", "None"); break;
				case 1: Settings.SetValue("Emulator.Serial", "Pipe"); break;
				case 2: Settings.SetValue("Emulator.Serial", "TCPServer"); break;
				case 3: Settings.SetValue("Emulator.Serial", "TCPClient"); break;
				default: Settings.ClearProperty("Emulator.Serial"); break;
			}

			switch (cbBootFileSystem.SelectedIndex)
			{
				case 0: Settings.SetValue("Image.FileSystem", "FAT12"); break;
				case 1: Settings.SetValue("Image.FileSystem", "FAT16"); break;
				default: Settings.ClearProperty("Image.FileSystem"); break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: Settings.SetValue("Compiler.Platform", "x86"); break;
				case 1: Settings.SetValue("Compiler.Platform", "x64"); break;
				default: Settings.SetValue("Compiler.Platform", "x86"); break;
			}

			switch (cbBootLoader.SelectedIndex)
			{
				case 0: Settings.SetValue("Image.BootLoader", "syslinux3.72"); break;
				case 1: Settings.SetValue("Image.BootLoader", "syslinux6.03"); break;
				case 2: Settings.SetValue("Image.BootLoader", "grub0.97"); break;
				case 3: Settings.SetValue("Image.BootLoader", "grub2.00"); break;
				default: Settings.ClearProperty("Image.BootLoader"); break;
			}
		}

		private void UpdateDisplay()
		{
			cbEnableSSA.Checked = Settings.GetValue("Launcher.SSA", true);
			cbEnableIROptimizations.Checked = Settings.GetValue("Optimizations.Basic", true);
			cbEnableSparseConditionalConstantPropagation.Checked = Settings.GetValue("Optimizations.SCCP", true);
			cbGenerateNASMFile.Checked = Settings.GetValue("CompilerDebug.NasmFile", string.Empty) == "%DEFAULT%";
			cbGenerateASMFile.Checked = Settings.GetValue("CompilerDebug.AsmFile", string.Empty) == "%DEFAULT%";
			cbGenerateMapFile.Checked = Settings.GetValue("CompilerDebug.MapFile", string.Empty) == "%DEFAULT%";
			cbGenerateDebugInfoFile.Checked = Settings.GetValue("CompilerDebug.DebugFile", string.Empty) == "%DEFAULT%";
			cbExitOnLaunch.Checked = Settings.GetValue("Launcher.Exit", true);
			cbEnableQemuGDB.Checked = Settings.GetValue("Emulator.GDB", false);
			cbLaunchGDB.Checked = Settings.GetValue("Launcher.Advance.LaunchGDB", false);
			cbLaunchMosaDebugger.Checked = Settings.GetValue("Launcher.Advance.LaunchGDBDebugger", false);
			cbInline.Checked = Settings.GetValue("Optimizations.Inline", true);
			cbInlineExplicitOnly.Checked = Settings.GetValue("Optimizations.Inline.ExplicitOnly", false);
			cbCompilerUsesMultipleThreads.Checked = Settings.GetValue("Compiler.Multithreading", false);
			nmMemory.Value = Settings.GetValue("Emulator.Memory", 128);
			cbVBEVideo.Checked = Settings.GetValue("Multiboot.Video", false);
			tbBaseAddress.Text = "0x" + Settings.GetValue("Compiler.BaseAddress", 0x00400000).ToString("x8");
			cbRelocationTable.Checked = Settings.GetValue("Linker.EmitStaticRelocations", false);
			cbEmitSymbolTable.Checked = Settings.GetValue("Linker.EmitAllSymbols", false);
			cbLongExpansion.Checked = Settings.GetValue("Optimizations.LongExpansion", true);
			cbTwoPassOptimizations.Checked = Settings.GetValue("Optimizations.TwoPass", true);
			cbValueNumbering.Checked = Settings.GetValue("Optimizations.ValueNumbering", true);
			cbEnableMethodScanner.Checked = Settings.GetValue("Compiler.MethodScanner", false);
			cbGenerateCompilerTime.Checked = Settings.GetValue("CompilerDebug.CompileTimeFile", false);
			cbBitTracker.Checked = Settings.GetValue("Optimizations.BitTracker", false);
			cbPlatformOptimizations.Checked = Settings.GetValue("Optimizations.Platform", false);
			cbLoopInvariantCodeMotion.Checked = Settings.GetValue("Optimizations.LoopInvariantCodeMotion", false);
			tbMode.Text = Settings.GetValue("Multiboot.Width", "640") + "x" + Settings.GetValue("Multiboot.Height", "480") + "x" + Settings.GetValue("Multiboot.Depth", "32");

			switch (Settings.GetValue("Image.Format", string.Empty).ToUpper())
			{
				case "IMG": cbImageFormat.SelectedIndex = 0; break;
				case "ISO": cbImageFormat.SelectedIndex = 1; break;
				case "VHD": cbImageFormat.SelectedIndex = 2; break;
				case "VDI": cbImageFormat.SelectedIndex = 3; break;
				case "VMDK": cbImageFormat.SelectedIndex = 4; break;
				default: break;
			}

			switch (Settings.GetValue("Emulator", string.Empty).ToLower())
			{
				case "qemu": cbEmulator.SelectedIndex = 0; break;
				case "bochs": cbEmulator.SelectedIndex = 1; break;
				case "vmware": cbEmulator.SelectedIndex = 2; break;
				default: cbEmulator.SelectedIndex = -1; break;
			}

			switch (Settings.GetValue("Image.FileSystem", string.Empty).ToLower())
			{
				case "fat12": cbBootFileSystem.SelectedIndex = 0; break;
				case "fat16": cbBootFileSystem.SelectedIndex = 1; break;
				case "fat32": cbBootFileSystem.SelectedIndex = 2; break;
				default: break;
			}

			switch (Settings.GetValue("Image.BootLoader", string.Empty).ToLower())
			{
				case "syslinux3.72": cbBootLoader.SelectedIndex = 0; break;
				case "syslinux6.03": cbBootLoader.SelectedIndex = 1; break;
				case "grub_0_97": cbBootLoader.SelectedIndex = 2; break;
				case "grub_2_00": cbBootLoader.SelectedIndex = 3; break;
				default: break;
			}

			switch (Settings.GetValue("Compiler.Platform", string.Empty).ToLower())
			{
				case "x86": cbPlatform.SelectedIndex = 0; break;
				case "x64": cbPlatform.SelectedIndex = 1; break;
				default: cbPlatform.SelectedIndex = 0; break;
			}

			switch (Settings.GetValue("Emulator.Serial", string.Empty).ToLower())
			{
				case "none": cbDebugConnectionOption.SelectedIndex = 0; break;
				case "pipe": cbDebugConnectionOption.SelectedIndex = 1; break;
				case "tcpserver": cbDebugConnectionOption.SelectedIndex = 2; break;
				case "tcpclient": cbDebugConnectionOption.SelectedIndex = 3; break;
				default: cbDebugConnectionOption.SelectedIndex = 0; break;
			}

			lbDestinationDirectory.Text = Settings.GetValue("Image.Destination", string.Empty); ;

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");
			var sourcefile = sourcefiles[0];

			string filename = sourcefiles != null && sourcefiles.Count >= 1 ? sourcefile : null;

			//lbSourceDirectory.Text = (filename != null) ? Path.GetDirectoryName(filename) : string.Empty;
			lbSource.Text = (filename != null) ? Path.GetFileName(filename) : string.Empty;
		}

		private void lbSourceDirectory_Click(object sender, EventArgs e)
		{

		}
	}
}
