using System.Collections.Generic;
using System.Linq;
using Porter.Diagnostics.Decorator;
using RuntimeArchitecture = Microsoft.Diagnostics.Runtime.Architecture;

namespace Porter
{
	internal sealed class ExtendedDebugger : IExtendedDebugger
	{
		private readonly DataTargetDecorator _dump;

		public ArchitectureType Architecture
		{
			get
			{
					switch (_dump.Architecture)
					{
						case RuntimeArchitecture.X86:
							return ArchitectureType.X86;

						case RuntimeArchitecture.Amd64:
							return ArchitectureType.X64;

						case RuntimeArchitecture.Arm:
							return ArchitectureType.Arm;

						default:
							return ArchitectureType.Unknown;
					}
				
			}
		}

		public IEnumerable<IClrData> GetClrs()
		{
			return _dump.ClrVersions.Select(clrVersion => (IClrData)new ClrData(_dump.CreateRuntime(clrVersion.TryGetDacLocation() ?? clrVersion.TryDownloadDac())));
		}

		public ExtendedDebugger(string dumpFilePath)
		{
			_dump = DataTargetDecorator.LoadDump(dumpFilePath);
		}

		public void Dispose()
		{
			_dump.Dispose();
		}
	}
}