using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ZipUpdater.Monitor.Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//new UpdateHelper("http://localhost:81/release/demo.xml", Application.ExecutablePath,
			// Process.GetCurrentProcess().ProcessName, ProductName, Assembly.GetExecutingAssembly().GetName().Version.ToString()
			// ).CheckVersion();
			//var version = new VersionInfo
			//    { FileUrl = "", ReleaseDate = DateTime.Now, UpdateLog = "xxxxxxxxxx\r\nxxxxxxxxxxxx", Version = "3.1.0.0" };
			//var xml = XmlSerializerHelper.GetXmlStringFromObject(version);
			//var doc = new XmlDocument();
			//doc.LoadXml(xml);
			//doc.Save(Application.StartupPath+"\\VersionInfo.xml");
			//version = XmlSerializerHelper.LoadObjectFromXml<VersionInfo>(Application.StartupPath + "\\VersionInfo.xml");
			//MessageBox.Show(version.UpdateLog);
		}
	}
}
