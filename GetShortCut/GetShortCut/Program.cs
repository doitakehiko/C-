using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetShortCut
{
    class Program
    {
        static void Main(string[] args)
        {
            const string STR_LNK = ".lnk";
            const string STR_DIR_NOT_FOUND = "ディレクトリがありません。";
            const string STR_USAGE = "Usage:GetShotCut DirectoryPath";
            const string STR_BORKEN_LINK = "Broken link:";
            const string STR_TMP_DIR = "L:\\tmp\\";
            const string STR_REPLACE = ":\\";
            const string STR_ECHO_FILE = "echo F | xcopy \"";
            const string STR_ECHO_DIR = "echo D | XCOPY /H /K /S /E \"";
            const string STR_SPACE = "\" \"";
            const string STR_END = "\"";
            string strSCF = "";
            if (args.Length == 1)
            {
                strSCF = args[0];
                if (!Directory.Exists(strSCF))
                {
                    Console.WriteLine(STR_DIR_NOT_FOUND);
                    return;
                }
            } else if ( args.Length == 0) 
            {
                Console.WriteLine(STR_USAGE);
                return;
            }
            string[] fromfiles = System.IO.Directory.GetFiles(strSCF, "*", System.IO.SearchOption.TopDirectoryOnly);
            string str_fname = "";
            string strPath = "";
            string strTmpPath = "";
            for (int i = 0; i < fromfiles.Length; i++)//元フォルダ一覧でループ
            {
                str_fname = System.IO.Path.GetFullPath(fromfiles[i]);
                string strExtension = System.IO.Path.GetExtension(str_fname);//拡張子を取得
                if (strExtension == STR_LNK)//拡張子がlnkの場合
                {
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(str_fname);
                    strPath = shortcut.TargetPath.ToString();//ショートカットのターケゲットパスを取得
                    strTmpPath = STR_TMP_DIR + strPath.Replace(STR_REPLACE, "");
                    if (!File.Exists(strPath) && !Directory.Exists(strPath))//ファイルの存在チェック
                    {
                        Console.WriteLine( STR_BORKEN_LINK + strPath);//リンク切れを出力
                    }
                    else
                    {
                        if (File.Exists(strPath))
                        {
                            Console.WriteLine(STR_ECHO_FILE + strPath + STR_SPACE + strTmpPath + STR_END);
                        }
                        else if (Directory.Exists(strPath))
                        {
                            Console.WriteLine(STR_ECHO_DIR + strPath + STR_SPACE + strTmpPath + STR_END);
                        }
                        
                    }

                    //後始末
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
                }
            }
        }
    }
}