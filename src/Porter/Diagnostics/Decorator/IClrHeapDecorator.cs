using System.Collections.Generic;

namespace Porter.Diagnostics.Decorator
{
	internal interface IClrHeapDecorator
	{
		IEnumerable<ulong> EnumerateObjects();
		IClrTypeDecorator GetObjectType(ulong objRef);
		IEnumerable<IClrTypeDecorator> EnumerateTypes();
		ThreadDispatcher Dispatcher { get; }
	}
}