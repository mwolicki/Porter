using System;
using System.Collections.Generic;

namespace Porter
{
	public interface IExtendedDebugger : IDisposable
	{
		ArchitectureType Architecture { get; }
		IEnumerable<IClrData> GetClrs();
	}
}