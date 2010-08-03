using System;
using System.Collections.Generic;
using System.Text;

namespace PlsqlParser
{
    public enum PlsqlObjectType
    {
        Procedure, Function, PackageBody, PackageHeader
    }

    public class PlsqlObject : IComparable
    {
        public PlsqlObjectType type;
        public string name;
        public string packageName;
        public string schemaName;
        public string isInternal; //no PKS declaration
        public string fileName;
        public int lineNumberStarting; //proc/fun starts here
        public int lineNumberEnding;
        public string parameters;
        //public PlsqlObject[] innerObjects; TODO

        public override string ToString()
        {
            return "name: " + name + ", type:" + type +  ", line: " + lineNumberStarting  ;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var o = obj as PlsqlObject;

            return String.Compare(this.name, o.name);

        }

        #endregion
    }

}
