using System;
using System.Collections.Generic;
using System.Text;

namespace PlsqlParser
{
    public interface IParser
    {
        List<PlsqlObject> Objects { get;}
    }
}
