using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using Ionic.Zlib;
using Chilkat;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        const string STR_ZIP = ".zip";
        const string STR_RAR = ".rar";
        const string STR_YEN = "\\";

        public Form1()
        {
            InitializeComponent();
        }

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
                 * zip解凍実行関数
                 * 引数：strFromFile解凍するzipファイルのパス　stToPath 圧縮先のフォルダへのパス
                 * 戻り値：bool　true処理成功　false処理失敗
                 */
        static bool runUnZip(string strFromFile, string stToPath)
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
        static bool runUnRar(string strFromFile, string strToPath)
        {
            bool success;                           //bool型変数作っとく
            Chilkat.Rar rar = new Chilkat.Rar();    //インスタンス作る
            success = rar.Open(strFromFile);       //開く、インスタンスにファイル名渡す
            if (success != true)
            {
                return false;
            }
            success = rar.Unrar(strToPath + STR_YEN);//解凍処理
            if (success != true)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        //*********************************************************************************
        // ファイルドラッグ時のハンドラ
        //*********************************************************************************
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            string[] fileNameArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (!e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                /* ファイル以外のドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }

            if (fileNameArray.Length > 1)
            {
                /* 複数ファイルのドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }
            /* 上記以外は受け入れる */
            e.Effect = DragDropEffects.All;
        }
        //*********************************************************************************
        // ファイルドロップ時のハンドラ
        //*********************************************************************************
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNameArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string fileName = fileNameArray[0];
            const string stTestTo = @"L:\manga\";//解凍先のフォルダ名

            //拡張子を除くファイル名を取得
            string strFileNameWoEx = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string strExtension = System.IO.Path.GetExtension(fileName);//拡張子を取得
            if (strExtension == STR_ZIP)//拡張子がzipの場合
            {
                if (runUnZip(fileName, stTestTo + strFileNameWoEx))
                {
                    //解凍先フォルダを開く
                    openFolder(stTestTo + strFileNameWoEx);
                }
                else
                {
                }

            }//拡張子が.zip
            else if (strExtension == STR_RAR)//拡張子がrarの場合
            {
                if (runUnRar(fileName, stTestTo + strFileNameWoEx))
                {
                    openFolder(stTestTo + strFileNameWoEx);//解凍先フォルダを開く
                }
                else
                {
                }
            }//拡張子が.rar
            Console.Beep();

        }

    }
}
