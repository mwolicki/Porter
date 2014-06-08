using Porter;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class ExtendedDebuggerFactorySpy : IExtendedDebuggerFactory
	{
		public bool Executed { get; private set; }

		public string FilePath { get; private set; }

		public IExtendedDebugger Create(string dumpFilePath)
		{
			Executed = true;
			FilePath = dumpFilePath;
			return new NotEmptyDebuggerStub();
		}
	}
}