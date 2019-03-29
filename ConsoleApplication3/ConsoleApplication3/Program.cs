using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;
using Chilkat;
namespace ConsoleApplication1
{
    class Program
    {
        const string STR_ZIP = ".zip";
        const string STR_RAR = ".rar";
        const string STR_PART1_RAR = "part1.rar";
        const string STR_YEN = "\\";
        /*
         * フォルダを開く関数
         * 引数：strPath　開くフォルダのパス
         * 戻り値：なし
         */
        static void openFolder(string strPath)
        {
            System.Diagnostics.Process.Start(strPath);
        }
        /* 
         * 例外チェック関数 
         * 引数：strFileName 圧縮ファイルの名前
         * 戻り値：true処理する　false処理しない
         */
        static bool checkException(string strFileName)
        {
            const string STR_ZIPYARUO = "【zip】";
            const string STR_MATCH_2TO9 = @"part\d[.]rar";
            const string STR_MATCH_10TO99 = @"part\d\d[.]rar";
            string strExtension = System.IO.Path.GetExtension(strFileName);//拡張子を取得

            if (strExtension.CompareTo(STR_ZIP) == 0)//拡張子がzipの場合
            {
                if (strFileName.IndexOf(STR_ZIPYARUO) > 0)//zipでやる夫を除外
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
                if (strFileName.IndexOf(STR_PART1_RAR) > 0)//part1は解凍処理を行う
                {
                    return true;
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(strFileName, STR_MATCH_2TO9))//分割ファイル2～9を除外
                {
                    return false;
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(strFileName, STR_MATCH_10TO99))//分割ファイル10～99を除外
                {
                    return false;
                }
                else 
                {
                    return true;
                }
            }
            else//zipでもaraでもない拡張子のファイルは処理しない
            {
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
                if (fileNameWoEx.CompareTo(directryNam) == 0)//圧縮ファイル名と解凍先フォルダ名が一致する場合
                {
                    return false;//解凍処理を行わない
                }
            }
            return true;//すべてのフォルダを調べ終わってreturnしてなかったらtrueを返す
        }
        /*
         * zip解凍実行関数
         * 引数：strFromFile解凍するzipファイルのパス　stToPath 圧縮先のフォルダへのパス
         * 戻り値：bool　true処理成功　false処理失敗
         */
        static bool runUnZip(string strFromFile, string stToPath )
        {
            try
            {
                // zipファイル読み込み
                ZipFile zip = new ZipFile(strFromFile, Encoding.GetEncoding("shift_jis"));
                // 展開する。すでにあれば上書きする。
                zip.ExtractAll(stToPath, ExtractExistingFileAction.OverwriteSilently);
                // 後始末。面倒ならusingしておいてもおｋ
                zip.Dispose();
                return true;
            }
            catch (ZipException obj)
            {
                return false;
            }
        }
        /*
         * rar解凍実行関数
         * 引数：strFromFile解凍するrarファイルのパス　stToPath 圧縮先のフォルダへのパス
         * 戻り値：bool　true処理成功　false処理失敗
         */
        static bool runUnRar( string strFromFile, string strToPath ) 
        {
            bool success;                           //bool型変数作っとく
            Chilkat.Rar rar = new Chilkat.Rar();    //インスタンス作る
            success = rar.Open(strFromFile);       //開く、インスタンスにファイル名渡す
            if (success != true)
            {
                Console.WriteLine(rar.LastErrorText);   //オープン失敗でエラー表示
                return false;
            }
            success = rar.Unrar(strToPath + STR_YEN);//解凍処理
            if (success != true)
            {
                Console.WriteLine(rar.LastErrorText);//解凍失敗　エラーメッセージを表示
                return false;
            }
            else
            {
                return true;
            }

        }
        static void Main(string[] args)
        {
            const string stTestFrom = @"H:\Downloads\";//圧縮ファイルが存在する元フォルダ名
            const string stTestTo = @"L:\manga\";//解凍先のフォルダ名
            const string STR_SUCCESS = "Success.:";
            const string STR_FAILD = "Faild.:";

            string[] fromfiles = System.IO.Directory.GetFiles(
                stTestFrom, "*", System.IO.SearchOption.TopDirectoryOnly);//圧縮ファイルがある元フォルダのファイル名一覧を取得
            string[] tofiles = Directory.GetDirectories(stTestTo);//解凍先のディレクトリ一覧を取得
            for (int i = 0; i < fromfiles.Length; i++)//元フォルダ一覧でループ
            {
                //分割ファイルとzipでやる夫を除外
                if (checkException(fromfiles[i]))
                {
                    //拡張子を除くファイル名を取得
                    string strFileNameWoEx = System.IO.Path.GetFileNameWithoutExtension(fromfiles[i]);
                    if (checkExistence(strFileNameWoEx, tofiles))//解凍処理を行うかどうかの分岐
                    {
                        string strExtension = System.IO.Path.GetExtension(fromfiles[i]);//拡張子を取得
                        if (strExtension == STR_ZIP)//拡張子がzipの場合
                        {
                            //Console.WriteLine(fromfiles[i]);
                            if( runUnZip( fromfiles[i], stTestTo + strFileNameWoEx) )
                            {
                                //解凍先フォルダを開く
                                openFolder(stTestTo + strFileNameWoEx);
                                //成功したファイル名をコンソールに表示
                                Console.WriteLine(STR_SUCCESS + strFileNameWoEx);
                            }
                            else
                            {
                                //解凍失敗エラーのファイル名を表示
                                Console.WriteLine(STR_FAILD + strFileNameWoEx);
                            }

                        }//拡張子が.zip
                        else if (strExtension == STR_RAR)//拡張子がrarの場合
                        {
                            //Console.WriteLine(fromfiles[i]);
                            if ( runUnRar(fromfiles[i], stTestTo + strFileNameWoEx ) )
                            {
                                Console.WriteLine(STR_SUCCESS + strFileNameWoEx);//成功したファイルをコンソールに表示
                                openFolder(stTestTo + strFileNameWoEx);//解凍先フォルダを開く
                            }
                            else
                            {
                                Console.WriteLine(STR_FAILD + strFileNameWoEx);//解凍失敗エラーのファイル名を表示
                            }
                        }//拡張子が.rar
                    }//すでに解凍フォルダが存在するif
                }//part2やpart3などの分割ファイルを除外 zipでやる夫除外if
            }//元ファイルのループ
            Console.Beep();
        }//main
    }//class 
}//namespace 