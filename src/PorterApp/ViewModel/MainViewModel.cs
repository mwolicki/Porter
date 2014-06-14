using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PorterApp.Command;

namespace PorterApp.ViewModel
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
}