using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void printMoveTailToHead(string line)
        {
            string[] stArrayData = line.Split(' ');//スペースで区切って配列に入れる
            string[] stArrayResult = new string[stArrayData.Length];//結果の配列を生成
            stArrayResult[0] = stArrayData[stArrayData.Length - 1];//配列の最後を結果の先頭に入れる
            for (int i = 1; i < stArrayData.Length; i++)//結果の配列の2番めから始める
            {
                stArrayResult[i] = stArrayData[i - 1];//最後の要素を先頭にして、残りはそのまま
            }
            foreach (string stData in stArrayResult)
            {
                Console.Write(stData + " ");//区切りとしてスーペースを出力
            }
            Console.WriteLine("");//改行を出力
            return;
        }
        static void Main(string[] args)
        {
            string strPath = "";
            const string strEncod = "shift_jis";
            if (args.Length == 1)
            {
                strPath = args[0];
                if (!File.Exists(strPath))
                {
                    string strFileExit = strPath + "がありません。";
                    Console.WriteLine(strFileExit);
                    return;
                }
                System.IO.StreamReader sr = new System.IO.StreamReader(strPath, System.Text.Encoding.GetEncoding(strEncod));
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    printMoveTailToHead(line);
                }
            }
            else
            {
                if (args.Length == 0)
                {
                    TextReader input = Console.In;
                    string line = "";
                    while ((line = input.ReadLine()) != null)
                    {
                        printMoveTailToHead(line);
                    }
                    input.Dispose();
                }
            }
        }
    }
}
