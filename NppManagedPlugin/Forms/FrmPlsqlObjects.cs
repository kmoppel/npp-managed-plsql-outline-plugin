using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using PlsqlParser;

namespace NppPluginNET
{
    partial class FrmPlsqlObjects : Form
    {
        PluginBase pluginBase;
        private List<PlsqlObject> _plSqlObjects;
        private bool _isAlphabeticallySorted = false;

        public FrmPlsqlObjects(PluginBase plgBase)
        {
            pluginBase = plgBase;
            InitializeComponent();
        }

        
        void FrmPlsqlObjectsVisibleChanged(object sender, EventArgs e)
        {
        	if ( ! Visible)
        	{
                Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
        		                  pluginBase._funcItems.Items[pluginBase.menuItemId]._cmdID, 0);
        	}
        }

        public void ParseFile(string filename)
        {
            
            Thread worker = new Thread(new ParameterizedThreadStart(ParseThreadFunc));
            try
            {
                worker.Start(filename);
                worker.Join(5000); // lets give our plugin a max of 5 seconds for parsing
                FillListView();
            }
            catch(Exception e)
            {
                DisplayUserMessage(e.Message);
            }
        }

        private void ParseThreadFunc(object filename)

        {
            IParser parser = new LineBasedParser(filename.ToString());
            _plSqlObjects = parser.Objects;            
        }

        public void FillListView()
        {
            ListViewItem item;

            listView1.Items.Clear(); //needed because of a-z button

            List<PlsqlObject> plSqlObjectsToDisplay;

            if ( _isAlphabeticallySorted )
            {

                plSqlObjectsToDisplay = new List<PlsqlObject>(_plSqlObjects);
                plSqlObjectsToDisplay.Sort();
            }
            else
            {
                plSqlObjectsToDisplay = _plSqlObjects;
            }

            foreach (PlsqlObject o in plSqlObjectsToDisplay)
            {
                item = new ListViewItem(o.name.ToLower());
                item.SubItems.Add(o.lineNumberStarting.ToString());
                item.ToolTipText = o.parameters;
                listView1.Items.Add(item);
            }

            if (listView1.Items.Count > 0)
                listView1.Items[0].Selected = true;
        }

        public void DisplayUserMessage(string msg)
        {
            var item = new ListViewItem(msg);
            listView1.Items.Add(item);            
        }


        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var listView = sender as ListView;
            int line;
            var success = Int32.TryParse(listView.SelectedItems[0].SubItems[1].Text, out line);
            if (! success )
                return;                

            IntPtr curScintilla = pluginBase.GetCurrentScintilla();
            Win32.SendMessage(curScintilla, SciMsg.SCI_ENSUREVISIBLE, line - 1, 0);
            Win32.SendMessage(curScintilla, SciMsg.SCI_GOTOLINE, line - 1, 0);
            Win32.SendMessage(curScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
        }

        private void bnSortOrder_Click(object sender, EventArgs e)
        {
            _isAlphabeticallySorted = !_isAlphabeticallySorted;
            FillListView();
        }

    }
}
