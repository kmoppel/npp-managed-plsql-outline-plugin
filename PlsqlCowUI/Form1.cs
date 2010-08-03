using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlsqlParser;

namespace PlsqlCowUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {            
            var file = @"C:\Documents and Settings\K.FS7020\My Documents\My Dropbox\"
                + @"PlSQLParser\TestData\GL_LOAN.TEST.PKB";
            IParser lbp = new LineBasedParser(file);

            foreach (PlsqlObject i in lbp.Objects)
            {
                ListViewItem item = new ListViewItem(i.type.ToString().Substring(0, 1));
                //item.SubItems.Add(i.type.ToString().Substring(0, 1));
                item.SubItems.Add(i.name);
                item.SubItems.Add(i.fileName);
                item.SubItems.Add(i.lineNumberStarting.ToString());
                listView1.Items.Add(item);
            }

            //NotepadPlusPlusUtil.OpenFileOnLineNumber(lbp.Objects[0].fileName, lbp.Objects[0].lineNumber);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var o = sender as ListView;
            var file = o.SelectedItems[0].SubItems[2].Text;
            var line = o.SelectedItems[0].SubItems[3].Text;
            NotepadPlusPlusUtil.OpenFileOnLineNumber(file, Int32.Parse(line));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
                textBox1.Text = fbd.SelectedPath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

    }
}
