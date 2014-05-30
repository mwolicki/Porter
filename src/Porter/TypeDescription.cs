using System.Collections.Generic;

namespace Porter
{
	public sealed class TypeDescription
	{
		public string Name { get; private set; }

		public IEnumerable<string> Fields { get; private set; }

		public IEnumerable<string> Methods { get; private set; }

		internal TypeDescription(string name, IEnumerable<string> fields, IEnumerable<string> methods)
		{
			Name = name;
			Fields = fields;
			Methods = methods;
		}
	}
}