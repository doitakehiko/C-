using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string stTestFrom = @"F:\Downloads\";//圧縮ファイルが存在する元フォルダ名
            string stTestTo = @"D:\manga\";//解凍先のフォルダ名
            string[] fromfiles = System.IO.Directory.GetFiles(
                stTestFrom, "*", System.IO.SearchOption.TopDirectoryOnly);//圧縮ファイルがある元フォルダのファイル名一覧を取得
            string[] tofiles = Directory.GetDirectories(stTestTo);//解凍先のディレクトリ一覧を取得
            for (int i = 0; i < tofiles.Length; i++)
            {
                bool brkFlg = true;//このフラグがtrueの時、圧縮を実行する
                string directryNam = tofiles[i].Substring(tofiles[i].LastIndexOf("\\") + 1);//フォルダ名取得
                for (int n = 0; n < fromfiles.Length; n++) 
                {
                    //拡張子を除くファイル名を取得
                    string fileNameWoEx = System.IO.Path.GetFileNameWithoutExtension(fromfiles[n]);
                    //末尾のドットとスペースを消す。フォルダ名の末尾にドットとスペースがあると消されるため
                    while (fileNameWoEx.Substring(fileNameWoEx.Length - 1).CompareTo(".") == 0 | fileNameWoEx.Substring(fileNameWoEx.Length - 1).CompareTo(" ") == 0)
                    {
                        fileNameWoEx = fileNameWoEx.Remove(fileNameWoEx.Length - 1);
                    }
                    if (fileNameWoEx.CompareTo(directryNam) == 0)//圧縮ファイル名と解凍先フォルダ名が一致する場合
                    {
                        brkFlg = false;//解凍処理を行わない
                        break;
                    }
                }
                if (brkFlg)//解凍処理を行うかどうかの分岐
                {
                    Console.WriteLine( "rmdir " + tofiles[i]);
                }

            }
     
        }
    }
}
