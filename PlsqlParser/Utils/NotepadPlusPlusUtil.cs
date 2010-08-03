using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PlsqlParser
{
    public class NotepadPlusPlusUtil
    {
        public static void OpenFileOnLineNumber(string filePath, int lineNumber)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"c:\program files\notepad++\notepad++.exe"; //TODO make configurable
            psi.Arguments = "-n" + lineNumber +
                    " \"" + filePath.Replace(@"\\", @"\")  + "\"";
            
            Process.Start(psi);
        }
    }
}
