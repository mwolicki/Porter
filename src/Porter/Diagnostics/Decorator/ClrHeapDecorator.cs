using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrHeapDecorator : IClrHeapDecorator
	{
		private readonly ThreadDispatcher _threadDispatcher;
		private readonly ClrHeap _clrHeap;

		public ClrHeapDecorator(ClrRuntime clrRuntime, ThreadDispatcher threadDispatcher)
		{
			_threadDispatcher = threadDispatcher;
			_clrHeap = threadDispatcher.Process(() => clrRuntime.GetHeap());
		}

		public ThreadDispatcher Dispatcher
		{
			get { return _threadDispatcher; }
		}


		public IEnumerable<ulong> EnumerateObjects()
		{
			return _threadDispatcher.Process(() => _clrHeap.EnumerateObjects()).Dispatch(_threadDispatcher);
		}

		public IClrTypeDecorator GetObjectType(ulong objRef)
		{
			return new ClrTypeDecorator(this, _threadDispatcher, _threadDispatcher.Process(() => _clrHeap.GetObjectType(objRef)));
		}

		public IEnumerable<IClrTypeDecorator> EnumerateTypes()
		{
			return _threadDispatcher.Process(() => _clrHeap.EnumerateTypes().Select(p => new ClrTypeDecorator(this, _threadDispatcher, p))).Dispatch(_threadDispatcher);
		}
	}
}