﻿using System.IO;
using System.Text;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Delivery.VisualStudio.PackingTool
{
    public class CacheHelper<T> where T : new()
    {

        public T Get(string dir)
        {
            if (!File.Exists(dir))
            {
                throw new FileNotFoundException("cache file not found.", dir);
            }
            var obj = XmlSerializerHelper.LoadObjectFromXml<T>(dir);
            return obj;
        }

        public void Save(T project, string dir)
        {
            SaveXml(project, dir);
        }


        private void SaveXml(T cache, string dir)
        {
            var xml = XmlSerializerHelper.GetXmlStringFromObject(cache);
            FileUtil.WriteText(dir, xml, Encoding.UTF8);
        }
    }

}
