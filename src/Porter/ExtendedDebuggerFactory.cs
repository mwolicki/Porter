namespace Porter
{
	public sealed class ExtendedDebuggerFactory : IExtendedDebuggerFactory
	{
		public IExtendedDebugger Create(string dumpFilePath)
		{
			return new ExtendedDebugger(dumpFilePath);
		}
	}
}