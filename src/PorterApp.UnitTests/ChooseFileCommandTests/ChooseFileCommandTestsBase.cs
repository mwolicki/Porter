using System;
using Porter;
using PorterApp.Command;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public class ChooseFileCommandTestsBase
	{
		protected readonly DumpFileNotFoundAlertSpy DumpFileNotFoundAlertSpy = new DumpFileNotFoundAlertSpy();
		internal readonly TypesTreeViewModelSpy TypesTreeViewModelSpy = new TypesTreeViewModelSpy();

		internal ChooseFileCommand GetChooseFileCommand(Func<IExtendedDebuggerFactory> extendedDebugger, Func<IOpenFileDialog> openDumpFileFactory, IDumpFileNotFoundAlert dumpFileNotFoundAlertSpy)
		{
			return new ChooseFileCommand(extendedDebugger, openDumpFileFactory,
				dumpFileNotFoundAlertSpy);
		}
	}
}