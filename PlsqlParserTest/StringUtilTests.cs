using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PlsqlParser;

namespace PlsqlParserTest
{
    [TestFixture]
    public class StringUtilTests
    {
        [Test]
        public void parseSchemaNameFromFilename_CorrectInput_ReturnsGL_LOAN()
        {
            var file = @"C:\Documents and Settings\K.FS7020\My Documents\My Dropbox\"
    + @"PlSQLParser\TestData\GL_LOAN.TEST.PKB";
            var schemaName = StringUtil.parseSchemaNameFromFilename(file);
            Assert.IsTrue(schemaName == "GL_LOAN");
        }

        [Test]
        public void cleanParameterDeclarationLine()
        {
            var input_line =
                @"  payment_day          IN         VARCHAR2,";
            var clean_line = StringUtil.cleanParameterDeclarationLine(input_line);
            Assert.IsTrue(clean_line == "payment_day IN varchar2");
        }

    }
}
