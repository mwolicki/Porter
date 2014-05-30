using System;
using System.Collections.Generic;

namespace Porter
{
	public interface IClrData
	{
		IEnumerable<Func<ReferenceObject>> GetHeapObjects();
	}
}