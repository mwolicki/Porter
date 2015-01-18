using System.Collections.Generic;

namespace Porter.Models
{
	public class TypeHierarchy : ITypeNode
	{
		public string Name { get; set; }
		public IEnumerable<ITypeNode> Elements { get; set; }
	}
}