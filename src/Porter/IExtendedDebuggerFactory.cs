namespace Porter
{
	public interface IExtendedDebuggerFactory
	{
		IExtendedDebugger Create(string dumpFilePath);
	}
}