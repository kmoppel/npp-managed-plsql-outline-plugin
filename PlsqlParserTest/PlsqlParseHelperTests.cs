using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PlsqlParser;

namespace PlsqlParserTest
{
    [TestFixture]
    public class PlsqlParseHelperTests
    {
        #region IsSingleLineComment

        [Test]    
        public void  IsSingleLineComment_RegularCodeLine_ReturnsFalse()
        {
            const string s = "    i number;";
            Assert.IsFalse(s.isSingleLineComment());
            const string s1 = "    /*  */ i number;";
            Assert.IsTrue(s1.isSingleLineComment());
        }     
        
        [Test]    
        public void  IsSingleLineComment_SingleCommentLine_ReturnsTrue()
        {
            const string s = "    --i number;";
            Assert.IsTrue(s.isSingleLineComment());
            
            const string s1 = "    /* i number;  */";
            Assert.IsTrue(s1.isSingleLineComment());
            
        }

        [Test]
        public void IsSingleLineComment_CodeWithSingleCommentAtEnd_ReturnsFalse()
        {
            const string s1 = " procedure x --comment";
            Assert.IsFalse(s1.isSingleLineComment());
        }

        #endregion




        #region isStartingMultilineComment

        [Test]
        public void IsStartingMultilineComment_RegularCodeLine_ReturnsFalse()
        {
            const string s = "    i number;";
            Assert.IsFalse(s.isStartingMultilineComment());
        }
        /* */
        [Test]
        public void IsStartingMultilineComment_StartsMultilineComment_ReturnsTrue()
        {
            const string s = "    /* i number;";
            Assert.IsTrue(s.isStartingMultilineComment());
        }
        #endregion
    }
}
