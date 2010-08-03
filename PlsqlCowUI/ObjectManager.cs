using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using PlsqlParser;

namespace PlsqlParserDesktop
{
    public class ObjectManager
    {
        List<PlsqlObject> allObjects = new List<PlsqlObject>();
        string _currentPath; 
        //TODO  - make possible to select some folders which will be scanned
        //and then whole set of schemas/procs would be available from comboboxes
        //with possibility to launch NPP from correct place


        public void setPath(string path)
        {
            _currentPath = path;
        }

        public List<PlsqlObject> ParseDirectory(string directoryPath)
        {

            if (!Directory.Exists(directoryPath))
                throw new Exception("No such directory! \"" + directoryPath + "\"");

            RecurseDirectory(allObjects, directoryPath);

            return allObjects;
        }

        private void RecurseDirectory(List<PlsqlObject> plsqlObjects, string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);

            if (di.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    RecurseDirectory(plsqlObjects, dir.FullName);
                }
            }

            foreach (FileInfo file in di.GetFiles())
            {
                List<string> list = new List<string>(PlsqlConstants.plsqlAnalyzedEtensions);
                if (!list.Contains(file.Extension))
                    continue;

                IParser p = new LineBasedParser(file.FullName);

                if (p.Objects.Count > 0)
                {
                    foreach (PlsqlObject o in p.Objects)
                    {
                        plsqlObjects.Add(o); 
                    }
                }
            }
        }

    }
}
