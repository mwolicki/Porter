using System.Collections.ObjectModel;

namespace PorterApp.ViewModel
{
	internal interface ITypesTreeViewModel
	{
		ObservableCollection<TreeItem> TypesTree { get; set; }
	}
}