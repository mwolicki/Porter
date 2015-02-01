using System.Collections.Generic;

namespace Porter.Models
{
	public class TypeHierarchy : ITypeNode
	{
		public string Name { get; set; }
		public ISingleThreadEnumerable<ITypeNode> Elements { get; set; }
	}
}