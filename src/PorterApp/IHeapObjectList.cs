using System.Collections.ObjectModel;

namespace PorterApp
{
	internal interface IHeapObjectList
	{
		ObservableCollection<ObjectViewModel> Objects { get; set; }
	}
}