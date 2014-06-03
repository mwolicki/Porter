using Porter;
using System.Globalization;
using Xunit;

namespace PorterApp.UnitTests
{
	public sealed class SizeValueConverterTests
	{
		[Fact]
		public void Convert_ValueIsNull_ReturnUnknownSize()
		{
			Assert.Equal("(Unknown)", Convert(null));
		}

		[Fact]
		public void Convert_ValueIsNumber_ReturnFormatedString()
		{
			Assert.Equal("1 b", Convert(1));
			Assert.Equal("2 b", Convert(2));
			Assert.Equal("1 B", Convert(1 * 8));
			Assert.Equal("1 KB", Convert(8 * 1024));
			Assert.Equal("1.5 KB", Convert(12 * 1024));
			Assert.Equal("2 KB", Convert(2 * 8 * 1024));
			Assert.Equal("1 MB", Convert(8 * 1024 * 1024));
			Assert.Equal("3 GB", Convert(3L * 8 * 1024 * 1024 * 1024));
		}

		private static object Convert(ulong? value)
		{
			var converter = new SizeValueConverter();

			var convert = converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);
			return convert;
		}
	}

	public sealed class ChooseFileCommandTests
	{
		[Fact]
		public void Execute_NotSelectedFile_DoNothing()
		{
			var extendedDebuggerFactorySpy = new ExtendedDebuggerFactorySpy();
			var chooseFileCommand = new ChooseFileCommand(() => extendedDebuggerFactorySpy, () => new OpenDumpFileDialogDummy());
			chooseFileCommand.Execute(null);
			Assert.False(extendedDebuggerFactorySpy.Executed);
		}
	}

	public class ExtendedDebuggerFactorySpy : IExtendedDebuggerFactory
	{
		public bool Executed { get; private set; }

		public IExtendedDebugger Create(string dumpFilePath)
		{
			Executed = true;
			return null;
		}
	}

	public class OpenDumpFileDialogDummy : IOpenFileDialog
	{
		public bool TryGetFileName(out string fileName)
		{
			fileName = null;
			return false;
		}
	}
}