using Porter;
using System;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public class ChooseFileCommandTestsBase
	{
		protected readonly DumpFileNotFoundAlertSpy DumpFileNotFoundAlertSpy = new DumpFileNotFoundAlertSpy();
		internal readonly HeapObjectListSpy HeapObjectListSpy = new HeapObjectListSpy();

		internal ChooseFileCommand GetChooseFileCommand(Func<IExtendedDebuggerFactory> extendedDebugger, Func<IOpenFileDialog> openDumpFileFactory, IDumpFileNotFoundAlert dumpFileNotFoundAlertSpy)
		{
			return new ChooseFileCommand(extendedDebugger, openDumpFileFactory,
				dumpFileNotFoundAlertSpy);
		}
	}
}