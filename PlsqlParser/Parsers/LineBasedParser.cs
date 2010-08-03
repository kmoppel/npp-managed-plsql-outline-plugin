using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace PlsqlParser
{

    public class LineBasedParser : IParser
    {
        public List<PlsqlObject> objects = new List<PlsqlObject>();

        private string[] _lines;
        private int _counter = 0;
        private int _marker;
        private string _fileToParse;
        private string _schemaName;

        public LineBasedParser(string fileToParse)
        {
            string line;
            List<string> source = new List<string>();

            _fileToParse = fileToParse;
            _schemaName = StringUtil.parseSchemaNameFromFilename(fileToParse);

            using (StreamReader sr = new StreamReader(fileToParse))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    source.Add(line);
                }
            }
            _lines = source.ToArray();

                
                while ((line = Next()) != null)
                {
                    if (line.isEmpty())
                        continue;
                    if (line.isSingleLineComment())
                        continue;
                    if (line.isStartingMultilineComment())
                        skipCommentedCode();
                    if (line.isPlSqlObjectDeclaration())
                        objects.Add(extractObject(line));
                    
                }
        }

        void skipCommentedCode()
        {
            string line;

            while ((line = Next()) != null)
            {
                if (line.isEndingMultilineComment() == false)
                    continue;
                else
                    goto endloop;
            }
            endloop:
            return;
        }

        //doesnt move current line counter
        int skipCommentedCode(int startingLine) 
        {
            string line;
            int lineCounter = startingLine;

            while ((line = GetLine(lineCounter++)) != null)
            {
                if (line.isEndingMultilineComment() == false)
                    continue;
                else
                    goto endloop;
            }
            endloop:
            return lineCounter;
        }


        PlsqlObject extractObject(string startingline)
        {
            PlsqlObject o = new PlsqlObject();

            var j = @"\b(?<type>(procedure|function))\b \s+ \b(?<name>\w+)\b";
            MatchCollection m = Regex.Matches(startingline, j, RegexOptions.IgnoreCase |RegexOptions.IgnorePatternWhitespace);
            o.name = m[0].Groups["name"].Value;
            //\s*(procedure)\s*(\w)? \(?.* 
            //    .*(\bprocedure\b)\s*(\b\w*\b).*
            

            //o.name = startingline.Substring(i + 1);
            //o.name = m[1].Result();
            var t = m[0].Groups["type"].Value;
            if (t.ToUpper() == "PROCEDURE")
                o.type = PlsqlObjectType.Procedure;
            else if (t.ToUpper() == "FUNCTION")
                o.type = PlsqlObjectType.Function;
            else
                throw new Exception("Unknow PlsqlObjectType");
            o.lineNumberStarting = _counter + 1;
            o.fileName = _fileToParse;
            o.schemaName = _schemaName;
            o.parameters = GetParametersFromUnitDeclaration(_counter + 1);
            return o;
        }

        public List<PlsqlObject> Objects
        {
            get { return objects; }
        }

        string Next()
        {
                if (_counter + 1 < _lines.Length)
                    return _lines[++_counter];
                else return null;
        }


        string Prev()
        {
                if (_counter >= 1)
                {
                    return _lines[--_counter];
                }
                else
                    return null;
        }

        string GetLine(int lineNumber)
        {
            if (lineNumber <= _lines.Length - 1)
                return _lines[lineNumber];
            else return null;
        }

         void SetMarker()
        {
            _marker = _counter;
        }

         void RestoreMarker()
        {
            _counter = _marker;
        }

        string GetParametersFromUnitDeclaration(int startingLine)
        {
            var sb = new StringBuilder(512);
            string line;
            int lineCounter = startingLine;
            int i, j;

            //System.Diagnostics.Debug.WriteLine("startingLine:" + startingLine);

            while ((line = GetLine(lineCounter++)) != null)
            {

                if (line.isEmpty())
                        continue;
                    if (line.isSingleLineComment())
                        continue;
                    if (line.isStartingMultilineComment())
                    {
                        lineCounter = skipCommentedCode(lineCounter) + 1;
                        continue;
                    }


                i = line.IndexOf("(");
                j = line.IndexOf(")");

                //parameters on same line
                if (i > 0 && j > 0)
                {
                    if (sb.Length == 0)
                        return StringUtil.cleanParameterDeclarationLine(line.Substring(i + 2, j - i - 1));
                }
                //multiline parameter block started
                else if (i > 0 && j == -1)
                    sb.Append(StringUtil.cleanParameterDeclarationLine(line.Substring(i + 1).Trim()) + "\n");
                //multiline block closed
                else if (i == -1 && j > 0)
                    return sb.Append(
                        StringUtil.cleanParameterDeclarationLine(
                            line.Substring(0,j).Trim()
                            )
                           ).ToString();
                //in the middle of a multiline block
                else
                    sb.Append(StringUtil.cleanParameterDeclarationLine(line.Trim()) + "\n");
                    
            }
            return null;
        }
        
    }
}
