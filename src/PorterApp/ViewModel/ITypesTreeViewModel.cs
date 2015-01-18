using System.Collections.ObjectModel;

namespace PorterApp.ViewModel
{
	internal interface ITypesTreeViewModel
	{
		ObservableCollection<ObservableCollection<TreeItem>> TypesTree { get; set; }
	}
}