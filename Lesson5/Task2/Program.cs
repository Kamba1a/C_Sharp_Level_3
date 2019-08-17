using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//2.	* Написать приложение, выполняющее парсинг CSV-файла произвольной структуры и сохраняющего его в обычный txt-файл.
//    Все операции проходят в потоках.CSV-файл заведомо имеет большой объем.


    //Не очень понятно, что именно хотят в задании, т.к. насколько я понимаю считывание и запись файла в принципе происходит только в одном в потоке
    //В первом варианте попыталась сделать считывание и запись по принципу 1 строка-1 поток, но по-моему все это как-то бессмысленно
    //Во втором просто вынесла операцию чтения/записи в отдельный поток, но наверное это не то, что было нужно, тк для задания со звездочкой это как-то слишком просто

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Вариант 1 - попытка сделать считывание и запись файла по строкам - не знаю, как закрыть стримы ПОСЛЕ того, как отработали ВСЕ вторичные потоки
            /*
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            object forLocker = new object();
            StreamReader sr = new StreamReader("myTestCSV.csv");
            StreamWriter sw = new StreamWriter("myTestTXT.txt");

            //ThreadPool.SetMinThreads(2, 2);
            //ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

            string line;
            while (!sr.EndOfStream)
            {
                //ThreadPool.QueueUserWorkItem(o => myMethod());    //с ThreadPool почему-то вообще не работает толком
                Thread thread = new Thread(() => myMethod());
                thread.Start();
                //thread.Join();    //если сделать так, то главный поток будет дожидаться завершения каждого из вторичных потоков,но смысл потоков по-моему вообще исчезает
            }

            autoResetEvent.WaitOne();   //так главный поток останавливается, но непонятно, как уведомить его, что ВСЕ вторичные потоки отработали, чтобы можно было закрыть StreamReader и StreamWriter

            sr.Close();
            sw.Close();
            Console.WriteLine("Потоки StreamReader и StreamWriter закрыты");

            void myMethod()
            {
                lock (forLocker)
                {
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                    if (line != null)
                    {
                        sw.WriteLine(line, true);
                        Console.WriteLine("Строка записана в txt файл");
                    }
                    //autoResetEvent.Set();     //если сделать так, то флаг снимается уже после первого отработавшего потока, поэтому это не подходит для уведомления главного потока о завершении ВСЕХ потоков
                }
            }
            Console.ReadKey();
            */
            #endregion

            #region Вариант 2 - операция чтения/записи просто запущена в отдельном потоке

            StreamReader sr = new StreamReader("myTestCSV.csv");
            StreamWriter sw = new StreamWriter("myTestTXT.txt");

            Thread thread = new Thread(() => sw.Write(sr.ReadToEnd()));
            thread.Start();

            thread.Join();

            sr.Close();
            sw.Close();
            Console.WriteLine("Данные из csv файла записаны в txt");

            Console.ReadKey();

            #endregion
        }
    }
}
