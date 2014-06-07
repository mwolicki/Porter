using Porter;
using System;
using Xunit;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class FileNotFound : ChosenFile
	{
		private readonly Func<FileNotFoundExtendedDebugger> _extendedDebugger = () => new FileNotFoundExtendedDebugger();

		[Fact]
		public void Execute_FileNotFound_AlertUser()
		{
			var chooseFileCommand = GetChooseFileCommand();
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(1, DumpFileNotFoundAlertSpy.Count);
		}

		[Fact]
		public void Execute_FileNotFound_AlertUserWithCorrectFilePath()
		{
			const string fileName = "expectedFileName";
			var chooseFileCommand = GetChooseFileCommand();
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(fileName, DumpFileNotFoundAlertSpy.FilePath);
		}

		private ChooseFileCommand GetChooseFileCommand()
		{
			Func<FileNotFoundExtendedDebugger> extendedDebugger = _extendedDebugger;
			Func<OpenDumpFileDialogStub> openDumpFileFactory = OpenDumpFile("expectedFileName");
			DumpFileNotFoundAlertSpy dumpFileNotFoundAlertSpy = DumpFileNotFoundAlertSpy;
			return GetChooseFileCommand(extendedDebugger, openDumpFileFactory, dumpFileNotFoundAlertSpy);
		}
	}
}