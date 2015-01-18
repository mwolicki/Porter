using System;
using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal interface IDataTargetDecorator : IDisposable
	{
		Architecture Architecture { get; }
		IEnumerable<ClrInfoDecorator> ClrVersions { get; }
		ClrRuntimeDecorator CreateRuntime(string dacFileName);
	}
}