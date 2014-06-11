using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PorterApp
{
	internal sealed class MainViewModel : NotifyPropertyChange, IHeapObjectList
	{
		public ICommand ChooseFileCommand
		{
			get { return new ChooseFileCommand(); }
		}

		private ObservableCollection<ObjectViewModel> _objects = new ObservableCollection<ObjectViewModel>();

		public ObservableCollection<ObjectViewModel> Objects
		{
			get { return _objects; }
			set
			{
				if (Equals(value, _objects)) return;
				_objects = value;
				OnPropertyChanged();
			}
		}

		public ICommand OpenObjectViewCommand
		{
			get { return new OpenObjectViewCommand(); }
		}

		public ObjectViewModel SelectedObject { get; set; }

		public override event PropertyChangedEventHandler PropertyChanged;
	}

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

	internal class ObjectDetailsViewModel
	{
		public ulong ObjectRef { get; set; }
	}
}