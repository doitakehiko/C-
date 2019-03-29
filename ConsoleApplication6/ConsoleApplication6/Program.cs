using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ConsoleApplication1
{
    class Program
    {
        const string stTestFrom = @"F:\Downloads\";//圧縮ファイルが存在する元フォルダ名
        const string stTestTo = @"D:\manga\";//解凍先のフォルダ名
        const string STR_ZIP = ".zip";
        const string STR_RAR = ".rar";
        const string STR_PART1_RAR = "part1.rar";
        const string STR_YEN = "\\";
        const string STR_MATCH_2TO9 = @"part\d";
        const string STR_MATCH_10TO99 = @"part\d\d";
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
                if (strFileName.IndexOf(STR_ZIPYARUO) > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (strExtension.CompareTo(STR_RAR) == 0)//拡張子がrarの場合
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static bool IsEmptyDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                // ディレクトリが存在しなければ空でないとする
                return false;
            }
            try
            {
                string[] entries = Directory.GetFileSystemEntries(dir);
                return entries.Length == 0;
            }
            catch
            {
                // アクセス権がないなどの場合は空でないとする
                return false;
            }
        }
        /*
          存在チェック関数
         * 引数：fileNameWoEx　圧縮ファイルの拡張子をとったファイル名　
         * 戻り値：bool　true処理する　false処理しない
         */
        static bool checkExistence(string fileNameWoEx, string[] arrToFiles)
        {
            const string STR_DOT = ".";
            const string STR_SPACE = " ";
            //末尾のドットとスペースを消す。フォルダ名の末尾にドットとスペースがあると消されるため
            while (fileNameWoEx.Substring(fileNameWoEx.Length - 1).CompareTo(STR_DOT) == 0 | fileNameWoEx.Substring(fileNameWoEx.Length - 1).CompareTo(STR_SPACE) == 0)
            {
                fileNameWoEx = fileNameWoEx.Remove(fileNameWoEx.Length - 1);
            }
            for (int n = 0; n < arrToFiles.Length; n++)//解凍先のフォルダ名をファイル名で探すためのループ
            {
                string directryNam = arrToFiles[n].Substring(arrToFiles[n].LastIndexOf(STR_YEN) + 1);//フォルダ名取得
                if (System.Text.RegularExpressions.Regex.IsMatch(fileNameWoEx, STR_MATCH_2TO9))
                {
                    string strFileName = fileNameWoEx.Remove(fileNameWoEx.IndexOf("part") );
                    strFileName = strFileName + "part1";
                    if (strFileName.CompareTo(directryNam) == 0)//圧縮ファイル名と解凍先フォルダ名が一致する場合
                    {
                        if (IsEmptyDirectory(stTestTo + strFileName))
                            return true;
                        else
                            return false;
                    }
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(fileNameWoEx, STR_MATCH_10TO99))
                {
                    string strFileName = fileNameWoEx.Remove(fileNameWoEx.IndexOf("part"));
                    strFileName = strFileName + "part1";
                    if (strFileName.CompareTo(directryNam) == 0)//圧縮ファイル名と解凍先フォルダ名が一致する場合
                    {
                        if (IsEmptyDirectory(stTestTo + strFileName))
                            return true;
                        else
                            return false;
                    }
                }
                if (fileNameWoEx.CompareTo(directryNam) == 0)//圧縮ファイル名と解凍先フォルダ名が一致する場合
                {
                    if (IsEmptyDirectory(stTestTo + directryNam))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        static void Main(string[] args)
        {
            string[] fromfiles = System.IO.Directory.GetFiles(
                stTestFrom, "*", System.IO.SearchOption.TopDirectoryOnly);//圧縮ファイルがある元フォルダのファイル名一覧を取得
            string[] tofiles = Directory.GetDirectories(stTestTo);//解凍先のディレクトリ一覧を取得
            for (int i = 0; i < fromfiles.Length; i++)//元フォルダ一覧でループ
            {
                if (checkException(fromfiles[i]))
                {
                    //拡張子を除くファイル名を取得
                    string strFileNameWoEx = System.IO.Path.GetFileNameWithoutExtension(fromfiles[i]);
                    if (!checkExistence(strFileNameWoEx, tofiles))
                    {
                        Console.WriteLine(fromfiles[i]);
                    }//すでに解凍フォルダが存在するif
                }//part2やpart3などの分割ファイルを除外 zipでやる夫除外if
            }//元ファイルのループ
        }//main
    }//class 
}//namespace 