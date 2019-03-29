using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exclusion
{
    class Program
    {
        static void Main(string[] args)
        {
            const string STR_USAGE = "Usage:Exclusion ExclusionFilePath";
            const string STR_FILE_NOT_FOUND = "ファイルがありません。";
            string strExcFilePath = "";
            if (args.Length == 1)
            {
                strExcFilePath = args[0];
                if (File.Exists(strExcFilePath))
                {
                    Console.WriteLine(STR_FILE_NOT_FOUND);
                    return;
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine(STR_USAGE);
                return;
            }
        }//main
    }//class
}//namespace
