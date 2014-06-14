using System;
using System.Windows.Input;

namespace PorterApp
{
	internal class OpenObjectViewCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var objectDetails = new ObjectDetails
			{
				DataContext = new ObjectDetailsViewModel
				{
					ObjectRef = ((MainViewModel) parameter).SelectedObject.ObjectRef
				}
			};
			objectDetails.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}