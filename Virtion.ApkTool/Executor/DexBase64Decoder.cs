using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtion.Util;


namespace Virtion.ApkTool.Executor
{
    class DexBase64Decoder
    {
        public static void Decode()
        {
            Console.WriteLine("拖入Dex文件:");
            string path = Console.ReadLine();
            string result = FileManager.ReadFile(path);
            byte[] data = Convert.FromBase64String(result);
            FileManager.WriteFile("a.dex", data);
        }

    }
}
