using System.Collections.ObjectModel;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	internal sealed class HeapObjectListSpy : IHeapObjectList
	{
		public HeapObjectListSpy()
		{
			TypesTree = new ObservableCollection<TreeItem>();
		}

		public ObservableCollection<TreeItem> TypesTree { get; set; }
	}
}