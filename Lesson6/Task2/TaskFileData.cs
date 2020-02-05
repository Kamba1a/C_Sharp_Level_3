using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task2
{
    class TaskFileData
    {
        /// <summary>
        /// Вид операции
        /// </summary>
        public int Operation { get; set; }
        /// <summary>
        /// Число 1
        /// </summary>
        public double Number1 { get; set; }
        /// <summary>
        /// Число 2
        /// </summary>
        public double Number2 { get; set; }


        /// <summary>
        /// Конструктор - считывает данные из файла по указанному пути
        /// </summary>
        /// <param name="path"></param>
        public TaskFileData(string path)
        {
            StreamReader sr = new StreamReader(path);
            string fileDataStr = sr.ReadToEnd();
            sr.Close();

            string[] fileDataArr = fileDataStr.Split(' ');
            Operation = int.Parse(fileDataArr[0]);
            Number1 = double.Parse(fileDataArr[1]);
            Number2 = double.Parse(fileDataArr[2]);
        }

        /// <summary>
        /// Возвращает результат проведенной операции с данными файла
        /// </summary>
        /// <returns></returns>
        public double Result()
        {
            double res;
            switch (Operation)
            {
                case 1: res = Number1 * Number2; break;
                case 2: res = Number1 / Number2; break;
                default: throw new Exception(nameof(Result) + ": Unknown type of operation");
            }
            return res;
        }
    }
}
