using System;
using System.Collections.Generic;

namespace Porter.Models
{
	public class TypeHierarchy : ITypeNode
	{
		public Func<IEnumerable<ITypeNode>> Elements { get; set; }
		public string Name { get; set; }
	}
}