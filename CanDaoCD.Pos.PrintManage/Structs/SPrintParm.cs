using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CanDaoCD.Pos.PrintManage.Structs
{
    public struct StatuRes
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] ResBytes;
    }
}
