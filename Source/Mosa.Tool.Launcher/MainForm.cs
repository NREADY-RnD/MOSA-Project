// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using MetroFramework.Forms;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.BootImage;
using Mosa.Utility.Compiler;
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

		public Builder Builder { get; }

		public LauncherOptions LauncherOptions { get; }

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

			LauncherOptions = new LauncherOptions();
			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Builder = new Builder(LauncherOptions, AppLocations, this);

			dataGridView1.DataSource = includedEntries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 175;
			dataGridView1.Columns[2].Width = 500;

			AddOutput("Current Directory: " + Environment.CurrentDirectory);
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

		private void UpdateBuilderOptions()
		{
			LauncherOptions.EnableSSA = cbEnableSSA.Checked;
			LauncherOptions.EnableBasicOptimizations = cbEnableIROptimizations.Checked;
			LauncherOptions.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			LauncherOptions.GenerateNASMFile = cbGenerateNASMFile.Checked;
			LauncherOptions.GenerateASMFile = cbGenerateASMFile.Checked;
			LauncherOptions.GenerateMapFile = cbGenerateMapFile.Checked;
			LauncherOptions.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			LauncherOptions.ExitOnLaunch = cbExitOnLaunch.Checked;
			LauncherOptions.EnableQemuGDB = cbEnableQemuGDB.Checked;
			LauncherOptions.LaunchGDB = cbLaunchGDB.Checked;
			LauncherOptions.LaunchGDBDebugger = cbLaunchMosaDebugger.Checked;
			LauncherOptions.EnableMultiThreading = cbCompilerUsesMultipleThreads.Checked;
			LauncherOptions.EmulatorMemoryInMB = (uint)nmMemory.Value;
			LauncherOptions.EnableInlineMethods = cbInline.Checked;
			LauncherOptions.InlineExplicitOnly = cbInlineExplicitOnly.Checked;
			LauncherOptions.VBEVideo = cbVBEVideo.Checked;
			LauncherOptions.EnableLongExpansion = cbLongExpansion.Checked;
			LauncherOptions.TwoPassOptimizations = cbTwoPassOptimizations.Checked;
			LauncherOptions.EnableLongExpansion = cbLongExpansion.Checked;
			LauncherOptions.EnableValueNumbering = cbValueNumbering.Checked;
			LauncherOptions.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			LauncherOptions.BaseAddress = tbBaseAddress.Text.ParseHexOrInteger();
			LauncherOptions.EmitAllSymbols = cbEmitSymbolTable.Checked;
			LauncherOptions.EmitStaticRelocations = cbRelocationTable.Checked;
			LauncherOptions.EnableMethodScanner = cbEnableMethodScanner.Checked;
			LauncherOptions.GenerateCompileTimeFile = cbGenerateCompilerTime.Checked;
			LauncherOptions.EnableBitTracker = cbBitTracker.Checked;
			LauncherOptions.EnablePlatformOptimizations = cbPlatformOptimizations.Checked;
			LauncherOptions.EnableLoopInvariantCodeMotion = cbLoopInvariantCodeMotion.Checked;

			//Options.OsName = tbOsName.Text;

			if (LauncherOptions.VBEVideo)
			{
				var Mode = tbMode.Text.Split('x');

				if (Mode.Length == 3)
				{
					try
					{
						int ModeWidth = int.Parse(Mode[0]); //Get Mode Width
						int ModeHeight = int.Parse(Mode[1]); //Get Mode Height
						int ModeDepth = int.Parse(Mode[2]); //Get Mode Depth

						LauncherOptions.Width = ModeWidth;
						LauncherOptions.Height = ModeHeight;
						LauncherOptions.Depth = ModeDepth;
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
				case 0: LauncherOptions.ImageFormat = ImageFormat.IMG; break;
				case 1: LauncherOptions.ImageFormat = ImageFormat.ISO; break;
				case 2: LauncherOptions.ImageFormat = ImageFormat.VHD; break;
				case 3: LauncherOptions.ImageFormat = ImageFormat.VDI; break;
				case 4: LauncherOptions.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (cbEmulator.SelectedIndex)
			{
				case 0: LauncherOptions.Emulator = EmulatorType.Qemu; break;
				case 1: LauncherOptions.Emulator = EmulatorType.Bochs; break;
				case 2: LauncherOptions.Emulator = EmulatorType.VMware; break;
				default: break;
			}

			switch (cbDebugConnectionOption.SelectedIndex)
			{
				case 0: LauncherOptions.SerialConnectionOption = SerialConnectionOption.None; break;
				case 1: LauncherOptions.SerialConnectionOption = SerialConnectionOption.Pipe; break;
				case 2: LauncherOptions.SerialConnectionOption = SerialConnectionOption.TCPServer; break;
				case 3: LauncherOptions.SerialConnectionOption = SerialConnectionOption.TCPClient; break;
				default: break;
			}

			switch (cbBootFormat.SelectedIndex)
			{
				case 0: LauncherOptions.MultibootSpecification = MultibootSpecification.V1; break;
				case 1: LauncherOptions.MultibootSpecification = MultibootSpecification.V2; break;
				default: LauncherOptions.MultibootSpecification = MultibootSpecification.None; break;
			}

			switch (cbBootFileSystem.SelectedIndex)
			{
				case 0: LauncherOptions.FileSystem = Utility.BootImage.FileSystem.FAT12; break;
				case 1: LauncherOptions.FileSystem = Utility.BootImage.FileSystem.FAT16; break;
				default: break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: LauncherOptions.PlatformType = PlatformType.x86; break;
				case 1: LauncherOptions.PlatformType = PlatformType.x64; break;
				default: LauncherOptions.PlatformType = PlatformType.NotSpecified; break;
			}

			switch (cbBootLoader.SelectedIndex)
			{
				case 0: LauncherOptions.BootLoader = BootLoader.Syslinux_3_72; break;
				case 1: LauncherOptions.BootLoader = BootLoader.Syslinux_6_03; break;
				case 2: LauncherOptions.BootLoader = BootLoader.Grub_0_97; break;
				case 3: LauncherOptions.BootLoader = BootLoader.Grub_2_00; break;
				default: break;
			}

			LauncherOptions.IncludeFiles.Clear();

			foreach (var entry in includedEntries)
			{
				LauncherOptions.IncludeFiles.Add(entry.IncludeFile);
			}
		}

		private void UpdateInterfaceOptions()
		{
			cbEnableSSA.Checked = LauncherOptions.EnableSSA;
			cbEnableIROptimizations.Checked = LauncherOptions.EnableBasicOptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = LauncherOptions.EnableSparseConditionalConstantPropagation;
			cbGenerateNASMFile.Checked = LauncherOptions.GenerateNASMFile;
			cbGenerateASMFile.Checked = LauncherOptions.GenerateASMFile;
			cbGenerateMapFile.Checked = LauncherOptions.GenerateMapFile;
			cbGenerateDebugInfoFile.Checked = LauncherOptions.GenerateDebugFile;
			cbExitOnLaunch.Checked = LauncherOptions.ExitOnLaunch;
			cbEnableQemuGDB.Checked = LauncherOptions.EnableQemuGDB;
			cbLaunchGDB.Checked = LauncherOptions.LaunchGDB;
			cbLaunchMosaDebugger.Checked = LauncherOptions.LaunchGDBDebugger;
			cbInline.Checked = LauncherOptions.EnableInlineMethods;
			cbInlineExplicitOnly.Checked = LauncherOptions.InlineExplicitOnly;
			cbCompilerUsesMultipleThreads.Checked = LauncherOptions.EnableMultiThreading;
			nmMemory.Value = LauncherOptions.EmulatorMemoryInMB;
			cbVBEVideo.Checked = LauncherOptions.VBEVideo;
			tbBaseAddress.Text = "0x" + LauncherOptions.BaseAddress.ToString("x8");
			cbRelocationTable.Checked = LauncherOptions.EmitStaticRelocations;
			cbEmitSymbolTable.Checked = LauncherOptions.EmitAllSymbols;
			tbMode.Text = LauncherOptions.Width + "x" + LauncherOptions.Height + "x" + LauncherOptions.Depth;
			cbLongExpansion.Checked = LauncherOptions.EnableLongExpansion;
			cbTwoPassOptimizations.Checked = LauncherOptions.TwoPassOptimizations;
			cbValueNumbering.Checked = LauncherOptions.EnableValueNumbering;
			cbEnableMethodScanner.Checked = LauncherOptions.EnableMethodScanner;
			cbGenerateCompilerTime.Checked = LauncherOptions.GenerateCompileTimeFile;
			cbBitTracker.Checked = LauncherOptions.EnableBitTracker;
			cbPlatformOptimizations.Checked = LauncherOptions.EnablePlatformOptimizations;
			cbLoopInvariantCodeMotion.Checked = LauncherOptions.EnableLoopInvariantCodeMotion;

			switch (LauncherOptions.ImageFormat)
			{
				case ImageFormat.IMG: cbImageFormat.SelectedIndex = 0; break;
				case ImageFormat.ISO: cbImageFormat.SelectedIndex = 1; break;
				case ImageFormat.VHD: cbImageFormat.SelectedIndex = 2; break;
				case ImageFormat.VDI: cbImageFormat.SelectedIndex = 3; break;
				case ImageFormat.VMDK: cbImageFormat.SelectedIndex = 4; break;
				default: break;
			}

			switch (LauncherOptions.Emulator)
			{
				case EmulatorType.Qemu: cbEmulator.SelectedIndex = 0; break;
				case EmulatorType.Bochs: cbEmulator.SelectedIndex = 1; break;
				case EmulatorType.VMware: cbEmulator.SelectedIndex = 2; break;
				default: break;
			}

			switch (LauncherOptions.FileSystem)
			{
				case Utility.BootImage.FileSystem.FAT12: cbBootFileSystem.SelectedIndex = 0; break;
				case Utility.BootImage.FileSystem.FAT16: cbBootFileSystem.SelectedIndex = 1; break;
				default: break;
			}

			switch (LauncherOptions.BootLoader)
			{
				case BootLoader.Syslinux_3_72: cbBootLoader.SelectedIndex = 0; break;
				case BootLoader.Syslinux_6_03: cbBootLoader.SelectedIndex = 1; break;
				case BootLoader.Grub_0_97: cbBootLoader.SelectedIndex = 2; break;
				case BootLoader.Grub_2_00: cbBootLoader.SelectedIndex = 3; break;
				default: break;
			}

			switch (LauncherOptions.PlatformType)
			{
				case PlatformType.x86: cbPlatform.SelectedIndex = 0; break;
				case PlatformType.x64: cbPlatform.SelectedIndex = 1; break;
				default: cbPlatform.SelectedIndex = 0; break;
			}

			switch (LauncherOptions.MultibootSpecification)
			{
				case MultibootSpecification.V1: cbBootFormat.SelectedIndex = 0; break;

				//case MultibootSpecification.V2: cbBootFormat.SelectedIndex = 1; break;
				default: cbBootFormat.SelectedIndex = 0; break;
			}

			switch (LauncherOptions.SerialConnectionOption)
			{
				case SerialConnectionOption.None: cbDebugConnectionOption.SelectedIndex = 0; break;
				case SerialConnectionOption.Pipe: cbDebugConnectionOption.SelectedIndex = 1; break;
				case SerialConnectionOption.TCPServer: cbDebugConnectionOption.SelectedIndex = 2; break;
				case SerialConnectionOption.TCPClient: cbDebugConnectionOption.SelectedIndex = 3; break;
				default: break;
			}

			lbDestinationDirectory.Text = LauncherOptions.DestinationDirectory;
			lbSource.Text = LauncherOptions.SourceFile;
			lbSourceDirectory.Text = Path.GetDirectoryName(LauncherOptions.SourceFile);
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
			UpdateInterfaceOptions();
			UpdateInterfaceAppLocations();

			Refresh();

			if (LauncherOptions.AutoStart)
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

				LauncherOptions.SourceFile = filename;
				LauncherOptions.Paths.Clear();
				LauncherOptions.Paths.Add(Path.GetDirectoryName(filename));

				lbSource.Text = Path.GetFileName(LauncherOptions.SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(LauncherOptions.SourceFile);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text = "MOSA Launcher v" + CompilerVersion.VersionString;
			tbApplicationLocations.SelectedTab = tabOptions;

			foreach (var includeFile in LauncherOptions.IncludeFiles)
			{
				includedEntries.Add(new IncludedEntry(includeFile));
			}
		}

		private void BtnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				LauncherOptions.DestinationDirectory = folderBrowserDialog1.SelectedPath;

				lbDestinationDirectory.Text = LauncherOptions.DestinationDirectory;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F5)
				CompileBuildAndStart();

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

				LauncherOptions.ImageFile = LauncherOptions.BootLoaderImage ?? Builder.ImageFile;

				var starter = new Starter(LauncherOptions, AppLocations, this, Builder.Linker);

				starter.Launch();
			}

			if (LauncherOptions.ExitOnLaunch)
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
			UpdateBuilderOptions();

			var result = CheckOptions.Verify(LauncherOptions);

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

		public void LoadArguments(string[] args)
		{
			var cliParser = new Parser(config => config.HelpWriter = Console.Out);

			cliParser.ParseArguments(() => LauncherOptions, args);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgress();
		}

		public void LoadArgumentsV2(string[] args)
		{
			var arguments = Reader.ParseArguments(args, CommandLineArguments.GetMap());

			Settings.Merge(arguments);

			UpdateDisplay(Settings);
		}

		private void UpdateDisplay(Settings settings)
		{
		}
	}
}
