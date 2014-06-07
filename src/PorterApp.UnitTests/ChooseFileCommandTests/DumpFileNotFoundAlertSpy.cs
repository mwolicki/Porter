namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class DumpFileNotFoundAlertSpy : IDumpFileNotFoundAlert
	{
		public void Display(string filePath)
		{
			Count++;
			FilePath = filePath;
		}

		public string FilePath { get; private set; }

		public int Count { get; private set; }
	}
}