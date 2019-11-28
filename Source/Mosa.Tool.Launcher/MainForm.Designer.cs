﻿namespace Mosa.Tool.Launcher
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.progressBar1 = new MetroFramework.Controls.MetroProgressBar();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tbApplicationLocations = new MetroFramework.Controls.MetroTabControl();
			this.tabOptions = new MetroFramework.Controls.MetroTabPage();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.cbEnableMethodScanner = new MetroFramework.Controls.MetroCheckBox();
			this.cbCompilerUsesMultipleThreads = new MetroFramework.Controls.MetroCheckBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbGenerateCompilerTime = new MetroFramework.Controls.MetroCheckBox();
			this.cbGenerateDebugInfoFile = new MetroFramework.Controls.MetroCheckBox();
			this.cbGenerateASMFile = new MetroFramework.Controls.MetroCheckBox();
			this.cbGenerateMapFile = new MetroFramework.Controls.MetroCheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnSource = new MetroFramework.Controls.MetroButton();
			this.lbSource = new MetroFramework.Controls.MetroLabel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnDestination = new MetroFramework.Controls.MetroButton();
			this.lbDestinationDirectory = new MetroFramework.Controls.MetroLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbPlatformOptimizations = new MetroFramework.Controls.MetroCheckBox();
			this.cbLoopInvariantCodeMotion = new MetroFramework.Controls.MetroCheckBox();
			this.cbTwoPassOptimizations = new MetroFramework.Controls.MetroCheckBox();
			this.cbInlineExplicitOnly = new MetroFramework.Controls.MetroCheckBox();
			this.cbBitTracker = new MetroFramework.Controls.MetroCheckBox();
			this.cbValueNumbering = new MetroFramework.Controls.MetroCheckBox();
			this.cbLongExpansion = new MetroFramework.Controls.MetroCheckBox();
			this.cbEnableSparseConditionalConstantPropagation = new MetroFramework.Controls.MetroCheckBox();
			this.cbEnableIROptimizations = new MetroFramework.Controls.MetroCheckBox();
			this.cbEnableSSA = new MetroFramework.Controls.MetroCheckBox();
			this.cbInline = new MetroFramework.Controls.MetroCheckBox();
			this.tabAdvanced = new MetroFramework.Controls.MetroTabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.cbBootLoader = new MetroFramework.Controls.MetroComboBox();
			this.label1 = new MetroFramework.Controls.MetroLabel();
			this.label4 = new MetroFramework.Controls.MetroLabel();
			this.cbBootFileSystem = new MetroFramework.Controls.MetroComboBox();
			this.cbBootFormat = new MetroFramework.Controls.MetroComboBox();
			this.label2 = new MetroFramework.Controls.MetroLabel();
			this.groupBox15 = new System.Windows.Forms.GroupBox();
			this.cbDebugConnectionOption = new MetroFramework.Controls.MetroComboBox();
			this.label8 = new MetroFramework.Controls.MetroLabel();
			this.cbLaunchMosaDebugger = new MetroFramework.Controls.MetroCheckBox();
			this.cbLaunchGDB = new MetroFramework.Controls.MetroCheckBox();
			this.cbEnableQemuGDB = new MetroFramework.Controls.MetroCheckBox();
			this.groupBox14 = new System.Windows.Forms.GroupBox();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.tbOsName = new MetroFramework.Controls.MetroTextBox();
			this.cbPlugKorlib = new MetroFramework.Controls.MetroCheckBox();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.label9 = new MetroFramework.Controls.MetroLabel();
			this.tbBaseAddress = new MetroFramework.Controls.MetroTextBox();
			this.cbRelocationTable = new MetroFramework.Controls.MetroCheckBox();
			this.cbEmitSymbolTable = new MetroFramework.Controls.MetroCheckBox();
			this.tabFiles = new System.Windows.Forms.TabPage();
			this.panelAdditionalFiles = new System.Windows.Forms.Panel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.btnRemoveFiles = new MetroFramework.Controls.MetroButton();
			this.btnAddFiles = new MetroFramework.Controls.MetroButton();
			this.tabApplicationLocations = new MetroFramework.Controls.MetroTabPage();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.button7 = new MetroFramework.Controls.MetroButton();
			this.lbmkisofsExecutable = new MetroFramework.Controls.MetroLabel();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.button4 = new MetroFramework.Controls.MetroButton();
			this.lbVMwarePlayerExecutable = new MetroFramework.Controls.MetroLabel();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.button6 = new MetroFramework.Controls.MetroButton();
			this.lbBOCHSExecutable = new MetroFramework.Controls.MetroLabel();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.button5 = new MetroFramework.Controls.MetroButton();
			this.lbNDISASMExecutable = new MetroFramework.Controls.MetroLabel();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.lbQEMUImgApplication = new MetroFramework.Controls.MetroLabel();
			this.button8 = new MetroFramework.Controls.MetroButton();
			this.lbQEMUBIOSDirectory = new MetroFramework.Controls.MetroLabel();
			this.button3 = new MetroFramework.Controls.MetroButton();
			this.button2 = new MetroFramework.Controls.MetroButton();
			this.lbQEMUExecutable = new MetroFramework.Controls.MetroLabel();
			this.tabCounters = new MetroFramework.Controls.MetroTabPage();
			this.rtbCounters = new System.Windows.Forms.RichTextBox();
			this.tabOutput = new MetroFramework.Controls.MetroTabPage();
			this.rtbOutput = new System.Windows.Forms.RichTextBox();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.tbMode = new MetroFramework.Controls.MetroTextBox();
			this.cbVBEVideo = new MetroFramework.Controls.MetroCheckBox();
			this.groupBox17 = new System.Windows.Forms.GroupBox();
			this.label5 = new MetroFramework.Controls.MetroLabel();
			this.cbPlatform = new MetroFramework.Controls.MetroComboBox();
			this.cbImageFormat = new MetroFramework.Controls.MetroComboBox();
			this.label3 = new MetroFramework.Controls.MetroLabel();
			this.cbEmulator = new MetroFramework.Controls.MetroComboBox();
			this.label6 = new MetroFramework.Controls.MetroLabel();
			this.nmMemory = new System.Windows.Forms.NumericUpDown();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.button1 = new MetroFramework.Controls.MetroButton();
			this.cbExitOnLaunch = new MetroFramework.Controls.MetroCheckBox();
			this.cbGenerateNASMFile = new MetroFramework.Controls.MetroCheckBox();
			this.tbApplicationLocations.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabAdvanced.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox15.SuspendLayout();
			this.groupBox14.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.tabFiles.SuspendLayout();
			this.panelAdditionalFiles.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tabApplicationLocations.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabCounters.SuspendLayout();
			this.tabOutput.SuspendLayout();
			this.groupBox17.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nmMemory)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "*.exe";
			this.openFileDialog1.Filter = "Assemblies|*.exe";
			this.openFileDialog1.Title = "Select Assembly";
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(0, 63);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(661, 20);
			this.progressBar1.Step = 1;
			this.progressBar1.Style = MetroFramework.MetroColorStyle.Blue;
			this.progressBar1.TabIndex = 3;
			this.progressBar1.Theme = MetroFramework.MetroThemeStyle.Light;
			// 
			// tbApplicationLocations
			// 
			this.tbApplicationLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbApplicationLocations.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tbApplicationLocations.Controls.Add(this.tabOptions);
			this.tbApplicationLocations.Controls.Add(this.tabAdvanced);
			this.tbApplicationLocations.Controls.Add(this.tabFiles);
			this.tbApplicationLocations.Controls.Add(this.tabApplicationLocations);
			this.tbApplicationLocations.Controls.Add(this.tabCounters);
			this.tbApplicationLocations.Controls.Add(this.tabOutput);
			this.tbApplicationLocations.ItemSize = new System.Drawing.Size(48, 18);
			this.tbApplicationLocations.Location = new System.Drawing.Point(0, 86);
			this.tbApplicationLocations.Margin = new System.Windows.Forms.Padding(0);
			this.tbApplicationLocations.Multiline = true;
			this.tbApplicationLocations.Name = "tbApplicationLocations";
			this.tbApplicationLocations.SelectedIndex = 0;
			this.tbApplicationLocations.Size = new System.Drawing.Size(661, 430);
			this.tbApplicationLocations.Style = MetroFramework.MetroColorStyle.Blue;
			this.tbApplicationLocations.TabIndex = 22;
			this.tbApplicationLocations.UseSelectable = true;
			// 
			// tabOptions
			// 
			this.tabOptions.BackColor = System.Drawing.SystemColors.Control;
			this.tabOptions.Controls.Add(this.cbExitOnLaunch);
			this.tabOptions.Controls.Add(this.button1);
			this.tabOptions.Controls.Add(this.groupBox17);
			this.tabOptions.Controls.Add(this.groupBox12);
			this.tabOptions.Controls.Add(this.statusStrip1);
			this.tabOptions.Controls.Add(this.groupBox2);
			this.tabOptions.Controls.Add(this.groupBox4);
			this.tabOptions.Controls.Add(this.groupBox3);
			this.tabOptions.Controls.Add(this.groupBox1);
			this.tabOptions.HorizontalScrollbarBarColor = true;
			this.tabOptions.HorizontalScrollbarHighlightOnWheel = false;
			this.tabOptions.HorizontalScrollbarSize = 10;
			this.tabOptions.Location = new System.Drawing.Point(4, 22);
			this.tabOptions.Margin = new System.Windows.Forms.Padding(0);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.Size = new System.Drawing.Size(653, 404);
			this.tabOptions.TabIndex = 0;
			this.tabOptions.Text = "Compile Options";
			this.tabOptions.VerticalScrollbarBarColor = true;
			this.tabOptions.VerticalScrollbarHighlightOnWheel = false;
			this.tabOptions.VerticalScrollbarSize = 10;
			// 
			// groupBox12
			// 
			this.groupBox12.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox12.Controls.Add(this.cbEnableMethodScanner);
			this.groupBox12.Controls.Add(this.cbCompilerUsesMultipleThreads);
			this.groupBox12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox12.Location = new System.Drawing.Point(264, 277);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(158, 100);
			this.groupBox12.TabIndex = 30;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Misc Options:";
			// 
			// cbEnableMethodScanner
			// 
			this.cbEnableMethodScanner.AutoSize = true;
			this.cbEnableMethodScanner.Checked = true;
			this.cbEnableMethodScanner.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableMethodScanner.Location = new System.Drawing.Point(7, 40);
			this.cbEnableMethodScanner.Name = "cbEnableMethodScanner";
			this.cbEnableMethodScanner.Size = new System.Drawing.Size(148, 15);
			this.cbEnableMethodScanner.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEnableMethodScanner.TabIndex = 14;
			this.cbEnableMethodScanner.Text = "Enable Method Scanner";
			this.cbEnableMethodScanner.Theme = MetroFramework.MetroThemeStyle.Light;
			this.cbEnableMethodScanner.UseCustomBackColor = true;
			this.cbEnableMethodScanner.UseSelectable = true;
			// 
			// cbCompilerUsesMultipleThreads
			// 
			this.cbCompilerUsesMultipleThreads.AutoSize = true;
			this.cbCompilerUsesMultipleThreads.Checked = true;
			this.cbCompilerUsesMultipleThreads.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCompilerUsesMultipleThreads.Location = new System.Drawing.Point(7, 19);
			this.cbCompilerUsesMultipleThreads.Name = "cbCompilerUsesMultipleThreads";
			this.cbCompilerUsesMultipleThreads.Size = new System.Drawing.Size(133, 15);
			this.cbCompilerUsesMultipleThreads.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbCompilerUsesMultipleThreads.TabIndex = 13;
			this.cbCompilerUsesMultipleThreads.Text = "Use Multiple Threads";
			this.cbCompilerUsesMultipleThreads.Theme = MetroFramework.MetroThemeStyle.Light;
			this.cbCompilerUsesMultipleThreads.UseCustomBackColor = true;
			this.cbCompilerUsesMultipleThreads.UseSelectable = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 382);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(653, 22);
			this.statusStrip1.TabIndex = 32;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsStatusLabel
			// 
			this.tsStatusLabel.Name = "tsStatusLabel";
			this.tsStatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox2.Controls.Add(this.cbGenerateNASMFile);
			this.groupBox2.Controls.Add(this.cbGenerateCompilerTime);
			this.groupBox2.Controls.Add(this.cbGenerateDebugInfoFile);
			this.groupBox2.Controls.Add(this.cbGenerateASMFile);
			this.groupBox2.Controls.Add(this.cbGenerateMapFile);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(264, 125);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(158, 146);
			this.groupBox2.TabIndex = 29;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Diagnostic Output:";
			// 
			// cbGenerateCompilerTime
			// 
			this.cbGenerateCompilerTime.AutoSize = true;
			this.cbGenerateCompilerTime.Location = new System.Drawing.Point(6, 102);
			this.cbGenerateCompilerTime.Name = "cbGenerateCompilerTime";
			this.cbGenerateCompilerTime.Size = new System.Drawing.Size(118, 15);
			this.cbGenerateCompilerTime.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbGenerateCompilerTime.TabIndex = 49;
			this.cbGenerateCompilerTime.Text = "Compile Time File";
			this.cbGenerateCompilerTime.UseCustomBackColor = true;
			this.cbGenerateCompilerTime.UseSelectable = true;
			// 
			// cbGenerateDebugInfoFile
			// 
			this.cbGenerateDebugInfoFile.AutoSize = true;
			this.cbGenerateDebugInfoFile.Location = new System.Drawing.Point(6, 81);
			this.cbGenerateDebugInfoFile.Name = "cbGenerateDebugInfoFile";
			this.cbGenerateDebugInfoFile.Size = new System.Drawing.Size(79, 15);
			this.cbGenerateDebugInfoFile.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbGenerateDebugInfoFile.TabIndex = 48;
			this.cbGenerateDebugInfoFile.Text = "Debug File";
			this.cbGenerateDebugInfoFile.UseCustomBackColor = true;
			this.cbGenerateDebugInfoFile.UseSelectable = true;
			// 
			// cbGenerateASMFile
			// 
			this.cbGenerateASMFile.AutoSize = true;
			this.cbGenerateASMFile.Location = new System.Drawing.Point(6, 39);
			this.cbGenerateASMFile.Name = "cbGenerateASMFile";
			this.cbGenerateASMFile.Size = new System.Drawing.Size(69, 15);
			this.cbGenerateASMFile.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbGenerateASMFile.TabIndex = 15;
			this.cbGenerateASMFile.Text = "ASM File";
			this.cbGenerateASMFile.UseCustomBackColor = true;
			this.cbGenerateASMFile.UseSelectable = true;
			// 
			// cbGenerateMapFile
			// 
			this.cbGenerateMapFile.AutoSize = true;
			this.cbGenerateMapFile.Location = new System.Drawing.Point(6, 18);
			this.cbGenerateMapFile.Name = "cbGenerateMapFile";
			this.cbGenerateMapFile.Size = new System.Drawing.Size(70, 15);
			this.cbGenerateMapFile.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbGenerateMapFile.TabIndex = 13;
			this.cbGenerateMapFile.Text = "MAP File";
			this.cbGenerateMapFile.UseCustomBackColor = true;
			this.cbGenerateMapFile.UseSelectable = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox4.Controls.Add(this.btnSource);
			this.groupBox4.Controls.Add(this.lbSource);
			this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.Location = new System.Drawing.Point(5, 7);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(638, 53);
			this.groupBox4.TabIndex = 25;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Source:";
			// 
			// btnSource
			// 
			this.btnSource.Location = new System.Drawing.Point(10, 19);
			this.btnSource.Name = "btnSource";
			this.btnSource.Size = new System.Drawing.Size(103, 23);
			this.btnSource.TabIndex = 18;
			this.btnSource.Text = "Source:";
			this.btnSource.UseSelectable = true;
			this.btnSource.Click += new System.EventHandler(this.BtnSource_Click);
			// 
			// lbSource
			// 
			this.lbSource.AutoSize = true;
			this.lbSource.FontSize = MetroFramework.MetroLabelSize.Small;
			this.lbSource.Location = new System.Drawing.Point(119, 21);
			this.lbSource.Name = "lbSource";
			this.lbSource.Size = new System.Drawing.Size(57, 19);
			this.lbSource.TabIndex = 17;
			this.lbSource.Text = "{Source}";
			this.lbSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbSource.UseCustomBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox3.Controls.Add(this.btnDestination);
			this.groupBox3.Controls.Add(this.lbDestinationDirectory);
			this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(5, 63);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(638, 53);
			this.groupBox3.TabIndex = 24;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Output:";
			// 
			// btnDestination
			// 
			this.btnDestination.Location = new System.Drawing.Point(6, 18);
			this.btnDestination.Name = "btnDestination";
			this.btnDestination.Size = new System.Drawing.Size(107, 23);
			this.btnDestination.TabIndex = 17;
			this.btnDestination.Text = "Destination:";
			this.btnDestination.UseSelectable = true;
			this.btnDestination.Click += new System.EventHandler(this.BtnDestination_Click);
			// 
			// lbDestinationDirectory
			// 
			this.lbDestinationDirectory.AutoSize = true;
			this.lbDestinationDirectory.FontSize = MetroFramework.MetroLabelSize.Small;
			this.lbDestinationDirectory.Location = new System.Drawing.Point(119, 20);
			this.lbDestinationDirectory.Name = "lbDestinationDirectory";
			this.lbDestinationDirectory.Size = new System.Drawing.Size(139, 19);
			this.lbDestinationDirectory.TabIndex = 14;
			this.lbDestinationDirectory.Text = "{Destination Directory}";
			this.lbDestinationDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbDestinationDirectory.UseCustomBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox1.Controls.Add(this.cbPlatformOptimizations);
			this.groupBox1.Controls.Add(this.cbLoopInvariantCodeMotion);
			this.groupBox1.Controls.Add(this.cbTwoPassOptimizations);
			this.groupBox1.Controls.Add(this.cbInlineExplicitOnly);
			this.groupBox1.Controls.Add(this.cbBitTracker);
			this.groupBox1.Controls.Add(this.cbValueNumbering);
			this.groupBox1.Controls.Add(this.cbLongExpansion);
			this.groupBox1.Controls.Add(this.cbEnableSparseConditionalConstantPropagation);
			this.groupBox1.Controls.Add(this.cbEnableIROptimizations);
			this.groupBox1.Controls.Add(this.cbEnableSSA);
			this.groupBox1.Controls.Add(this.cbInline);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(5, 125);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(253, 252);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Optimizations:";
			// 
			// cbPlatformOptimizations
			// 
			this.cbPlatformOptimizations.AutoSize = true;
			this.cbPlatformOptimizations.Checked = true;
			this.cbPlatformOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbPlatformOptimizations.Location = new System.Drawing.Point(6, 228);
			this.cbPlatformOptimizations.Name = "cbPlatformOptimizations";
			this.cbPlatformOptimizations.Size = new System.Drawing.Size(146, 15);
			this.cbPlatformOptimizations.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbPlatformOptimizations.TabIndex = 16;
			this.cbPlatformOptimizations.Text = "Platform Optimizations";
			this.cbPlatformOptimizations.UseCustomBackColor = true;
			this.cbPlatformOptimizations.UseSelectable = true;
			// 
			// cbLoopInvariantCodeMotion
			// 
			this.cbLoopInvariantCodeMotion.AutoSize = true;
			this.cbLoopInvariantCodeMotion.Checked = true;
			this.cbLoopInvariantCodeMotion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbLoopInvariantCodeMotion.Location = new System.Drawing.Point(6, 102);
			this.cbLoopInvariantCodeMotion.Name = "cbLoopInvariantCodeMotion";
			this.cbLoopInvariantCodeMotion.Size = new System.Drawing.Size(172, 15);
			this.cbLoopInvariantCodeMotion.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbLoopInvariantCodeMotion.TabIndex = 15;
			this.cbLoopInvariantCodeMotion.Text = "Loop Invariant Code Motion";
			this.cbLoopInvariantCodeMotion.UseCustomBackColor = true;
			this.cbLoopInvariantCodeMotion.UseSelectable = true;
			// 
			// cbTwoPassOptimizations
			// 
			this.cbTwoPassOptimizations.AutoSize = true;
			this.cbTwoPassOptimizations.Checked = true;
			this.cbTwoPassOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTwoPassOptimizations.Location = new System.Drawing.Point(6, 207);
			this.cbTwoPassOptimizations.Name = "cbTwoPassOptimizations";
			this.cbTwoPassOptimizations.Size = new System.Drawing.Size(153, 15);
			this.cbTwoPassOptimizations.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbTwoPassOptimizations.TabIndex = 11;
			this.cbTwoPassOptimizations.Text = "Two Optimization Passes";
			this.cbTwoPassOptimizations.UseCustomBackColor = true;
			this.cbTwoPassOptimizations.UseSelectable = true;
			// 
			// cbInlineExplicitOnly
			// 
			this.cbInlineExplicitOnly.AutoSize = true;
			this.cbInlineExplicitOnly.Checked = true;
			this.cbInlineExplicitOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbInlineExplicitOnly.Location = new System.Drawing.Point(6, 186);
			this.cbInlineExplicitOnly.Name = "cbInlineExplicitOnly";
			this.cbInlineExplicitOnly.Size = new System.Drawing.Size(121, 15);
			this.cbInlineExplicitOnly.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbInlineExplicitOnly.TabIndex = 14;
			this.cbInlineExplicitOnly.Text = "Inline Explicit Only";
			this.cbInlineExplicitOnly.UseCustomBackColor = true;
			this.cbInlineExplicitOnly.UseSelectable = true;
			// 
			// cbBitTracker
			// 
			this.cbBitTracker.AutoSize = true;
			this.cbBitTracker.Checked = true;
			this.cbBitTracker.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbBitTracker.Location = new System.Drawing.Point(6, 123);
			this.cbBitTracker.Name = "cbBitTracker";
			this.cbBitTracker.Size = new System.Drawing.Size(77, 15);
			this.cbBitTracker.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbBitTracker.TabIndex = 13;
			this.cbBitTracker.Text = "Bit Tracker";
			this.cbBitTracker.UseCustomBackColor = true;
			this.cbBitTracker.UseSelectable = true;
			// 
			// cbValueNumbering
			// 
			this.cbValueNumbering.AutoSize = true;
			this.cbValueNumbering.Checked = true;
			this.cbValueNumbering.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbValueNumbering.Location = new System.Drawing.Point(6, 60);
			this.cbValueNumbering.Name = "cbValueNumbering";
			this.cbValueNumbering.Size = new System.Drawing.Size(115, 15);
			this.cbValueNumbering.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbValueNumbering.TabIndex = 12;
			this.cbValueNumbering.Text = "Value Numbering";
			this.cbValueNumbering.UseCustomBackColor = true;
			this.cbValueNumbering.UseSelectable = true;
			// 
			// cbLongExpansion
			// 
			this.cbLongExpansion.AutoSize = true;
			this.cbLongExpansion.Checked = true;
			this.cbLongExpansion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbLongExpansion.Location = new System.Drawing.Point(6, 144);
			this.cbLongExpansion.Name = "cbLongExpansion";
			this.cbLongExpansion.Size = new System.Drawing.Size(107, 15);
			this.cbLongExpansion.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbLongExpansion.TabIndex = 10;
			this.cbLongExpansion.Text = "Long Expansion";
			this.cbLongExpansion.UseCustomBackColor = true;
			this.cbLongExpansion.UseSelectable = true;
			// 
			// cbEnableSparseConditionalConstantPropagation
			// 
			this.cbEnableSparseConditionalConstantPropagation.AutoSize = true;
			this.cbEnableSparseConditionalConstantPropagation.Checked = true;
			this.cbEnableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSparseConditionalConstantPropagation.Location = new System.Drawing.Point(6, 81);
			this.cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
			this.cbEnableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(241, 15);
			this.cbEnableSparseConditionalConstantPropagation.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEnableSparseConditionalConstantPropagation.TabIndex = 8;
			this.cbEnableSparseConditionalConstantPropagation.Text = "Sparse Conditional Constant Propagation";
			this.cbEnableSparseConditionalConstantPropagation.UseCustomBackColor = true;
			this.cbEnableSparseConditionalConstantPropagation.UseSelectable = true;
			// 
			// cbEnableIROptimizations
			// 
			this.cbEnableIROptimizations.AutoSize = true;
			this.cbEnableIROptimizations.Checked = true;
			this.cbEnableIROptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableIROptimizations.Location = new System.Drawing.Point(6, 39);
			this.cbEnableIROptimizations.Name = "cbEnableIROptimizations";
			this.cbEnableIROptimizations.Size = new System.Drawing.Size(140, 15);
			this.cbEnableIROptimizations.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEnableIROptimizations.TabIndex = 7;
			this.cbEnableIROptimizations.Text = "IR Level Optimizations";
			this.cbEnableIROptimizations.UseCustomBackColor = true;
			this.cbEnableIROptimizations.UseSelectable = true;
			// 
			// cbEnableSSA
			// 
			this.cbEnableSSA.AutoSize = true;
			this.cbEnableSSA.Checked = true;
			this.cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSSA.Location = new System.Drawing.Point(6, 18);
			this.cbEnableSSA.Name = "cbEnableSSA";
			this.cbEnableSSA.Size = new System.Drawing.Size(184, 15);
			this.cbEnableSSA.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEnableSSA.TabIndex = 6;
			this.cbEnableSSA.Text = "Static Single Assignment (SSA)";
			this.cbEnableSSA.UseCustomBackColor = true;
			this.cbEnableSSA.UseSelectable = true;
			// 
			// cbInline
			// 
			this.cbInline.AutoSize = true;
			this.cbInline.Checked = true;
			this.cbInline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbInline.Location = new System.Drawing.Point(6, 165);
			this.cbInline.Name = "cbInline";
			this.cbInline.Size = new System.Drawing.Size(52, 15);
			this.cbInline.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbInline.TabIndex = 9;
			this.cbInline.Text = "Inline";
			this.cbInline.UseCustomBackColor = true;
			this.cbInline.UseSelectable = true;
			// 
			// tabAdvanced
			// 
			this.tabAdvanced.Controls.Add(this.groupBox5);
			this.tabAdvanced.Controls.Add(this.groupBox15);
			this.tabAdvanced.Controls.Add(this.groupBox14);
			this.tabAdvanced.Controls.Add(this.groupBox13);
			this.tabAdvanced.HorizontalScrollbarBarColor = true;
			this.tabAdvanced.HorizontalScrollbarHighlightOnWheel = false;
			this.tabAdvanced.HorizontalScrollbarSize = 10;
			this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tabAdvanced.Name = "tabAdvanced";
			this.tabAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tabAdvanced.Size = new System.Drawing.Size(653, 404);
			this.tabAdvanced.TabIndex = 4;
			this.tabAdvanced.Text = "Advanced Options";
			this.tabAdvanced.UseVisualStyleBackColor = true;
			this.tabAdvanced.VerticalScrollbarBarColor = true;
			this.tabAdvanced.VerticalScrollbarHighlightOnWheel = false;
			this.tabAdvanced.VerticalScrollbarSize = 10;
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox5.Controls.Add(this.tbMode);
			this.groupBox5.Controls.Add(this.cbVBEVideo);
			this.groupBox5.Controls.Add(this.cbBootLoader);
			this.groupBox5.Controls.Add(this.label1);
			this.groupBox5.Controls.Add(this.label4);
			this.groupBox5.Controls.Add(this.cbBootFileSystem);
			this.groupBox5.Controls.Add(this.cbBootFormat);
			this.groupBox5.Controls.Add(this.label2);
			this.groupBox5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox5.Location = new System.Drawing.Point(432, 18);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(206, 225);
			this.groupBox5.TabIndex = 50;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Image:";
			// 
			// cbBootLoader
			// 
			this.cbBootLoader.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbBootLoader.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbBootLoader.FormattingEnabled = true;
			this.cbBootLoader.ItemHeight = 19;
			this.cbBootLoader.Items.AddRange(new object[] {
            "Syslinux 3.72",
            "Syslinux 6.03",
            "Grub 0.97",
            "Grub 2.00"});
			this.cbBootLoader.Location = new System.Drawing.Point(6, 140);
			this.cbBootLoader.Name = "cbBootLoader";
			this.cbBootLoader.Size = new System.Drawing.Size(170, 25);
			this.cbBootLoader.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbBootLoader.TabIndex = 55;
			this.cbBootLoader.UseCustomBackColor = true;
			this.cbBootLoader.UseSelectable = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label1.Location = new System.Drawing.Point(6, 123);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 15);
			this.label1.TabIndex = 56;
			this.label1.Text = "Boot Loader:";
			this.label1.UseCustomBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label4.Location = new System.Drawing.Point(6, 18);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 15);
			this.label4.TabIndex = 54;
			this.label4.Text = "File System:";
			this.label4.UseCustomBackColor = true;
			// 
			// cbBootFileSystem
			// 
			this.cbBootFileSystem.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbBootFileSystem.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbBootFileSystem.FormattingEnabled = true;
			this.cbBootFileSystem.ItemHeight = 19;
			this.cbBootFileSystem.Items.AddRange(new object[] {
            "FAT12",
            "FAT16"});
			this.cbBootFileSystem.Location = new System.Drawing.Point(6, 37);
			this.cbBootFileSystem.Name = "cbBootFileSystem";
			this.cbBootFileSystem.Size = new System.Drawing.Size(170, 25);
			this.cbBootFileSystem.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbBootFileSystem.TabIndex = 53;
			this.cbBootFileSystem.UseCustomBackColor = true;
			this.cbBootFileSystem.UseSelectable = true;
			// 
			// cbBootFormat
			// 
			this.cbBootFormat.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbBootFormat.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbBootFormat.FormattingEnabled = true;
			this.cbBootFormat.ItemHeight = 19;
			this.cbBootFormat.Items.AddRange(new object[] {
            "Multiboot v1"});
			this.cbBootFormat.Location = new System.Drawing.Point(6, 86);
			this.cbBootFormat.Name = "cbBootFormat";
			this.cbBootFormat.Size = new System.Drawing.Size(170, 25);
			this.cbBootFormat.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbBootFormat.TabIndex = 51;
			this.cbBootFormat.UseCustomBackColor = true;
			this.cbBootFormat.UseSelectable = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label2.Location = new System.Drawing.Point(6, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 15);
			this.label2.TabIndex = 52;
			this.label2.Text = "Boot Format:";
			this.label2.UseCustomBackColor = true;
			// 
			// groupBox15
			// 
			this.groupBox15.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox15.Controls.Add(this.cbDebugConnectionOption);
			this.groupBox15.Controls.Add(this.label8);
			this.groupBox15.Controls.Add(this.cbLaunchMosaDebugger);
			this.groupBox15.Controls.Add(this.cbLaunchGDB);
			this.groupBox15.Controls.Add(this.cbEnableQemuGDB);
			this.groupBox15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox15.Location = new System.Drawing.Point(8, 18);
			this.groupBox15.Name = "groupBox15";
			this.groupBox15.Size = new System.Drawing.Size(206, 225);
			this.groupBox15.TabIndex = 44;
			this.groupBox15.TabStop = false;
			this.groupBox15.Text = "Debugger:";
			// 
			// cbDebugConnectionOption
			// 
			this.cbDebugConnectionOption.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbDebugConnectionOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDebugConnectionOption.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbDebugConnectionOption.FormattingEnabled = true;
			this.cbDebugConnectionOption.ItemHeight = 19;
			this.cbDebugConnectionOption.Items.AddRange(new object[] {
            "None",
            "Pipe",
            "TCP Server",
            "TCP Client"});
			this.cbDebugConnectionOption.Location = new System.Drawing.Point(6, 127);
			this.cbDebugConnectionOption.Name = "cbDebugConnectionOption";
			this.cbDebugConnectionOption.Size = new System.Drawing.Size(170, 25);
			this.cbDebugConnectionOption.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbDebugConnectionOption.TabIndex = 38;
			this.cbDebugConnectionOption.UseCustomBackColor = true;
			this.cbDebugConnectionOption.UseSelectable = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label8.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label8.Location = new System.Drawing.Point(6, 105);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(110, 15);
			this.label8.TabIndex = 37;
			this.label8.Text = "Debug Connection:";
			this.label8.UseCustomBackColor = true;
			// 
			// cbLaunchMosaDebugger
			// 
			this.cbLaunchMosaDebugger.AutoSize = true;
			this.cbLaunchMosaDebugger.Checked = true;
			this.cbLaunchMosaDebugger.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbLaunchMosaDebugger.Location = new System.Drawing.Point(6, 72);
			this.cbLaunchMosaDebugger.Name = "cbLaunchMosaDebugger";
			this.cbLaunchMosaDebugger.Size = new System.Drawing.Size(154, 15);
			this.cbLaunchMosaDebugger.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbLaunchMosaDebugger.TabIndex = 8;
			this.cbLaunchMosaDebugger.Text = "Launch MOSA Debugger";
			this.cbLaunchMosaDebugger.UseCustomBackColor = true;
			this.cbLaunchMosaDebugger.UseSelectable = true;
			// 
			// cbLaunchGDB
			// 
			this.cbLaunchGDB.AutoSize = true;
			this.cbLaunchGDB.Checked = true;
			this.cbLaunchGDB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbLaunchGDB.Location = new System.Drawing.Point(6, 51);
			this.cbLaunchGDB.Name = "cbLaunchGDB";
			this.cbLaunchGDB.Size = new System.Drawing.Size(88, 15);
			this.cbLaunchGDB.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbLaunchGDB.TabIndex = 7;
			this.cbLaunchGDB.Text = "Launch GDB";
			this.cbLaunchGDB.UseCustomBackColor = true;
			this.cbLaunchGDB.UseSelectable = true;
			// 
			// cbEnableQemuGDB
			// 
			this.cbEnableQemuGDB.AutoSize = true;
			this.cbEnableQemuGDB.Checked = true;
			this.cbEnableQemuGDB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableQemuGDB.Location = new System.Drawing.Point(6, 28);
			this.cbEnableQemuGDB.Name = "cbEnableQemuGDB";
			this.cbEnableQemuGDB.Size = new System.Drawing.Size(121, 15);
			this.cbEnableQemuGDB.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEnableQemuGDB.TabIndex = 6;
			this.cbEnableQemuGDB.Text = "Enable QEMU GDB";
			this.cbEnableQemuGDB.UseCustomBackColor = true;
			this.cbEnableQemuGDB.UseSelectable = true;
			// 
			// groupBox14
			// 
			this.groupBox14.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox14.Controls.Add(this.metroLabel1);
			this.groupBox14.Controls.Add(this.tbOsName);
			this.groupBox14.Controls.Add(this.cbPlugKorlib);
			this.groupBox14.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox14.Location = new System.Drawing.Point(220, 148);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.Size = new System.Drawing.Size(206, 95);
			this.groupBox14.TabIndex = 43;
			this.groupBox14.TabStop = false;
			this.groupBox14.Text = "Other:";
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
			this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel1.Location = new System.Drawing.Point(6, 42);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(60, 15);
			this.metroLabel1.TabIndex = 43;
			this.metroLabel1.Text = "OS Name:";
			this.metroLabel1.UseCustomBackColor = true;
			// 
			// tbOsName
			// 
			// 
			// 
			// 
			this.tbOsName.CustomButton.Image = null;
			this.tbOsName.CustomButton.Location = new System.Drawing.Point(111, 2);
			this.tbOsName.CustomButton.Name = "";
			this.tbOsName.CustomButton.Size = new System.Drawing.Size(15, 15);
			this.tbOsName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.tbOsName.CustomButton.TabIndex = 1;
			this.tbOsName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.tbOsName.CustomButton.UseSelectable = true;
			this.tbOsName.CustomButton.Visible = false;
			this.tbOsName.Enabled = false;
			this.tbOsName.Lines = new string[] {
        "{Name}"};
			this.tbOsName.Location = new System.Drawing.Point(72, 39);
			this.tbOsName.MaxLength = 32767;
			this.tbOsName.Name = "tbOsName";
			this.tbOsName.PasswordChar = '\0';
			this.tbOsName.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbOsName.SelectedText = "";
			this.tbOsName.SelectionLength = 0;
			this.tbOsName.SelectionStart = 0;
			this.tbOsName.ShortcutsEnabled = true;
			this.tbOsName.Size = new System.Drawing.Size(129, 20);
			this.tbOsName.TabIndex = 39;
			this.tbOsName.Text = "{Name}";
			this.tbOsName.UseSelectable = true;
			this.tbOsName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.tbOsName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// cbPlugKorlib
			// 
			this.cbPlugKorlib.AutoSize = true;
			this.cbPlugKorlib.Checked = true;
			this.cbPlugKorlib.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbPlugKorlib.Location = new System.Drawing.Point(6, 19);
			this.cbPlugKorlib.Name = "cbPlugKorlib";
			this.cbPlugKorlib.Size = new System.Drawing.Size(81, 15);
			this.cbPlugKorlib.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbPlugKorlib.TabIndex = 7;
			this.cbPlugKorlib.Text = "Plug Korlib";
			this.cbPlugKorlib.UseCustomBackColor = true;
			this.cbPlugKorlib.UseSelectable = true;
			// 
			// groupBox13
			// 
			this.groupBox13.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox13.Controls.Add(this.label9);
			this.groupBox13.Controls.Add(this.tbBaseAddress);
			this.groupBox13.Controls.Add(this.cbRelocationTable);
			this.groupBox13.Controls.Add(this.cbEmitSymbolTable);
			this.groupBox13.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox13.Location = new System.Drawing.Point(220, 18);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Size = new System.Drawing.Size(206, 124);
			this.groupBox13.TabIndex = 23;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "Linker Options:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label9.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label9.Location = new System.Drawing.Point(6, 91);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(79, 15);
			this.label9.TabIndex = 42;
			this.label9.Text = "Base Address:";
			this.label9.UseCustomBackColor = true;
			// 
			// tbBaseAddress
			// 
			// 
			// 
			// 
			this.tbBaseAddress.CustomButton.Image = null;
			this.tbBaseAddress.CustomButton.Location = new System.Drawing.Point(71, 2);
			this.tbBaseAddress.CustomButton.Name = "";
			this.tbBaseAddress.CustomButton.Size = new System.Drawing.Size(15, 15);
			this.tbBaseAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.tbBaseAddress.CustomButton.TabIndex = 1;
			this.tbBaseAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.tbBaseAddress.CustomButton.UseSelectable = true;
			this.tbBaseAddress.CustomButton.Visible = false;
			this.tbBaseAddress.Lines = new string[] {
        "0x00400000"};
			this.tbBaseAddress.Location = new System.Drawing.Point(102, 90);
			this.tbBaseAddress.MaxLength = 32767;
			this.tbBaseAddress.Name = "tbBaseAddress";
			this.tbBaseAddress.PasswordChar = '\0';
			this.tbBaseAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.tbBaseAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbBaseAddress.SelectedText = "";
			this.tbBaseAddress.SelectionLength = 0;
			this.tbBaseAddress.SelectionStart = 0;
			this.tbBaseAddress.ShortcutsEnabled = true;
			this.tbBaseAddress.Size = new System.Drawing.Size(89, 20);
			this.tbBaseAddress.TabIndex = 41;
			this.tbBaseAddress.Text = "0x00400000";
			this.tbBaseAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbBaseAddress.UseSelectable = true;
			this.tbBaseAddress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.tbBaseAddress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// cbRelocationTable
			// 
			this.cbRelocationTable.AutoSize = true;
			this.cbRelocationTable.Checked = true;
			this.cbRelocationTable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRelocationTable.Location = new System.Drawing.Point(6, 51);
			this.cbRelocationTable.Name = "cbRelocationTable";
			this.cbRelocationTable.Size = new System.Drawing.Size(143, 15);
			this.cbRelocationTable.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbRelocationTable.TabIndex = 7;
			this.cbRelocationTable.Text = "Emit Static Relocations";
			this.cbRelocationTable.UseCustomBackColor = true;
			this.cbRelocationTable.UseSelectable = true;
			// 
			// cbEmitSymbolTable
			// 
			this.cbEmitSymbolTable.AutoSize = true;
			this.cbEmitSymbolTable.Checked = true;
			this.cbEmitSymbolTable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEmitSymbolTable.Location = new System.Drawing.Point(6, 28);
			this.cbEmitSymbolTable.Name = "cbEmitSymbolTable";
			this.cbEmitSymbolTable.Size = new System.Drawing.Size(112, 15);
			this.cbEmitSymbolTable.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEmitSymbolTable.TabIndex = 6;
			this.cbEmitSymbolTable.Text = "Emit All Symbols";
			this.cbEmitSymbolTable.UseCustomBackColor = true;
			this.cbEmitSymbolTable.UseSelectable = true;
			// 
			// tabFiles
			// 
			this.tabFiles.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.tabFiles.Controls.Add(this.panelAdditionalFiles);
			this.tabFiles.Location = new System.Drawing.Point(4, 22);
			this.tabFiles.Name = "tabFiles";
			this.tabFiles.Size = new System.Drawing.Size(653, 446);
			this.tabFiles.TabIndex = 5;
			this.tabFiles.Text = "Included Files";
			// 
			// panelAdditionalFiles
			// 
			this.panelAdditionalFiles.Controls.Add(this.dataGridView1);
			this.panelAdditionalFiles.Controls.Add(this.btnRemoveFiles);
			this.panelAdditionalFiles.Controls.Add(this.btnAddFiles);
			this.panelAdditionalFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelAdditionalFiles.Location = new System.Drawing.Point(0, 0);
			this.panelAdditionalFiles.Name = "panelAdditionalFiles";
			this.panelAdditionalFiles.Size = new System.Drawing.Size(653, 446);
			this.panelAdditionalFiles.TabIndex = 23;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(3, 3);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(647, 440);
			this.dataGridView1.TabIndex = 32;
			// 
			// btnRemoveFiles
			// 
			this.btnRemoveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveFiles.Location = new System.Drawing.Point(626, 68);
			this.btnRemoveFiles.Name = "btnRemoveFiles";
			this.btnRemoveFiles.Size = new System.Drawing.Size(25, 25);
			this.btnRemoveFiles.TabIndex = 30;
			this.btnRemoveFiles.Text = "-";
			this.btnRemoveFiles.UseSelectable = true;
			this.btnRemoveFiles.Click += new System.EventHandler(this.BtnRemoveFiles_Click);
			// 
			// btnAddFiles
			// 
			this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddFiles.Location = new System.Drawing.Point(626, 37);
			this.btnAddFiles.Name = "btnAddFiles";
			this.btnAddFiles.Size = new System.Drawing.Size(25, 25);
			this.btnAddFiles.TabIndex = 29;
			this.btnAddFiles.Text = "+";
			this.btnAddFiles.UseSelectable = true;
			this.btnAddFiles.Click += new System.EventHandler(this.BtnAddFiles_Click);
			// 
			// tabApplicationLocations
			// 
			this.tabApplicationLocations.Controls.Add(this.groupBox11);
			this.tabApplicationLocations.Controls.Add(this.groupBox10);
			this.tabApplicationLocations.Controls.Add(this.groupBox9);
			this.tabApplicationLocations.Controls.Add(this.groupBox8);
			this.tabApplicationLocations.Controls.Add(this.groupBox7);
			this.tabApplicationLocations.HorizontalScrollbarBarColor = true;
			this.tabApplicationLocations.HorizontalScrollbarHighlightOnWheel = false;
			this.tabApplicationLocations.HorizontalScrollbarSize = 10;
			this.tabApplicationLocations.Location = new System.Drawing.Point(4, 22);
			this.tabApplicationLocations.Name = "tabApplicationLocations";
			this.tabApplicationLocations.Padding = new System.Windows.Forms.Padding(3);
			this.tabApplicationLocations.Size = new System.Drawing.Size(653, 446);
			this.tabApplicationLocations.TabIndex = 3;
			this.tabApplicationLocations.Text = "Application Locations";
			this.tabApplicationLocations.UseVisualStyleBackColor = true;
			this.tabApplicationLocations.VerticalScrollbarBarColor = true;
			this.tabApplicationLocations.VerticalScrollbarHighlightOnWheel = false;
			this.tabApplicationLocations.VerticalScrollbarSize = 10;
			// 
			// groupBox11
			// 
			this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox11.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox11.Controls.Add(this.button7);
			this.groupBox11.Controls.Add(this.lbmkisofsExecutable);
			this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox11.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox11.Location = new System.Drawing.Point(8, 291);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(564, 50);
			this.groupBox11.TabIndex = 25;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "mkisofs:";
			// 
			// button7
			// 
			this.button7.Enabled = false;
			this.button7.Location = new System.Drawing.Point(6, 19);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(106, 23);
			this.button7.TabIndex = 20;
			this.button7.Text = "Executable:";
			this.button7.UseSelectable = true;
			// 
			// lbmkisofsExecutable
			// 
			this.lbmkisofsExecutable.AutoSize = true;
			this.lbmkisofsExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbmkisofsExecutable.Name = "lbmkisofsExecutable";
			this.lbmkisofsExecutable.Size = new System.Drawing.Size(149, 19);
			this.lbmkisofsExecutable.TabIndex = 19;
			this.lbmkisofsExecutable.Text = "{mkisofs.exe Executable}";
			this.lbmkisofsExecutable.UseCustomBackColor = true;
			// 
			// groupBox10
			// 
			this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox10.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox10.Controls.Add(this.button4);
			this.groupBox10.Controls.Add(this.lbVMwarePlayerExecutable);
			this.groupBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox10.Location = new System.Drawing.Point(8, 235);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(564, 50);
			this.groupBox10.TabIndex = 24;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "VMware Player:";
			// 
			// button4
			// 
			this.button4.Enabled = false;
			this.button4.Location = new System.Drawing.Point(6, 19);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(106, 23);
			this.button4.TabIndex = 20;
			this.button4.Text = "Executable:";
			this.button4.UseSelectable = true;
			// 
			// lbVMwarePlayerExecutable
			// 
			this.lbVMwarePlayerExecutable.AutoSize = true;
			this.lbVMwarePlayerExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbVMwarePlayerExecutable.Name = "lbVMwarePlayerExecutable";
			this.lbVMwarePlayerExecutable.Size = new System.Drawing.Size(171, 19);
			this.lbVMwarePlayerExecutable.TabIndex = 19;
			this.lbVMwarePlayerExecutable.Text = "{VMware Player Executable}";
			this.lbVMwarePlayerExecutable.UseCustomBackColor = true;
			// 
			// groupBox9
			// 
			this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox9.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox9.Controls.Add(this.button6);
			this.groupBox9.Controls.Add(this.lbBOCHSExecutable);
			this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox9.Location = new System.Drawing.Point(8, 179);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(564, 50);
			this.groupBox9.TabIndex = 23;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "BOCHS:";
			// 
			// button6
			// 
			this.button6.Enabled = false;
			this.button6.Location = new System.Drawing.Point(6, 19);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(106, 23);
			this.button6.TabIndex = 20;
			this.button6.Text = "Executable:";
			this.button6.UseSelectable = true;
			// 
			// lbBOCHSExecutable
			// 
			this.lbBOCHSExecutable.AutoSize = true;
			this.lbBOCHSExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbBOCHSExecutable.Name = "lbBOCHSExecutable";
			this.lbBOCHSExecutable.Size = new System.Drawing.Size(127, 19);
			this.lbBOCHSExecutable.TabIndex = 19;
			this.lbBOCHSExecutable.Text = "{BOCHS Executable}";
			this.lbBOCHSExecutable.UseCustomBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox8.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox8.Controls.Add(this.button5);
			this.groupBox8.Controls.Add(this.lbNDISASMExecutable);
			this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox8.Location = new System.Drawing.Point(8, 17);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(564, 50);
			this.groupBox8.TabIndex = 1;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "NDISASM:";
			// 
			// button5
			// 
			this.button5.Enabled = false;
			this.button5.Location = new System.Drawing.Point(6, 19);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(106, 23);
			this.button5.TabIndex = 20;
			this.button5.Text = "Executable:";
			this.button5.UseSelectable = true;
			// 
			// lbNDISASMExecutable
			// 
			this.lbNDISASMExecutable.AutoSize = true;
			this.lbNDISASMExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbNDISASMExecutable.Name = "lbNDISASMExecutable";
			this.lbNDISASMExecutable.Size = new System.Drawing.Size(140, 19);
			this.lbNDISASMExecutable.TabIndex = 19;
			this.lbNDISASMExecutable.Text = "{NDISASM Executable}";
			this.lbNDISASMExecutable.UseCustomBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox7.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox7.Controls.Add(this.lbQEMUImgApplication);
			this.groupBox7.Controls.Add(this.button8);
			this.groupBox7.Controls.Add(this.lbQEMUBIOSDirectory);
			this.groupBox7.Controls.Add(this.button3);
			this.groupBox7.Controls.Add(this.button2);
			this.groupBox7.Controls.Add(this.lbQEMUExecutable);
			this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox7.Location = new System.Drawing.Point(8, 73);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(564, 100);
			this.groupBox7.TabIndex = 0;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "QEMU:";
			// 
			// lbQEMUImgApplication
			// 
			this.lbQEMUImgApplication.AutoSize = true;
			this.lbQEMUImgApplication.Location = new System.Drawing.Point(118, 74);
			this.lbQEMUImgApplication.Name = "lbQEMUImgApplication";
			this.lbQEMUImgApplication.Size = new System.Drawing.Size(163, 19);
			this.lbQEMUImgApplication.TabIndex = 24;
			this.lbQEMUImgApplication.Text = "{QEMU Image Executable}";
			this.lbQEMUImgApplication.UseCustomBackColor = true;
			// 
			// button8
			// 
			this.button8.Enabled = false;
			this.button8.Location = new System.Drawing.Point(6, 69);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(106, 23);
			this.button8.TabIndex = 23;
			this.button8.Text = "QEMU Image:";
			this.button8.UseSelectable = true;
			// 
			// lbQEMUBIOSDirectory
			// 
			this.lbQEMUBIOSDirectory.AutoSize = true;
			this.lbQEMUBIOSDirectory.Location = new System.Drawing.Point(118, 49);
			this.lbQEMUBIOSDirectory.Name = "lbQEMUBIOSDirectory";
			this.lbQEMUBIOSDirectory.Size = new System.Drawing.Size(147, 19);
			this.lbQEMUBIOSDirectory.TabIndex = 22;
			this.lbQEMUBIOSDirectory.Text = "{QEMU BIOS Directory}";
			this.lbQEMUBIOSDirectory.UseCustomBackColor = true;
			// 
			// button3
			// 
			this.button3.Enabled = false;
			this.button3.Location = new System.Drawing.Point(6, 44);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(106, 23);
			this.button3.TabIndex = 21;
			this.button3.Text = "BIOS Directory:";
			this.button3.UseSelectable = true;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(6, 19);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(106, 23);
			this.button2.TabIndex = 20;
			this.button2.Text = "Executable:";
			this.button2.UseSelectable = true;
			// 
			// lbQEMUExecutable
			// 
			this.lbQEMUExecutable.AutoSize = true;
			this.lbQEMUExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbQEMUExecutable.Name = "lbQEMUExecutable";
			this.lbQEMUExecutable.Size = new System.Drawing.Size(122, 19);
			this.lbQEMUExecutable.TabIndex = 19;
			this.lbQEMUExecutable.Text = "{QEMU Executable}";
			this.lbQEMUExecutable.UseCustomBackColor = true;
			// 
			// tabCounters
			// 
			this.tabCounters.Controls.Add(this.rtbCounters);
			this.tabCounters.HorizontalScrollbarBarColor = true;
			this.tabCounters.HorizontalScrollbarHighlightOnWheel = false;
			this.tabCounters.HorizontalScrollbarSize = 10;
			this.tabCounters.Location = new System.Drawing.Point(4, 22);
			this.tabCounters.Name = "tabCounters";
			this.tabCounters.Padding = new System.Windows.Forms.Padding(3);
			this.tabCounters.Size = new System.Drawing.Size(653, 446);
			this.tabCounters.TabIndex = 2;
			this.tabCounters.Text = "Counters";
			this.tabCounters.UseVisualStyleBackColor = true;
			this.tabCounters.VerticalScrollbarBarColor = true;
			this.tabCounters.VerticalScrollbarHighlightOnWheel = false;
			this.tabCounters.VerticalScrollbarSize = 10;
			// 
			// rtbCounters
			// 
			this.rtbCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbCounters.Font = new System.Drawing.Font("Consolas", 8F);
			this.rtbCounters.Location = new System.Drawing.Point(0, 0);
			this.rtbCounters.Name = "rtbCounters";
			this.rtbCounters.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.rtbCounters.Size = new System.Drawing.Size(653, 427);
			this.rtbCounters.TabIndex = 1;
			this.rtbCounters.Text = "";
			this.rtbCounters.WordWrap = false;
			// 
			// tabOutput
			// 
			this.tabOutput.Controls.Add(this.rtbOutput);
			this.tabOutput.HorizontalScrollbarBarColor = true;
			this.tabOutput.HorizontalScrollbarHighlightOnWheel = false;
			this.tabOutput.HorizontalScrollbarSize = 10;
			this.tabOutput.Location = new System.Drawing.Point(4, 22);
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
			this.tabOutput.Size = new System.Drawing.Size(653, 446);
			this.tabOutput.TabIndex = 1;
			this.tabOutput.Text = "Output";
			this.tabOutput.UseVisualStyleBackColor = true;
			this.tabOutput.VerticalScrollbarBarColor = true;
			this.tabOutput.VerticalScrollbarHighlightOnWheel = false;
			this.tabOutput.VerticalScrollbarSize = 10;
			// 
			// rtbOutput
			// 
			this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbOutput.Font = new System.Drawing.Font("Consolas", 8F);
			this.rtbOutput.Location = new System.Drawing.Point(3, 0);
			this.rtbOutput.Name = "rtbOutput";
			this.rtbOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.rtbOutput.Size = new System.Drawing.Size(650, 427);
			this.rtbOutput.TabIndex = 0;
			this.rtbOutput.Text = "";
			this.rtbOutput.WordWrap = false;
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.DefaultExt = "*.exe";
			this.openFileDialog2.Filter = "Executable|*.exe";
			this.openFileDialog2.Title = "Select Assembly";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// tbMode
			// 
			// 
			// 
			// 
			this.tbMode.CustomButton.Image = null;
			this.tbMode.CustomButton.Location = new System.Drawing.Point(80, 2);
			this.tbMode.CustomButton.Name = "";
			this.tbMode.CustomButton.Size = new System.Drawing.Size(15, 15);
			this.tbMode.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.tbMode.CustomButton.TabIndex = 1;
			this.tbMode.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.tbMode.CustomButton.UseSelectable = true;
			this.tbMode.CustomButton.Visible = false;
			this.tbMode.Enabled = false;
			this.tbMode.Lines = new string[] {
        "{Mode}"};
			this.tbMode.Location = new System.Drawing.Point(26, 193);
			this.tbMode.MaxLength = 32767;
			this.tbMode.Name = "tbMode";
			this.tbMode.PasswordChar = '\0';
			this.tbMode.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbMode.SelectedText = "";
			this.tbMode.SelectionLength = 0;
			this.tbMode.SelectionStart = 0;
			this.tbMode.ShortcutsEnabled = true;
			this.tbMode.Size = new System.Drawing.Size(98, 20);
			this.tbMode.TabIndex = 58;
			this.tbMode.Text = "{Mode}";
			this.tbMode.UseSelectable = true;
			this.tbMode.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.tbMode.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// cbVBEVideo
			// 
			this.cbVBEVideo.AutoSize = true;
			this.cbVBEVideo.Location = new System.Drawing.Point(7, 175);
			this.cbVBEVideo.Name = "cbVBEVideo";
			this.cbVBEVideo.Size = new System.Drawing.Size(117, 15);
			this.cbVBEVideo.TabIndex = 57;
			this.cbVBEVideo.Text = "Enable VBE Video:";
			this.cbVBEVideo.UseCustomBackColor = true;
			this.cbVBEVideo.UseSelectable = true;
			// 
			// groupBox17
			// 
			this.groupBox17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox17.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox17.Controls.Add(this.metroLabel2);
			this.groupBox17.Controls.Add(this.label6);
			this.groupBox17.Controls.Add(this.nmMemory);
			this.groupBox17.Controls.Add(this.cbEmulator);
			this.groupBox17.Controls.Add(this.cbImageFormat);
			this.groupBox17.Controls.Add(this.label3);
			this.groupBox17.Controls.Add(this.label5);
			this.groupBox17.Controls.Add(this.cbPlatform);
			this.groupBox17.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox17.Location = new System.Drawing.Point(428, 125);
			this.groupBox17.Name = "groupBox17";
			this.groupBox17.Size = new System.Drawing.Size(215, 146);
			this.groupBox17.TabIndex = 33;
			this.groupBox17.TabStop = false;
			this.groupBox17.Text = "Target:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label5.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label5.Location = new System.Drawing.Point(6, 23);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 15);
			this.label5.TabIndex = 32;
			this.label5.Text = "Platform:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label5.UseCustomBackColor = true;
			// 
			// cbPlatform
			// 
			this.cbPlatform.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbPlatform.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbPlatform.FormattingEnabled = true;
			this.cbPlatform.ItemHeight = 19;
			this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "ARMv6"});
			this.cbPlatform.Location = new System.Drawing.Point(97, 18);
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(109, 25);
			this.cbPlatform.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbPlatform.TabIndex = 31;
			this.cbPlatform.UseCustomBackColor = true;
			this.cbPlatform.UseSelectable = true;
			// 
			// cbImageFormat
			// 
			this.cbImageFormat.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbImageFormat.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbImageFormat.FormattingEnabled = true;
			this.cbImageFormat.ItemHeight = 19;
			this.cbImageFormat.Items.AddRange(new object[] {
            "IMG (.img)",
            "ISO Image (.iso)",
            "Microsoft (.vhd)",
            "Virtual Box (.vdi)",
            "VMware (.vmdk)"});
			this.cbImageFormat.Location = new System.Drawing.Point(97, 49);
			this.cbImageFormat.Name = "cbImageFormat";
			this.cbImageFormat.Size = new System.Drawing.Size(109, 25);
			this.cbImageFormat.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbImageFormat.TabIndex = 34;
			this.cbImageFormat.UseCustomBackColor = true;
			this.cbImageFormat.UseSelectable = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label3.Location = new System.Drawing.Point(6, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 15);
			this.label3.TabIndex = 33;
			this.label3.Text = "Image Format:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label3.UseCustomBackColor = true;
			// 
			// cbEmulator
			// 
			this.cbEmulator.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cbEmulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEmulator.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cbEmulator.FormattingEnabled = true;
			this.cbEmulator.ItemHeight = 19;
			this.cbEmulator.Items.AddRange(new object[] {
            "QEMU",
            "Bochs",
            "VMware"});
			this.cbEmulator.Location = new System.Drawing.Point(97, 81);
			this.cbEmulator.Name = "cbEmulator";
			this.cbEmulator.Size = new System.Drawing.Size(109, 25);
			this.cbEmulator.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbEmulator.TabIndex = 35;
			this.cbEmulator.UseCustomBackColor = true;
			this.cbEmulator.UseSelectable = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.FontSize = MetroFramework.MetroLabelSize.Small;
			this.label6.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.label6.Location = new System.Drawing.Point(6, 113);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 15);
			this.label6.TabIndex = 37;
			this.label6.Text = "Memory:";
			this.label6.UseCustomBackColor = true;
			// 
			// nmMemory
			// 
			this.nmMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nmMemory.Location = new System.Drawing.Point(97, 113);
			this.nmMemory.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
			this.nmMemory.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.nmMemory.Name = "nmMemory";
			this.nmMemory.Size = new System.Drawing.Size(56, 20);
			this.nmMemory.TabIndex = 36;
			this.nmMemory.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
			this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel2.Location = new System.Drawing.Point(6, 86);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(58, 15);
			this.metroLabel2.TabIndex = 38;
			this.metroLabel2.Text = "Emulator:";
			this.metroLabel2.UseCustomBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(452, 345);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(168, 23);
			this.button1.TabIndex = 39;
			this.button1.Text = "Compile and Run (F5)";
			this.button1.UseSelectable = true;
			// 
			// cbExitOnLaunch
			// 
			this.cbExitOnLaunch.AutoSize = true;
			this.cbExitOnLaunch.Checked = true;
			this.cbExitOnLaunch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbExitOnLaunch.Location = new System.Drawing.Point(452, 324);
			this.cbExitOnLaunch.Name = "cbExitOnLaunch";
			this.cbExitOnLaunch.Size = new System.Drawing.Size(101, 15);
			this.cbExitOnLaunch.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbExitOnLaunch.TabIndex = 40;
			this.cbExitOnLaunch.Text = "Exit on Launch";
			this.cbExitOnLaunch.UseCustomBackColor = true;
			this.cbExitOnLaunch.UseSelectable = true;
			// 
			// cbGenerateNASMFile
			// 
			this.cbGenerateNASMFile.AutoSize = true;
			this.cbGenerateNASMFile.Location = new System.Drawing.Point(6, 60);
			this.cbGenerateNASMFile.Name = "cbGenerateNASMFile";
			this.cbGenerateNASMFile.Size = new System.Drawing.Size(78, 15);
			this.cbGenerateNASMFile.Style = MetroFramework.MetroColorStyle.Blue;
			this.cbGenerateNASMFile.TabIndex = 50;
			this.cbGenerateNASMFile.Text = "NASM File";
			this.cbGenerateNASMFile.UseCustomBackColor = true;
			this.cbGenerateNASMFile.UseSelectable = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(659, 516);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.tbApplicationLocations);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Resizable = false;
			this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "MOSA Launcher";
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.tbApplicationLocations.ResumeLayout(false);
			this.tabOptions.ResumeLayout(false);
			this.tabOptions.PerformLayout();
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabAdvanced.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox15.ResumeLayout(false);
			this.groupBox15.PerformLayout();
			this.groupBox14.ResumeLayout(false);
			this.groupBox14.PerformLayout();
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.tabFiles.ResumeLayout(false);
			this.panelAdditionalFiles.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tabApplicationLocations.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.tabCounters.ResumeLayout(false);
			this.tabOutput.ResumeLayout(false);
			this.groupBox17.ResumeLayout(false);
			this.groupBox17.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nmMemory)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private MetroFramework.Controls.MetroProgressBar progressBar1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private MetroFramework.Controls.MetroTabControl tbApplicationLocations;
		private MetroFramework.Controls.MetroTabPage tabOutput;
		private System.Windows.Forms.GroupBox groupBox4;
		private MetroFramework.Controls.MetroButton btnSource;
		private MetroFramework.Controls.MetroLabel lbSource;
		private System.Windows.Forms.GroupBox groupBox3;
		private MetroFramework.Controls.MetroButton btnDestination;
		private System.Windows.Forms.GroupBox groupBox1;
		private MetroFramework.Controls.MetroCheckBox cbEnableIROptimizations;
		private MetroFramework.Controls.MetroCheckBox cbEnableSSA;
		private MetroFramework.Controls.MetroTabPage tabOptions;
		private System.Windows.Forms.RichTextBox rtbOutput;
		private MetroFramework.Controls.MetroTabPage tabCounters;
		private System.Windows.Forms.RichTextBox rtbCounters;
		private MetroFramework.Controls.MetroTabPage tabApplicationLocations;
		private System.Windows.Forms.GroupBox groupBox7;
		private MetroFramework.Controls.MetroLabel lbQEMUBIOSDirectory;
		private MetroFramework.Controls.MetroButton button3;
		private MetroFramework.Controls.MetroButton button2;
		private MetroFramework.Controls.MetroLabel lbQEMUExecutable;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.GroupBox groupBox8;
		private MetroFramework.Controls.MetroButton button5;
		private MetroFramework.Controls.MetroLabel lbNDISASMExecutable;
		private System.Windows.Forms.GroupBox groupBox9;
		private MetroFramework.Controls.MetroButton button6;
		private MetroFramework.Controls.MetroLabel lbBOCHSExecutable;
		private System.Windows.Forms.GroupBox groupBox10;
		private MetroFramework.Controls.MetroButton button4;
		private MetroFramework.Controls.MetroLabel lbVMwarePlayerExecutable;
		private System.Windows.Forms.GroupBox groupBox11;
		private MetroFramework.Controls.MetroButton button7;
		private MetroFramework.Controls.MetroLabel lbmkisofsExecutable;
		private MetroFramework.Controls.MetroLabel lbQEMUImgApplication;
		private MetroFramework.Controls.MetroButton button8;
		private System.Windows.Forms.GroupBox groupBox2;
		private MetroFramework.Controls.MetroCheckBox cbGenerateMapFile;
		private MetroFramework.Controls.MetroCheckBox cbEnableSparseConditionalConstantPropagation;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
		private System.Windows.Forms.GroupBox groupBox12;
		private MetroFramework.Controls.MetroCheckBox cbCompilerUsesMultipleThreads;
		private MetroFramework.Controls.MetroCheckBox cbInline;
		private MetroFramework.Controls.MetroTabPage tabAdvanced;
		private System.Windows.Forms.GroupBox groupBox13;
		private MetroFramework.Controls.MetroCheckBox cbRelocationTable;
		private MetroFramework.Controls.MetroCheckBox cbEmitSymbolTable;
		private MetroFramework.Controls.MetroTextBox tbBaseAddress;
		private MetroFramework.Controls.MetroLabel label9;
		private System.Windows.Forms.GroupBox groupBox14;
		private MetroFramework.Controls.MetroLabel lbDestinationDirectory;
		private MetroFramework.Controls.MetroCheckBox cbGenerateASMFile;
		private System.Windows.Forms.GroupBox groupBox15;
		private MetroFramework.Controls.MetroCheckBox cbEnableQemuGDB;
		private MetroFramework.Controls.MetroCheckBox cbLaunchGDB;
		private MetroFramework.Controls.MetroCheckBox cbGenerateDebugInfoFile;
		private MetroFramework.Controls.MetroCheckBox cbLaunchMosaDebugger;
		private System.Windows.Forms.TabPage tabFiles;
		private System.Windows.Forms.Panel panelAdditionalFiles;
		private MetroFramework.Controls.MetroButton btnRemoveFiles;
		private MetroFramework.Controls.MetroButton btnAddFiles;
		private MetroFramework.Controls.MetroCheckBox cbTwoPassOptimizations;
		private MetroFramework.Controls.MetroCheckBox cbLongExpansion;
		private System.Windows.Forms.DataGridView dataGridView1;
		private MetroFramework.Controls.MetroCheckBox cbValueNumbering;
		private MetroFramework.Controls.MetroCheckBox cbPlugKorlib;
		private MetroFramework.Controls.MetroCheckBox cbEnableMethodScanner;
		private System.Windows.Forms.Timer timer1;
		private MetroFramework.Controls.MetroCheckBox cbGenerateCompilerTime;
		private MetroFramework.Controls.MetroCheckBox cbBitTracker;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroTextBox tbOsName;
		private MetroFramework.Controls.MetroCheckBox cbInlineExplicitOnly;
		private MetroFramework.Controls.MetroCheckBox cbPlatformOptimizations;
		private MetroFramework.Controls.MetroCheckBox cbLoopInvariantCodeMotion;
		private MetroFramework.Controls.MetroComboBox cbDebugConnectionOption;
		private MetroFramework.Controls.MetroLabel label8;
		private System.Windows.Forms.GroupBox groupBox5;
		private MetroFramework.Controls.MetroComboBox cbBootLoader;
		private MetroFramework.Controls.MetroLabel label1;
		private MetroFramework.Controls.MetroLabel label4;
		private MetroFramework.Controls.MetroComboBox cbBootFileSystem;
		private MetroFramework.Controls.MetroComboBox cbBootFormat;
		private MetroFramework.Controls.MetroLabel label2;
		private System.Windows.Forms.GroupBox groupBox17;
		private MetroFramework.Controls.MetroComboBox cbImageFormat;
		private MetroFramework.Controls.MetroLabel label3;
		private MetroFramework.Controls.MetroLabel label5;
		private MetroFramework.Controls.MetroComboBox cbPlatform;
		private MetroFramework.Controls.MetroTextBox tbMode;
		private MetroFramework.Controls.MetroCheckBox cbVBEVideo;
		private MetroFramework.Controls.MetroCheckBox cbExitOnLaunch;
		private MetroFramework.Controls.MetroButton button1;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private MetroFramework.Controls.MetroLabel label6;
		private System.Windows.Forms.NumericUpDown nmMemory;
		private MetroFramework.Controls.MetroComboBox cbEmulator;
		private MetroFramework.Controls.MetroCheckBox cbGenerateNASMFile;
	}
}
