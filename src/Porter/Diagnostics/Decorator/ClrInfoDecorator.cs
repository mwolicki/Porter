using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrInfoDecorator
	{
		private readonly ClrInfo _clrInfo;
		private readonly ThreadDispatcher _threadDispatcher;

		public ClrInfoDecorator(ClrInfo clrInfo, ThreadDispatcher threadDispatcher)
		{
			_clrInfo = clrInfo;
			_threadDispatcher = threadDispatcher;
		}

		public string TryGetDacLocation()
		{
			return _threadDispatcher.ProcessAsync(() => _clrInfo.TryGetDacLocation()).Result;
		}

		public string TryDownloadDac()
		{
			return _threadDispatcher.ProcessAsync(() => _clrInfo.TryDownloadDac()).Result;
		}
	}
}