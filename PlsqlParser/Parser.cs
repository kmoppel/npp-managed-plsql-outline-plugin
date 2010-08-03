using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace PlsqlParser
{
    public class Parser
    {
        public bool ParseAndSaveToXml(string filePath)
        {
            return true;
        }

        public ArrayList ParseFile(string filePath)
        {
            ArrayList al = new ArrayList();
            PlsqlObject o = new PlsqlObject();
            //filePath = @"e:\oracle\lrc_plsql_parser.pkb";
            filePath = @"C:\Documents and Settings\K.FS7020\My Documents\My Dropbox\PlSQLParser\TestData";
            o.name = "func";
            o.packageName = "funcPkg";
            o.type = PlsqlObjectType.PackageBody;
            o.fileName = "file.tx";
            al.Add(o);
            return al;
        }

        public LinkedList<PlsqlObject> ParseDirectory(string directoryPath)
        {
            LinkedList <PlsqlObject> allObjects = new LinkedList<PlsqlObject>();

            if (!Directory.Exists(directoryPath))
                throw new Exception("No such directory! \"" + directoryPath + "\"");

            RecurseDirectory(allObjects, directoryPath);

            return allObjects;
        }

        private void RecurseDirectory(LinkedList<PlsqlObject> plsqlObjects, string directoryPath)
        {
            ArrayList objectsFromOneFile;

            DirectoryInfo di = new DirectoryInfo(directoryPath);

            if (di.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    RecurseDirectory(plsqlObjects, dir.FullName);            		 
                }
            }

            //lets go through all files first 
            foreach (FileInfo file in di.GetFiles())
            {
                List<string> list = new List<string>(PlsqlConstants.plsqlAnalyzedEtensions);
                //we looka only at regular PlSql extensions
                if (!list.Contains(file.Extension)) 
                    continue;
                if ((objectsFromOneFile = ParseFile(file.FullName)).Count > 0)
                {
                    foreach (PlsqlObject o in objectsFromOneFile)
                    {
                        plsqlObjects.AddLast(o);
                    } 
                }
            }

        }

    }
   
}
