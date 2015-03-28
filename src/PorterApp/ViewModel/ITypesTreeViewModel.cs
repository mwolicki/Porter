using System.Collections.ObjectModel;
using PorterApp.UserControls;

namespace PorterApp.ViewModel
{
	internal interface ITypesTreeViewModel
	{
		ObservableCollection<ObservableCollection<ITreeItem>> TypesTree { get; set; }
	}
}