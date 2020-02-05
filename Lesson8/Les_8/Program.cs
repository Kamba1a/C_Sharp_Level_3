using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
namespace Les_8
{
	[Serializable, Description("Класс приложения")]
	class Program
	{
		[Description("Точка входа в приложение")]
		static void Main([Description("Аргументы командной строки")] string[] args)
		{
			const string asmName = "Lib.dll";
			//Загружаем сборку в память
			var asm = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory,
				asmName));
			// Извлекаем все типы из сборки
			foreach (var type in asm.GetTypes())
			{
				Console.WriteLine(type.FullName);
			}
			Console.WriteLine();
			// Запрашиваем тип по полному имени, включающему имя пространства имён
			Type testType = asm.GetType("Lib.PublicTestClass1");
			// Создаём объект типа, используя рефлексию
			object testObject = Activator.CreateInstance(testType);
			// Извлекаем из типа информацию о поле
			FieldInfo fieldInfo = testType.GetField("_data",
			BindingFlags.NonPublic // Поле не должно являться публичным
			| BindingFlags.Instance); // Поле должно пренадлежать экземпляру объекта класса
									  // Получаем значение поля через рефлексию
			int fieldValue = (int)fieldInfo.GetValue(testObject);
			fieldInfo.SetValue(testObject, 13); // Устанавливаем значение поля
			testType = asm.GetType("Lib.PublicTestClass2");
			testObject = Activator.CreateInstance(testType);
			// Извлекаем информацию о методе типа
			MethodInfo methodInfo = testType.GetMethod("PrintString",
		   BindingFlags.Instance | BindingFlags.NonPublic);
			// Получаем сведенья о имени и возвращаемом значении
			Console.WriteLine("{0}:{1}", methodInfo.Name, methodInfo.ReturnType);
			// Перечисляем параметры метода
			foreach (var parameterInfo in methodInfo.GetParameters())
				Console.WriteLine("\t{0}:{1}", parameterInfo.Name,
			   parameterInfo.ParameterType);
			// Вызваем метод
			var methodResult = methodInfo.Invoke(testObject, new object[] { "teststring"});
			// Формируем делегат (получаем ссылку на функцию)
			Action<string> printer =
		   (Action<string>)methodInfo.CreateDelegate(typeof(Action<string>), testObject);
			// Вызваем метод через делегат (без рефлексии - гораздо быстрее)
			printer("test_string 228!");
			// Так можно задачть дерево выражения LINQ через лямда-синтаксис
			Expression<Action<string>> testExpression = str => Console.WriteLine(str);
			// Так можно построить дерево выражение вручную
			var methodCallParameter = Expression.Parameter(typeof(string),
		   "parameter_name"); // Создаём параметр - узел дерева (лист)
			var invokeExpression = Expression.Call( // Создаём узел дерева - вызов функции

			Expression.Constant(testObject), // Передаём в узел вызова функции объект, для которого функция будет вызывана(его тоже упаковываем в узел дерева)

			methodInfo, // Передаём информацию о вызываемом методе

			methodCallParameter); // Передаём узел дерева-параметра
									// Создаём выражение на основе лямда-выраженя с указанием требуемого нам типа делегата,
									// указываем в параметрах дерево и список его параметров
			var callExpression = Expression.Lambda<Action<string>>(invokeExpression,
		   methodCallParameter);
			var printer2 = callExpression.Compile(); // Компилируем дерево в байт-код- получаем интересующий нас делегат
			printer2("test_string 3!!!"); // Вызываем делегат, как обычную функцию.
			testType = asm.GetType("Lib.InternalTestClass1");
			// Получаем информацию о публичном статическом методе
			methodInfo = testType.GetMethod("CreateClass", BindingFlags.Static |
		   BindingFlags.Public);
			testObject = methodInfo.Invoke(null, new object[0]);

			testType = typeof(Program);
			// Извлекаем информацию о атрибутах типа
			var attributes = testType.GetCustomAttributes();
			foreach (var attribute in attributes)
			{
				Console.WriteLine(attribute.ToString());
			}
			methodInfo = testType.GetMethod("Main", BindingFlags.Static |
		BindingFlags.NonPublic);
			// Извлекаем информацию обо всех атрибутах нужного нам типа
			attributes = methodInfo.GetCustomAttributes(typeof(DescriptionAttribute));
			foreach (var attribute in attributes.OfType<DescriptionAttribute>())
			{
				Console.WriteLine("Description {0}", attribute.Description);
			}
			// Проходим по всем параметрам метода
			foreach (var parameterInfo in methodInfo.GetParameters())
			{
				Console.WriteLine("{0}:{1}", parameterInfo.Name,
			   parameterInfo.ParameterType);
				// Извлекаем информацию об атрибутах метода
				foreach (var attribute in
			   parameterInfo.GetCustomAttributes().OfType<DescriptionAttribute>())
				{
					Console.WriteLine("\tdescription {0}", attribute.Description);
				}
			}
			Console.ReadLine();
		}
	}
}
