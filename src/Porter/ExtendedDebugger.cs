using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;
using System.Linq;
using RuntimeArchitecture = Microsoft.Diagnostics.Runtime.Architecture;

namespace Porter
{
	internal sealed class ExtendedDebugger : IExtendedDebugger
	{
		private readonly DataTarget _dump;

		public Architecture Architecture
		{
			get
			{
				switch (_dump.Architecture)
				{
					case RuntimeArchitecture.X86:
						return Architecture.X86;

					case RuntimeArchitecture.Amd64:
						return Architecture.X64;

					case RuntimeArchitecture.Arm:
						return Architecture.Arm;

					default:
						return Architecture.Unknown;
				}
			}
		}

		public IEnumerable<IClrData> GetClrs()
		{
			return _dump.ClrVersions.Select(clrVersion => new ClrData(_dump.CreateRuntime(clrVersion.TryGetDacLocation() ?? clrVersion.TryDownloadDac())));
		}

		public ExtendedDebugger(string dumpFilePath)
		{
			_dump = DataTarget.LoadCrashDump(dumpFilePath);
		}

		public void Dispose()
		{
			_dump.Dispose();
		}
	}
}