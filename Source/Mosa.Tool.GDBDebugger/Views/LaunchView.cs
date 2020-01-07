// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class LaunchView : DebugDockContent
	{
		private BindingList<LaunchEntry> launchEntries = new BindingList<LaunchEntry>();

		private List<string> ImageExtensions = new List<string>() { ".img", ".iso" };

		private class LaunchEntry
		{
			[Browsable(false)]
			public string ImageFile { get; set; }

			public string Image { get { return Path.GetFileName(ImageFile); } }

			public string Directory { get { return Path.GetDirectoryName(ImageFile); } }

			public string DebugFile { get; set; }

			public string BreakpointFile { get; set; }

			public string WatchFile { get; set; }
		}

		public LaunchView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();

			AddImages();

			dataGridView1.DataSource = launchEntries;
			dataGridView1.AutoResizeColumns();

			dataGridView1.Columns[0].Width = 300;
			dataGridView1.Columns[1].Width = 300;
			dataGridView1.Columns[2].Width = 200;
			dataGridView1.Columns[3].Width = 200;
			dataGridView1.Columns[4].Width = 200;
		}

		public override void OnRunning()
		{
			//launchEntries.Clear();
		}

		public override void OnPause()
		{
		}

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			dataGridView1.ClearSelection();
			dataGridView1.Rows[e.RowIndex].Selected = true;
			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as LaunchEntry;

			var menu = new MenuItem(clickedEntry.Image);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Image });

			m.Show(dataGridView1, relativeMousePosition);
		}

		private void AddImages()
		{
			SearchImages(Path.GetTempPath(), ImageExtensions);
			SearchImages(Path.Combine(Path.GetTempPath(), "MOSA"), ImageExtensions);
		}

		private void SearchImages(string directory, List<string> patterns)
		{
			if (!Directory.Exists(directory))
				return;

			foreach (var pattern in patterns)
			{
				SearchImages(directory, pattern);
			}
		}

		private void SearchImages(string directory, string pattern)
		{
			if (!Directory.Exists(directory))
				return;

			foreach (var file in Directory.GetFiles(directory, "*" + pattern))
			{
				AddEntry(file);
			}
		}

		private void AddEntry(string imagefile)
		{
			string directory = Path.GetDirectoryName(imagefile);
			string imagefileWithException = Path.GetFileNameWithoutExtension(imagefile);

			var debugFile = Path.Combine(directory, imagefileWithException + ".debug");
			var breakpointFile = Path.Combine(directory, imagefileWithException + ".breakpoints");
			var watchFile = Path.Combine(directory, imagefileWithException + ".watches");

			if (!File.Exists(debugFile))
			{
				debugFile = null;
			}

			if (!File.Exists(breakpointFile))
			{
				breakpointFile = null;
			}

			if (!File.Exists(watchFile))
			{
				watchFile = null;
			}

			launchEntries.Add(new LaunchEntry()
			{
				ImageFile = imagefile,
				DebugFile = debugFile,
				WatchFile = watchFile,
				BreakpointFile = breakpointFile
			});
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}
	}
}
