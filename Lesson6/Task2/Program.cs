using System;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

/*
2.	* В директории лежат файлы. По структуре они содержат три числа, разделенные пробелами.
Первое число — целое, обозначает действие: 1- — умножение и 2 — деление.
Остальные два — числа с плавающей точкой.
Задача: написать многопоточное приложение, выполняющее эти действия над числами и сохраняющее результат в файл result.dat
Файлов в директории заведомо много.

Мои примечания:
 - использовала только расширения *.txt для файлов - суть не меняется, зато удобней открывать проверять
 - помоему многопоточная запись результатов одновременно в один и тот же файл - не лучшая идея, корректно ли условие?
 - можно попробовать решить, чтобы многопоточная запись результатов велась сначала в массив, а затем уже одним потоком - в файл result.dat
 - при таком решении, эффективность многопоточности проявляется только при количестве файлов от 2500
*/

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            string filesDirectory = "TaskFiles";

            //TaskFiles.Create(filesDirectory, 2500); //создание файлов для задачи

            if (File.Exists(filesDirectory + "/result.txt")) File.Delete(filesDirectory + "/result.txt");

            string[] allFiles = Directory.GetFiles(filesDirectory);
            double[] results = new double[allFiles.Length];

            Stopwatch stopwatch = new Stopwatch();

            //в один поток:
            stopwatch.Start();
            for (int i = 0; i < allFiles.Length; i++)
            {
                TaskFileData taskFileData = new TaskFileData(allFiles[i]);
                results[i] = taskFileData.Result();
            }
            stopwatch.Stop();
            Console.WriteLine($"Время обработки в одном потоке составило {stopwatch.ElapsedMilliseconds} мс");

            //в несколько потоков через Parallel.For:
            stopwatch = Stopwatch.StartNew();
            Parallel.For(0, allFiles.Length, (i) =>
            {
                TaskFileData taskFileData = new TaskFileData(allFiles[i]);
                results[i] = taskFileData.Result();
            }); //можно задать макс. кол-во потоков: new ParallelOptions { MaxDegreeOfParallelism = 10}, но автомат показался эффективней
            stopwatch.Stop();
            Console.WriteLine($"Время обработки через Parallel.For составило {stopwatch.ElapsedMilliseconds} мс");


            StreamWriter sw = new StreamWriter(filesDirectory + "/result.txt", true);
            for (int i = 0; i < results.Length; i++) sw.WriteLine(results[i]);
            sw.Close();
        }
    }
}