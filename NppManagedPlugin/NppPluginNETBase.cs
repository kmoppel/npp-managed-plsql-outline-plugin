using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlsqlParser;

namespace NppPluginNET
{
    public partial class PluginBase
    {
        #region " Fields "
        public string _pluginBaseName = null;
        public string _pluginModuleName = null;
        public NppData nppData;
        public FuncItems _funcItems = new FuncItems();

        //appspecific
        private bool _readySignalReceived = false;
        private string _currentFile;

        #endregion

        #region " Notepad++ callbacks "
        public PluginBase(string pluginBaseName)
        {
            _pluginBaseName = pluginBaseName;
            _pluginModuleName = pluginBaseName + ".dll";
        }
        public bool __isUnicode()
        {
            return true;
        }
        public void __setInfo(NppData notpadPlusData)
        {
            nppData = notpadPlusData;
            CommandMenuInit();
        }
        public IntPtr __getFuncsArray(ref int nbF)
        {
            nbF = _funcItems.Items.Count;
            return _funcItems.NativePointer;
        }
        public uint __messageProc(uint Message, uint wParam, uint lParam)
        {
            return 1;
        }
        public string __getName()
        {
            return _pluginBaseName;
        }
        public void __beNotified(SCNotification notifyCode)
        {
            if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                PluginCleanUp();
            }
            else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                SetToolBarIcon();
            }
            //else if (notifyCode.nmhdr.code == (uint)SciMsg.SCN_CHARADDED)
            //{
            //    doInsertHtmlCloseTag((char)notifyCode.ch);
            //}
            else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_READY)
            {
                _readySignalReceived = true;

                _currentFile = GetOpenFileFullPath();
                SendParseMessage(_currentFile);
                //logToText("_readySignalReceived");
            }
            else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_BUFFERACTIVATED)
            {
                if (!_readySignalReceived)
                    return;
                //logToText("NPPN_BUFFERACTIVATED");
                string bufferFile = GetOpenFileFullPath();
                if (_currentFile != GetOpenFileFullPath())
                {
                    _currentFile = bufferFile;
                    SendParseMessage(_currentFile);
                }

                //StringBuilder path = new StringBuilder(Win32.MAX_PATH);
                //Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFILENAME, 0, path);

            }
            else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_FILEOPENED)
            {
                if (_frmPlsqlObjects == null || !_frmPlsqlObjects.Visible)
                    return;
                if (!_readySignalReceived)
                    return;

                _currentFile = GetOpenFileFullPath();

                SendParseMessage(_currentFile);
            }
        }

        private void SendParseMessage(string fileToParse)
        {
            //first clear procs from previous file 
            _frmPlsqlObjects.listView1.Items.Clear();

            //start parsing only if file extension is typical to Plsql
            List<string> list = new List<string>(PlsqlConstants.plsqlAnalyzedEtensions);
            FileInfo fileInfo = new FileInfo(fileToParse);
            if (string.Empty == fileInfo.Extension ||
                    ! list.Contains(fileInfo.Extension.ToLower()))
            {
                _frmPlsqlObjects.DisplayUserMessage("Not supported file type");
                return; 
            }

            try
            {
                _frmPlsqlObjects.ParseFile(fileToParse);
            }
            catch (Exception e)
            {
                logToText(e.Message);
            }
        }

        private string GetOpenFileFullPath()
        {
            var path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);
            return path.ToString();
        }

        #endregion

        #region " Helper "
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        public IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
        }

        public void logToText(string msg)
        {
            using (StreamWriter writer = new StreamWriter("plsqlparserlog.txt", true))
            {
                writer.WriteLine(msg);
            }
        }
        #endregion
    }
}
