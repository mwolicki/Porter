using System;
using Porter.Diagnostics.Decorator;

namespace Porter.Models
{
	public class TypeLeaf : ITypeNode
	{
		public string Name { get; set; }
		public ISingleThreadEnumerable<Func<IReferenceObject>> Instances { get; set; }
	}
}