using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleConsoleApp
{
	public class ExamplePublicClass
	{
		public string Name { get; set; }

		private int age;

		public ExamplePublicClass(string name, int age)
		{
			Name = name;
			this.age = age;
		}

		public int GetYear()
		{
			return DateTime.Now.Year - age;
		}
	}
}
