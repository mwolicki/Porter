using System.Collections.Generic;

namespace Porter.Models
{
	public interface ITypeDescription
	{
		string Name { get; }
		IEnumerable<string> Fields { get; }
		IEnumerable<string> Methods { get; }
	}
}