using System;
using System.Collections.Generic;

namespace Porter.Models
{
	public class TypeLeaf : ITypeNode
	{
		public string Name { get; set; }
		public ISingleThreadEnumerable<Func<IReferenceObject>> Instances { get; set; }
	}

	public interface ISingleThreadEnumerable<T>:IEnumerable<T>
	{
		T[] ToArray();
		//List<T> ToList();
		//T Single();
		//T SingleOrDefault();
		//T First();
		//T FirstOrDefault();
	}
}