using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Models;

namespace CanDaoCD.Pos.Common.PublicValues
{
   public class PvSystemConfig
   {
       static PvSystemConfig()
       {
           VSystemConfig = new MSystemConfig();
           VSystemConfigFile = AppDomain.CurrentDomain.BaseDirectory + @"config\SystemConfig.Data";
       }

       /// <summary>
       /// 系统配置值
       /// </summary>
       public static MSystemConfig VSystemConfig;
       /// <summary>
       /// 系统配置文件地址
       /// </summary>
       public static string VSystemConfigFile;
   }
}
