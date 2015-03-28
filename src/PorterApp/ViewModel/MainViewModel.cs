using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PorterApp.Command;
using PorterApp.UserControls;

namespace PorterApp.ViewModel
{
	internal sealed class MainViewModel : NotifyPropertyChange, ITypesTreeViewModel
	{
		public ICommand ChooseFileCommand
		{
			get { return new ChooseFileCommand(); }
		}

		private ObservableCollection<ObservableCollection<ITreeItem>> _objects = new ObservableCollection<ObservableCollection<ITreeItem>> {new ObservableCollection<ITreeItem>()};

		public ObservableCollection<ObservableCollection<ITreeItem>> TypesTree
		{
			get { return _objects; }
			set
			{
				if (Equals(value, _objects)) return;
				_objects = value;
				OnPropertyChanged();
			}
		}

		public ObjectViewModel SelectedObject { get; set; }


		public override event PropertyChangedEventHandler PropertyChanged;
	}
}