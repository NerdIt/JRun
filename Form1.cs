using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace JRun
{
	public partial class Form1 : Form
	{
		Form1 thisForm;
		public Form1()
		{
			thisForm = this;
			InitializeComponent();
			button1.AllowDrop = true;
			button1.DragEnter += new DragEventHandler(Button1_DragEnter);
			button1.DragDrop += new DragEventHandler(Button1_DragDrop);
		}





		private void Button1_DragDrop(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0)
			{
				if(files[0].Contains(".class"))
				{

					DialogResult dialogResult = MessageBox.Show("Would you like to run this .class file?", "Confirmation", MessageBoxButtons.OKCancel);
					if (dialogResult == DialogResult.OK)
					{
						string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
						string exeLocation = "";
						if (!Directory.Exists(appData + @"\NerdIt"))
						{
							Directory.CreateDirectory(appData + @"\NerdIt");
						}
						exeLocation = appData + @"\NerdIt";

						var d = Directory.CreateDirectory(exeLocation + @"\$jcache$");
						var f = File.Create(exeLocation + @"\$jcache$\jrun$.bat");
						f.Close();
						using (StreamWriter sw = new StreamWriter(exeLocation + @"\$jcache$\jrun$.bat"))
						{
							string directory = "";
							files[0] = files[0].Replace(@"\", "?");
							string[] pathSplit = files[0].Split('?');
							for (int i = 0; i < pathSplit.Length - 1; i++)
							{
								directory += pathSplit[i] + @"\";
							}
							sw.WriteLine("@echo off");
							sw.WriteLine("java -classpath " + directory + " " + pathSplit[pathSplit.Length - 1].Replace(".class", ""));
							//sw.WriteLine("pause");
							sw.Flush();
							sw.Close();
						}

						var p = Process.Start(exeLocation + @"\$jcache$\jrun$.bat");

						p.WaitForExit();

						Directory.Delete(exeLocation + @"\$jcache$", true);
					}
					
					
				}
			}
		}

		private void Button1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}
	}
}
