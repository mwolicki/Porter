using System;
using Xunit;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public class ChosenFile : ChooseFileCommandTestsBase
	{
		protected readonly Func<string, Func<OpenDumpFileDialogStub>> OpenDumpFile
			= fileName => new Func<OpenDumpFileDialogStub>(() => new OpenDumpFileDialogStub(fileName));

		protected readonly ExtendedDebuggerFactorySpy ExtendedDebuggerFactorySpy = new ExtendedDebuggerFactorySpy();

		[Fact]
		public void Execute_ParamsIsNotHeapObjectList_DoNothing()
		{
			var chooseFileCommand = new ChooseFileCommand(() => ExtendedDebuggerFactorySpy, () => new OpenDumpFileDialogStub(), DumpFileNotFoundAlertSpy);
			chooseFileCommand.Execute(new object());
			Assert.False(ExtendedDebuggerFactorySpy.Executed);
		}

		internal ChooseFileCommand GetChooseFileCommand(string expectedFileName = null)
		{
			return GetChooseFileCommand(() => ExtendedDebuggerFactorySpy, () => new OpenDumpFileDialogStub(expectedFileName), DumpFileNotFoundAlertSpy);
		}
	}
}