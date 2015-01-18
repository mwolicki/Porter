using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrHeapDecorator : IClrHeapDecorator
	{
		private readonly ThreadDispatcher _threadDispatcher;
		private readonly Task<ClrHeap> _clrHeap;

		public ClrHeapDecorator(ClrRuntime clrRuntime, ThreadDispatcher threadDispatcher)
		{
			_threadDispatcher = threadDispatcher;
			_clrHeap = threadDispatcher.ProcessAsync(() => clrRuntime.GetHeap());
		}

		private ClrHeap ClrHeap
		{
			get { return _clrHeap.Result; }
		}


		public IEnumerable<ulong> EnumerateObjects()
		{
			return _threadDispatcher.Process(() => ClrHeap.EnumerateObjects()).Dispatch(_threadDispatcher);
		}

		public IClrTypeDecorator GetObjectType(ulong objRef)
		{
			return new ClrTypeDecorator(this, _threadDispatcher, _threadDispatcher.Process(() => ClrHeap.GetObjectType(objRef)));
		}

		public IEnumerable<IClrTypeDecorator> EnumerateTypes()
		{
			return _threadDispatcher.Process(() => ClrHeap.EnumerateTypes().Select(p => new ClrTypeDecorator(this, _threadDispatcher, p))).Dispatch(_threadDispatcher);
		}
	}
}