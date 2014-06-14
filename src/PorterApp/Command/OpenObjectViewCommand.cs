using System;
using System.Windows.Input;
using PorterApp.ViewModel;

namespace PorterApp.Command
{
	internal sealed class OpenObjectViewCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var objectDetails = new ObjectDetails
			{
				DataContext = new ObjectDetailsViewModel(((MainViewModel)parameter).SelectedObject.ReferenceObject)
			};
			objectDetails.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}