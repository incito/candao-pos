using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace CanDaoCD.Pos.Common.Operates.FileOperate
{
   public class OXmlOperate
    {
        #region Xml与对象操作
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            try
            {
                using (var sr = new StringReader(xml))
                {
                    var xmldes = new XmlSerializer(typeof(T));
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {

               return default(T);
            }
        }

       public static T DeserializeFile<T>(string fileName)
       {
         return Deserialize<T>(ReadFile(fileName));
       }

       /// <summary>
       /// 序列化
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="obj"></param>
       /// <returns></returns>
       public static string Serializer<T>(T obj)
       {
           MemoryStream Stream = new MemoryStream();
           XmlSerializer xml = new XmlSerializer(typeof(T));
           try
           {
               //序列化对象
               xml.Serialize(Stream, obj);

               Stream.Position = 0;
               StreamReader sr = new StreamReader(Stream);
               string str = sr.ReadToEnd();

               sr.Dispose();
               Stream.Dispose();
               return str;
           }
           catch (Exception)
           {
               return string.Empty;
           }

       }

       public static void SerializerFile<T>(T obj,string fileName)
       {
           SaveFile(fileName,Serializer<T>(obj));
       }
       /// <summary>
       /// 保存Xml文件
       /// </summary>
       /// <param name="xmlString"></param>
       public static void SaveFileShow(string xmlString)
       {
           try
           {
              var dlg = new OpenFileDialog();
            dlg.FileName = "系统配置文件";
            dlg.DefaultExt = "*.*";
            dlg.Filter = "全部文件(.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                SaveFile(dlg.FileName, xmlString);
               
            }
              
           }
           catch
           {
            
           }
        
       }

       /// <summary>
       /// 保存Xml文件
       /// </summary>
       /// <param name="xmlString"></param>
       public static void SaveFile(string fileName,string xmlString)
       {
           try
           {

               File.WriteAllText(fileName, xmlString);
           
           }
           catch
           {

           }

       }
       /// <summary>
       /// 读取文件
       /// </summary>
       /// <param name="fileName"></param>
       /// <returns></returns>
       public static string ReadFile(string fileName)
       {
           if (File.Exists(fileName))
           {
               return File.ReadAllText(fileName);
           }
           else
           {
               return null;
           }
       }
       #endregion
    }
}
