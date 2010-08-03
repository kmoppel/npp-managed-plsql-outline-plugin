using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PlsqlParser;

namespace PlsqlParser
{
    public class FileUtil
    {
        public static string [] GetPlsqlFilesFromDirectory(string directory)
        {
            var di = new DirectoryInfo(directory);
            var fi = di.GetFiles();
            var list = new List<string>();

            foreach (var fileInfo in fi)
            {
                if (IsPlsqlFileExtension(fileInfo.Extension))
                    list.Add(fileInfo.FullName);
            }
            return list.ToArray();
        }

        public static bool IsPlsqlFileExtension(string fileExtension)
        {
            return PlsqlConstants.plsqlAnalyzedEtensions.ToArray()
                .Contains(fileExtension.ToLower());
        }
    }
}
