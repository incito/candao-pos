using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CnSharp.Windows.Updater.Util;
using EnvDTE;

namespace CnSharp.Delivery.VisualStudio.PackingTool.Utils
{
    public static class ProjectUtil
    {
        public static  ProjectAssemblyInfo GetProjectAssemblyInfo(Project project)
        {
            string assemblyInfoFile;
            var assemblyInfo = GetAssemblyInfo(project, out assemblyInfoFile);
            if(string.IsNullOrEmpty(assemblyInfo))
                throw new Exception("Assembly info file of project '"+project.FullName+"' missed.");
            var currentVersion =
                Regex.Match(assemblyInfo, @"AssemblyFileVersion\(""(?<version>.+)""\)").Groups["version"].Value;
            if (string.IsNullOrEmpty(currentVersion))
            {
               currentVersion = Regex.Match(assemblyInfo, @"AssemblyVersion\(""(?<version>.+)""\)").Groups["version"].Value;
            }
            var currentProductName =
                Regex.Match(assemblyInfo, @"AssemblyProduct\(""(?<product>.+)""\)").Groups["product"].Value;
            var currentCompanyName = Regex.Match(assemblyInfo, @"AssemblyCompany\(""(?<company>.+)""\)").Groups["company"].Value;
            return new ProjectAssemblyInfo(project) { Version = currentVersion, ProductName = currentProductName, CompanyName = currentCompanyName };
        }

        public static ProductInfo GetProductInfo(Project project)
        {
            string assemblyInfoFile;
            var assemblyInfo = GetAssemblyInfo(project, out assemblyInfoFile);
            var currentVersion =
                Regex.Match(assemblyInfo, @"AssemblyFileVersion\(""(?<version>.+)""\)").Groups["version"].Value;
            var currentProductName =
                Regex.Match(assemblyInfo, @"AssemblyProduct\(""(?<product>.+)""\)").Groups["product"].Value;
            var currentCompanyName = Regex.Match(assemblyInfo, @"AssemblyCompany\(""(?<company>.+)""\)").Groups["company"].Value;
            return new ProductInfo() { Version = currentVersion, Name = currentProductName, CompanyName = currentCompanyName };
        }

        public static string GetAssemblyInfo(Project project, out string assemblyInfoFile)
        {
            var prjDir = Path.GetDirectoryName(project.FileName);
            assemblyInfoFile = prjDir + "\\Properties\\AssemblyInfo.cs";
            if (!File.Exists(assemblyInfoFile))
            {
                assemblyInfoFile = prjDir + "\\AssemblyInfo.cs";
            }
            if (!File.Exists(assemblyInfoFile))
            {
                return null;
            }
            var assemblyInfo = FileUtil.ReadText(assemblyInfoFile, Encoding.Default);
            return assemblyInfo;
        }
    }
}
