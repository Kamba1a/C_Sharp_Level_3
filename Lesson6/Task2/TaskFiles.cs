using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task2
{
    static class TaskFiles
    {
        /// <summary>
        /// Создает файлы *.txt с данными для задания
        /// </summary>
        /// <param name="directory">Имя директории</param>
        /// <param name="filesAmount">Количество файлов</param>
        public static void Create(string directory, int filesAmount)
        {
            Random rand = new Random();
            Directory.CreateDirectory(directory);

            for (int i = 1; i <= filesAmount; i++)
            {
                string fileName = i.ToString();
                StreamWriter sw = new StreamWriter(directory +"/"+ fileName + ".txt", false);

                int randOp = rand.Next(1, 3);
                double randNum1 = (double)rand.Next(1000, 10000) / 100;
                double randNum2 = (double)rand.Next(1000, 10000) / 100;

                string fileText = randOp + " " + randNum1 + " " + randNum2;
                sw.Write(fileText);
                sw.Close();
            }
        }
    }
}
