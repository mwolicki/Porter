using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrRuntimeDecorator : IClrRuntimeDecorator
	{
		private readonly ThreadDispatcher _threadDispatcher;
		private readonly Task<ClrRuntime> _clrRuntime;

		public ClrRuntimeDecorator(DataTarget dataTarget, string dacFileName, ThreadDispatcher threadDispatcher)
		{
			_threadDispatcher = threadDispatcher;
			_clrRuntime = threadDispatcher.ProcessAsync(() => dataTarget.CreateRuntime(dacFileName));
		}

		public IClrHeapDecorator GetHeap()
		{
			return new ClrHeapDecorator(_clrRuntime.Result, _threadDispatcher);
		}
	}
}