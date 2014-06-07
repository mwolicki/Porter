using Xunit;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class ChooseFileCanceled : ChooseFileCommandTestsBase
	{
		private readonly ExtendedDebuggerFactorySpy _extendedDebuggerFactorySpy = new ExtendedDebuggerFactorySpy();

		[Fact]
		public void Execute_NotSelectedFile_DoNothing()
		{
			var chooseFileCommand = GetChooseFileCommand(() => _extendedDebuggerFactorySpy, () => new OpenDumpFileDialogStub(),
				DumpFileNotFoundAlertSpy);

			chooseFileCommand.Execute(HeapObjectListSpy);

			Assert.False(_extendedDebuggerFactorySpy.Executed);
		}
	}
}