using System.Collections.Generic;
using Porter.Models;

namespace Porter
{
	public interface IClrData
	{
		IEnumerable<ITypeNode> GetTypeHierarchy();
	}
}