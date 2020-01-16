using System;
using System.Diagnostics;

/*
1.	Даны две двумерных матрицы размерностью 100х100 каждая.
    Задача: написать приложение, производящее их параллельное умножение.
    Матрицы заполняются случайными целыми числами от 0 до 10.

    Мои примечания:
    - при размерности матриц 100х100 нет эффекта от разделения на потоки, т.к. в одном потоке получается даже быстрее
    - зато эффект становится  заметен при умножении матриц размерностью хотя бы от 300х300, а лучше еще больше
*/

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region настройки окна консоли
            //Console.SetBufferSize(640, 480);
            //Console.SetWindowSize(200, 75);
            //Console.SetWindowPosition(0, 0);
            #endregion

            int MATRIX_SIZE;
            do
            {
                Console.WriteLine("Задайте размерность матриц");
            }
            while (!int.TryParse(Console.ReadLine(), out MATRIX_SIZE));

            Matrix matrix1 = new Matrix(MATRIX_SIZE);
            Matrix matrix2 = new Matrix(MATRIX_SIZE);

            matrix1.FillRandomNumbers();
            matrix2.FillRandomNumbers();

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine($"Начало перемножения двух матриц {MATRIX_SIZE}x{MATRIX_SIZE}");

            stopwatch.Start();
            Matrix multMatrixResult = matrix1.Mult(matrix2);
            stopwatch.Stop();

            #region вывод матриц на экран
            //Console.WriteLine("\nМатрица 1:");
            //matrix1.Print();
            //Console.WriteLine("\nМатрица 2:");
            //matrix2.Print();
            //Console.WriteLine("\nРезультат умножения матрицы 1 на матрицу 2:");
            //multMatrixResult.Print();
            #endregion

            Console.WriteLine($"Затраченное время на умножение матриц обычным методом: {stopwatch.ElapsedMilliseconds} мс");

            stopwatch = Stopwatch.StartNew();
            multMatrixResult = matrix1.MultWithTask(matrix2);
            stopwatch.Stop();
            Console.WriteLine($"Затраченное время на умножение матриц с использованием Task: {stopwatch.ElapsedMilliseconds} мс");

            stopwatch = Stopwatch.StartNew();
            multMatrixResult = matrix1.MultWithParallel(matrix2);
            stopwatch.Stop();
            Console.WriteLine($"Затраченное время на умножение матриц с использованием Parallel: {stopwatch.ElapsedMilliseconds} мс");

            stopwatch = Stopwatch.StartNew();
            multMatrixResult = matrix1.MultAsync(matrix2).Result;
            stopwatch.Stop();
            Console.WriteLine($"Затраченное время на умножение матриц с использованием Async: {stopwatch.ElapsedMilliseconds} мс");

            Console.WriteLine("Программа завершена");
            Console.ReadKey();
        }
    }
}
