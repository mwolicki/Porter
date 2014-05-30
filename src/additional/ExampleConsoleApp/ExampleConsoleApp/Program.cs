using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			for (int i = 0; i < 10; i++)
			{
				new ExamplePublicClass("asdasd", i).GetYear();
			}
			Console.ReadLine();
		}
	}
}
