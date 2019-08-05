﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MailSender.Tests
{
    [TestClass()]
    public class SchedulerClassTests
    {
        SchedulerClass sc;
        TimeSpan ts;

        // Запускается перед стартом каждого тестирующего метода. 
        //[ClassInitialize] - с этим атрибутом тест почему-то не проходит
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");
            sc = new SchedulerClass("", "", new List<string>());
            ts = new TimeSpan();    // возвращаем в случае ошибочно введенного времени

            sc.DatesEmailTexts = new Dictionary<DateTime, string>()
            {
                { new DateTime(2016, 12, 24, 22, 0, 0), "text1" },
                { new DateTime(2016, 12, 24, 22, 30, 0), "text2" },
                { new DateTime(2016, 12, 24, 23, 0, 0), "text3" }
            };

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

        [TestMethod()]
        public void TimeTick_Dictionare_correct()
        {
            DateTime dt1 = new DateTime(2016, 12, 24, 22, 0, 0);
            DateTime dt2 = new DateTime(2016, 12, 24, 22, 30, 0);
            DateTime dt3 = new DateTime(2016, 12, 24, 23, 0, 0);

            if (sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString() == dt1.ToShortTimeString())
            {
                Debug.WriteLine("Body " + sc.DatesEmailTexts[sc.DatesEmailTexts.Keys.First<DateTime>()]);
                Debug.WriteLine("Subject " + $"Рассылка от {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortDateString()}  {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString()}");
                sc.DatesEmailTexts.Remove(sc.DatesEmailTexts.Keys.First<DateTime>());
            }

            if (sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString() == dt2.ToShortTimeString())
            {
                Debug.WriteLine("Body " + sc.DatesEmailTexts[sc.DatesEmailTexts.Keys.First<DateTime>()]);
                Debug.WriteLine("Subject " + $"Рассылка от {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortDateString()}  {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString()}");
                sc.DatesEmailTexts.Remove(sc.DatesEmailTexts.Keys.First<DateTime>());
            }

            if (sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString() == dt3.ToShortTimeString())
            {
                Debug.WriteLine("Body " + sc.DatesEmailTexts[sc.DatesEmailTexts.Keys.First<DateTime>()]);
                Debug.WriteLine("Subject " + $"Рассылка от {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortDateString()}  {sc.DatesEmailTexts.Keys.First<DateTime>().ToShortTimeString()}");
                sc.DatesEmailTexts.Remove(sc.DatesEmailTexts.Keys.First<DateTime>());
            }

            Assert.AreEqual(0, sc.DatesEmailTexts.Count);
        }

    }
}