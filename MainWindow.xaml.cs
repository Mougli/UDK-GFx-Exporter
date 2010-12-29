using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace UDKGFxExporter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string GFxPath = "Binaries\\GFx";

		public MainWindow()
		{
			InitializeComponent();
		}

		private void PopulateClipList()
		{
			Lst_MovieClips.Items.Clear();

			foreach (string file in Directory.GetFiles(Properties.Settings.Default.UDKPath, "*.swf", SearchOption.AllDirectories))
			{
				Lst_MovieClips.Items.Add(file);
			}
		}

		private void Btn_BrowsePath_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();

			dlg.Description = "Select the folder where UDK is installed.";
			dlg.ShowNewFolderButton = false;
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Txt_UDKPath.Text = dlg.SelectedPath;
				PopulateClipList();
			}
		}

		private void Btn_RefreshClips_Click(object sender, RoutedEventArgs e)
		{
			if (Directory.Exists(Properties.Settings.Default.UDKPath))
			{
				PopulateClipList();
			}
		}

		private void PreviewClip()
		{
			if (Lst_MovieClips.SelectedItem != null)
			{
				string exporter_exe = "FxMediaPlayerNoDebug.exe";
				
				if (Properties.Settings.Default.UseDebugPreview)
				{
					exporter_exe = "FxMediaPlayer.exe";
				}

				string previewer_complete_path = System.IO.Path.Combine(Properties.Settings.Default.UDKPath, GFxPath, exporter_exe);

				if (File.Exists(previewer_complete_path))
				{
					Process.Start(previewer_complete_path, (string)Lst_MovieClips.SelectedItem);
				}
				else
				{
					System.Windows.MessageBox.Show("Can't find " + previewer_complete_path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			Properties.Settings.Default.Save();
			base.OnClosing(e);
		}

		private void Btn_Preview_Click(object sender, RoutedEventArgs e)
		{
			PreviewClip();
		}

		private void Btn_Export_Click(object sender, RoutedEventArgs e)
		{
			string args = "";

			foreach (object swf in Lst_MovieClips.Items)
			{
				args += " " + (string)swf;
			}

			string exporter_complete_path = System.IO.Path.Combine(Properties.Settings.Default.UDKPath, GFxPath, "gfxexport.exe");
			if (File.Exists(exporter_complete_path))
			{
				Process p = Process.Start(exporter_complete_path, args + " " + Properties.Settings.Default.ExporterOptions);
			}
			else
			{
				System.Windows.MessageBox.Show("Can't find " + exporter_complete_path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Lst_MovieClips_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			PreviewClip();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			PopulateClipList();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			//string message = string.Format("UDK GFx Exporter v{0}, by Cédric Hauteville\nThis software is NOT endorsed by Epic Games or Scaleform");
			WPFAboutBox1 about = new WPFAboutBox1(this);
			about.ShowDialog();
		}
	}
}
