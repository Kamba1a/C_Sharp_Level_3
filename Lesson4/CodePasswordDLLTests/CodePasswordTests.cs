using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePasswordDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodePasswordDLL.Tests
{
    [TestClass()]
    public class CodePasswordTests
    {
        string abc;
        string bcd;
        string empty;

        [TestInitialize] //добавлено для ДЗ
        public void TestInitialize()
        {
            //с помощью [TestInitialize] можно инициализировать какой-либо объект перед выполненнием каждого теста
            //с помощью [ClassInitialize] можно инициализировать один static объект на весь класс для теста всех методов

            Debug.WriteLine("Class Initialize");
            abc = "abc";
            bcd = "bcd";
            empty = "";
        }

        [TestCleanup] //добавлено для ДЗ
        public void ClassCleanUp()
        {
            //здесь можно удалить какой-то объект, если у него есть Dispose()
            //[TestCleanup] для удаления объекта после каждого теста
            //[ClassCleanup] для удаления объекта после последнего теста

            Debug.WriteLine("Class CleanUp");
            abc = null;
            bcd = null;
            empty = null;
        }

        [TestMethod()]
        public void GetCodPasswordTest_abc_bcd()
        {
            Debug.WriteLine("Тест шифрования пароля");
            string input = abc;
            string expect = bcd;
            string result = CodePassword.GetCodPassword(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void GetCodPasswordTest_empty_empty()
        {
            Debug.WriteLine("Тест шифрования пустой строки");
            string input = empty;
            string expect = empty;
            string result = CodePassword.GetCodPassword(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void GetPasswordTest_bcd_abc()
        {
            Debug.WriteLine("Тест дешифрования пароля");
            string input = bcd;
            string expect = abc;
            string result = CodePassword.GetPassword(input);
            Assert.AreEqual(expect, result, "некорректное дешифрование пароля");
        }

        [TestMethod()]
        public void GetPasswordTest_empty_empty()
        {
            Debug.WriteLine("Тест дешифрования пустой строки");
            string input = empty;
            string expect = empty;
            string result = CodePassword.GetPassword(input);
            Assert.AreEqual(expect, result, "метод не возвращает пустую строку при получении такой же строки");
        }
    }
}