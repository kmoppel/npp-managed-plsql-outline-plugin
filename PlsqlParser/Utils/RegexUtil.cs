using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PlsqlParser
{
    public static class RegexUtil
    {

        public static bool IsMatch(string stringToMatch, string pattern)
        {
            RegexOptions ro = RegexOptions.IgnorePatternWhitespace 
                            | RegexOptions.IgnoreCase ;
            return Regex.IsMatch(stringToMatch, pattern, ro);
        }

    }
}
