using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class DataTargetDecorator : IDataTargetDecorator
	{
		private readonly string _fileName;
		private readonly ThreadDispatcher _threadDispatcher = new ThreadDispatcher();
		private readonly Task<DataTarget> _dataTarget;

		private DataTargetDecorator(string fileName)
		{
			_fileName = fileName;
			_dataTarget = _threadDispatcher.ProcessAsync(() => DataTarget.LoadCrashDump(fileName));
		}

		public Architecture Architecture
		{
			get { return _threadDispatcher.Process(() => _dataTarget.Result.Architecture); }
		}

		public IEnumerable<ClrInfoDecorator> ClrVersions
		{
			get { return _threadDispatcher.Process(() => _dataTarget.Result.ClrVersions.Select(p => new ClrInfoDecorator(p, _threadDispatcher))).Dispatch(_threadDispatcher); }
		}


		public static DataTargetDecorator LoadDump(string fileName)
		{
			return new DataTargetDecorator(fileName);
		}

		public void Dispose()
		{
			_threadDispatcher.Process(() => _dataTarget.Result.Dispose());
		}

		public ClrRuntimeDecorator CreateRuntime(string dacFileName)
		{
			return new ClrRuntimeDecorator(_dataTarget.Result, dacFileName, _threadDispatcher);
		}
	}
}
