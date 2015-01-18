using System.Collections.Generic;

namespace Porter.Diagnostics.Decorator
{
	internal interface IClrTypeDecorator
	{
		IClrHeapDecorator Heap { get; }
		bool IsPrimitive { get; }
		bool IsString { get; }
		IEnumerable<ClrMethodDecorator> Methods { get; }
		bool IsObjectReference { get; }
		IEnumerable<ClrInstanceFieldDecorator> Fields { get; }
		string Name { get; }
		bool HasSimpleValue { get; }
		ulong GetSize(ulong objRef);
		object GetValue(ulong address);
	}
}