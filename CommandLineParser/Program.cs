using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlsqlParser;

namespace CommandLineParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.test2();
        }

        void test2()
        {
            var file = @"C:\Users\Kaarel\Documents\My Dropbox\PlSQLParser\TestData\LAEN.TEST.PKB";

            IParser lbp = new LineBasedParser(file);
            
            foreach (PlsqlObject i in lbp.Objects)
            {
                Console.WriteLine(i);
            }

            //NotepadPlusPlusUtil.OpenFileOnLineNumber(lbp.Objects[0].fileName, lbp.Objects[0].lineNumberStarting);
            Console.ReadLine();
        }
    }
}
