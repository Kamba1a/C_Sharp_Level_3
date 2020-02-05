using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MailSender.Tests
{
    [TestClass()]
    public class SchedulerClassTests
    {
        static SchedulerClass sc;
        static TimeSpan ts;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Debug.WriteLine("Class Initialize");
            sc = new SchedulerClass(new List<string>());
            ts = new TimeSpan();
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
        public void TimerTick_Dictionare_correct()
        {
            sc.DatesEmailTexts = new Dictionary<DateTime, string>()
            {
                { new DateTime(2019, 08, 15, 17, 0, 0), "text1" },
                { new DateTime(2019, 08, 15, 17, 30, 0), "text2" },
                { new DateTime(2019, 08, 15, 18, 0, 0), "text3" }
            };

            DateTime dt1 = new DateTime(2019, 08, 15, 17, 0, 0);
            DateTime dt2 = new DateTime(2019, 08, 15, 17, 30, 0);
            DateTime dt3 = new DateTime(2019, 08, 15, 18, 0, 0);

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

        [TestMethod()]  //добавлено для ДЗ - тест на работу сортировки словаря
        public void DatesEmailTexts_dictionary_OrderBy()
        {
            DateTime dt1 = new DateTime(2020, 02, 02, 0, 0, 0);
            DateTime dt2 = new DateTime(2021, 01, 01, 0, 0, 0);
            DateTime dt3 = new DateTime(2022, 12, 12, 0, 0, 0);
            string text = "text";

            Dictionary<DateTime, string> input = new Dictionary<DateTime, string>()
            {
                { dt3, text },
                { dt2, text },
                { dt1, text },
            };

            Dictionary<DateTime, string> expect = new Dictionary<DateTime, string>()
            {
                { dt1, text },
                { dt2, text },
                { dt3, text }
            };

            sc.DatesEmailTexts = input;

            CollectionAssert.AreEqual(expect, sc.DatesEmailTexts);
        }

        //[TestMethod()] //проверка на добавление в словарь одинакового времени - просто смотрела что будет
        //public void DatesEmailTexts_colToDic_addSameKey()
        //{
        //    DateTime dt1 = new DateTime(2020, 02, 02, 0, 0, 0);
        //    DateTime dt2 = new DateTime(2021, 01, 01, 0, 0, 0);
        //    List<DateTime> input = new List<DateTime>() { dt1, dt2, dt1};
        //    sc.DatesEmailTexts = input.ToDictionary<DateTime,DateTime,string>(key=>key,el=>"");
        //}
    }
}