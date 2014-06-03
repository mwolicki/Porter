using System.Collections.Generic;

namespace Porter.Models
{
	internal sealed class TypeDescription : ITypeDescription
	{
		public string Name { get; set; }

		public IEnumerable<string> Fields { get; set; }

		public IEnumerable<string> Methods { get; set; }
	}
}