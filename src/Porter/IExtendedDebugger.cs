using System;
using System.Collections.Generic;

namespace Porter
{
	public interface IExtendedDebugger : IDisposable
	{
		Architecture Architecture { get; }
		IEnumerable<IClrData> GetClrs();
	}
}