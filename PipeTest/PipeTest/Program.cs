using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PipeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TextReader input;

            if (args.Length == 0) {
                // 読み込み元は標準入力
                input = Console.In;
            } else {
                // 読み込み元はファイル
                input = new StreamReader(args[0],
                System.Text.Encoding.GetEncoding("Shift_JIS"));
            }
            CatNum(input);
            input.Dispose();
        }

        static void CatNum(TextReader tr) {
            string line;
            int num = 1;

            while ((line = tr.ReadLine()) != null)
            {
                // {0, 6}は右詰め6けたの指定
                Console.WriteLine("{0, 6} : {1}", num, line);
                num++;
            }
        }
    }
}
