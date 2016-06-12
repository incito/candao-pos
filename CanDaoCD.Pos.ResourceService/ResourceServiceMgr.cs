using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CanDao.Pos.ResourceServer
{
    /// <summary>
    /// 资源服务
    /// </summary>
    public static class ResourceServiceMgr
    {
        #region 构造函数
        static ResourceServiceMgr()
        {
            var resourceDictionary = new ResourceDictionary();
            resourceDictionary = Application.LoadComponent(new Uri("CanDao.Pos.ResourceServer;Component/DictionaryServer.xaml", UriKind.Relative)) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取资源Value
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static object GetValue(string Key)
        {

            return Application.Current.Resources[Key];
        }

        /// <summary>
        /// 获取Image资源
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static BitmapImage GetImageForKey(string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Key))
                {
                    return null;
                }

                object value = GetValue(Key);
                if (value != null)
                {
                    return (BitmapImage)GetValue(Key);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文字资源
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetStringForKey(string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Key))
                {
                    return string.Empty;
                }


                object value = GetValue(Key);
                if (value != null)
                {
                    return (string)GetValue(Key);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取样式资源
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static Style GetStyleForKey(string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Key))
                {
                    return null;
                }


                object value = GetValue(Key);
                if (value != null)
                {
                    return (Style)GetValue(Key);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
