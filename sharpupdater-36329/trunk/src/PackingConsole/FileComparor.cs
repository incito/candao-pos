using System.IO;

namespace PackingConsole
{
    internal class FileComparor
    {
        public static bool IsSame(string file1, string file2)
        {
            int file1byte = 0;

            int file2byte = 0;


            using (FileStream fs1 = new FileStream(file1, FileMode.Open),
                fs2 = new FileStream(file2, FileMode.Open))

            {
//  检查文件大小。如果两个文件的大小并不相同,则视为不相同。

                if (fs1.Length != fs2.Length)

                {

                    fs1.Close();

                    fs2.Close();

                    return false;
                }


//  逐一比较两个文件的每一个字节,直到发现不相符或已到达文件尾端为止。

                do

                {
// 从每一个文件读取一个字节。

                    file1byte = fs1.ReadByte();

                    file2byte = fs2.ReadByte();
                } while ((file1byte == file2byte) && (file1byte != -1));

                fs1.Close();

                fs2.Close();
            }


//  返回比较的结果。在这个时候,只有当两个文件的内容完全相同时,


            return ((file1byte - file2byte) == 0);
        }
    }
}