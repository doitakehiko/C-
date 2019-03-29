using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ConsoleApplication1
{
    class Program
    {
        //がソースファイルの一番上に書かれているものとする

        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        /// <param name="newerOnly">新しいファイルのみコピーする</param>
        /// <param name="sync">sourceDirNameにないファイルを削除する</param>
        public static void CopyDirectory(
            string sourceDirName,
            string destDirName,
            bool newerOnly,
            bool sync)
        {
            //コピー先のディレクトリがないときは作る
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                //属性もコピー
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
            {
                destDirName = destDirName + Path.DirectorySeparatorChar;
            }

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string f in files)
            {
                string destFileName = destDirName + Path.GetFileName(f);
                //コピー先にファイルが存在し、
                //コピー元より更新日時が古い時はコピーする
                if (!newerOnly ||
                    !File.Exists(destFileName) ||
                    File.GetLastWriteTime(destFileName) < File.GetLastWriteTime(f))
                {
                    File.Copy(f, destFileName, true);
                }
            }

            //コピー先にあってコピー元にないファイルを削除
            if (sync)
            {
                DeleteNotExistFiles(sourceDirName, destDirName);
            }

            //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir), newerOnly, sync);
            }
        }

        /// <summary>
        /// destDirNameにありsourceDirNameにないファイルを削除する
        /// </summary>
        /// <param name="sourceDirName">比較先のフォルダ</param>
        /// <param name="destDirName">比較もとのフォルダ</param>
        private static void DeleteNotExistFiles(
            string sourceDirName,
            string destDirName)
        {
            //sourceDirNameの末尾に"\"をつける
            if (sourceDirName[sourceDirName.Length - 1] != Path.DirectorySeparatorChar)
            {
                sourceDirName = sourceDirName + Path.DirectorySeparatorChar;
            }

            //destDirNameにありsourceDirNameにないファイルを削除する
            string[] files = Directory.GetFiles(destDirName);
            foreach (string f in files)
            {
                if (!File.Exists(sourceDirName + Path.GetFileName(f)))
                {
                    File.Delete(f);
                }
            }

            //destDirNameにありsourceDirNameにないフォルダを削除する
            string[] folders = Directory.GetDirectories(destDirName);
            foreach (string folder in folders)
            {
                if (!Directory.Exists(sourceDirName + Path.GetFileName(folder)))
                {
                    Directory.Delete(folder, true);
                }
            }
        }
        static void Main(string[] args)
        {
            CopyDirectory("D:\\", "G:\\", true, true);
        }
    }
}
