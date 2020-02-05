using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


//1.	Написать приложение, считающее в раздельных потоках: 
//a.факториал числа N, которое вводится с клавиатуры;
//b.сумму целых чисел до N.


namespace Task1
{
    class Program
    {
        /// <summary>
        /// Факториал числа n
        /// </summary>
        /// <param name="n"></param>
        static void FactorialResult(ulong n)
        {
            Console.WriteLine("Запуск подсчета факториала...");
            ulong factorial = 1;
            for (ulong i = 1; i <= n; i++) factorial *= i;
            Console.WriteLine($"Факториал числа {n} = {factorial}");
        }

        /// <summary>
        /// Сумма целых чисел до числа n
        /// </summary>
        /// <param name="n"></param>
        static void SumResult(ulong n)
        {
            Console.WriteLine("Запуск подсчета суммы чисел...");
            ulong sum = 0;
            for (ulong i = 1; i <= n; i++) sum += i;
            Console.WriteLine($"Сумма целых чисел до {n} = {sum}");
        }

        static void Main(string[] args)
        {
            ulong n;
            do { Console.WriteLine("Введите число:"); }
            while (!ulong.TryParse(Console.ReadLine(), out n));

            Thread thread = new Thread(() => SumResult(n));             //подсчет суммы чисел во вторичном потоке
            Console.WriteLine("Старт вторичного потока...");
            thread.Start();
            Console.WriteLine("Запуск функции из главного потока...");
            FactorialResult(n);                                         //подсчет факториала в главном потоке
            Console.WriteLine("Конец работы главного потока.");

            Console.ReadKey();
        }
    }
}
