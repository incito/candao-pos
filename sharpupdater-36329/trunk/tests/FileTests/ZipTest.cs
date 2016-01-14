using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileTests
{
	[TestClass]
	public class ZipTest
	{
		[TestMethod]
		public void TestUnzip()
		{
			Unzip(@"E:\host\cnsharp.com\release\demo.zip", @"D:\qinqinbo\Desktop");
		}

		private static void Unzip(string zipFile, string targetFolder)
		{
			var ass = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory , "CnSharp.ZipUtil.dll"));
			var type = ass.GetType("CnSharp.IO.ZipUtil");
			var m = type.GetMethod("Unzip");
			m.Invoke(null, new object[] { zipFile, targetFolder });
		}
	}
}
