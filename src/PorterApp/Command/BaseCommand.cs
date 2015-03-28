using System;

namespace PorterApp.Command
{
	public abstract class BaseCommand<T> : ICommand<T> where T:class
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var t = parameter as T;
			if (t != null)
			{
				Execute(t);
			}
		}

		public abstract void Execute(T parameter);

		public event EventHandler CanExecuteChanged;
	}
}