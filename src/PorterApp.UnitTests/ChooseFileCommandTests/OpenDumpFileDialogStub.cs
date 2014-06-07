namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class OpenDumpFileDialogStub : IOpenFileDialog
	{
		private readonly bool _fileExists;
		private readonly string _expectedFileName;

		public OpenDumpFileDialogStub()
		{
			_fileExists = false;
		}

		public OpenDumpFileDialogStub(string expectedFileName)
		{
			_fileExists = true;
			_expectedFileName = expectedFileName;
		}

		public bool TryGetFileName(out string fileName)
		{
			fileName = _expectedFileName;
			return _fileExists;
		}
	}
}