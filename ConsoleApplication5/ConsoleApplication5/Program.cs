using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        const string STR_ZIP = ".zip";
        const string STR_RAR = ".rar";
        const string STR_CRDOWN = ".crdownload";
        const string STR_YEN = "\\";
        /* 
         * 例外チェック関数 
         * 引数：strFileName 圧縮ファイルの名前
         * 戻り値：true処理する　false処理しない
         */
        static bool checkException(string strFileName)
        {
            const string STR_ZIPYARUO = "【zip】";
            string strExtension = System.IO.Path.GetExtension(strFileName);//拡張子を取得

            if (strExtension.CompareTo(STR_ZIP) == 0)//拡張子がzipの場合
            {
                if (strFileName.IndexOf(STR_ZIPYARUO) > 0)//zipでやる夫をを処理する
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (strExtension.CompareTo(STR_RAR) == 0)//拡張子がrarの場合
            {
                return false;
            }
            else if (strExtension.CompareTo(STR_CRDOWN) == 0)//拡張子がcrdownloadの場合
            {
                return false;
            }
            else//zipでもaraでもない拡張子のファイルは処理する
            {
                return true;
            }
        }

        /*
          存在チェック関数
         * 引数：fileName　ファイル名　
         * 戻り値：bool　true処理する　false処理しない
         */
        static bool checkExistence(string fileName, string[] arrToFiles)
        {
            string fromFile = fileName.Substring(fileName.LastIndexOf(STR_YEN) + 1);//ファイル名取得
            for (int n = 0; n < arrToFiles.Length; n++)//解凍先のフォルダ名をファイル名で探すためのループ
            {
                string toFile = arrToFiles[n].Substring(arrToFiles[n].LastIndexOf(STR_YEN) + 1);//ファイル名取得
                if (fromFile.CompareTo(toFile ) == 0)//ファイル名が一致する場合
                {
                    return false;//解凍処理を行わない
                }
            }
            return true;//すべてのフォルダを調べ終わってreturnしてなかったらtrueを返す
        }
        static void Main(string[] args)
        {
            const string stTestFrom = @"H:\Downloads\";//元フォルダ
            const string stTestTo = @"L:\Downloads\";//バックアップ先のフォルダ
            //UTF-16 1LE, BOM無しを指定する
            Console.OutputEncoding = new UnicodeEncoding();

            string[] fromfiles = System.IO.Directory.GetFiles(
                stTestFrom, "*", System.IO.SearchOption.TopDirectoryOnly);//元フォルダのファイル名一覧を取得
            string[] tofiles = System.IO.Directory.GetFiles(
                stTestTo, "*", System.IO.SearchOption.TopDirectoryOnly);//バックアップ先のファイル名一覧を取得
            for (int i = 0; i < fromfiles.Length; i++)//元フォルダ一覧でループ
            {
                //zipでやる夫を除くzipとrarを除く
                if (checkException(fromfiles[i]))
                {
                    if (checkExistence(fromfiles[i], tofiles))//コピー処理を行うかどうかの分岐
                    {
                        string fileName = fromfiles[i].Substring(fromfiles[i].LastIndexOf(STR_YEN) + 1);//ファイル名取得
                        System.IO.File.Copy(fromfiles[i], stTestTo + fileName, true);
                        Console.WriteLine(fileName);

                    }
                }
            }//元ファイルのループ
        }//main
    }
}
