using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            long total = 0;
            string movedir = "D:\\backup\\";
            string[] files = System.IO.Directory.GetFiles(
                @"D:\Downloads", "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(files[i]);
                //ファイルのサイズを取得
                long filesize = fi.Length;
                total = total + filesize;
                if (total <= 25000000000)
                {
                    System.IO.File.Move(files[i], movedir + System.IO.Path.GetFileName(files[i]));
                    //Console.WriteLine(movedir + System.IO.Path.GetFileName(files[i]));
                }
            }
        }
    }
}
