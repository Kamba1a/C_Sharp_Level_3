using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Tests
{
    [TestClass()]
    public class SchedulerClassTests
    {
        SchedulerClass sc;
        TimeSpan ts;

        // Запускается перед стартом каждого тестирующего метода. 
        [TestInitialize]
        public void TestInitialize()
        {
            sc = new SchedulerClass("", "", new List<string>());
            ts = new TimeSpan();    // возвращаем в случае ошибочно введенного времени
        }

        [TestMethod()]
        public void GetSendTimeTest_empty_ts()
        {
            string input = "";
            TimeSpan expect = ts;
            TimeSpan result = sc.GetSendTime(input);
            Assert.AreEqual(expect, result);
        }

        public void GetSendTimeTest_sdf_ts()
        {
            string input = "sdg";
            TimeSpan expect = ts;
            TimeSpan result = sc.GetSendTime(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void GetSendTimeTest_correctTime_Equal()
        {
            string input = "12:12";
            TimeSpan expect = new TimeSpan(12, 12, 0);
            TimeSpan result = sc.GetSendTime(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void GetSendTimeTest_inCorrectHour_Equal()
        {
            string input = "25:12";
            TimeSpan expect = ts;
            TimeSpan result = sc.GetSendTime(input);
            Assert.AreEqual(expect, result);
        }

        [TestMethod()]
        public void GetSendTimeTest_inCorrecMin_Equal()
        {
            string input = "12:75";
            TimeSpan expect = ts;
            TimeSpan result = sc.GetSendTime(input);
            Assert.AreEqual(expect, result);
        }
    }
}