using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//2.	* Написать приложение, выполняющее парсинг CSV-файла произвольной структуры и сохраняющего его в обычный txt-файл.
//    Все операции проходят в потоках.CSV-файл заведомо имеет большой объем.


namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Программа считывает файл и разбивает его на несколько частей указанного размера, после чего сохраняет их в формате .txt
            //Сохранение каждой одтельной части файла реализовано в отдельных потоках

            Console.WriteLine("Программа разбивает файл на несколько частей в зависимости от указанного размера, и сохраняет их в формате .txt");

            int filePartSize;   //указанный размер одной части файла в байтах
            do { Console.WriteLine("Укажите желаемый размер части (в байтах), например, 256"); }
            while (!int.TryParse(Console.ReadLine(), out filePartSize));

            using (FileStream fileReader = File.OpenRead("myTestCSV.csv"))
            {
                long fileSize = fileReader.Length;  //размер считываемого файла в байтах
                if (fileSize <= filePartSize)   //если размер файла меньше указанного размера части, то просто записываем один txt файл
                {
                    int fileSizeInt = (int)fileSize;
                    byte[] textByte = new byte[fileSize];
                    fileReader.Read(textByte, 0, fileSizeInt);
                    using (FileStream fileWriter = new FileStream("myTestTXT.txt", FileMode.Create))
                    {
                        fileWriter.Write(textByte, 0, fileSizeInt);
                    }
                }
                else //иначе записываем несколько частей
                {
                    double filePartsAmount = Math.Ceiling((double)fileSize / filePartSize);     //определяет количество записываемых частей файла
                    for (int i = 1; i <= filePartsAmount; i++)
                    {
                        byte[] textByte = new byte[filePartSize];
                        Thread.Sleep(2000); //имитация чтения части файла большого объема
                        fileReader.Read(textByte, 0, filePartSize);

                        int filePartNumber = i; //переменная для передачи потокам в качестве параметра
                        ThreadPool.QueueUserWorkItem((obj) => CreatePartTxtFile(textByte, filePartNumber));

                        //или тоже самое через ручное создание потоков:
                        //Thread thread = new Thread(()=> CreatePartTxtFile(textByte,filePartNumber));
                        //thread.Start();
                    }
                }
            }
            Console.WriteLine("Конец работы главного потока");
            Console.ReadKey();

            //запись одной части файла
            void CreatePartTxtFile(byte[] textByteArray, int filePartNumber)
            {
                using (FileStream fileWriter = new FileStream("myTestTXT_p" + filePartNumber + ".txt", FileMode.Create))
                {
                    Console.WriteLine("Начало записи части " + filePartNumber);
                    Thread.Sleep(5000); //имитация записи большой части файла
                    fileWriter.Write(textByteArray, 0, filePartSize);
                    Console.WriteLine("Часть "+ filePartNumber + " записана");
                }
            }
        }
    }
}
