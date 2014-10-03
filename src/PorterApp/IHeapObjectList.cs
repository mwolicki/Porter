using System.Collections.ObjectModel;
using PorterApp.ViewModel;

namespace PorterApp
{
	internal interface IHeapObjectList
	{
		ObservableCollection<TreeItem> TypesTree { get; set; }
	}
}