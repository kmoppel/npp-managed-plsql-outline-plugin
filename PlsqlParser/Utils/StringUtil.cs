using System;
using System.Collections.Generic;
using System.Text;

namespace PlsqlParser
{
    public class StringUtil
    {
        public static string parseSchemaNameFromFilename(string fileToParse)
        {
            var lastSlash = fileToParse.LastIndexOf("\\");
            var schemaComma = fileToParse.IndexOf(".", lastSlash);
            return fileToParse.Substring(lastSlash + 1, schemaComma - lastSlash - 1);
        }

        
        //    payment_day          IN       VARCHAR2
        // >>>>
        //payment_day IN varchar2

        public static string cleanParameterDeclarationLine(string parameterLine)
        {
            var ret = new StringBuilder();
            string [] capitalized_keywords = {"in","out","inout","nocopy"};
            var cap_list = new List<string>(capitalized_keywords);

            int i = parameterLine.IndexOf("--");
            if (i == -1)
                i = parameterLine.IndexOf("/*");
            if (i >= 0)
                parameterLine = parameterLine.Substring(0,i);

            var tokens = parameterLine.ToLower().Trim(',').Split(new char[] {' ', '\t', '\n'}); 
            
            foreach (string s in tokens)
            {
                if (s.isEmpty())
                    continue;
                if (cap_list.Contains(s))
                    ret.Append(s.ToUpper() + " ");
                else
                    ret.Append(s + " ");
            }
            return ret.ToString().TrimEnd();
        }
    }
}
