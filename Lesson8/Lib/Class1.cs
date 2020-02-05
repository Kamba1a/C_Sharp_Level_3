using System;

namespace Lib
{
	public class PublicTestClass1
	{
		private int _data;
		public int Data => _data;
	}
	public class PublicTestClass2
	{
		private void PrintString(string str) => Console.WriteLine("Private print {0}",
			str);
	}
	internal class InternalTestClass1
	{
		public static InternalTestClass1 CreateClass() => new InternalTestClass1();
	}
	internal class InternalTestClass2
	{
		static InternalTestClass2()
		{
			Console.WriteLine("InternalTestClass2 static .ctor");
		}
	}
}
