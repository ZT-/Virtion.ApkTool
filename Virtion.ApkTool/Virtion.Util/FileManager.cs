using System;
using System.Text;
using System.IO;
using System.Windows;
//using System.Drawing;

namespace Virtion.Util
{
    class FileManager
    {

        //public static Bitmap ReadImageFile(string path)
        //{
        //    FileStream fs = File.OpenRead(path); //OpenRead
        //    int filelength = 0;
        //    filelength = (int)fs.Length; //获得文件长度 
        //    Byte[] image = new Byte[filelength]; //建立一个字节数组 
        //    fs.Read(image, 0, filelength); //按字节流读取 
        //    System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
        //    fs.Close();
        //    Bitmap bit = new Bitmap(result);
        //    return bit;
        //}


        public static byte[] ReadFileByte(String path)
        {
            if (File.Exists(path) == false)
            {
                MessageBox.Show("File is not found! " + path);
                return null;
            }

            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            fs.Close();
            return image;
        }

        public static String ReadFile(String path)
        {
            if (File.Exists(path) == false)
            {
                MessageBox.Show("File is not found! " + path);
                return null;
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Console.WriteLine(line.ToString());
                stringBuilder.Append(line);
            }
            return stringBuilder.ToString();
        }

        public static void WriteFile(String path, String data)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            //开始写入
            sw.Write(data);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static void WriteFile(String path, byte[] data)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(data);
            bw.Flush();
            bw.Close();
            fs.Close();
        }

    }

}
