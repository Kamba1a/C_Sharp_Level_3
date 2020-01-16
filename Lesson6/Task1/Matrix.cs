using System;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Класс "Матрица"
    /// </summary>
    class Matrix
    {
        int[,] _matrix; //матрица - двумерный массив

        /// <summary>
        /// конструктор квадратной матрицы
        /// </summary>
        /// <param name="size"></param>
        public Matrix(int size)
        {
            _matrix = new int[size, size];
            //_matrix = new int [3,3]{ { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }; //для теста
        }

        /// <summary>
        /// конструктор для матрицы N x M
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Matrix(int rows, int columns)
        {
            _matrix = new int[rows, columns];
        }

        /// <summary>
        /// индексатор
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int this[int i, int j]
        {
            get { return _matrix[i, j]; }
            set { _matrix[i, j] = value; }
        }

        /// <summary>
        /// заполнение матрицы случайными числами (по умолч. от 0 до 10)
        /// </summary>
        /// <param name="array"></param>
        public void FillRandomNumbers(int minNumber = 0, int maxNumber = 10)
        {
            if (minNumber < 0 || maxNumber < 0) throw new Exception(nameof(FillRandomNumbers) + ": parameter value cannot be less than 0");
            if (minNumber > maxNumber) throw new Exception(nameof(FillRandomNumbers) + ": minNumber cannot be more than maxNumber");

            Random random = new Random();
            for (int i = 0; i < this.Rows(); i++)
            {
                for (int j = 0; j < this.Columns(); j++)
                {
                    _matrix[i, j] = random.Next(minNumber, maxNumber + 1);
                }
            }
        }

        /// <summary>
        /// вывод элементов матрицы на экран
        /// </summary>
        /// <param name="array"></param>
        public void Print()
        {
            for (int i = 0; i < this.Rows(); i++)
            {
                for (int j = 0; j < this.Columns(); j++)
                {
                    Console.Write($"{_matrix[i, j],-7}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Проверка возможности умножения текущей матрицы на указанную матрицу
        /// </summary>
        /// <param name="matrix"></param>
        private void CheckMatrices(Matrix matrix)
        {
            if (this.Rows() != matrix.Columns()) throw new Exception("Заданные матрицы не могут быть перемножены");
        }

        /// <summary>
        /// умножение текущей матрицы на указанную матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Mult(Matrix matrix)
        {
            CheckMatrices(matrix);

            Matrix resMatrix = new Matrix(this.Rows(), matrix.Columns());

            for (int i = 0; i < resMatrix.Rows(); i++)
            {
                for (int j = 0; j < resMatrix.Columns(); j++)
                {
                    resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j));
                }
            }
            return resMatrix;
        }

        /// <summary>
        /// умножение текущей матрицы на указанную матрицу с использованием Task
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix MultWithTask(Matrix matrix)
        {
            CheckMatrices(matrix);

            Matrix resMatrix = new Matrix(this.Rows(), matrix.Columns());

            #region Мой первоначальный вариант (некорректный)
            //думаю, что такой вариант не правильный, т.к. основной поток скорее всего возвращает resMatrix, не дожидаясь выполнения всех Task в цикле:
            //for (int i = 0; i < resMatrix.Rows(); i++)
            //{
            //    int index = i; //для корректной работы Task
            //    Task task = new Task(() => TaskMethod(index));
            //    task.Start();
            //}
            //return resMatrix;
            #endregion

            Task[] taskArr = new Task[resMatrix.Rows()]; //для Task.WaitAll()

            for (int i = 0; i < taskArr.Length; i++)
            {
                int index = i; //для корректной работы Task
                taskArr[index] = Task.Factory.StartNew(()=>TaskMethod(index));

                #region Task.Factory.StartNew() работает также, как запись в три строки:
                //Task task = new Task(() => TaskMethod(index));
                //taskArr[index] = task;
                //task.Start();
                #endregion
            }

            Task.WaitAll(taskArr);
            return resMatrix;

            void TaskMethod(int i)
            {
                for (int j = 0; j < resMatrix.Columns(); j++)
                {
                    resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j));

                    #region можно еще создать отдельный Task, используя task.Result для каждой операции (хотя это не эффективно):
                    //Task<int> task = new Task<int>(() => MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j)));
                    //task.Start();
                    //resMatrix[i, j] = task.Result;
                    #endregion
                }
            }
        }

        /// <summary>
        /// умножение текущей матрицы на указанную матрицу с использованием Parallel
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix MultWithParallel(Matrix matrix)
        {
            CheckMatrices(matrix);

            Matrix resMatrix = new Matrix(this.Rows(), matrix.Columns());

            Parallel.For(0, resMatrix.Rows(), (i) => ParallelMethod(i));
            //здесь не нужен аналог Task.WaitAll(), т.к. пока не завершатся задачи в Parallel.For(), программа будет ждать
            return resMatrix;

            #region иные варианты (не эффективные):
            //распараллеливание и по строкам, и по колонкам (скорее всего, неэффективно):
            //Parallel.For(0, resMatrix.Rows(), (i)=>
            //    Parallel.For(0, resMatrix.Columns(), (j) =>
            //        resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j))));

            //распараллеливание только по колонкам, а строки в обычном цикле (так вообще нет смысла наверное..):
            //for (int i = 0; i < resMatrix.Rows(); i++) Parallel.For(0, resMatrix.Columns(), (j) => resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j)));
            #endregion

            void ParallelMethod(int i)
            {
                for (int j = 0; j < resMatrix.Columns(); j++)
                {
                    resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j));
                }
            }
        }

        /// <summary>
        /// умножение текущей матрицы на указанную матрицу с использованием ASYNC
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public async Task<Matrix> MultAsync(Matrix matrix)
        {
            CheckMatrices(matrix);

            Matrix resMatrix = new Matrix(this.Rows(), matrix.Columns());

            #region если просто сделать так, то все await'ы будут запускаться поочередно и отличия по времени от синхронного кода не будет:
            //    for (int i = 0; i < resMatrix.Rows(); i++)
            //    {
            //        await AsyncAwaitMethod(i);
            //    }
            #endregion

            #region для запуска всех await'ов одновременно, надо сделать почти также, как и с обычным способом Task
            //на первый взгляд, этот способ почти не отличается от обычного использования Task, но для работы в WPF это различие важно (в теории):

            Task[] tasks = new Task[resMatrix.Rows()]; //массив Task'ов для команды Task.WhenAll
            for (int i = 0; i < resMatrix.Rows(); i++)
            {
                tasks[i] = AsyncAwaitMethod(i); //или если метод AsyncAwaitMethod просто void, то как и раньше: tasks[i] = new Task(() => AsyncAwaitMethod(i));
            }
            await Task.WhenAll(tasks); //как понимаю, эта команда запускает все задачи из массива, а затем ждет их завершения (в отличие от WaitAll, которая просто ждет)
            #endregion

            return resMatrix;

            Task AsyncAwaitMethod(int i) //можно и просто обычный void метод написать, без return Task.Run
            {
                return Task.Run(() =>
                {
                    for (int j = 0; j < resMatrix.Columns(); j++)
                    {
                        resMatrix[i, j] = MultRowColumn(MatrixRowToArray(i), matrix.MatrixColumnToArray(j));
                    }
                });
            }
        }

        /// <summary>
        /// умножение i-го элемента одного массива на i-ый элемент другого массива
        /// </summary>
        /// <param name="matrixRow"></param>
        /// <param name="matrixColumn"></param>
        /// <returns></returns>
        private int MultRowColumn(int[] matrixRow, int[] matrixColumn)
        {
            int result = 0;
            for (int i = 0; i < matrixRow.Length; i++)
            {
                result = result + matrixRow[i] * matrixColumn[i];
            }
            return result;
        }

        /// <summary>
        /// запись элементов строки двумерного массива в обычный массив
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private int[] MatrixRowToArray(int rowNumber)
        {
            int[] matrixRow = new int[this.Columns()];
            for (int i = 0; i < matrixRow.Length; i++)
            {
                matrixRow[i] = _matrix[rowNumber, i];
            }
            return matrixRow;
        }

        /// <summary>
        /// запись элементов стобца двумерного массива в обычный массив
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private int[] MatrixColumnToArray(int columnNumber)
        {
            int[] matrixColumn = new int[this.Rows()];
            for (int i = 0; i < matrixColumn.Length; i++)
            {
                matrixColumn[i] = _matrix[i, columnNumber];
            }
            return matrixColumn;
        }

        /// <summary>
        /// количество строк в матрице
        /// </summary>
        /// <returns></returns>
        public int Rows()
        {
            return _matrix.GetLength(0);
        }

        /// <summary>
        /// количество столбцов в матрице
        /// </summary>
        /// <returns></returns>
        public int Columns()
        {
            return _matrix.GetLength(1);
        }

        public void PrintElem(int i, int j)
        {
            Console.WriteLine(_matrix[i,j]);
        }
    }
}
