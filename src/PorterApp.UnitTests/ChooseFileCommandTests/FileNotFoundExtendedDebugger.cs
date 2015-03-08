using System.IO;
using Porter;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class FileNotFoundExtendedDebugger : IExtendedDebuggerFactory
	{
		public IExtendedDebugger Create(string dumpFilePath)
		{
			throw new FileNotFoundException();
		}
	}
}