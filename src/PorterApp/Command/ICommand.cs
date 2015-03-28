using System.Windows.Input;

namespace PorterApp.Command
{
	public interface ICommand<in T> : ICommand where T : class
	{
		void Execute(T parameter);
	}
}