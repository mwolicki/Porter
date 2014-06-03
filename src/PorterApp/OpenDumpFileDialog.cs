using Microsoft.Win32;

namespace PorterApp
{
	internal sealed class OpenDumpFileDialog : IOpenFileDialog
	{
		public bool TryGetFileName(out string fileName)
		{
			var fileDialog = new OpenFileDialog { DefaultExt = ".dmp", Filter = "Memory dump (*.dmp)|*.dmp" };

			if (fileDialog.ShowDialog() == true)
			{
				fileName = fileDialog.FileName;
				return true;
			}

			fileName = string.Empty;
			return false;
		}
	}
}