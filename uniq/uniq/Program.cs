using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace uniq
{
    class Program
    {
        static void Main(string[] args)
        {
            const string STR_USAGE = "Usage:uniq OriginalFilePath UpdateFilePath";
            const string STR_FILE_NOT_FOUND = " is not found.";
            string strOriginalPath = "";
            string strUpdatePath = "";
            if (args.Length == 2)
            {
                strOriginalPath = args[0];
                strUpdatePath = args[1];
                if (!File.Exists(strOriginalPath))
                {
                    Console.WriteLine( strOriginalPath + STR_FILE_NOT_FOUND);
                    return;
                }
                if (!File.Exists(strUpdatePath))
                {
                    Console.WriteLine(strUpdatePath + STR_FILE_NOT_FOUND);
                    return;
                }

            }
            else if (args.Length <= 1)
            {
                Console.WriteLine(STR_USAGE);
                return;
            }

            // StreamReader の新しいインスタンスを生成する
            System.IO.StreamReader originalReader = (
                new System.IO.StreamReader(strOriginalPath, System.Text.Encoding.Default)
            );

            System.IO.StreamReader updateReader = (
                new System.IO.StreamReader(strUpdatePath, System.Text.Encoding.Default)
            );

            // ファイルの最後まで読み込む
            string strOriginalBuff = originalReader.ReadToEnd();

            // 読み込みできる文字がなくなるまで繰り返す
            while (updateReader.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む  
                string stBuffer = updateReader.ReadLine();
                if (!strOriginalBuff.Contains(stBuffer))
                {
                    Console.WriteLine(stBuffer);
                }

            }

            // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
            originalReader.Close();
            updateReader.Close();

        }//main
    }//class
}//namespace
