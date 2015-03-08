using System;
using System.Collections.Generic;
using Porter.Models;

namespace Porter.Diagnostics.Decorator
{
	internal interface IClrHeapDecorator
	{
		IEnumerable<ulong> EnumerateObjects();
		IClrTypeDecorator GetObjectType(ulong objRef);
		IEnumerable<IClrTypeDecorator> EnumerateTypes();
		ThreadDispatcher Dispatcher { get; }
		ISingleThreadEnumerable<Func<IReferenceObject>> GetHeapObjects(string typeName);
		IEnumerable<string> GetTypeNames();
	}
}