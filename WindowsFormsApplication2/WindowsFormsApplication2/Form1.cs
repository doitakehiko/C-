using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {//ドロップされたデータがstring型か調べる
            e.Effect = DragDropEffects.All;
        }
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            string strFileName = "";
            string strPath = "";
            const string STR_YEN = "\\";
            for (i = 0; i < s.Length; i++)
            {
                strFileName = System.IO.Path.GetFileName(s[i]);
                strFileName = strFileName.Remove(0, 1);
                strPath = s[i].Substring(0, s[i].LastIndexOf(STR_YEN) + 1);//フォルダ名取得
                System.IO.File.Move(s[i], strPath + strFileName);
                textBox1.Text += strPath + strFileName + "\n";
            }
        }
    }
}
