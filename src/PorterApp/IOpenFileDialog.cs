namespace PorterApp
{
	public interface IOpenFileDialog
	{
		bool TryGetFileName(out string fileName);
	}
}