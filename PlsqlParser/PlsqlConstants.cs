using System;
using System.Collections.Generic;
using System.Text;

namespace PlsqlParser
{
    public static class PlsqlConstants
    {
        public static  string procedure = "PROCEDURE";
        public static  string function = "FUNCTION";
        public static  string packageHeader = "PACKAGE";
        public static  string packageBody = "PACKAGE BODY";
        public static  string[] plsqlAnalyzedEtensions = { ".pkb", ".prc", ".pks", ".syn" }; //TODO should be made editable from config file

    }
}
