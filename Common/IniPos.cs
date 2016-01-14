using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class IniPos
    {
       public static string posini = "pos.ini";
       public static string getPosIniValue(string apppath,string Section, string Key, string defValue)
       {
           string inipath=apppath+posini;
           IniFile inifiles=new IniFile(inipath);
           string iniValue=inifiles.IniReadValue(Section,Key);
           if (iniValue.Equals(null) || (iniValue.Equals("")))
               iniValue=defValue;
           return iniValue;
       }
        public static void setPosIniVlaue(string apppath,string Section, string Key, string Value)
        {
           string inipath=apppath+posini;
           IniFile inifiles=new IniFile(inipath);
           inifiles.IniWriteValue(Section,Key,Value);
        }

    }
}
