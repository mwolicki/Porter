using System;
using System.Collections.Generic;

namespace Porter.Models
{
	public class TypeLeaf : ITypeNode
	{
		public string Name { get; set; }
		public IEnumerable<Func<IReferenceObject>> Instances { get; set; }
	}
}