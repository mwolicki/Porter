using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PorterApp.UserControls
{
	public interface ITreeViewModel
	{
		ObservableCollection<ITreeItem> TreeItems { get; }
		string Title { get; }
	}

	public class TreeViewModel : ITreeViewModel
	{
		public ObservableCollection<ITreeItem> TreeItems { get; set; }

		public string Title { get; set; }
	}
}