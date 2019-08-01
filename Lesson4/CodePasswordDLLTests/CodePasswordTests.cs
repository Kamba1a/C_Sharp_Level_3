using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePasswordDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePasswordDLL.Tests
{
    [TestClass()]
    public class CodePasswordTests
    {
        [TestMethod()]
        public void getCodPasswordTest_abc_bcd()
        {
            string input = "abc";
            string expect = "bcd";
            string result = CodePassword.getCodPassword(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void getCodPasswordTest_empty_empty()
        {
            string input = "";
            string expect = "";
            string result = CodePassword.getCodPassword(input);
            Assert.AreEqual(expect, result);
        }
    }
}