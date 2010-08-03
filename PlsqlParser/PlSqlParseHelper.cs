using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace PlsqlParser
{
    public static class PlSqlParseHelper
    {
        public static bool isSingleLineComment(this string line)
        {
            bool b;
            //b = RegexUtil.IsMatch(line, @"\s* -- l
            b = RegexUtil.IsMatch(line, @"^\S{0}--");
            
            if (b)
                return true;
            return RegexUtil.IsMatch(line, @"\s* /\* .* \*/ \s*");
        }

        public static bool isStartingMultilineComment(this string line)
        {
            return RegexUtil.IsMatch(line, @"\s* /\* .*");
        }

        public static bool isEndingMultilineComment(this string line)
        {
            return RegexUtil.IsMatch(line, @".* \*/ .*");
        }

        public static bool isPlSqlObjectDeclaration(this string line)
        {
            //TODO returns true for following line
              //END CalcPaymentDayCoeff;   -- FUNCTION CalcPaymentDayCoeff
            var r = @"\b(procedure|function)\b \s+ \b(\w+)\b";
            return RegexUtil.IsMatch(line, r);
        }

        public static bool isEmpty(this string line)
        {
            if (line == string.Empty)
                return true;
            else
                return false;
        }
    }
}
