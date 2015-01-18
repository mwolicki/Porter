using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrMethodDecorator
	{
		private readonly ClrMethod _clrMethod;
		private readonly ThreadDispatcher _threadDispatcher;

		public ClrMethodDecorator(ClrMethod clrMethod, ThreadDispatcher threadDispatcher)
		{
			_clrMethod = clrMethod;
			_threadDispatcher = threadDispatcher;
		}

		public string Name
		{
			get { return _threadDispatcher.Process(() => _clrMethod.Name); }
		}
	}
}